using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace OilMagnate
{
    public class Moneytext : ManagedMono
    {
        [SerializeField] GameObject textObj;
        [SerializeField] GameObject Canvas;
        
        public override void MUpdate()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GameObject plef = Instantiate(textObj);
                plef.transform.SetParent(Canvas.transform, false);
            }
        }

    }
}