using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepSound : MonoBehaviour
{
    public PlayerMovement controller;
    private AudioSource source;
    public AudioClip[] clips;

    public float delay;
    private float footstepTimer;
    private bool isFirstStep = true;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (controller.MoveInput == Vector3.zero)
        {
            footstepTimer = delay;
            isFirstStep = true;
        }
        else
        {
            if (isFirstStep)
            {
                source.PlayOneShot(GetRandomClip());
                isFirstStep = false;
                footstepTimer = delay;
            }
            else
            {
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0f)
                {
                    source.PlayOneShot(GetRandomClip());
                    footstepTimer = delay;
                }
            }
        }
    }

    private AudioClip GetRandomClip()
    {
        source.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        return clips[Random.Range(0, clips.Length)];
    }
}
