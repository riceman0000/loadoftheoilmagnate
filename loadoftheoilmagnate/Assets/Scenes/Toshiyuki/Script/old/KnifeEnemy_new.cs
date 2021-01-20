using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate.StageScene
{
    public class KnifeEnemy_new : MonoBehaviour
{
        [SerializeField] float turn_speed;

    /// <summary>
    /// trueなら右を向く、falseなら左を向く
    /// </summary>
    [SerializeField] bool rightFlag, m_human1_stopFlag;

    [SerializeField] Sprite To_R_Sprite, To_L_Sprite;

    Rigidbody2D rig;
    SpriteRenderer spriteRenderer;
    IEnumerator m_moveEnemy;

    StageManager _stage;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        m_moveEnemy = MoveEnemy();
        m_human1_stopFlag = false;
        rig = this.GetComponent<Rigidbody2D>();

        _stage = StageManager.Instance;

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
        if (state.HasFlag(StageStateMachine.StageState.InStage))
        {
            StopCoroutine(m_moveEnemy);
        }
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
        while (!m_human1_stopFlag)
        {
                if (rightFlag)
                {
                    RightMoving();
                    yield return new WaitForSeconds(turn_speed);
                    rightFlag = false;
                }
                else
                {
                    LeftMoving();
                    rightFlag = true;
                    yield return new WaitForSeconds(turn_speed);
                }       
        }
        StopCoroutine(m_moveEnemy);
    }

    /// <summary>
    /// 右を向く
    /// </summary>
    private void RightMoving()
    {
        spriteRenderer.sprite = To_R_Sprite;
    }

    /// <summary>
    /// 左を向く
    /// </summary>
    private void LeftMoving()
    {
        spriteRenderer.sprite = To_L_Sprite;
    }


}

      
    }
