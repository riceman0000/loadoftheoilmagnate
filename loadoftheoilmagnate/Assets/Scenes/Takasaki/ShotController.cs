using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    // Don't use this script!! 使うな!! 
    public class ShotController : MonoBehaviour
    {
        public float shotSpeed = 2.0f;

        public void ShotMissile(GameObject missilePrefab, Transform playerTrans)
        {
            GameObject missile = Instantiate(missilePrefab,transform); // NullReferenceException 吐く
            Rigidbody2D missileRb2d = missile.GetComponent<Rigidbody2D>();
            var vec2 = (playerTrans.position - transform.position).normalized;

            missileRb2d.velocity = vec2 * shotSpeed;
        }
    }
}