using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] int hp = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hp > 0 && collision.gameObject.tag == "player")
        {
            // hp -= collision.gameObject.GetComponent<AttackController>().power;
            hp -= 100;
        }
        else if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
