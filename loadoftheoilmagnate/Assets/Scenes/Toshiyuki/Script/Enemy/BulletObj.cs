using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    public class BulletObj : MonoBehaviour
    {
        Rigidbody2D rig;
        Human2 human2;
        [SerializeField] float deleteTime;
        [SerializeField] GameObject Lbullet;
        [SerializeField] GameObject Rbullet;
        [SerializeField]
        long _attack = 10000;

        void Start()
        {
            human2 = this.transform.parent.GetComponent<Human2>();
            rig = this.GetComponent<Rigidbody2D>();
            StartCoroutine(DeleteBullet());
            if (this.gameObject == Lbullet)
            {
                rig.velocity = new Vector2(-human2.bulletSpeed, 0);
            }
            else if (this.gameObject == Rbullet)
            {
                rig.velocity = new Vector2(human2.bulletSpeed, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"col.tag = {collision.tag}");
            IGimmick gimmick = null;
            if (collision.TryGetComponent(out gimmick))
            {
                if (!gimmick.ID.HasFlag(GimmickIdentifier.ZakoGun)
                    && !gimmick.ID.HasFlag(GimmickIdentifier.ZakoKnife)
                    && !gimmick.ID.HasFlag(GimmickIdentifier.Fighter))
                {
                    gimmick.ReduceDurability(rig, 1);
                }
            }
            if (collision.tag.Equals(ObjTag.Player.ToString()))
            {
                StageManager.Instance.Player.MPManager.TakeDamage(_attack);
            }
        }

        IEnumerator DeleteBullet()
        {
            yield return new WaitForSeconds(deleteTime);
            Destroy(this.gameObject);
        }
    }
}