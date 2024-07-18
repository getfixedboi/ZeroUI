using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : Interactable
{
    public GameObject ToggleObject;
    protected override void Awake()
    {
        base.Awake();
        Type = ItemHandler.TypeList.wrench;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Q))
        {
            return;
        }
        else
        {
            if (ItemHandler.CurrentType == ItemHandler.TypeList.wrench)
            {
                source.PlayOneShot(clip);
                Switcher(gameObject, true);
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
        }
    }
    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.wrench || ItemHandler.CurrentType == ItemHandler.TypeList.none)
        {
            if (ToggleObject.activeSelf)
            {
                source.PlayOneShot(clip);
                Switcher(gameObject, true);
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
            else
            {
                source.PlayOneShot(clip);
                Switcher(gameObject, false);
                ToggleObject.gameObject.SetActive(true);
                ItemHandler.CurrentType = ItemHandler.TypeList.wrench;
            }
        }
    }
    public override void OnFocus() { }
    public override void OnLoseFocus() { }
    private void Switcher(GameObject _object, bool state)
    {
        _object.transform.GetChild(0).gameObject.SetActive(state);
        _object.transform.GetChild(1).gameObject.SetActive(state);
    }
}
