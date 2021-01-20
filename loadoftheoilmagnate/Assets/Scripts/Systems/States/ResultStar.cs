using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace OilMagnate.StageScene
{
    public class ResultStar : MonoBehaviour
    {
        [SerializeField]
        Image _image;

        [SerializeField]
        Sprite _disableStar;

        [SerializeField]
        Sprite _activeStar;

        public void SetStar(bool isActive)
        {
            _image.sprite = isActive ? _activeStar : _disableStar;
        }
    }
}
