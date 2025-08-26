using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class InventorySlotView : UiBase
{
    public InventorySlot Slot { get; private set; }

    public override void Awake()
    {
        base.Awake();

        Slot = GetComponentInParent<InventorySlot>();
    }

    //public void OP() 
    //{
    //    Rt.localScale = Vector3.zero;
    //    Rt.DOScale(1f, 0.15f).SetDelay(0.001f * Slot.transform.GetSiblingIndex());
    //}
}
