using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampKrishkaSwitcher : Interactable
{
    public GameObject anotherKrishka;
    public Lampochka soundSource;
    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        anotherKrishka.SetActive(true);
        soundSource.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        gameObject.SetActive(false);
    }

    public override void OnLoseFocus()
    {
    }
}
