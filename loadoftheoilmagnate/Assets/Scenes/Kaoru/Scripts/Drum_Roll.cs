using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate
{
    public class Drum_Roll : ManagedMono
    {
        Rigidbody2D rb;
        private int rl = 0;
        private bool jumpFlag = false;
        public float jump;
        private float time = 0;
        private void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }
        public override void MUpdate()
        {
            Jump();
        }
        private void OnCollisionEnter2D(Collision2D c)
        {
            if (c.gameObject.tag == "Player")
            {
                var pos = c.transform.position.x;
                if (pos > this.transform.position.x)
                {
                    rl = -1;
                    Move(rl);//左向き
                }
                else if (pos < this.transform.position.x)
                {
                    rl = 1;
                    Move(rl);//右向き
                }
            }
            if (c.gameObject.tag == "Container")
            {
                rb.AddForce(Vector2.up * 200f);
                jumpFlag = true;
                jump = this.transform.position.y + 0.05f;
                if (rl == 1) rl = -1;
                else rl = 1;
            }
        }
        private void OnCollisionStay2D(Collision2D c)
        {
            if (c.gameObject.tag == "Player")
            {
                var pos = c.transform.position.x;
                if (pos > this.transform.position.x)
                {
                    rl = -1;
                    Move(rl);//左向き
                }
                else if (pos < this.transform.position.x)
                {
                    rl = 1;
                    Move(rl);//右向き
                }
            }
        }
        private void Jump()
        {
            if (jumpFlag)
            {
                time += Time.deltaTime;
                if (jump >= this.transform.position.y && time > 0.5f)
                {
                    Move(rl);
                    jumpFlag = false;
                    time = 0f;
                }
            }
        }
        
        private void Move(int rlMove)
        {
            rb.velocity = new Vector2(rlMove * 10f,0);
        }
    }
}
