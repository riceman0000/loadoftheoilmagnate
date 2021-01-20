using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OilMagnate
{
    public class SceneBack : MonoBehaviour
    {
        SceneFader sceneFader;
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

        public void LoadScene()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            sceneFader.FadeOut(SceneTitle.Title,1f);
            //SceneManager.LoadScene("Menu");
        }
    }
}
