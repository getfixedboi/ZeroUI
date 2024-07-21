using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : Interactable
{
    public GameManager manager;
    public GameObject engine;
    public Material PressedButton;

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (ItemHandler.CurrentType == ItemHandler.TypeList.none)
        {
            gameObject.GetComponent<Renderer>().material = PressedButton;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z-0.03f);
            engine.SetActive(true);
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
