using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OilMagnate
{

    public class DebuggerView : MonoSingleton<DebuggerView>
    {
        [SerializeField]
        Text _text1;

        [SerializeField]
        Text _text2;

        [SerializeField]
        Text _text3;

        [SerializeField]
        Text _text4;

        [SerializeField]
        Text _text5;

        public Text Text1 => _text1;

        public Text Text2 => _text2;

        public Text Text3 => _text3;

        public Text Text4 => _text4;

        public Text Text5 => _text5;
    }
}