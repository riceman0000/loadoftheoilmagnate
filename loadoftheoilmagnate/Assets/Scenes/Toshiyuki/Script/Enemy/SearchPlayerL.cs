using OilMagnate.StageScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPlayerL : MonoBehaviour
{
    Human2 human2;
    private void Start()
    {
        human2 = this.transform.parent.parent.GetComponent<Human2>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(human2.get_Player);

        if (collision == human2.get_Player)
            {
            Debug.Log(collision);
            
            human2.seachColEnterFlag_L = true;
            }

       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == human2.get_Player)
        {
            human2.seachColEnterFlag_L = false;
        }
    }
}
