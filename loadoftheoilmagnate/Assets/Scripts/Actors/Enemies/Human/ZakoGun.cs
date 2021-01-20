using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OilMagnate.StageScene
{
    public class ZakoGun : MonoBehaviour
    {
        [SerializeField]
        GameObject _bulletPrefab;

        [SerializeField]
        float _attackInterval = 3f;

        [SerializeField]
        float _burstInterval = .5f;

        [SerializeField]
        float _bulletSpd = 10f;

        [SerializeField]
        int _burstAmount = 3;

        [SerializeField]
        float _detectRange;

        [SerializeField]
        Transform _emitNode;

        List<Bullet> _emittedBullets = new List<Bullet>();

        Transform _player;

        bool _isAttackable;

        public float Magnitude
        {
            get
            {
                var p = _player.position;
                p.z = 0f;
                var pos = transform.position;
                pos.z = 0f;
                return (p - pos).magnitude;
            }
        }

        public bool IsWithinRange
        {
            get
            {
                Debug.Log($"Magnitude = {Magnitude}, range = {_detectRange} ,{ Magnitude < _detectRange}");
                return Magnitude < _detectRange;
            }
        }

        private void Start()
        {
            _player = StageManager.Instance.Player.transform;
            var elapsedTime = 0f;

            this.UpdateAsObservable()
                .Where(_ => !_isAttackable)
                .Subscribe(_ =>
                {
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > _attackInterval)
                    {
                        _isAttackable = true;
                        elapsedTime = 0f;
                    }
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => IsWithinRange && _isAttackable)
                .Subscribe(_ =>
                {
                    Fire();
                })
                .AddTo(this);
        }

        async void Fire()
        {
            _isAttackable = false;
            var diff = StageManager.Instance.Player.transform.position - transform.position;
            diff.y = 0;
            diff.x = diff.x >= 0 ? 1f : -1f;
            transform.localScale = new Vector3(diff.x, 1, 1);

            for (int count = 0; count < _burstAmount; count++)
            {
                if (!this) return;
                var go = Instantiate(_bulletPrefab, _emitNode.position, Quaternion.Euler(0, 0, 90f));
                var bullet = go.GetComponent<Bullet>();
                bullet.Initialize(this);
                var force = diff.normalized * _bulletSpd;
                bullet.Fire(force);
                _emittedBullets.Add(bullet);
                await UniTask.Delay(TimeSpan.FromSeconds(_burstInterval));
            }
        }

        public void DisposeBullets()
        {
            foreach (var bullet in _emittedBullets)
            {
                Destroy(bullet.gameObject);
            }
            _emittedBullets.Clear();
        }
    }
}