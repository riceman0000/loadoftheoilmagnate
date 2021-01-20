using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    public class BillExplosion : MonoBehaviour
    {
        ParticleSystem _ps;
        SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();

        private void Start()
        {
            _ps = GetComponent<ParticleSystem>();
            _ps.Play();
            SEManager.Instance.SEPlay(SETag.BillBundle);


            disposable.Disposable =   this.UpdateAsObservable()
                .First(_ => !_ps.isPlaying)
                 .Subscribe(_ =>
                 {
                     Destroy(gameObject);
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