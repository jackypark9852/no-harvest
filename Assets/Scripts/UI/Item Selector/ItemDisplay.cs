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

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = item.sprite;
    }

    public void HandleClick()
    {
        ItemSelectionManager<T>.Instance.SelectItem(this);
    }

    public void SetFrameActive(bool active)
    {
        frameImage.gameObject.SetActive(active);
    }
}
