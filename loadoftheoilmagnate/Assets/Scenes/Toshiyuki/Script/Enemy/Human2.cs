using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace OilMagnate.StageScene
{
    public class Human2 : MonoBehaviour
    {
        public Collider2D get_Player { get; set; }
        [SerializeField] GameObject LeftBullet, RightBullet;
        // [SerializeField] Sprite To_L_Sprite, To_R_Sprite;

        //右の索敵範囲ににプレイヤーが入ったときのフラグ。(trueで右に攻撃し続けるフラグ)
        public bool seachColEnterFlag_R { get; set; }
        //左の索敵範囲ににプレイヤーが入ったときのフラグ。(trueで左に攻撃し続けるフラグ)
        public bool seachColEnterFlag_L { get; set; }
        //敵が動いているかどうかのフラグ(falseで動き続ける)
        public bool m_human2_stopFlag { get; set; }

        //oneceのほうで一回の攻撃のスパンスを、unitのほうでひとまとまりの攻撃のスパンスを指定
        [SerializeField] float once_AtackSpance, unit_AtackSpance;
        //一回の攻撃回数
        [SerializeField] int AtackCount;

        StageManager _stage;

        IEnumerator _coroutine;

        //クールタイム中かどうか判定
        bool coolTimeFlag = false;

        [field: SerializeField]
        public float bulletSpeed { get; set; }


        float _elapsedTime;
        float _startTime;
        void Start()
        {
            get_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            _coroutine = AtackEnemy();
            m_human2_stopFlag = false;

            seachColEnterFlag_R = false;
            seachColEnterFlag_L = false;

            _stage = StageManager.Instance;

            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnInStageEnter);//これ書いて、メソッドの
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Exit, OnStageExit);
            Debug.Log(this.gameObject.transform.GetChild(0).transform.localRotation);
            Debug.Log(this.gameObject.transform.transform.localRotation);
            this.gameObject.transform.GetChild(0).transform.localRotation = this.gameObject.transform.localRotation;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {

                })
                .AddTo(this);


            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.tag.Equals(ObjTag.Player.ToString()))
                .Subscribe(col =>
                {
                    StartCoroutine(AtackEnemy());
                })
                .AddTo(this);
        }
        private void OnStageExit(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            StopCoroutine(_coroutine);
        }

        private void OnInStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            StopCoroutine(_coroutine);
        }

        List<GameObject> _bullets = new List<GameObject>();
        IEnumerator AtackEnemy()
        {
            while (true)
            {
                while (!m_human2_stopFlag)
                {
                    while (seachColEnterFlag_R)
                    {
                        if (m_human2_stopFlag) break;
                        StartCoroutine(RightAtack());
                        yield return new WaitForSeconds(unit_AtackSpance);//ここの戻り値で攻撃のスパンスを決める
                    }
                    while (seachColEnterFlag_L)
                    {
                        if (m_human2_stopFlag) break;
                        StartCoroutine(LeftAtack());
                        yield return new WaitForSeconds(unit_AtackSpance);
                    }
                    yield return null;
                    foreach (var bullet in _bullets)
                    {
                        Destroy(bullet.gameObject);
                    }
                    _bullets.Clear();
                    StopCoroutine(_coroutine);
                }
                while (m_human2_stopFlag) { yield return null; }
            }

        }





        //ここで攻撃方法を決める。現状は弾を一つ生成するだけにしておく。必要なら、3連射にも可能(その場合
        IEnumerator RightAtack()
        {
            //何かｱﾆﾒｰｼｮﾝさせるなら、ここにそのフラグを書き込む
            this.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

            //
            for (int i = 0; i < AtackCount; i++)//ここで一回の攻撃の発射回数を指定
            {
                var go = Instantiate(RightBullet, this.transform);
                _bullets.Add(go);
                SEManager.Instance.SEPlay(SETag.Shoot);
                yield return new WaitForSeconds(once_AtackSpance);//ここで一回の攻撃の発射感覚を指定
            }
        }

        IEnumerator LeftAtack()
        {
            //何かｱﾆﾒｰｼｮﾝさせるなら、ここにそのフラグを書き込む

            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            this.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            //
            for (int i = 0; i < AtackCount; i++)//ここで一回の攻撃の発射回数を指定
            {
                var go = Instantiate(LeftBullet, this.transform);
                _bullets.Add(go);
                SEManager.Instance.SEPlay(SETag.Shoot);
                yield return new WaitForSeconds(once_AtackSpance);//ここで一回の攻撃の発射感覚を指定
            }
        }
    }
}