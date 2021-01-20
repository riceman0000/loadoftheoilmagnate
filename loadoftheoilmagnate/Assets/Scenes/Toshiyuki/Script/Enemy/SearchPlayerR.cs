using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OilMagnate.StageScene;

public class SearchPlayerR : MonoBehaviour
{
    Human2 human2;
    private void Start()
    {
        human2 = this.transform.parent.parent.GetComponent<Human2>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == human2.get_Player)
            {
                human2.seachColEnterFlag_R = true;
            }           
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == human2.get_Player)
        {
            human2.seachColEnterFlag_R = false;
        }
    }
}
   
