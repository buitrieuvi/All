using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InventorySlotView View { get; private set; }

    public void Awake()
    {
        View = GetComponentInChildren<InventorySlotView>();
    }
}
