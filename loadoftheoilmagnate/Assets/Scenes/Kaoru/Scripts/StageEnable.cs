using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace OilMagnate
{
    public class StageEnable : MonoBehaviour
    {
        [SerializeField] SceneTitle clearFlag;
        [SerializeField] GameObject frame;
        [SerializeField] StageChange stageChange;
        [SerializeField] Sprite buckGroundSprite;
        [SerializeField] Image buckGround;
        Game game;

        private void Start()
        {
            game = Game.Instance;
            if (game.SaveData.Progress.HasFlag(clearFlag))
            {
                this.gameObject.SetActive(true);
            }
            else this.gameObject.SetActive(false);
        }

        public void Stagefirst()
        {
            CommonProcess();
            stageChange.TargetStage.Value = SceneTitle.StageOne;
        }

        public void Stagetwo()
        {
            CommonProcess();
            stageChange.TargetStage.Value = SceneTitle.StageTwo;
        }

        public void StageThree()
        {
            CommonProcess();
            stageChange.TargetStage.Value = SceneTitle.StageThree;
        }

        public void StageFour()
        {
            CommonProcess();
            stageChange.TargetStage.Value = SceneTitle.StageFour;
        }
        private void CommonProcess()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            buckGround.sprite = buckGroundSprite;
            stageChange.ToggleFrameVisible(this);   
            frame.transform.position = this.gameObject.transform.position;
            stageChange.IfOn();
        }

        public void FrameSetActive(bool isActive)
        {
            frame.SetActive(isActive);
        }


    }
}