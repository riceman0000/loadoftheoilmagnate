using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace OilMagnate.StageScene
{
    public class ResultView : MonoBehaviour
    {
        [SerializeField]
        ResultStar _firstStar;

        [SerializeField]
        ResultStar _secondStar;

        [SerializeField]
        ResultStar _thirdStar;

        [SerializeField]
        Text _scoreBoard;

        [SerializeField]
        Text _newRecord;

        [SerializeField]
        [Range(0f, 1f)]
        float _firstCondition;

        [SerializeField]
        [Range(0f, 1f)]
        float _secondCondition;

        [SerializeField]
        [Range(0f, 1f)]
        float _thirdCondition;

        [SerializeField]
        SceneTitle _nextScene;

        Player.Player _player;

        SceneFader _sceneFader;

        private void Awake()
        {
            _sceneFader = SceneFader.Instance;
            _player = StageManager.Instance.Player;
        }

        public void SetResult(MPParameter param)
        {
            SEManager.Instance.SEPlay(SETag.Result);

            // 数値が昇順になる様クランプを掛ける
            _secondCondition = _secondCondition < _firstCondition ? _firstCondition : _secondCondition;
            _thirdCondition = _thirdCondition < _secondCondition ? _secondCondition : _thirdCondition;

            float rate = (float)param.Current / param.Max;
            Debug.Log($"rate = {rate}");

            _firstStar.SetStar(rate >= _firstCondition);
            _secondStar.SetStar(rate >= _secondCondition);
            _thirdStar.SetStar(rate >= _thirdCondition);

            if (!_player.IsDebug)
            {
                if (StageManager.Instance.MPGauge.Param.Current > _player.MyData.Status.MPParam.Current)
                {
                    _newRecord.text = "しんきろく！";
                }
            }

            _scoreBoard.text = $"{param.Current}$";
        }

        public void GoToTitle()
        {
            _sceneFader.FadeOut(SceneTitle.Title);
        }

        public void Continue()
        {
            _sceneFader.FadeOut(_sceneFader.CurrentScene);
        }

        public void Next()
        {
            _sceneFader.FadeOut(_nextScene);
        }
    }
}
