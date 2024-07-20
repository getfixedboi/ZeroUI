using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lampochka : Interactable
{
    public GameObject ToggleObject;
    protected override void Awake()
    {
        base.Awake();
        Type = ItemHandler.TypeList.lamp;
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Q))
        {
            return;
        }
        else
        {
            if (ItemHandler.CurrentType == ItemHandler.TypeList.lamp)
            {
                StartCoroutine(C_PickUpCD());
                source.PlayOneShot(clip);
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
        }
    }
    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.lamp || ItemHandler.CurrentType == ItemHandler.TypeList.none)
        {
            if (ToggleObject.activeSelf)
            {
                StartCoroutine(C_PickUpCD());
                source.PlayOneShot(clip);
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
            else
            {
                if (canPickup)
                {
                    source.PlayOneShot(clip);
                    ToggleObject.gameObject.SetActive(true);
                    ItemHandler.CurrentType = ItemHandler.TypeList.lamp;
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
    private IEnumerator C_PickUpCD()
    {
        canPickup = false;
        yield return new WaitForSeconds(0f);
        canPickup = true;
    }
}
