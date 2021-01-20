using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OilMagnate.Player;
using UnityEngine.UI;

namespace OilMagnate
{

    public class Stageload : MonoBehaviour
    {
        public SceneTitle scene_load;
        int stage = 0;
        SceneFader sceneFader;
        [SerializeField]
        GameObject frame;

        private void Awake()
        {
            sceneFader = SceneFader.Instance;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Frameset(Vector3 vec)
        {
            if (!frame.activeSelf)
            {
                gameObject.GetComponent<Button>().interactable = true;
                frame.SetActive(true);
            }
            frame.transform.position = vec;
        }
        public void Load()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            /*switch (stage)
            {
                case 1:
                    sceneFader.FadeOut(SceneTitle.StageOne, 1f);
                    break;
                case 2:
                    sceneFader.FadeOut(SceneTitle.StageTwo, 1f);
                    break;
                default:
                    break;
            }*/
            sceneFader.FadeOut(scene_load, 1f);
        }
    }
}