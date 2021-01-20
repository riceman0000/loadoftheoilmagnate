using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate
{
    public class ChangeGravity : ManagedMono
    {
        [SerializeField] float p_GravitySetting = 6.3f;//playerの重力加速度
        Rigidbody2D rb;
        Physics2D physics2D;
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }
        public override void MFixedUpdate()
        {
            //rb.AddForce(Vector2.down * p_GravitySetting * 1000);
            //Physics2D.gravity = new Vector2(0f, -p_GravitySetting);
        }
    }
}