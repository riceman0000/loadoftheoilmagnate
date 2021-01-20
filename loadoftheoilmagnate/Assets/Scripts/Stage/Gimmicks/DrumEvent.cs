using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using OilMagnate.Player;
using System.Linq;

namespace OilMagnate.StageScene
{
    public class DrumEvent : MonoBehaviour
    {
        [SerializeField]
        Collider2D _col2d;

        Drum[] _drums = null;
        SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
        private void Start()
        {
            _drums = transform.GetComponentsInChildren<Drum>();

            Player.Player player = null;
            disposable.Disposable = _col2d.OnTriggerEnter2DAsObservable()
                .First(col => col.TryGetComponent(out player))
                .Subscribe(col =>
                {
                    foreach (var drum in _drums)
                    {
                        drum.Roll();
                    }
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