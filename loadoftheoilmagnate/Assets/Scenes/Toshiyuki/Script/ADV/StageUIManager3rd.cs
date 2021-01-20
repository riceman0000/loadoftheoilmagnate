using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIManager3rd : MonoBehaviour
{
    //MPゲージ
    [SerializeField] Image MPGauge1st;
    [SerializeField] Image RedMPGauge1st;
    [SerializeField] Image MPGauge2nd;
    [SerializeField] Image RedMPGauge2nd;
    [SerializeField] Text MPText;

    //パラメーター
    [SerializeField] float startMP_UniteMPgauge;//ゲーム開始MP,ゲージ一本分のMP
    //float MPValue;
    //float maxMP;//最初にデータから引っ張ってくる
    float maxMPLimit;

    int stageNumber;

    float maxTime;

    //時間
    [SerializeField] Text TimeText;

    //コルーチンを復帰させるための入れ物
    IEnumerator TimeMethod;
    IEnumerator DamageChangeCoroutin;

    //ステージ
    [SerializeField] Text StageText;

    //フラグ
    bool DamageFlag = false;
    float Damage_Index;


    float mMP = 6500;//最大体力
    float mp = 6500;//体力

    private void Start()
    {
        /////debug用////////
        float mMP = 6500;//最大体力
        float mp = 6500;//体力
        stageNumber = 2;
        maxTime = 605;
        /////////////////


        StageTextScript(mMP,stageNumber, StageText);//ここでmaxMPに値が入る処理
        GaugeSetup(mp,mMP);
        TimeMethod = timeScript();//時間経過開始

        StartCoroutine(TimeMethod);
        StartCoroutine(DebugCorutin(mp,mMP));
    }



    
    //コルーチンを再開
    void StartNeedCoroutin()
    {
        StartCoroutine(DamageChangeCoroutin);
        StartCoroutine(TimeMethod);
    }


    /// ステージ用表示クラス(stage番号　表示させたいテキストオブジェクト)
    /// </summary>
    /// <param name="max_param"></param>
    /// <param name="param"></param>
    /// <param name="text"></param>
    void StageTextScript(float maxMP, int stage, Text text)
    {
        switch (stage)
        {
            case 1:
                text.text += (stage.ToString() + "st");
                maxMPLimit = startMP_UniteMPgauge * 1f;
                maxMP = startMP_UniteMPgauge;
                break;
            case 2:
                text.text += (stage.ToString() + "nd");
                maxMPLimit = startMP_UniteMPgauge * 1.5f;
                //現状のmaxMPのデータを引っ張ってくる
                break;
            case 3:
                text.text += (stage.ToString() + "rd");
                maxMPLimit = startMP_UniteMPgauge * 2f;
                //現状のmaxMPのデータを引っ張ってくる
                break;
            default:
                text.text += (stage.ToString() + "th");
                maxMPLimit = startMP_UniteMPgauge * 2f;
                //現状のmaxMPのデータを引っ張ってくる
                break;
        }

    }

    /// <summary>
    /// ゲージ初期値セッティング
    /// </summary>
    void GaugeSetup(float MPValue,float maxMP)
    {
        MPValue = maxMP;//体力最大まで回復
        MPGauge1st.fillAmount = 1;//一本目は最初最大値確定
        RedMPGauge1st.fillAmount = MPGauge1st.fillAmount;
        MPText.text = (MPValue.ToString("F0") + "/" + maxMP.ToString("F0"));
        if (maxMP > startMP_UniteMPgauge)
        {
            float x = maxMP - startMP_UniteMPgauge;
            MPGauge2nd.fillAmount = x / startMP_UniteMPgauge;
            RedMPGauge2nd.fillAmount = MPGauge2nd.fillAmount;
            return;
        }
        MPGauge2nd.fillAmount = 0;
        RedMPGauge2nd.fillAmount = MPGauge2nd.fillAmount;
    }

    //時間経過で回復、時間表示
    IEnumerator timeScript()
    {
        float elpasedTime = 0f;
        float timeValue = maxTime;
        while (timeValue>0)
        {
            if (0.1f < elpasedTime)
            {
                elpasedTime -= 0.1f;
                timeValue -= 0.1F;
                TimeText.text = ((int)(timeValue / 60)).ToString("F0") + ":" + ((int)(timeValue % 60)).ToString("F0");
                
                if (mp < mMP)
                {
                    mp += mMP * 0.0005f;
                    // GetDamageScript(-maxMP * 0.0005f);
                    TimeGaugeChecker(mp, mMP);
                }
            }
            yield return null;
            elpasedTime += Time.deltaTime;
        }                              
    }

    void TimeGaugeChecker(float MPValue,float maxMP)
    {
        if (DamageFlag != false)
        {
            if (MPValue >= maxMP)
            {
                MPValue = maxMP;
                StartCoroutine(ChangeGauge(MPValue,MPGauge2nd, RedMPGauge2nd, (MPValue - startMP_UniteMPgauge) / startMP_UniteMPgauge));
                MPText.text = (MPValue.ToString("F0") + "/" + maxMP.ToString("F0"));
                return;
            }

            MPText.text = (MPValue.ToString("F0") + "/" + maxMP.ToString("F0"));
            if (MPValue > startMP_UniteMPgauge)//ゲージが二本目に行ってるかどうか
            {

                StartCoroutine(ChangeGauge(MPValue,MPGauge1st, RedMPGauge1st, 1));
                StartCoroutine(ChangeGauge(MPValue,MPGauge2nd, RedMPGauge2nd, (MPValue - startMP_UniteMPgauge) / startMP_UniteMPgauge));
                //SecoundGauge();
            }
            else
            {
                StartCoroutine(ChangeGauge(MPValue, MPGauge2nd, RedMPGauge2nd, 0));
                StartCoroutine(ChangeGauge(MPValue,MPGauge1st, RedMPGauge1st, MPValue / startMP_UniteMPgauge));
                //FirstGauge();
            }
        }
        else
        {
            if (MPValue >= maxMP)
            {
                MPValue = maxMP;
                MPText.text = (MPValue.ToString("F0") + "/" + maxMP.ToString("F0"));
                return;
            }

            MPText.text = (MPValue.ToString("F0") + "/" + maxMP.ToString("F0"));
            if (MPValue > startMP_UniteMPgauge)//ゲージが二本目に行ってるかどうか
            {
                StartCoroutine(ChangeGauge(MPGauge1st, 1));
                StartCoroutine(ChangeGauge(MPGauge2nd, (MPValue - startMP_UniteMPgauge)/ startMP_UniteMPgauge));
                //MPGauge2nd.fillAmount = (MPValue - startMP_UniteMPgauge) / startMP_UniteMPgauge;          
            }
            else
            {
                StartCoroutine(ChangeGauge(MPGauge2nd, 0));
                // MPGauge2nd.fillAmount = 0;
                StartCoroutine(ChangeGauge(MPGauge1st, MPValue / startMP_UniteMPgauge));
                // MPGauge1st.fillAmount = MPValue / startMP_UniteMPgauge;
                //FirstGauge();
            }
        }
    }   
   

    //ダメージを受けた時。回復するときは「-」を引数にする
    IEnumerator GetDamageScript_Coroutine(float MPValue,float maxMP,float damage)
    {
        DamageFlag = true;
        MPValue -= damage;
        MPText.text = (MPValue.ToString("F0") + "/" + maxMP.ToString("F0"));
        IEnumerator enumerator;
        float x;
        if (MPValue>startMP_UniteMPgauge)//処理後のMPの数値をゲージにしたとき２本目に行くかどうか
        {
            x = (MPValue - startMP_UniteMPgauge) / startMP_UniteMPgauge;
            enumerator = ChangeGauge(MPValue,MPGauge2nd, RedMPGauge2nd, x);
            StartCoroutine(enumerator);
            yield return new WaitForSeconds(0.5f);
            yield return enumerator;
            DamageFlag = false;
        }
        else
        {
            enumerator = ChangeGauge(MPGauge2nd, 0);
            StartCoroutine(enumerator);
            x = MPValue / startMP_UniteMPgauge;

            yield return enumerator;//普通のゲージの一本目が減るまでの時間

            enumerator = ChangeGauge(MPGauge1st, x);
            StartCoroutine(enumerator);
            yield return enumerator;

            enumerator = ChangeGauge(RedMPGauge2nd, 0);
            StartCoroutine(enumerator);
            yield return enumerator;
            enumerator = ChangeGauge(RedMPGauge1st, x);
            StartCoroutine(enumerator);
            yield return enumerator;
            DamageFlag = false;
        }
    }

    //コルーチンを呼び出すスクリプト。普通のメソッドと同じように使える。
    void GetDamageScript(float MPvalue,float maxMP,float damage)
    {
        DamageChangeCoroutin = GetDamageScript_Coroutine(MPvalue,maxMP,damage);
        StartCoroutine(DamageChangeCoroutin);
    }

    /// <summary>
    /// ゲージを持っていきたい場所まで時間をかけて持っていくスクリプト
    /// toValueは持っていきたいgauge.value(割合の数字)
    /// </summary>
    /// <param name="gauge"></param>
    /// <param name="redGauge"></param>
    /// <param name="toValue"></param>
    /// <returns></returns>
    IEnumerator ChangeGauge(float MPvalue,Image gauge ,Image redGauge,  float toValue)
    {        
        Damage_Index = toValue;

        float x =Mathf.Abs(gauge.fillAmount - toValue);
        if (gauge.fillAmount > toValue)
        {
            while (gauge.fillAmount > toValue)
            {
                gauge.fillAmount -= x / 25;
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.5f);
            while (redGauge.fillAmount > toValue)
            {
                redGauge.fillAmount -= x / 10;
                yield return new WaitForSeconds(0.02f);
            }
            redGauge.fillAmount = gauge.fillAmount;
            yield break;
        }

        if (gauge.fillAmount < toValue)
        {
            while (gauge.fillAmount < toValue)
            {
                gauge.fillAmount += x / 25;
                redGauge.fillAmount += x / 25;
                yield return new WaitForSeconds(0.02f);
            }
            redGauge.fillAmount = gauge.fillAmount;
            yield break;
        }
        redGauge.fillAmount = gauge.fillAmount;
    }
    //一本用
    IEnumerator ChangeGauge(Image gauge,float toValue)
    {
        Damage_Index = toValue;

        float x = Mathf.Abs(gauge.fillAmount - toValue);
        if (gauge.fillAmount > toValue)
        {
            while (gauge.fillAmount > toValue)
            {
                gauge.fillAmount -= x / 25;
                yield return new WaitForSeconds(0.02f);
            }    
            // redGauge.value = gauge.value;
            yield break;
        }

        if (gauge.fillAmount < toValue)
        {
            while (gauge.fillAmount < toValue)
            {
                gauge.fillAmount += x / 25;           
                yield return new WaitForSeconds(0.02f);
            }
            // redGauge.fillAmount = gauge.fillAmount;
            yield break;
        }     
    }

    void MPmaxUP(float MPValue,float maxMP,float getMP)
    {
        maxMP += getMP;
        if (maxMP>maxMPLimit)
        {
            maxMP = maxMPLimit;
        }
        MPText.text = (MPValue.ToString("F0") + "/" + maxMP.ToString("F0"));
    }
   
    IEnumerator DebugCorutin(float MP , float maxMP)
    {
        yield return new WaitForSeconds(2f);
        MPmaxUP(MP,maxMP,2000);
        mMP += 2000;
        yield return new WaitForSeconds(2f);
        GetDamageScript(MP,maxMP,3000);
        mp -= 3000;
        yield return new WaitForSeconds(0.3f);
        //StopAllCoroutines();//コルーチンを止める。
    }
}
