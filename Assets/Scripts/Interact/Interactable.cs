using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public abstract class Interactable : MonoBehaviour
{
    protected static bool canPickup=true; 
    
    [HideInInspector]
    public ItemHandler.TypeList Type;
    /// <summary>
    /// Была ли последняя интеракия с объектом
    /// </summary>
    public bool IsLastInteracted { get; protected set; }
    [SerializeField]
    protected AudioClip clip;
    [SerializeField]
    protected AudioClip errorSound;
    protected AudioSource source;
    protected virtual void Awake()
    {
        canPickup=true;
        IsLastInteracted = false;
        source = GetComponent<AudioSource>();
    }
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
    public abstract void OnInteract();
}
