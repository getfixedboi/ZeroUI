using UnityEngine;
using System.Collections;
using System;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector]
    public ItemHandler.TypeList Type;
    /// <summary>
    /// Была ли последняя интеракия с объектом
    /// </summary>
    public bool IsLastInteracted { get; protected set; }
    public bool IsPicked{ get; protected set; }
    protected virtual void Awake()
    {
        IsLastInteracted = false;
        IsPicked = false;
    }
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
    public abstract void OnInteract();
}
