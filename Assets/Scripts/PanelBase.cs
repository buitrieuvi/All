using DG.Tweening;
using UnityEngine;
using Zenject;
[RequireComponent(typeof(CanvasGroup))]
public abstract class PanelBase : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private float _openAnimationDuration = 0.4f;
    private float _closeAnimationDuration = 0.3f;

    [Inject] public PanelManager PanelManager;
    [Inject] public InputController Input;

    public virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

    }

    public virtual void Start()
    {

    }

    public virtual void Open()
    {
        gameObject.SetActive(true);

        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.DOFade(1f, _openAnimationDuration)
            .OnComplete(OnCompleted);
    }

    public virtual void Close()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.DOFade(0f, _closeAnimationDuration)
            .OnComplete(OnCloseCompleted);
    }

    public virtual void OnCompleted() 
    {
        _canvasGroup.interactable = true;
    }

    protected virtual void OnCloseCompleted()
    {
        gameObject.SetActive(false);
    }

    public virtual void CloseImmediately() => OnCloseCompleted();

    public virtual void OnCloseButton() => Close();
}
