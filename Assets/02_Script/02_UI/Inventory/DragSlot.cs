using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;

    public Slot dragSlot;   // 드래그 중인 슬롯 저장

    [SerializeField] private Image imageItem;

    private void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image _itemImage)
    {
        Debug.Log("이미지설정");

        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
