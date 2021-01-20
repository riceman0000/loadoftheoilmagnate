using UnityEngine;
using System.Collections;
using OilMagnate.StageScene;
using System;
using UniRx;

namespace OilMagnate.Player
{
    public class PlayerMPManager : ManagedMono
    {

        Player _player;

        StageManager _stage;


        private void Start()
        {
            Initialize();
        }


        public override void MUpdate()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                ConsumeMP(.05f);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                GrowthMP(0.05f);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                HealthMP(0.05f);
            }
        }

        public void Initialize()
        {
            _player = GetComponent<Player>();
            _stage = StageManager.Instance;
            _stage.MPGauge.Initialize(_player.MyData.Status.MPParam);
            _stage.MPGauge.gameObject.SetActive(false);


            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Execute, OnStageExecute);
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnStageEnter);
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Exit, OnStageExit);
        }

        private void OnStageExit(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            _stage.MPGauge.gameObject.SetActive(false);

        }

        private void OnStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;

            _stage.MPGauge.gameObject.SetActive(true);
        }

        private void OnStageExecute(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;

            //ConsumeMP()
        }

        /// <summary>
        /// パーセンテージで消費させる。
        /// </summary>
        /// <param name="quantity"></param>
        public void ConsumeMP(float quantity)
        {
            var q = (long)(quantity * _stage.MPGauge.Param.Max);
            ConsumeMP(q);
        }

        /// <summary>
        /// <paramref name="consumptioinQuantity"/>分MPを消費。
        /// </summary>
        /// <param name="consumptioinQuantity"></param>
        public void ConsumeMP(long consumptioinQuantity)
        {
            _stage.MPGauge.Param.Current -= consumptioinQuantity;
            _stage.MPGauge.UpdateGauge(MPGaugeModel.AnimPattern.Consume);


            if (_stage.MPGauge.Param.Current <= 0L
                && StageManager.Instance.StateMachine.CurrentState.HasFlag(StageStateMachine.StageState.InStage))
            {
                _stage.MPGauge.Param.Current = 0L;
                _stage.GameOver();
            }
        }

        /// <summary>
        /// <paramref name="amount"/>分MPを消費。
        /// </summary>
        /// /// <param name="amount"></param>
        public void TakeDamageToProperty(long amount)
        {
            ConsumeMP(amount);
            _player.PlayDamageUI(amount);
            //Instantiate(billExplosion, transform.position + _offset, Quaternion.identity);
        }

        /// <summary>
        /// <paramref name="amount"/>分MPを消費。
        /// </summary>
        /// /// <param name="amount"></param>
        public void TakeDamage(long amount)
        {
            ConsumeMP(amount);
            _player.PlayDamageUI(amount);
            SEManager.Instance.SEPlay(SETag.ReceiveDamage);
            //Instantiate(billExplosion, transform.position + _offset, Quaternion.identity);
        }



        /// <summary>
        /// Money powerを取得する。
        /// </summary>
        /// <param name="getMpRate"></param>
        public void GrowthMP(float getMpRate)
        {
            if (getMpRate < 0f) throw new ArgumentException("引数マイナスダメ絶対。");

            var val = (long)(getMpRate * _stage.MPGauge.Param.Max) + _stage.MPGauge.Param.Current;

            _stage.MPGauge.Param.Max += val;
            _stage.MPGauge.UpdateGauge(MPGaugeModel.AnimPattern.Growth);
        }

        /// <summary>
        /// Money powerを取得する。
        /// </summary>
        /// <param name="healthRate"></param>
        public void HealthMP(float healthRate)
        {
            if (_stage.StateMachine)
                if (healthRate < 0f) throw new ArgumentException("引数マイナスダメ絶対。");
            if (_stage.MPGauge.Param.Ratio >= 1f) return;

            var val = (long)(healthRate * _stage.MPGauge.Param.Max) + _stage.MPGauge.Param.Current;
            // Clamp
            if (val > _stage.MPGauge.Param.Max)
            {
                val = _stage.MPGauge.Param.Max;
            }
            _stage.MPGauge.Param.Current = val;

            _stage.MPGauge.UpdateGauge(MPGaugeModel.AnimPattern.Health);
        }

        ///// <summary>
        ///// 不労所得。
        ///// <see cref="Status.IncomeInterval"/>毎に<see cref="Status.IncomeRate"/>分MP回復する。
        ///// </summary>
        //void PassiveIncome()
        //{
        //    var disposable = new SingleAssignmentDisposable();
        //    disposable.Disposable = Observable
        //        .Interval(TimeSpan.FromSeconds(_player.PlayerData.MyStatus.IncomeInterval))
        //        .Where(_ => _stage.StateMachine.IsMatchState(StageStateMachine.StageState.InStage))
        //        .Subscribe(_ =>
        //        {
        //            HealthMP(_player.PlayerData.MyStatus.IncomeRate);
        //        }).AddTo(this);
        //}
    }
}