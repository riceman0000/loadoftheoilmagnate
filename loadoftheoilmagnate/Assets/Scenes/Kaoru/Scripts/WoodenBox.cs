using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate.StageScene
{
    public class WoodenBox : MonoBehaviour,IDestructible
    {
        Animator animator;
        private void Start()
        {
            animator = this.GetComponent<Animator>();
        }

        private void BreakBox()
        {
            Debug.Log("Break");
            Destroy(this.gameObject);
            //とりあえずプレイヤーの攻撃に当たったら消える。
            //細かいアニメーションはまだ
        }

        /// <summary>
        /// 
        /// </summary>
        public void DestroyMyself()
        {
            animator.SetBool("Destroy", true);
        }

        public void Destruction()
        {
            throw new System.NotImplementedException();
        }
    }
}
