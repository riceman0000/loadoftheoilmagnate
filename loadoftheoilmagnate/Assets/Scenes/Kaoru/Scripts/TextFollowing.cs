using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate
{
    public class TextFollowing : ManagedMono
    {
        private RectTransform myRectTfm;
        private Vector3 offset = new Vector3(0, 1.5f, 0);
        StageScene.StageManager StageManager;
        Transform target;

        protected sealed override void Awake()
        {
            //何故かStart()だとMUpdate()から処理し始める？？？？
            //Start->Awakeに変更したらエラーにならなくなったけど
            //次はMUpdate()が呼ばれなくなりプレイヤーのポジションを追尾できない
            StageManager = GameObject.Find("StageManager").GetComponent<StageScene.StageManager>();
            Debug.Log(StageManager.name);
            myRectTfm = this.GetComponent<RectTransform>();
        }
        public override void MUpdate()
        {
            Debug.Log("update01");
            target = StageManager.Player.transform;
            Debug.Log("update02");
            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position + offset);
        }
        public void DestroyObj()
        {
            Destroy(this.gameObject);
        }
    }
}