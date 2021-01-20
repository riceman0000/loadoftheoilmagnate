using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    public class TestPlayer : ManagedMono
    {
        float speed = 8.0f;
        Rigidbody2D rb2d;

        // Start is called before the first frame update
        void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        public override void MUpdate()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector2 dir = new Vector2(h, v).normalized;
            rb2d.velocity = dir * speed;
        }
    }
}