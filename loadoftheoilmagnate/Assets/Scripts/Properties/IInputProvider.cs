using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OilMagnate
{
   public interface IInputProvider
    {
        float Horizontal { get; }
        float Vertical { get; }
        Vector2 Direction { get; }
    }
}
