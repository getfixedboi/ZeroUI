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
        hammer,
        lamp,
        gasoline,
        ognetushitel
    }
    public static TypeList CurrentType;
}
