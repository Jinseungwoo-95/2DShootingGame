using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Item item;

    [SerializeField] Image itemImage;
    [SerializeField] Text itemName;
    [SerializeField] Text itemPrice;
    Store store;

    public void SetItem(Item _item)
    {
        store = FindObjectOfType<Store>();
        item = _item;
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;
        itemPrice.text = item.buyPrice.ToString() + "원";
    }

    public void OnClickSlot()
    {
        store.BuyOpen(item);
    }

    // 마우스가 슬롯에 들어갈 때 발동
    public void OnPointerEnter(PointerEventData eventData)
    {
        SlotToolTip.instance.ShowToolTip(item, transform.position, false);
    }

    // 마우스가 슬롯에서 빠져나갈 때 발동
    public void OnPointerExit(PointerEventData eventData)
    {
        SlotToolTip.instance.HideToolTip();
    }
}
