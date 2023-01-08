using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemDisplay<T> : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] ItemData<T> item;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = item.sprite;
    }

    void OnSelect()
    {
        ItemSelectionManager<T>.Instance.SelectItem(item.item);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("here");
    }
}
