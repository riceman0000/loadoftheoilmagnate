using UnityEngine;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using OilMagnate.Player;
using System;

namespace OilMagnate.StageScene
{
    [RequireComponent(typeof(LineRenderer))]
    public class OilGeyser : ManagedMono
    {
        public Vector2 Force
        {
            get
            {
                var dir = (_endPos - _startPos).normalized;
                dir.z = 10f;
                var force = (_powerBuffer - _startPos).magnitude;
                return dir * force * _mat.Force;
            }
        }
        public bool IsCalledAtteunation { get; private set; } = false;

        /// <summary>
        /// ドル袋
        /// </summary>
        [SerializeField]
        GameObject _dollarBag;

        /// <summary>
        /// 石油の頭
        /// </summary>
        [SerializeField]
        GameObject _oilGeyserTop;

        /// <summary>
        /// oil geyser top animator.
        /// </summary>
        [SerializeField]
        Animator _animator;

        OilGenerator _generator;

        LineRenderer _lineR;

        PolygonCollider2D _pc2d;

        OilGeyserMaterial _mat;

        SpriteRenderer _ogTopRenderer;

        SpriteRenderer _dollarBagRenderer;

        Vector3 _startPos;

        Vector3 _endPos;

        Vector3 _powerBuffer;

        StageManager _stage;

        static PlayerMPManager _mPManager;

        protected override void Awake()
        {
            base.Awake();
            _stage = StageManager.Instance;
            _lineR = GetComponent<LineRenderer>();
            _pc2d = GetComponent<PolygonCollider2D>();
            _ogTopRenderer = _oilGeyserTop.GetComponent<SpriteRenderer>();
            _dollarBagRenderer = _dollarBag.GetComponent<SpriteRenderer>();

            _lineR.enabled = true;
            _pc2d.enabled = true;
            _pc2d.isTrigger = true;

            if (!_mPManager)
            {
                _mPManager = _stage.Player.MPManager;
            }
        }

        private void Start()
        {
            _mPManager.ConsumeMP(_mat.ConsumptionQuantity);
            SEManager.Instance.SEPlay(SETag.Oil);
            _animator.Play("Play", 0);
        }

        public override void MUpdate()
        {
            if (_lineR.enabled)
            {
                var mesh = new Mesh();
                _lineR.BakeMesh(mesh, true);
                var vertices = CustomUtilities.ConvertV3ArrayToV2Array(mesh.vertices);
                CustomUtilities.Swap(ref vertices, 0, 1);
                _pc2d.SetPath(0, vertices);

                UVScroll();

                var dir = _endPos - _startPos;
                var endPos = _lineR.GetPosition(1);

                _oilGeyserTop.transform.position = endPos;
                _dollarBag.transform.position = _startPos;
                _oilGeyserTop.transform.up = dir;
                _dollarBag.transform.up = dir;
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            IContactable contactable;
            if (collision.transform.TryGetComponent(out contactable))
            {
                if (!contactable.IsBreakable) return;
                contactable.BlowOff(Force, BlowMode.FromVelocity, ForceMode2D.Impulse);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IContactable contactable;
            if (collision.transform.TryGetComponent(out contactable))
            {
                if (!contactable.IsBreakable) return;
                contactable.BlowOff(Force, BlowMode.FromVelocity, ForceMode2D.Force);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IContactable contactable;
            if (collision.transform.TryGetComponent(out contactable))
            {
                if (!contactable.IsBreakable) return;

                contactable.BlowOff(Force, BlowMode.FromVelocity, ForceMode2D.Force);
            }
        }

        private void OnDisable() => StopAllCoroutines();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _generator?.UnsubscribeOilGeyser(this);
        }

        /// <summary>
        /// Initialize and play  Oil geyser.
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="mat"></param>
        public void InitAndPlay
            (OilGenerator generator, OilGeyserMaterial mat, Vector3 startPos, Vector3 endPos, Vector3 powerBuffer)
        {
            _generator = generator;
            _mat = mat;
            _startPos = startPos;
            _endPos = endPos;
            _powerBuffer = powerBuffer;
            _generator.SubscribeOilGeyser(this);

            StartCoroutine(EmitOilGeyser());
        }


        public void ToBackward()
        {
            _ogTopRenderer.sortingOrder--;
            _dollarBagRenderer.sortingOrder--;
        }
        private void UVScroll()
        {
            var distance = (_endPos - _startPos).magnitude;
            var tiling = new Vector4(Mathf.Abs(distance), 1f, 0f, 0f);

            _lineR.material.SetVector("_Tiling", tiling);
        }

        /// <summary>
        /// 非同期に石油を射出
        /// </summary>
        /// <returns></returns>
        public IEnumerator EmitOilGeyser()
        {
            var startTime = 0f;
            var elapsedTime = 0f;
            var ratio = 0f;
            var positions = new Vector3[] { _startPos, _endPos };

            _lineR.enabled = true;
            startTime = Time.timeSinceLevelLoad;
            while (ratio < 1f)
            {
                elapsedTime = Time.timeSinceLevelLoad - startTime;
                ratio = elapsedTime / _mat.AnimationTime;
                var nextEndPos = Vector3.Lerp(_startPos, _endPos, ratio);
                positions[1] = nextEndPos;
                if (gameObject) _lineR.SetPositions(positions);
                yield return null;
            }
            if (gameObject) _lineR.SetPosition(1, _endPos);

            yield return new WaitForSeconds(_mat.OilLifeTime);
            // 減衰処理開始
            StartCoroutine(AttenuateOilGeyser());
        }

        /// <summary>
        /// 非同期に減衰させる
        /// </summary>
        /// <returns></returns>
        public IEnumerator AttenuateOilGeyser()
        {
            /// 既に<see cref="AttenuateOilGeyser"/>が呼ばれていたらイテレーターを抜ける
            if (IsCalledAtteunation) yield break;
            IsCalledAtteunation = true;

            var startTime = 0f;
            var elapsedTime = 0f;
            var ratio = 0f;
            var positions = new Vector3[] { _startPos, _endPos };
            startTime = Time.timeSinceLevelLoad;
            while (ratio < 1f)
            {
                elapsedTime = Time.timeSinceLevelLoad - startTime;
                ratio = elapsedTime / _mat.AnimationTime;
                var nextEndPos = Vector3.Lerp(_endPos, _startPos, ratio);
                positions[1] = nextEndPos;
                if (_lineR) _lineR.SetPositions(positions);
                yield return null;
            }
            Destroy(gameObject);
        }

    }
}