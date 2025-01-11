using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vavle : Interactable
{
    public GameManager GameManager;
    public Camera _camera;

    public bool aboba = false;
    protected override void Awake()
    {
        base.Awake();
        Type = ItemHandler.TypeList.none;
        source.clip = clip;
        source.Play();
        source.Pause();
    }
    public override void OnFocus()
    {
    }

    private void Update()
    {
        if (!GameManager.StartGame)
        {

            return;
        }
        else if (ItemHandler.CurrentType == ItemHandler.TypeList.none && GameManager.StartGame)
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 2.3f))
            {
                if (hit.collider.gameObject.GetComponent<Vavle>())
                {
                    if (Input.GetButton("Fire1"))
                    {
                        source.UnPause();

                        transform.Rotate(0, 0, .5f);
                        GameManager.CurrentOxygenCapacity += GameManager._maxOxygenCapacity * 0.01f;
                        if (GameManager.CurrentOxygenCapacity > GameManager._maxOxygenCapacity)
                        {
                            GameManager.CurrentOxygenCapacity = GameManager._maxOxygenCapacity;
                        }
                    }
                    else
                    {
                        source.Pause();
                    }
                }
            }
        }
    }

    public override void OnInteract()
    {
        if (!aboba)
        {
            if (!GameManager.StartGame)
            {
                source.clip = errorSound;
                source.PlayOneShot(errorSound);
            }
            else
            {
                source.clip = clip;
                source.Play();
                source.Pause();
                aboba = true;
            }
        }
    }

    public override void OnLoseFocus()
    {
    }
}
