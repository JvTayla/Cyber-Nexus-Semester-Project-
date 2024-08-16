using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggleitem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public item item;
    [Header("UI")]
    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private void Start()
    {
        InitialiseItem(item);
    }

    public void InitialiseItem(item newItem)
    {
        image.sprite = newItem.image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

    }

}
