using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OilMagnate.Player;
using System;

namespace OilMagnate
{
    public class LastSelect : MonoBehaviour
    {
        SceneFader sceneFader;
        PlayerData _playerData;
        [SerializeField]
        SceneTitle _clearFlag;

        private void Awake()
        {
            sceneFader = SceneFader.Instance;
        }
        // Start is called before the first frame update
        void Start()
        {
            //if (_playerData.Progress.HasFlag(_clearFlag))
        }

        public void LoadScene()
        {
            //sceneFader.FadeOut(SceneTitle.StageThreeBoss, 1f);
            //SceneManager.LoadScene("StageThree_BOSS");
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
