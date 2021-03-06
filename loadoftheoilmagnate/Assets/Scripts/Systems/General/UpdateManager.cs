﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace OilMagnate
{
    /// <summary>
    /// <see cref="ManagedMono"/>の<see cref="ManagedMono.MUpdate"/>を統合して呼び出すクラス。.
    /// Class that unify and Call <see cref="ManagedMono.MUpdate"/> of <see cref="ManagedMono"/>.
    /// </summary>
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        List<IUpdatable> updatables = new List<IUpdatable>();
        private void Update()
        {
            for (int index = 0; index < updatables.Count; index++)
            {
                updatables[index]?.MUpdate();
            }
        }

        private void FixedUpdate()
        {
            for (int index = 0; index < updatables.Count; index++)
            {
                updatables[index].MFixedUpdate();
            }
        }

        public void SubscribeUpdate(IUpdatable updatable)
        {
            updatables.Add(updatable);
        }
        public void UnsubscribeUpdate(IUpdatable updatable)
        {
            updatables.Remove(updatable);
        }

        public override void OnInitialize() => DontDestroyOnLoad(gameObject);
    }
}