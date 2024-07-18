using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchNeeded : Interactable
{
    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if(ItemHandler.CurrentType == ItemHandler.TypeList.wrench)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLoseFocus()
    {
    }
}
