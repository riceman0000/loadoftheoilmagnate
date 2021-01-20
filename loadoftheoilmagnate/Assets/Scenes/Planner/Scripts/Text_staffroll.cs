using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace OilMagnate
{
    public class Text_staffroll : ManagedMono
    {
        //
        [SerializeField]
        private float Text_ScrollSpeed = 5;
        [SerializeField]
        private float Limit_Position = 2000;
        [SerializeField]
        private float LoadScene_Position;
        [SerializeField]
        AudioSource audioSource;
        Vector3 textScroll;
        private bool IsStopStuffroll;
        private RectTransform Stuffroll;
        SceneFader sceneFader;
        // Start is called before the first frame update
        void Start()
        {
            Stuffroll = GetComponent<RectTransform>();
            sceneFader = SceneFader.Instance;
            textScroll = new Vector3(0, Text_ScrollSpeed, 0);
        }

        // Update is called once per frame
        public override void MUpdate()
        {
            if (!IsStopStuffroll)
            {
                if (transform.position.y <= Limit_Position)
                {
                    Stuffroll.localPosition += textScroll;

                    if (transform.position.y >= LoadScene_Position)
                    {
                        IsStopStuffroll = true;
                        Totitle();
                    }
                }
            }
        }
        public void PushButtonToTitle()
        {
            //var audioPlayer = AudioManager.Instance.AudioPlayers;
            SEManager.Instance.SEPlay(SETag.Select);
            Totitle();
        }
        void Totitle()
        {
            sceneFader.FadeOut(SceneTitle.Title, 1f);
            if (audioSource)
            {
                audioSource.Stop();
            }
            Debug.Log("TitleLoad");

        }
    }
}