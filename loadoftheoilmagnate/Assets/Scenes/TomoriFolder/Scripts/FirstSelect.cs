using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OilMagnate.Player;
using UnityEngine.UI;

namespace OilMagnate
{

    public class FirstSelect : MonoBehaviour
    {
        
        SceneFader sceneFader;
        PlayerData _playerData;
        [SerializeField]
        SceneTitle clearFlag;

        private void Awake()
        {
            sceneFader = SceneFader.Instance;
            _playerData = PlayerData.Instance;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }


        public void Stagefirst()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            sceneFader.FadeOut(SceneTitle.StageOne,1f);
        }public void Stagetwo()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            sceneFader.FadeOut(SceneTitle.StageTwo, 1f);
        }
        public void StageThree()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            sceneFader.FadeOut(SceneTitle.StageThree, 1f);
        }
        public void StageFour()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            sceneFader.FadeOut(SceneTitle.StageFour, 1f);
        }
    }
}