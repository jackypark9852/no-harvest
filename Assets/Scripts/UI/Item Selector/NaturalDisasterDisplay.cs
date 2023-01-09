using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NaturalDisasterDisplay : ItemDisplay<NaturalDisasterType>
{
    Button button;
    [SerializeField] GameObject lockGO;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    public void UnlockOrLockDisplay(bool locked)
    {
        button.interactable = !locked;
        lockGO.SetActive(locked);
    }
}
