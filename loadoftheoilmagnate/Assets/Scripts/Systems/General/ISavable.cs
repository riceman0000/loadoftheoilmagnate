using UnityEngine;
using System;
using System.IO;

namespace OilMagnate
{
    interface ISavable
    {
        void SaveAsync();
        void Reset();
        void Clear();
    }
}