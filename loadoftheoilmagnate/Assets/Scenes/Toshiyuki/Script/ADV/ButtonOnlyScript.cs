using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OilMagnate.Player;

namespace OilMagnate
{
    public class ButtonOnlyScript : MonoBehaviour
    {
        SceneFader sceneFader;
        [SerializeField]
        GameObject obj;//debug用

        PopUI m_popUI = new PopUI();

        PlayerData _playerData;

        private void Awake()
        {
            sceneFader = SceneFader.Instance;

            _playerData = PlayerData.Instance;
        }

        public void Titlebutton()
        {
            Debug.Log("TitleSeneChange");
            /////////////////debug用////////////////////
            //GameObject.Find("TitleCanvas").SetActive(false);
            ////////////////////////////////////////////
            //SceneManager.LoadScene("");
            //m_popUI.MenuPop(obj);
            SEManager.Instance.SEPlay(SETag.Select);

            if (Game.Instance.SaveData.Progress.HasFlag(SceneTitle.Title))
            {
            sceneFader.FadeOut(SceneTitle.StageSelect);
            }
            else
            {
                sceneFader.FadeOut(SceneTitle.StageOne);
            }
        }

        public void HighScorebutton()
        {
            Debug.Log("ScoreSeneChange");
            /////////////////debug用////////////////////
            DebugGetTroph();
            GameObject.Find("HightScoreCanvas").SetActive(false);
            ////////////////////////////////////////////
            //SceneManager.LoadScene("");


        }


        void DebugGetTroph()//条件だけとれるか確認用
        {
            //var Condition = new List<GameObject>();
            GameObject[] Condition = new GameObject[13];
            for (int i = 0; i < 26; i++)
            {
                if (i % 2 == 1)
                {
                    Condition[i / 2] = obj.transform.GetChild(i).gameObject;
                }

            }
            Debug.Log(Condition[10]);
            Debug.Log(Condition[12]);
            Debug.Log(Condition[1]);
        }

        public void GameOverbutton()
        {
            Debug.Log("GameOverSeneChange");
            /////////////////debug用////////////////////
            //GameObject.Find("GameOverCanvas").SetActive(false);
            ////////////////////////////////////////////
            sceneFader.FadeOut(SceneTitle.Title, 1f);
        }

        public void Continuebutton()
        {
            Debug.Log("ContinueSeneChange");
            /////////////////debug用////////////////////
            GameObject.Find("ContinueCanvas").SetActive(false);
            ////////////////////////////////////////////
            //SceneManager.LoadScene();
            sceneFader.FadeOut(sceneFader.CurrentScene, 1f);
        }

        public void ContinueToGameOver()
        {
            Debug.Log("ContiToGameOverSeneChange");
            /////////////////debug用////////////////////
            GameObject.Find("ContinueCanvas").SetActive(false);
            ////////////////////////////////////////////
            //SceneManager.LoadScene("");
            sceneFader.FadeOut(SceneTitle.GameOver, 1f);

        }

        public void Storybutton()
        {
            Debug.Log("ContiToGameOverSeneChange");
            /////////////////debug用////////////////////
            //GameObject.Find("ContinueCanvas").SetActive(false);
            ////////////////////////////////////////////
            //SceneManager.LoadScene("");
            sceneFader.FadeOut(SceneTitle.Opening, 1f);
        }

        public void StageSelect()
        {
            Debug.Log("ContiToGameOverSeneChange");
            /////////////////debug用////////////////////
            //GameObject.Find("ContinueCanvas").SetActive(false);
            ////////////////////////////////////////////
            //SceneManager.LoadScene("");
            sceneFader.FadeOut(SceneTitle.StageSelect, 1f);
        }

        public void Recordbutton()
        {
            Debug.Log("ContiToGameOverSeneChange");
            /////////////////debug用////////////////////
            //GameObject.Find("ContinueCanvas").SetActive(false);
            ////////////////////////////////////////////
            //SceneManager.LoadScene("");
            sceneFader.FadeOut(SceneTitle.HighScore, 1f);

        }
        public void StaffRoll()
        {
            Debug.Log("ContiToStaffRollScene");
            sceneFader.FadeOut(SceneTitle.StaffRoll,1f);
        }

    }
    class PopUI
    {
        GameObject MenuUI;
        public void MenuPop(GameObject MenuUI)
        {
            MenuUI.SetActive(true);
        }
    }

}