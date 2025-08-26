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

    [SerializeField] private ButtonBase _btnClose;


    public override void Start()
    {
        base.Start();

        _slots = new List<InventorySlot>();

        if (_btnClose != null) 
        {
            _btnClose.Button.onClick.AddListener(() =>
            {
                PanelManager.OnInputAction<InventoryPanel>();
            });
        }

        for (int i = 0; i < 11 * 5; i++)
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

            // Thêm tween vào sequence, join để chạy song song
            seq.Join(
                slot.View.Rt.DOScale(1f, 0.15f)
                    .SetDelay(0.0001f * slot.transform.GetSiblingIndex())
            );
        }

        // Khi toàn bộ seq kết thúc mới gọi base.OnCompleted()
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
