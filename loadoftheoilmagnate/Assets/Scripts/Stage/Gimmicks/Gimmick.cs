using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public partial class Gimmick : ManagedMono, IGimmick
    {
        /// <summary>
        /// Backing field of <see cref="ID"/>
        /// </summary>
        [SerializeField]
        GimmickIdentifier _id;

        /// <summary>
        /// Master data of Gimmick.
        /// </summary>
        [SerializeField]
        GimmickParam _paramMaster;

        /// <summary>
        /// use collider.
        /// </summary>
        [SerializeField]
        Collider2D _col2d;

        /// <summary>
        /// 札束が舞うエフェクトのプレハブ。
        /// </summary>
        [SerializeField]
        GameObject _billExplosionPrefab;

        [SerializeField]
        EffectParam _effectParam;


        public event Action OnBreak = () => { };

        /// <summary>
        /// Backing field of <see cref="Param"/>
        /// </summary>
        GimmickParam _param;

        Rigidbody2D _rb2d;

        SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();

        /// <summary>
        /// 開始初期位置。
        /// </summary>
        Vector3 _initialPosition;

        StageManager _stageManager;

        /// <summary>
        /// 自身を破壊する時のコールバック
        /// </summary>
        public event Action OnDestroyEvent = () => { };


        /// <summary>
        /// Gimmickのパラメータ
        /// </summary>
        public GimmickParam Param { get => _param; }



        /// <summary>
        /// 自身の耐久値が0かどうか
        /// </summary>
        public bool IsBreakable => _param.Durability <= 0;

        /// <summary>
        /// 自分自身の初期位置からの距離がデッドラインを超えているか
        /// </summary>
        bool isOverDeadBorderLine => (transform.position - _initialPosition).magnitude > _param.DeadBorderLine;

        /// <summary>
        /// Gimmick ID.
        /// </summary>
        public GimmickIdentifier ID => _id;

        /// <summary>
        /// My transform.
        /// </summary>
        public Transform Transform => transform;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        float _massBuffer = 0f;
        float _dragBuffer = 0f;
        float _angularBuffer = 0f;
        private void Start()
        {
            /// <see cref="isOverDeadBorderLine"/>が<see cref="true"/>なら自身を破棄。
            disposable.Disposable = this.UpdateAsObservable()
                .First(_ => isOverDeadBorderLine)
                .Subscribe(_ => Destruction())
                .AddTo(this);

            if (Param.IsField)
            {
                _massBuffer = _rb2d.mass;
                _dragBuffer = _rb2d.drag;
                _angularBuffer = _rb2d.angularDrag;

                _rb2d.mass = 1000000;
                _rb2d.drag = 1000000;
                _rb2d.angularDrag = 1000000;
            }
        }

        private void OnDisable()
        {
            if (!disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        void Initialize()
        {
            _param = Instantiate(_paramMaster);
            _rb2d = GetComponent<Rigidbody2D>();
            _initialPosition = transform.position;
            if (!_col2d) _col2d = GetComponent<Collider2D>();

            _stageManager = StageManager.Instance;
            _stageManager.GimmickObserver.SubscribeGimmick(this);
        }

        /// <summary>
        /// 耐久値を削る。
        /// </summary>
        /// <param name="opponent">接触相手.</param>
        /// <param name="force"><paramref name="opponent"/>の速度.</param>
        public void ReduceDurability(Rigidbody2D opponent, Vector3 force)
        {
            _param.Durability -= force.magnitude;
            //DebuggerView.Instance.Text2.text = $"Durability of {gameObject.name} = {_param.Durability}";
            SEManager.Instance.SEPlay(SETag.Strike);

            if (IsBreakable)
            {
                _rb2d.simulated = true;
                BlowOff(opponent.velocity);
            }
        }
        public void ReduceDurability(Rigidbody2D opponent, float attack)
        {
            _param.Durability -= attack;
            //DebuggerView.Instance.Text2.text = $"Durability of {gameObject.name} = {_param.Durability}";
            SEManager.Instance.SEPlay(SETag.Strike);

            if (IsBreakable)
            {
                if (_param.IsField)
                {
                    _rb2d.mass = _massBuffer;
                    _rb2d.drag = _dragBuffer;
                    _rb2d.angularDrag = _angularBuffer;
                }
                BlowOff(opponent.velocity);
            }
        }

        /// <summary>
        /// 破壊時の処理。
        /// </summary>
        public void Destruction()
        {
            // will be implement some processing.
            _stageManager?.GimmickObserver?.UnsubscribeGimmick(this);
            Destroy(gameObject);
        }

        /// <summary>
        /// 一回目だけ呼び出すフラグ
        /// </summary>
        bool isFirst = true;

        /// <summary>
        /// <paramref name="force"/>の力積を与えて自身を吹き飛ばす.
        /// </summary>
        /// <param name="force">力積.</param>
        /// <param name="blowMode "><see cref="GimmickParam.BlowForce"/>を使用するかどうか.</param>
        /// /// <param name="forceMode">力の掛け方. 外力か,力積か.</param>
        public void BlowOff(
          Vector2 force, BlowMode blowMode = BlowMode.FromParam, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            switch (blowMode)
            {
                case BlowMode.FromParam:
                    force = force.normalized + _param.BlowForce;
                    break;
                case BlowMode.FromVelocity:
                    break;
                default:
                    break;
            }

            _rb2d.AddForce(force, forceMode);

            //コライダーをトリガーに設定してステージ外へ吹き飛ばす様にする。
            if (IsBreakable && isFirst)
            {
                Break();
            }
        }

        /// <summary>
        /// Break処理
        /// </summary>
        void Break()
        {
            isFirst = false;
            //_col2d.isTrigger = true;
            Destroy(_col2d);

            OnBreak?.Invoke();

            // ダメージ処理
            _stageManager.Player.MPManager.TakeDamageToProperty(_param.AssetAmount);
            //TODO: MP消費量分ダメージUI再生させる。

            // particleのradiusと生成座標を指定してエフェクト再生。
            var ps = _billExplosionPrefab.GetComponent<ParticleSystem>();
            var shape = ps.shape;
            shape.radius = _effectParam.Radius;
            Instantiate(_billExplosionPrefab, transform.position + _effectParam.Offset, Quaternion.identity);


        }



        public void Reflection(float coefficientOfRestitution)
        {
            throw new NotImplementedException();
        }


        [Serializable]
        public class EffectParam
        {
            /// <summary>
            /// <see cref="_billExplosionPrefab"/>を出す座標に対するオフセット
            /// </summary>
            [SerializeField]
            Vector3 _offset = Vector3.zero;

            /// <summary>
            /// <see cref="_billExplosionPrefab"/>のshapeの大きさ
            /// </summary>
            [SerializeField]
            float _radius = 10f;

            public Vector3 Offset { get => _offset; set => _offset = value; }
            public float Radius { get => _radius; set => _radius = value; }
        }
    }

    /// <summary>
    /// ギミックの識別子
    /// </summary>
    public enum GimmickIdentifier
    {
        None,
        Drum,
        WoodenBox,
        SideContainer,
        FrontContainer,
        Crane,
        Pipeline1,
        Pipeline2,
        Pier,
        Humvee,
        ZakoGun,
        ZakoKnife,
        Fighter,
        Aburauru,
    }
}