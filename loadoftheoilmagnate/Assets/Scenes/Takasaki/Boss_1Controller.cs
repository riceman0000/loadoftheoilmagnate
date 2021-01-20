using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    public class Boss_1Controller : ManagedMono
    {
        public Transform playerTrans;
        public GameObject missilePrefab;
        public GameObject bulletPrefab;
        public int moveSpeed = 10;
        public float interval = 5.0f;
        public int hP = 200;

        Vector2 myPosition;
        float elapsedTime = 0;
        Rigidbody2D rb2d;
        ShotController sc;

        bool isBulletAttacking = false;

        private void Start()
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
            myPosition = transform.position;

            sc = new ShotController();
        }

        // Update is called once per frame
        public override void MUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > interval)
            {
                playerTrans = playerTrans.transform;

                // 0, 1, 2 の3つを返す
                int random = Random.Range(0, 3);
                if (random == 0)
                {
                    ChargeAttack(playerTrans);
                }
                else if (random == 1)
                {
                    MissileAttack(playerTrans);
                }
                else if (random == 2)
                {

                    BulletAttack(playerTrans);
                }

                elapsedTime = 0;
            }
        }

        void ChargeAttack(Transform playerTrans)
        {
            Debug.Log("Charge");
            var vec2 = (playerTrans.position - transform.position).normalized;
            rb2d.velocity = vec2 * moveSpeed;
        }

        void MissileAttack(Transform playerTrans)
        {
            Debug.Log("missile");
            //sc.ShotMissile(missilePrefab, playerTrans);
        }

        void BulletAttack(Transform playerTrans)
        {
            Debug.Log("bullet");
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                rb2d.velocity = Vector2.zero;

                // 接触ダメージ処理

                ReturnPosition();
            }
        }

        void ReturnPosition()
        {
            transform.position = myPosition;
        }

        // プレイヤーの攻撃が当たった時にこれを呼ぶ
        public void TakeDamage(float powerRate)
        {
            hP = hP - (int)(hP * powerRate);
            Debug.Log(hP);
        }
    }
}