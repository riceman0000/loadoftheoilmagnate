using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    public class Attack : MonoBehaviour
    {
        TestHuman testHuman;
        float interval = 3.0f;
        float elapsedTime = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
            // 親オブジェクトのスクリプトを引っ張ってくる
            testHuman = GetComponentInParent<TestHuman>();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                elapsedTime += Time.deltaTime;
                if(elapsedTime > interval)
                {
                    Debug.Log("attack");

                    elapsedTime = 0.0f;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                elapsedTime = 0.0f;
            }
        }
    }
}