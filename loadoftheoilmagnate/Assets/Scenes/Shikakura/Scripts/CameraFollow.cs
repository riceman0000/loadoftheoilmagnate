using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate {
    public class CameraFollow : ManagedMono
    {
        public CameraMovementSetting CameraMove;
        public Transform player;
        //public float X_min, X_max;//CameraのX座標の限界値
        //public float Y_min, Y_max;//CameraのY座標の限界値
        //
        //public float offset_X, ofsest_Y;


        Vector3 offset,newPosition;
        float speed = 5.0f;
        // Start is called before the first frame update
        void Start()
        {
            if(!player)player = GameObject.FindGameObjectWithTag(ObjTag.Player.ToString()).GetComponent<Transform>();
            //プレイヤーとカメラ間の距離を設定
            //offset = new Vector3(offset_X,ofsest_Y);
            offset = new Vector3(CameraMove.offset_X,CameraMove.ofsest_Y);
        }

        public override void MUpdate()
        {
            //プレイヤーのx軸取得
            //newPosition = transform.position;
            //newPosition.x = player.transform.position.x;
            //newPosition.x = player.transform.position.x + offset.x;

            //newPosition = player.transform.position + offset;
            newPosition = player.position + offset;
            newPosition.z = transform.position.z;
            //カメラとプレイヤーの位置を合わせる
            transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);

            VerticalFollow();
            HorizontalFollow();

            /*if (player.transform.position.x > transform.position.x - 20)
            {
                Vector3 newPosition = transform.position;
                newPosition.x = player.transform.position.x + 20;
                transform.position = Vector3.Lerp(transform.position, newPosition, 5.0f * Time.deltaTime);
            }
            if (player.transform.position.x < transform.position.x - 70)
            {
                Vector3 newPosition = transform.position;
                newPosition.x = player.transform.position.x + 70;
                transform.position = Vector3.Lerp(transform.position, newPosition, 5.0f * Time.deltaTime);
            }*/
            //base.MUpdate();
        }
        public void VerticalFollow()
        {
            //限界値になったら止める
            if (transform.position.y < CameraMove.Y_min)
            {
                newPosition.y = CameraMove.Y_min;
                transform.position = newPosition;
            }
            if (transform.position.y >= CameraMove.Y_max)
            {
                newPosition.y = CameraMove.Y_max;
                transform.position = newPosition;
            }
        }
        public void HorizontalFollow()
        {
            //限界値になったら止める
            if (transform.position.x < CameraMove.X_min)
            {
                newPosition.x = CameraMove.X_min;
                transform.position = newPosition;
            }
            if (transform.position.x >= CameraMove.X_max)
            {
                newPosition.x = CameraMove.X_max;
                transform.position = newPosition;
            }
        }
    }
}
