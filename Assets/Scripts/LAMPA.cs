using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LAMPA : Interactable
{
    public GameManager GameManager;
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (!GameManager._gasTankOpened && ItemHandler.CurrentType == ItemHandler.TypeList.lamp && GameManager.StartGame)
        {
            GameManager._lampochka.SetActive(false);
            ItemHandler.CurrentType = ItemHandler.TypeList.none;
            source.PlayOneShot(GameManager._lampSound);
            StartCoroutine(C_InteractCD());
           GameManager.CurrentLampDutability += GameManager._maxLampDutability * 0.25f;
            if (GameManager.CurrentLampDutability > GameManager._maxLampDutability)
            {
                GameManager.CurrentLampDutability = GameManager._maxLampDutability;
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

    private IEnumerator C_InteractCD()
    {
       GameManager._interactCD = true;
        yield return new WaitForSeconds(.5f);
       GameManager._interactCD = false;
    }


}
