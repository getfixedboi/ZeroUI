using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Gasoline : Interactable
{
    public GameObject ToggleObject;
    protected override void Awake()
    {
        base.Awake();
        Type = ItemHandler.TypeList.gasoline;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Q))
        {
            return;
        }
        else
        {
            if (ItemHandler.CurrentType == ItemHandler.TypeList.gasoline)
            {
                StartCoroutine(C_PickUpCD());
                source.PlayOneShot(clip);
                Switcher(gameObject, true);
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
        }
    }
    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.gasoline || ItemHandler.CurrentType == ItemHandler.TypeList.none)
        {
            if (ToggleObject.activeSelf)
            {
                StartCoroutine(C_PickUpCD());
                source.PlayOneShot(clip);
                Switcher(gameObject, true);
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
            else
            {
                if (canPickup)
                {
                    source.PlayOneShot(clip);
                    Switcher(gameObject, false);
                    ToggleObject.gameObject.SetActive(true);
                    ItemHandler.CurrentType = ItemHandler.TypeList.gasoline;
                }
                else
                {
                    source.PlayOneShot(errorSound);
                }
            }
        }
    }
    public override void OnFocus() { }
    public override void OnLoseFocus() { }
    private void Switcher(GameObject _object, bool state)
    {
        _object.transform.GetChild(0).gameObject.SetActive(state);
        _object.transform.GetChild(1).gameObject.SetActive(state);
        _object.transform.GetChild(2).gameObject.SetActive(state);
    }
    private IEnumerator C_PickUpCD()
    {
        canPickup = false;
        yield return new WaitForSeconds(2f);
        canPickup = true;
    }
}
