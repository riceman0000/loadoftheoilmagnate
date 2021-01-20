using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    public class CheckAndCast
    {      
        public float Get_dLength(Vector3 playerPoint, Vector3 objPoint)
        {
            Vector3 deltaVec = new Vector3();
            deltaVec = playerPoint - objPoint;
            return Mathf.Sqrt((deltaVec.x * deltaVec.x) + (deltaVec.y * deltaVec.y));
        }
    }
}
