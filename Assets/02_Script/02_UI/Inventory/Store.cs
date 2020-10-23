using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [SerializeField] Item[] items;
    [SerializeField] Transform slotParent;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject InventoryBG;
    [SerializeField] GameObject failBuy;
    [SerializeField] Inventory inventory;

    // Buy 관련
    Item buyItem;
    [SerializeField] Slider slider;
    [SerializeField] GameObject buyBG;
    [SerializeField] Text countTxt;
    [SerializeField] Text itemTxt;


    List<StoreSlot> slots;  // 나중에 필요없으면 없애자.

    void Start()
    {
        slots = new List<StoreSlot>();

        for (int i = 0; i < items.Length; i++)
        {
            GameObject obj = Instantiate(slotPrefab, slotParent);
            slots.Add(obj.GetComponent<StoreSlot>());
            slots[i].SetItem(items[i]);
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && GameController.instance.OpenStore)
        {
            StoreClose();
        }
    }

    public void StoreOpen()
    {
        GameController.instance.CanMove = false;
        GameController.instance.OpenStore = true;
        gameObject.SetActive(true);
        InventoryBG.SetActive(true);
    }

    public void StoreClose()
    {
        GameController.instance.CanMove = true;
        GameController.instance.OpenStore = false;
        gameObject.SetActive(false);
        InventoryBG.SetActive(false);
        SlotToolTip.instance.HideToolTip();
    }

    public void BuyOpen(Item _item)
    {
        if (GameController.instance.coins >= _item.buyPrice)
        {
            buyItem = _item;

            itemTxt.text = buyItem.itemName + "을 구매하시겠습니까?";

            if (buyItem.itemType == Item.ItemType.Equipment)
            {
                slider.gameObject.SetActive(false);

            }
            else
            {
                slider.value = 1;
                slider.maxValue = Mathf.FloorToInt(GameController.instance.coins / buyItem.buyPrice);
                slider.gameObject.SetActive(true);
            }

            SlotToolTip.instance.HideToolTip();
            SlotToolTip.instance.openBuy = true;
            buyBG.SetActive(true);
        }
        else
        {
            failBuy.SetActive(true);
        }
    }

    public void BuyClose()
    {
        SlotToolTip.instance.openBuy = false;
        buyBG.SetActive(false);
    }

    public void SlideValueChange()
    {
        countTxt.text = slider.value.ToString();
    }

    public void OnClickBuy()
    {
        int count = int.Parse(countTxt.text);
                
        GameController.instance.AddCoin(-(buyItem.buyPrice * count));

        inventory.AcquireItem(buyItem, count);
        SlotToolTip.instance.openBuy = false;
        BuyClose();
    }

    public void OnClickCancel()
    {
        BuyClose();
    }

    public void OnClickOK()
    {
        failBuy.SetActive(false);
    }
}
