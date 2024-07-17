using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ItemHandler : MonoBehaviour
{
    public enum TypeList
    {
        none,
        wrench,
        hammer
    }
    public static TypeList CurrentType;
}
