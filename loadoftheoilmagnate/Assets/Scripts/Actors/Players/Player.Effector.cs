using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using OilMagnate.StageScene;

namespace OilMagnate.Player
{
    /// <summary>
    ///  Effecter.
    /// </summary>
    public partial class Player
    {
        [SerializeField]
        float shockWaveThreshold;
        [SerializeField]
        GameObject _touchTarget;
        public Vector3 Velocity { get; set; }

        public float Force => Velocity.magnitude;


        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        float _footSEInterval = .5f;
        float _footTime = .5f;
        void Move()
        {
            if (CurrentState.Value.HasFlag(PhysicalState.Knockback)) return;
            // Look at forward.
            //transform.right = new Vector2(-_inputProvider.Horizontal, 0f);
            if (CurrentState.Value.HasFlag(PhysicalState.Grounded))
            {
                _rb2d.velocity = _inputProvider.Direction * _playerData.Status.MoveSpeed;
                if (_footTime > _footSEInterval)
                {
                    SEManager.Instance.SEPlay(SETag.FootstepsAtShip);
                    _footTime = 0f;
                }
            }
            else
            {
                _rb2d.AddForce(_inputProvider.Direction * _playerData.Status.MoveSpeed, ForceMode2D.Force);
            }

            _footTime += Time.deltaTime;
        }
    }
}
