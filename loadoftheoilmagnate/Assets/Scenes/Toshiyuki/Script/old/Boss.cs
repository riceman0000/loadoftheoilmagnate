using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{

    public class Boss : MonoBehaviour
    { // Start is called before the first frame update
      //一番左の折り返し地点、右の折り返し地点、左側にある攻撃開始地点(右に移動中に攻撃する地点)、右側にある攻撃開始地点(左移動中に攻撃する地点)
        [SerializeField] Collider2D leftEnd, rightEnd, AtackPoint_L, AtackPoint_R;
        [SerializeField] float speed, atackSpance;

        //攻撃するときに生成するミサイルのオブジェクト
        [SerializeField] GameObject RMissile, LMissile;

        /// <summary>
        /// trueなら右に移動、falseなら左に移動してる
        /// </summary>
        [SerializeField] bool rightFlag;
        //atackFlagがtrueなら左にのみ攻撃可能、falseなら右にのみ攻撃可能
        bool upFlag, atackFlag = false;

        [SerializeField] int missileCount;

        [SerializeField] Sprite To_R_Boss, To_L_Boss;

        //enemyについてるrigidbody
        Rigidbody2D rig;
        SpriteRenderer spriteRenderer;

        // trueなら自分では動かない(最後のvectorそのまま受ける
        [SerializeField] bool m_boss_stopFlag;

        //ストップ用
        IEnumerator m_moveEnemy;

        StageManager _stage;


        void Start()
        {

            spriteRenderer = this.GetComponent<SpriteRenderer>();
            m_moveEnemy = MoveEnemy();
            m_boss_stopFlag = false;
            rig = this.GetComponent<Rigidbody2D>();
            _stage = StageManager.Instance;
            //  StartCoroutine(m_moveEnemy);

            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnInStageEnter);//これ書いて、メソッドの
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Exit, OnExitStageEnter);
        }

        private void OnExitStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            StopCoroutine(m_moveEnemy);
        }

        private void OnInStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
                StartCoroutine(m_moveEnemy);
            // in stageの時の処理
        }

        /// <summary>
        /// enemyの動き全体を制御
        /// </summary>
        IEnumerator MoveEnemy()
        {
            while (!m_boss_stopFlag)
            {

                while (rightFlag)
                {
                    RightMoving();
                    yield return null;
                }
                LeftMoving();
                yield return null;
            }
            StopCoroutine(m_moveEnemy);
        }

        /// <summary>
        ///時間止めるだけのスクリプト 
        /// </summary>
        /// <returns></returns>
        IEnumerator JustStop(float x)
        {
            yield return new WaitForSeconds(x);
        }

        /// <summary>
        /// 右に動く
        /// </summary>
        private void RightMoving()
        {
            rig.velocity = Vector2.right.normalized * speed;
            spriteRenderer.sprite = To_R_Boss;
        }

        /// <summary>
        /// 左に動く
        /// </summary>
        private void LeftMoving()
        {
            rig.velocity = Vector2.left.normalized * speed;
            spriteRenderer.sprite = To_L_Boss;
        }

        /// <summary>
        /// ぶつかったら右に動くのをやめる
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == leftEnd)
            {
                if (!upFlag)
                {
                    this.gameObject.transform.position += new Vector3(0, 2f, 0);
                    upFlag = true;
                }
                rightFlag = true;
            }
            if (collision == rightEnd)
            {
                if (upFlag)
                {
                    this.gameObject.transform.position -= new Vector3(0, 2f, 0);
                    upFlag = false;
                }

                rightFlag = false;
            }
            if (collision == AtackPoint_L)
            {
                if (rightFlag)
                {
                    if (!atackFlag)
                    {
                        StartCoroutine(AtackToRight());
                        Debug.Log("Right");
                        atackFlag = true;
                    }
                }

            }
            if (collision == AtackPoint_R)
            {
                if (!rightFlag)
                {
                    if (atackFlag)
                    {
                        StartCoroutine(AtackToLeft());
                        Debug.Log("Left");
                        atackFlag = false;
                    }
                }
            }

        }

        /// <summary>
        /// 右への攻撃
        /// </summary>
        IEnumerator AtackToRight()
        {
            for (int i = 0; i < missileCount; i++)
            {
                Instantiate(RMissile);
                yield return new WaitForSeconds(atackSpance);
            }

        }

        /// <summary>
        /// 左への攻撃アニメーション必要ならここで左用のアニメを動かして
        /// </summary>
        IEnumerator AtackToLeft()
        {
            for (int i = 0; i < missileCount; i++)
            {
                Instantiate(LMissile);
                yield return new WaitForSeconds(atackSpance);
            }
        }
    }

}