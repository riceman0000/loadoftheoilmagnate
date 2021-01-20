using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate {
    public class Stuffroll_SceneGo : MonoBehaviour
    {
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Stuffroll_Go()
        {
            //SceneManager.LoadScene("StaffRoll");
            SEManager.Instance.SEPlay(SETag.Select);
            SceneFader sceneFader;
            sceneFader = SceneFader.Instance;
            sceneFader.FadeOut(SceneTitle.StaffRoll, 1f);
            Debug.Log("StaffRoll");
        }
    }
}
