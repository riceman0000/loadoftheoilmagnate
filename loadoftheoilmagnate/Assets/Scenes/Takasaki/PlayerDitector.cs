using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    /// <summary>
    /// 索敵用のクラス
    /// </summary>
    /// <remarks>
    /// 索敵用コライダーがある子オブジェクトにつける
    /// </remarks>
    public class PlayerDitector : MonoBehaviour
    {
        //TestHuman testHuman;
        // Start is called before the first frame update
        void Start()
        {
            // 親オブジェクトのスクリプトを引っ張ってくる
            //testHuman = GetComponentInParent<TestHuman>();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                // プレイヤーとの距離が 2 以上なら移動する それ以外は止まる
                if (Vector2.Distance(col.transform.position, this.transform.position) > 2)
                {
                    //testHuman.ChasePlayer(col.transform);
                }
            }
        }
    }
}