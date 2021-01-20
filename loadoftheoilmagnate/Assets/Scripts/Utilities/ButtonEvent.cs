using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{

public class ButtonEvent : MonoBehaviour
{
        [SerializeField]
        SceneTitle nextScene;
        public void SceneFade()
        {
            SceneFader.Instance.FadeOut(nextScene);
        }

}
}
