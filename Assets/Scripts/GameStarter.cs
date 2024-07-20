using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : Interactable
{
    public GameManager manager;

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.none)
        {
            manager.StartGame = true;
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
