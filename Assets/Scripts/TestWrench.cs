using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWrench : Interactable
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
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                ItemHandler.CurrentType = ItemHandler.TypeList.none;
                ToggleObject.gameObject.SetActive(false);
            }
        }
    }
    public override void OnInteract()
    {
        if (ToggleObject.activeSelf)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            ItemHandler.CurrentType = ItemHandler.TypeList.none;
            ToggleObject.gameObject.SetActive(false);
        }
        else
        {
            ToggleObject.gameObject.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            ItemHandler.CurrentType = ItemHandler.TypeList.wrench;
        }
    }
    public override void OnFocus() { }
    public override void OnLoseFocus() { }
}
