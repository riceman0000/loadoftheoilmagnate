using System;
using UnityEngine;
using System.Collections;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// State machine.
    /// 
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <typeparam name="TEnum"></typeparam>
    public class StateMachine<TOwner, TEnum> where TOwner : class where TEnum : Enum, IConvertible
    {
        /// <summary>
        /// ステートが変わってから呼ばれる最初のフレームのみEnterをキックする為のフラグ
        /// </summary>
        bool isCalledEnter = false;

        /// <summary>
        /// Current state.
        /// 現在のステート。
        /// </summary>
        public State<TOwner, TEnum> CurrentState { get; private set; }

        /// <summary>
        /// ステート変更。
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(State<TOwner, TEnum> state)
        {
            CurrentState?.Exit();
            CurrentState = state;
            isCalledEnter = false;
        }
        
        public void Update()
        {
            if (!isCalledEnter)
            {
                CurrentState?.Enter();
                isCalledEnter = true;
            }
            CurrentState?.Execute();
        }
    }
}
