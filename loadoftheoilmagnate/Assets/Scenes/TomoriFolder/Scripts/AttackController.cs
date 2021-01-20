using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    //　プレイヤーの攻撃力
    [SerializeField] int power = 100;
    // プレイヤーの攻撃間隔
    [SerializeField] float interval = 0.1f;

    Animator anim;
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= interval)
        {
            Attack();
        }
    }

    void Attack()
    {
        timer = 0f;

        anim.Play("Attack");
    }

   

}
