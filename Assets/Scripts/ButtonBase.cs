using DG.Tweening;

public class ButtonBase : UiBase 
{
    public override void PointerEnter()
    {
        base.PointerEnter();
        FadeExtensions.DOScaleLoop(Rt);
    }

    public override void PointerExit() 
    {
        base.PointerExit();
        Rt.DOKill();
    }

}
