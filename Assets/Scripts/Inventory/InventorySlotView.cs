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

}
