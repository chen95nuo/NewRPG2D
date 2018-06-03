using UnityEngine;
using UnityEngine.EventSystems;
public class EventTriggerListener : MonoBehaviour , IPointerClickHandler
{
    public TDelegate<GameObject> onClick;
    public TDelegate<GameObject> onDown;
    public TDelegate<GameObject> onEnter;
    public TDelegate<GameObject> onExit;
    public TDelegate<GameObject> onUp;
    public TDelegate<GameObject> onSelect;
    public TDelegate<GameObject> onUpdateSelect;

    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }
    //public override void OnPointerDown(PointerEventData eventData)
    //{
    //    if (onDown != null) onDown(gameObject);
    //}
    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (onEnter != null) onEnter(gameObject);
    //}
    //public override void OnPointerExit(PointerEventData eventData)
    //{
    //    if (onExit != null) onExit(gameObject);
    //}
    //public override void OnPointerUp(PointerEventData eventData)
    //{
    //    if (onUp != null) onUp(gameObject);
    //}
    //public override void OnSelect(BaseEventData eventData)
    //{
    //    if (onSelect != null) onSelect(gameObject);
    //}
    //public override void OnUpdateSelected(BaseEventData eventData)
    //{
    //    if (onUpdateSelect != null) onUpdateSelect(gameObject);
    //}
}