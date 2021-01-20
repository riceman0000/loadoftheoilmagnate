using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    public class Drum : MonoBehaviour
    {
        Rigidbody2D _rb2d;

        [SerializeField]
        Vector2 _force = Vector2.left;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Roll();
            }
        }

        public void Roll()
        {
            _rb2d.AddForce(_force, ForceMode2D.Impulse);
            SEManager.Instance.SEPlay(SETag.Drum);
        }
    }
}