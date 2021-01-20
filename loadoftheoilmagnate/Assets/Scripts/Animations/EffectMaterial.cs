using System;
using UnityEngine;
namespace OilMagnate
{
    public /* abstract */ class EffectMaterial<TEnum> : MonoBehaviour where TEnum : Enum
    {

    }

    // TODO I'll make it a general class.
    public enum EffectTag
    {
        Fire,
        Water,
        Leaf,
    }
}