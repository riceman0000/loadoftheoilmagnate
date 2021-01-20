using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
namespace OilMagnate.StageScene
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        Rigidbody2D _rb2d;

        [SerializeField]
        long _atk = 10000;

        ZakoGun _owner;

        public void Fire(Vector2 velocity)
        {
            SEManager.Instance.SEPlay(SETag.Shoot);
            _rb2d = GetComponent<Rigidbody2D>();
            _rb2d.velocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals(ObjTag.Player.ToString()))
            {
                StageManager.Instance.Player.MPManager.TakeDamage(_atk);
                _owner.DisposeBullets();
                Destroy(gameObject);
            }

            IGimmick gimmick = null;
            if (!collision.TryGetComponent(out gimmick)) return;
            if (!gimmick.ID.HasFlag(GimmickIdentifier.ZakoGun)
                && !gimmick.ID.HasFlag(GimmickIdentifier.ZakoKnife)
                && !gimmick.ID.HasFlag(GimmickIdentifier.Fighter))
            {
                gimmick.ReduceDurability(_rb2d, 1);
                _owner.DisposeBullets();
                Destroy(gameObject);
            }
        }

        internal void Initialize(ZakoGun zakoGun)
        {
            _owner = zakoGun;
        }
    }
}