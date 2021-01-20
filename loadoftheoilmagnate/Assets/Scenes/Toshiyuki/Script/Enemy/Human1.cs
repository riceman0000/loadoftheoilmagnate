using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    public class Human1 : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Collider2D leftEnd,rightEnd;
        //[SerializeField] Collision2D groundOn;

        [SerializeField] float speed;

        //右に移動しているかどうかのフラグ(trueで右に移動)
        public bool rightFlag_knife { get; set; }

        ////敵が動いているかどうかのフラグ(falseで動き続ける)
        public bool m_human1_stopFlag { get; set; }

        Rigidbody2D rig;
        SpriteRenderer spriteRenderer;
        IEnumerator m_moveEnemy;

        StageManager _stage;

        //public GimmickParam Param => throw new System.NotImplementedException();

        //public bool IsBreakable => throw new System.NotImplementedException();

        void Start()
        {
            rightFlag_knife = false;
            spriteRenderer = this.GetComponent<SpriteRenderer>();
            m_moveEnemy = MoveEnemy();
            m_human1_stopFlag = false;
            rig = this.GetComponent<Rigidbody2D>();
            StartCoroutine(m_moveEnemy);

            _stage = StageManager.Instance;

            //Debug.Log(_stage);

            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnInStageEnter);//これ書いて、メソッドの
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Exit, OnExitStageEnter);

            Gimmick gm;
            gm = this.GetComponent<Gimmick>();
            gm.OnBreak += Gm_OnBreak;
        }

        private void Gm_OnBreak()
        {
            this.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            StartCoroutine(destroyEvent());
        }

        IEnumerator destroyEvent()
        {
            m_human1_stopFlag = true;
            rig.velocity = Vector2.zero;
            yield return new WaitForSeconds(5f);
        }

        private void OnExitStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            StopCoroutine(m_moveEnemy);
        }

        private void OnInStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            if (state.HasFlag(StageStateMachine.StageState.InStage))
            {
                StartCoroutine(m_moveEnemy);
            }           // in stageの時の処理
        }

        private void OnDisable()
        {
            _stage.StateMachine.DisposeStateEvent(StageStateMachine.When.Enter, OnInStageEnter);
            _stage.StateMachine.DisposeStateEvent(StageStateMachine.When.Enter, OnExitStageEnter);
        }



        /// <summary>
        /// enemyの動き全体を制御
        /// </summary>
        IEnumerator MoveEnemy()
        {
            while (true)
            {
                while (!m_human1_stopFlag)
                {
                    while (rightFlag_knife)
                    {
                        if (m_human1_stopFlag) break;
                        RightMoving();
                        yield return null;
                    }
                    if (m_human1_stopFlag) break;
                    LeftMoving();
                    yield return null;
                }

                while (m_human1_stopFlag) { yield return null;}
            }           
        }

        /// <summary>
        /// 右に動く
        /// </summary>
        private void RightMoving()
        {
            //
            rig.velocity = Vector2.right.normalized * speed;

        }

        /// <summary>
        /// 左に動く
        /// </summary>
        private void LeftMoving()
        {
            //
            rig.velocity = Vector2.left.normalized * speed;
        }
        
        /// <summary>
        /// ぶつかったら右に動くのをやめる
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == leftEnd)
            {          
                rightFlag_knife = true;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (collision == rightEnd)
            {
                rightFlag_knife = false;
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            
            
        }

       
    }
}