using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LAMPA : Interactable
{
    public GameManager gameManager;
    public Light popokKakObichno;
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.lamp && gameManager.StartGame && !gameManager._interactCD)
        {
            gameManager._lampochka.SetActive(false);
            ItemHandler.CurrentType = ItemHandler.TypeList.none;
            source.PlayOneShot(gameManager._lampSound);
            StartCoroutine(C_InteractCD());
            gameManager.CurrentLampDutability = gameManager._maxLampDutability;
            popokKakObichno.intensity = 1f;

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
        gameManager._interactCD = true;
        yield return new WaitForSeconds(.5f);
        gameManager._interactCD = false;
    }


}
