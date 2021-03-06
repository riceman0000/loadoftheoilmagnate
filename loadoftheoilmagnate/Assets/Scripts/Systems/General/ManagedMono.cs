﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OilMagnate
{
    /// <summary>
    /// マネージドなUpdate関数を扱う全てのMonoBehaviourの親クラス
    /// All MonoBehaviour parent classes that handle managed Update methods.
    /// </summary>
    public abstract class ManagedMono : MonoBehaviour, IUpdatable
    {
        UpdateManager updateManager;
        protected virtual void Awake()
        {
            updateManager = UpdateManager.Instance;
            updateManager.SubscribeUpdate(this);
            //Debug.Log($"SubscribeUpdate {name}");
        }
        protected virtual void OnDestroy()
        {
            if (updateManager)
            {
                updateManager.UnsubscribeUpdate(this);
                //Debug.Log($"UnsubscribeUpdate {name}");
            }
        }
        /// <summary>
        /// 呼び出しを一元化して処理を早くしてあるUpdate関数
        /// </summary>
        public virtual void MUpdate() { }
        /// <summary>
        /// 呼び出しを一元化して処理を早くしてあるFixedUpdate関数
        /// </summary>
        public virtual void MFixedUpdate() { }
    }
}