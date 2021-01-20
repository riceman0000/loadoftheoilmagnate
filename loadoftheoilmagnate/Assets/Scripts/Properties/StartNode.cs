using UnityEngine;
using System.Collections;
using OilMagnate.StageScene;

namespace OilMagnate
{
    /// <summary>
    /// Playerのスタート位置になるギミック
    /// </summary>
    public class StartNode : MonoBehaviour
    {
        private void Start()
        {
            Vector2 pos = transform.position;
            StageManager.Instance.Player.transform.position = pos;
        }
    }
}