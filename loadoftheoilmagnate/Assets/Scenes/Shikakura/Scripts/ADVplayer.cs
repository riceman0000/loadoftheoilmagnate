using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace OilMagnate.StageScene
{
    public class ADVplayer : ManagedMono
    {
        // セリフ開始時の左右指定
        enum LefRig
        {
            Left,
            Right
        }

        /// <summary>
        /// シナリオが終了した時に呼ばれるコールバック
        /// </summary>
        public event Action OnScenarioEnd = () => { };

        [SerializeField]
        LefRig lefrig;
        [SerializeField]
        Text L_name, R_name, L_text, R_text, LogText;
        [SerializeField]
        Image R_fukidashi, L_fukidashi, PlayerImage1, PlayerImage2, L_coin, R_coin;
        [SerializeField]
        Sprite left_top, left_under, right_top, right_under, Chara1, Chara2;
        [SerializeField]
        GameObject logpanel;

        IEnumerator routine;

        byte _talkTurn = 0;
        private bool textcheck = false;
        private bool scenechange = false;

        [SerializeField]
        TextAsset csvFile; // CSVファイル
        private List<string[]> VoiceData = new List<string[]>(); // CSVの中身を入れるリスト

        private int Element = 1;
        private int mojiCnt = 0;
        private int nameCnt = 0;

        void Start()
        {
            routine = Col(null, null);
            StartCoroutine("Coinflash");
            int height = 0;
            StringReader reader = new StringReader(csvFile.text);

            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();
                VoiceData.Add(line.Split(',')); // リストに入れる
                height++; // 行数加算
            }
            L_text.text = "";
            L_name.text = "";
            R_text.text = "";
            R_name.text = "";

            switch (lefrig)
            {
                case LefRig.Left:
                    routine = Col(L_name, L_text);
                    break;
                case LefRig.Right:
                    routine = Col(R_name, R_text);
                    _talkTurn++;
                    Fukidashi(L_name, L_text, R_name, R_text, L_fukidashi, R_fukidashi, left_top, left_under, right_top, right_under);
                    break;
                default:
                    break;
            }
            StartCoroutine(routine);
            PlayerImage1.sprite = Chara1;
            PlayerImage2.sprite = Chara2;
        }

        ///会話文表示メソッド
        IEnumerator Col(Text nameText, Text messageText)
        {
            nameText.text = VoiceData[Element][1];
            while (mojiCnt < VoiceData[Element][2].Length)
            {
                if (VoiceData[Element][2][mojiCnt] == '*')
                {
                    messageText.text += "\n";
                }
                else
                {
                    messageText.text += VoiceData[Element][2][mojiCnt];
                }
                    mojiCnt++;
                    yield return new WaitForSeconds(0.025f);
            }
            textcheck = true;
        }

        //会話切り替え
        void Fukidashi(Text L_name, Text L_message, Text R_name, Text R_message, Image L_Fukidashi, Image R_Fukidashi, Sprite L_Top, Sprite L_Under, Sprite R_Top, Sprite R_Under)
        {
            if (_talkTurn % 2 == 0)
            {
                L_coin.enabled = true;
                R_coin.enabled = false;
                L_name.color = new Color(255f, 255f, 255f, 255);
                L_message.color = new Color(255f, 255f, 255f, 255);
                L_Fukidashi.sprite = L_Top;
                L_Fukidashi.transform.SetAsLastSibling();
                R_name.color = new Color(255f, 255f, 255f, 0f);
                R_message.color = new Color(255f, 255f, 255f, 0f);
                R_Fukidashi.sprite = R_Under;

                routine = Col(L_name, L_message);
                StartCoroutine(routine);//左吹き出しの会話表示
                //_talkTurn += 1;
            }
            else
            {
                L_coin.enabled = false;
                R_coin.enabled = true;
                R_name.color = new Color(255f, 255f, 255f, 255f);
                R_message.color = new Color(255f, 255f, 255f, 255f);
                R_Fukidashi.sprite = R_Top;
                R_Fukidashi.transform.SetAsLastSibling();
                L_name.color = new Color(255f, 255f, 255f, 0);
                L_message.color = new Color(255f, 255f, 255f, 0);
                L_Fukidashi.sprite = L_Under;
                routine = Col(R_name, R_message);
                StartCoroutine(routine);//右吹き出しの会話表示

                //_talkTurn += 1;
            }
        }

        //Skipボタンクリック時
        public void OnSkipButton()
        {
            if (!logpanel.activeSelf)
            {
                SEManager.Instance.SEPlay(SETag.Select);

                Element = VoiceData.Count - 1;
                OnClick();
                StopCoroutine(routine);
                if (_talkTurn % 2 == 0)
                {
                    L_text.text = VoiceData[Element][2];
                }
                else
                {
                    R_text.text = VoiceData[Element][2];
                }
                scenechange = true;
            }
        }

        //Logボタンクリック時
        public void OnLogButton()
        {
            int mojiCntlog = 0;
            if (!logpanel.activeSelf)
            {
                SEManager.Instance.SEPlay(SETag.Select);

                logpanel.SetActive(true);
                LogText.text = "";
                for (int i = 1; i <= Element; i++)
                {
                    mojiCntlog = 0;
                    LogText.text += VoiceData[i][1] + ":  ";
                    while (mojiCntlog < VoiceData[i][2].Length)
                    {
                        if (VoiceData[i][2][mojiCntlog] == '*')
                        {
                            LogText.text += "\n";
                        }
                        else
                        {
                            LogText.text += VoiceData[i][2][mojiCntlog];
                        }

                        mojiCntlog++;
                    }
                    LogText.text += "\n" + "\n";
                    //LogText.text += VoiceData[i][1] + ":  " + VoiceData[i][2] + "\n" + "\n";
                }
            }
            else
            {
                SEManager.Instance.SEPlay(SETag.Select);
                logpanel.SetActive(false);
            }
        }

        //画面クリック時
        public void OnClick()
        {
            if (!logpanel.activeSelf)
            {
                SEManager.Instance.SEPlay(SETag.Select);
                if (textcheck)
                {
                    textcheck = false;
                    StopCoroutine(routine);
                    if (_talkTurn % 2 == 0)
                    {
                        L_text.text = VoiceData[Element][2];
                    }
                    else
                    {
                        R_text.text = VoiceData[Element][2];
                    }
                    if (Element + 1 < VoiceData.Count)
                    {

                        Element++;
                        L_text.text = "";
                        L_name.text = "";
                        R_text.text = "";
                        R_name.text = "";
                        mojiCnt = 0;
                        nameCnt = 0;

                        if (VoiceData[Element][1] != VoiceData[Element - 1][1])
                        {
                            _talkTurn += 1;
                            Fukidashi(L_name, L_text, R_name, R_text, L_fukidashi, R_fukidashi, left_top, left_under, right_top, right_under);
                        }
                        else
                        {
                            Fukidashi(L_name, L_text, R_name, R_text, L_fukidashi, R_fukidashi, left_top, left_under, right_top, right_under);
                        }
                    }
                    else
                    {
                        scenechange = true;
                        GameEnd();
                    }
                }
                else
                {
                    StopCoroutine(routine);
                    if (_talkTurn % 2 == 0)
                    {
                        while (mojiCnt < VoiceData[Element][2].Length)
                        {
                            if (VoiceData[Element][2][mojiCnt] == '*')
                            {
                                L_text.text += "\n";
                            }
                            else
                            {
                            L_text.text += VoiceData[Element][2][mojiCnt];
                            }
                            mojiCnt++;
                        }
                        //L_text.text = VoiceData[Element][2];
                    }
                    else
                    {
                        while (mojiCnt < VoiceData[Element][2].Length)
                        {
                            if (VoiceData[Element][2][mojiCnt] == '*')
                            {
                                R_text.text += "\n";
                            }
                            else
                            {
                                R_text.text += VoiceData[Element][2][mojiCnt];
                            }
                            mojiCnt++;
                        }
                        //R_text.text = VoiceData[Element][2];
                    }
                    textcheck = true;
                }
            }
        }
        //ADVplayerを終了させるときに呼ぶ
        public void GameEnd()
        {
            OnScenarioEnd?.Invoke();
            Destroy(gameObject);
        }
        /// <summary>
        /// コインの点滅をさせるコルーチン
        /// </summary>
        /// <returns></returns>
        IEnumerator Coinflash()
        {
            bool ss = false;
            while (true)
            {
                if (ss)
                {
                    L_coin.color = new Color(255f, 255f, 255f, 255f);
                    R_coin.color = new Color(255f, 255f, 255f, 255f);
                    ss = false;
                }
                else
                {
                    L_coin.color = new Color(255f, 255f, 255f, 0);
                    R_coin.color = new Color(255f, 255f, 255f, 0);
                    ss = true;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}