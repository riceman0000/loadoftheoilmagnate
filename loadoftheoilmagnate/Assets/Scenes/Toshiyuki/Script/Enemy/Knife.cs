using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    public class Knife : MonoBehaviour
    {
        Rigidbody2D rig;
        private void Start()
        {
            rig = this.GetComponent<Rigidbody2D>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<IGimmick>() != null)
            {
                IGimmick gimmick = collision.gameObject.GetComponent<IGimmick>();
                if (gimmick.ID.Equals(GimmickIdentifier.ZakoKnife)) return;
                gimmick.ReduceDurability(rig, 2);
            }
        }
      
    }
}