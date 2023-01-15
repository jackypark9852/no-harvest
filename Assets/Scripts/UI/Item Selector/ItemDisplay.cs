using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public abstract class ItemDisplay<T> : MonoBehaviour
{
    public ItemData<T> item;

    Image image;

    [SerializeField] Image frameImage;

    protected virtual void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = item.sprite;
    }


    public virtual void HandleClick()
    {
        ItemSelectionManager<T>.Instance.SelectItem(this);
    }

    public void SetFrameActive(bool active)
    {
        Debug.Log("Set frame active");
        frameImage.gameObject.SetActive(active);
    }
}
