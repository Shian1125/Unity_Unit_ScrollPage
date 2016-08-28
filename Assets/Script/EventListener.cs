using UnityEngine;
using UnityEngine.EventSystems;

public class EventListener : EventTrigger {
    public delegate void VoidDelegate(GameObject go);

    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;

    static public EventListener Get(GameObject go) {
        EventListener listener = go.GetComponent<EventListener>();

        if (listener == null)
            listener = go.AddComponent<EventListener>();

        return listener;
    }


    public override void OnPointerClick(PointerEventData eventData) {
        if (onClick != null)
            onClick(gameObject);
    }


    public override void OnPointerDown(PointerEventData eventData) {
        if (onDown != null)
            onDown(gameObject);
    }


    public override void OnPointerEnter(PointerEventData eventData) {
        if (onEnter != null)
            onEnter(gameObject);
    }


    public override void OnPointerExit(PointerEventData eventData) {
        if (onExit != null)
            onExit(gameObject);
    }


    public override void OnPointerUp(PointerEventData eventData) {
        if (onUp != null)
            onUp(gameObject);
    }
}
