using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;   // 획득한 아이템
    public int itemCount;   // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    // 필요한 컴포넌트
    [SerializeField] private Text text_Count;
    [SerializeField] private GameObject go_CountImage;

    // 아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    // 아이템 이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 슬롯 초기화
    private void ClearSlot()
    {
        SlotToolTip.instance.HideToolTip();
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    #region 인터페이스 구현

    // 마우스가 슬롯에 들어갈 때 발동
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            SlotToolTip.instance.ShowToolTip(item, transform.position, true);
    }

    // 마우스가 슬롯에서 빠져나갈 때 발동
    public void OnPointerExit(PointerEventData eventData)
    {
        if(item != null)
            SlotToolTip.instance.HideToolTip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                // 아이템 사용

                if (item.itemType == Item.ItemType.Used)
                {
                    GameController.instance.UseItem(item.name);
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.dragSlot = null;
        DragSlot.instance.SetColor(0);
    }
    
    // eventData의 위치에 있는 슬롯의 OnDrop이 호출됨!
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        // 슬롯 정보 저장 변수
        Item _tempItem = item;
        int _tempItemCount = itemCount;
        Inventory inventory = GameObject.FindObjectOfType<Inventory>();

        if(DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Bullet)
        {
            inventory.dicBulletInfo[DragSlot.instance.dragSlot.item.itemName].slotNum = int.Parse(transform.name);
        }

        // 드래그한 아이템 정보 저장
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        // 기존에 있던 아이템 정보 이동
        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
            if (item.itemType == Item.ItemType.Bullet)
            {
                inventory.dicBulletInfo[item.itemName].slotNum = int.Parse(DragSlot.instance.dragSlot.name);
            }
        }
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }

    #endregion

}
