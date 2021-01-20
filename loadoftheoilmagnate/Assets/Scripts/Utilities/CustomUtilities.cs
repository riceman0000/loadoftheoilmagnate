using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    public static class CustomUtilities
    {
        public static void Swap<T>(ref T[] array, int target1, int target2)
        {
            var temp = array[target1];
            array[target1] = array[target2];
            array[target2] = temp;
        }

        public static Vector2[] ConvertV3ArrayToV2Array(Vector3[] vector3Array)
        {
            var res = new Vector2[vector3Array.Length];
            for (int i = 0; i < vector3Array.Length; i++)
            {
                res[i] = new Vector2(vector3Array[i].x, vector3Array[i].y);
            }
            return res;
        }

        public static Vector2 GetVarticalVector(Vector2 calcTarget) => new Vector2(-calcTarget.y, calcTarget.x);
    }
}
