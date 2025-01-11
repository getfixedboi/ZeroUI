using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    public AudioClip wind;
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(wind);
    }

    
}
