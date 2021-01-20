using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OilMagnate
{
    public class TextFlashing : ManagedMono
    {
        private Text text;
        private Color colll;
        private void Start()
        {
            // Textコンポーネントを取得
            text = GetComponent<Text>();
            //colll = text.color;
            //コルーチンスタート
            StartCoroutine("Flashing");
        }

        IEnumerator Flashing()
        {
            bool colorchange = true;
            while (true)
            {
                if (colorchange)
                {
                    //colll = new Color(0, 0, 0, 0);
                    // 色を指定
                    text.color = new Color(0, 0, 0, 0);
                    colorchange = false;
                }
                else
                {
                    //colll = new Color(0, 0, 0, 1);
                    // 色を指定
                    text.color = new Color(0, 0, 0, 1);
                    colorchange = true;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        public void onnnClick()
        {
            Debug.Log("TouchStart");
        }
    }
}
