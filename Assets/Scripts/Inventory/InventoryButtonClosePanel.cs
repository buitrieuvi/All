using DG.Tweening;

public class InventoryButtonClosePanel : UiBase 
{
    public override void PointerEnter()
    {
        base.PointerEnter();

        Rt.DOKill();
        Rt.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public override void PointerExit() 
    {
        base.PointerExit();

        Rt.DOKill();
        Rt.DOScale(1f, 0.25f);
    }

}
