using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    public class BossInstance : MonoBehaviour
    {
        public Collider2D Player { get; set; }
        [SerializeField]
        GameObject Plane;
        bool planeActive = false;

        //右に向かって飛ぶかどうか(falseなら左に向かって飛ぶ。
        [field: SerializeField]
        public bool rightFly { get; set; }

        [field: SerializeField]
        public float speed { get; set; }

        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision);
            if (collision == Player && !planeActive)
            {
                planeActive = true;
                StartCoroutine(BossInstanceCoroutine());
            }
        }

        IEnumerator BossInstanceCoroutine()
        {
            SEManager.Instance.SEPlay(SETag.Plane);
            yield return null;
            Instantiate(Plane, this.transform.parent.transform.GetChild(1));
        }
    }
}