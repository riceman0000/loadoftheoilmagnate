using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    public class FlyPlane : MonoBehaviour
    {
        BossInstance bossInstance;
        Rigidbody2D rig;
        [SerializeField] float deleteTime;
        [SerializeField] Sprite R_Sprite, L_Sprite;
        SpriteRenderer spriteRenderer;
        void Start()
        {
            spriteRenderer = this.GetComponent<SpriteRenderer>();
            rig = this.GetComponent<Rigidbody2D>();
            bossInstance = this.transform.parent.parent.GetChild(0).gameObject.GetComponent<BossInstance>();
            StartCoroutine(DeletePlane());
            if (bossInstance.rightFly)
            {
                spriteRenderer.sprite = R_Sprite;
                rig.velocity = new Vector2(bossInstance.speed, 0);
            }
            else
            {
                spriteRenderer.sprite = L_Sprite;
                rig.velocity = new Vector2(-bossInstance.speed, 0);
            }

            IEnumerator DeletePlane()
            {
                yield return new WaitForSeconds(deleteTime);
                Destroy(this.gameObject);
            }
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.GetComponent<IGimmick>() != null)
        //    {
        //        IGimmick gimmick = collision.gameObject.GetComponent<IGimmick>();
        //        gimmick.ReduceDurability(rig, 2);
        //    }
        //}
    }
}