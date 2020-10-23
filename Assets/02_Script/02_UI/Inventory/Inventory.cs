using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletInfo
{
    public Item item;
    public int slotNum;
}

public class Inventory : MonoBehaviour
{
    public bool inventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField] public GameObject go_InventoryBase;
    [SerializeField] public GameObject go_SlotsParent;

    private Slot[] slots;

    public Slot[] GetSlots() { return slots; }

    public List<string> bulletName;
    public Dictionary<string, BulletInfo> dicBulletInfo;
    private TestGun testGun;

    private void Awake()
    {
        testGun = FindObjectOfType<TestGun>();
        dicBulletInfo = new Dictionary<string, BulletInfo>();
        bulletName = new List<string>();
    }

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    // 인벤토리 열기/닫기
    public void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I) && !GameController.instance.OpenStore)
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    public void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        SlotToolTip.instance.HideToolTip();
        go_InventoryBase.SetActive(false);
    }

    // 아이템 획득
    public void AcquireItem(Item _item, int _count = 1)
    {
        if (_item.itemType != Item.ItemType.Equipment)
        {
            // 인벤토리에 동일한 아이템이 있다면
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        if (_item.itemType == Item.ItemType.Bullet)
                        {
                            slots[i].SetSlotCount(_count * 100);
                        }
                        else
                            slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        // 빈자리에 아이템 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                if (_item.itemType == Item.ItemType.Bullet)
                {
                    BulletInfo bulletInfo = new BulletInfo();
                    bulletInfo.item = _item;
                    bulletInfo.slotNum = i;
                    dicBulletInfo.Add(_item.itemName, bulletInfo);
                    bulletName.Add(_item.itemName);
                    slots[i].AddItem(_item, _count * 100);
                }
                else
                    slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

    public Item GetBulletItem(int _index)
    {
        return dicBulletInfo[bulletName[_index]].item;
    }

    public void ShotBullet(string _name)
    {
        int n = dicBulletInfo[_name].slotNum;
        if (slots[n].itemCount <= 1)
        {
            dicBulletInfo.Remove(_name);
            bulletName.Remove(_name);
            testGun.SetBulletInfo();
        }
        slots[n].SetSlotCount(-1);
    }

    public string GetBulletCnt(string _name)
    {
        return slots[dicBulletInfo[_name].slotNum].itemCount.ToString();
    }
}
