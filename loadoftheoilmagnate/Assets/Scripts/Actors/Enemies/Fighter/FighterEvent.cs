using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    public class FighterEvent : MonoBehaviour
    {
        [SerializeField]
        Collider2D _col2d;

        [SerializeField]
        GameObject _bossPrefab;

        [SerializeField]
        Transform _spawnPosition;

        [SerializeField]
        bool _moveLeft = true;

        [SerializeField]
        float _force = 1f;

        [SerializeField]
        float _reflectionPower = 1f;
        SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();


        private void Start()
        {
            Player.Player player = null;
            disposable.Disposable = _col2d.OnTriggerEnter2DAsObservable()
                .First(col => col.TryGetComponent(out player))
                .Subscribe(col =>
                {
                    var go = Instantiate(_bossPrefab, _spawnPosition.position, Quaternion.identity);
                    var fighter = go.GetComponent<FighterAircraft>();
                    var dir = _moveLeft ? Vector3.left : Vector3.right;
                    fighter.ThrustFighter(dir, _force,_reflectionPower);
                })
                .AddTo(this);
        }
        private void OnDisable()
        {
            if (!disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }


    }
}