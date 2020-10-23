using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; // 아이템 이름
    [TextArea] public string itemDesc; // 아이템의 설명
    public Sprite itemImage;    // 아이템 이미지
    public ItemType itemType;
    public int value;   // 총알 => 공격력, 포션 => 회복값, 업그레이드 => 업그레이드 값
    public int buyPrice;
    public int sellPrice;
    public GameObject prefab;

    public enum ItemType
    {
        Equipment,
        Bullet,
        Used,
        Upgrade,
    }
}
