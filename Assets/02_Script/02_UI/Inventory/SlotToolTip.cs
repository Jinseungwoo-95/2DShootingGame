using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    public static SlotToolTip instance;
    public bool openBuy;    // Buy창이 켜져있을 경우 툴팁 비활성화 시키는 용도

    // 필요한 컴포넌트
    [SerializeField] private GameObject go_Base;
    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Text txt_ItemDesc;
    [SerializeField] private Text txt_ItemHowToUsed;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void ShowToolTip(Item _item, Vector3 _pos, bool _inventorySlot)
    {
        if (!openBuy)
        {
            go_Base.SetActive(true);
            _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height, 0);
            go_Base.transform.position = _pos;

            txt_ItemName.text = _item.itemName;
            txt_ItemDesc.text = _item.itemDesc;

            if (_inventorySlot)
            {
                if (_item.itemType == Item.ItemType.Equipment)
                    txt_ItemHowToUsed.text = "우클릭 - 장착";
                else if (_item.itemType == Item.ItemType.Used)
                    txt_ItemHowToUsed.text = "우클릭 - 먹기";
                else
                    txt_ItemHowToUsed.text = "";
            }
            else
            {
                txt_ItemHowToUsed.text = "";
            }
        }
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }

}
