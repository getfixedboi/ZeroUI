using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammer : Interactable
{
    public GameObject ToggleObject;
    protected override void Awake()
    {
        base.Awake();
        Type = ItemHandler.TypeList.hammer;
    }
    public override void OnFocus()
    {
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Q))
        {
            return;
        }
        else
        {
            if (ItemHandler.CurrentType == ItemHandler.TypeList.hammer)
            {
                source.PlayOneShot(clip);
                GetComponent<MeshRenderer>().enabled = true;
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
        }
    }
    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.hammer || ItemHandler.CurrentType == ItemHandler.TypeList.none)
        {
            if (ToggleObject.activeSelf)
            {
                source.PlayOneShot(clip);
                GetComponent<MeshRenderer>().enabled = true;
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
            else
            {
                source.PlayOneShot(clip);
                GetComponent<MeshRenderer>().enabled = false;
                ToggleObject.gameObject.SetActive(true);
                ItemHandler.CurrentType = ItemHandler.TypeList.hammer;
            }
        }
    }

    public override void OnLoseFocus()
    {
    }
}
