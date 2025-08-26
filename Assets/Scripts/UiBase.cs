using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UiBase : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    //public event Action OnClick;
    public Button Button { get; set; }

    public RectTransform Rt;

    public virtual void Awake()
    {
        Button = GetComponent<Button>();
        Rt = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Button != null) 
        {
            Button.onClick.AddListener(PointerClick);
            return;
        }

        PointerClick();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public virtual void PointerClick() 
    {

    }
}
