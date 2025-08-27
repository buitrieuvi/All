using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryPanel : PanelBase
{

    [SerializeField] private ScrollRect _scroll;

    [SerializeField] private InventorySlot _prefabs;

    [SerializeField] private List<InventorySlot> _slots;

    private InventorySlot _selected;

    public override void Start()
    {
        base.Start();

        _slots = new List<InventorySlot>();

        for (int i = 0; i < 8 * 5; i++)
        {
            InventorySlot slot = Instantiate(_prefabs, _scroll.content.transform);
            _slots.Add(slot);
        }
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void OnCompleted()
    {
        Sequence seq = DOTween.Sequence();

        foreach (InventorySlot slot in _slots)
        {
            slot.View.Rt.localScale = Vector3.zero;

            seq.Join(slot.View.Rt.DOScale(1f, 0.35f)
                .SetDelay(slot.transform.GetSiblingIndex() * 0.0005f));
        }

        seq.OnComplete(() => base.OnCompleted());
    }

    public override void CloseImmediately()
    {
        base.CloseImmediately();
    }

    public override void OnCloseButton()
    {
        base.OnCloseButton();
    }

    protected override void OnCloseCompleted()
    {
        base.OnCloseCompleted();
    }
}
