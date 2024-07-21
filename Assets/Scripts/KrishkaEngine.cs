using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrishkaEngine : Interactable
{
    public GameManager manager;

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.wrench && manager.StartGame && !manager._interactCD)
        {
            if (manager.GasTankOpened)
            {
                manager.gameObject.GetComponent<AudioSource>().PlayOneShot(manager._wrenchSound);
                manager.GasTankOpened = false;
                manager.StartCD();
                manager._closedKrishka.SetActive(true);
                manager._openedKrishka.SetActive(false);
            }
            else
            {
                manager.gameObject.GetComponent<AudioSource>().PlayOneShot(manager._wrenchSound);
                manager.GasTankOpened = true;
                manager.StartCD();
                manager._openedKrishka.SetActive(true);
                manager._closedKrishka.SetActive(false);
            }
        }
        else
        {
            source.PlayOneShot(errorSound);
        }
    }

    public override void OnLoseFocus()
    {
    }
}
