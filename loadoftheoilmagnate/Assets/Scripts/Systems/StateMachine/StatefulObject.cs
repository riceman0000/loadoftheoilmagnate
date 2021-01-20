using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// StateMachineを使うクラスのベース
    /// </summary>
    /// <typeparam name="TDerived">継承先クラスのtype</typeparam>
    /// <typeparam name="TEnum"><see cref="Enum"/> of State tag.</typeparam>
    public abstract class StatefulObject<TDerived, TEnum> : ManagedMono
        where TDerived : StatefulObject<TDerived, TEnum> where TEnum : Enum,IConvertible
    {
        protected StateMachine<TDerived, TEnum> stateMachine = new StateMachine<TDerived, TEnum>();
        protected List<State<TDerived, TEnum>> stateList = new List<State<TDerived, TEnum>>();

        /// <summary>
        /// 現在のステート
        /// </summary>
        public State<TDerived, TEnum> CurrentState => stateMachine?.CurrentState;

        public virtual void ChangeState(TEnum nextState)
        {
            if (stateMachine == null)
            {
                throw new NullReferenceException($"stateMachine はnullです。");
            }

            var targetState = stateList.Find(state =>
            {
                //Debug.Log($"target = {gameObject.name} state = {state.Identity} = {state.Identity.ToInt32(null)}\n " +
                    //$"nextState = {nextState}=  {nextState.ToInt32(null)}");
                if (state.Identity.ToInt32(null) == nextState.ToInt32(null))
                {
                    return true;
                }
                return false;
            });
            stateMachine.ChangeState(targetState);
        }

        /// <summary>
        /// 現在のステートと一緒かどうか
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public virtual bool IsCurrentState(TEnum identity)
        {
            if (stateMachine == null)
            {
                throw new NullReferenceException($"stateMachine はnullです");
            }
            return stateMachine.CurrentState.Identity.Equals(identity);
        }

        public override void MUpdate()
        {
            stateMachine?.Update();
        }
    }
}