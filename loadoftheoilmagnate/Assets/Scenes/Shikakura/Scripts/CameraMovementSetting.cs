using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate {
    [CreateAssetMenu(menuName = "CameraMovement", fileName = "CameraMovement")]
    public class CameraMovementSetting : ScriptableObject
    {
        [SerializeField]
        public float X_min, X_max;//CameraのX座標の限界値
        [SerializeField]
        public float Y_min, Y_max;//CameraのY座標の限界値
        [SerializeField]
        public float offset_X, ofsest_Y;
    }
}
