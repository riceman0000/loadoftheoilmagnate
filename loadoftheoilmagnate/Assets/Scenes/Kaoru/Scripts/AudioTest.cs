using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace OilMagnate
{
    /// <summary>
    /// Audioテスト用クラス(実装時にはいらない)
    /// </summary>
    public class AudioTest : ManagedMono
    {
        [SerializeField]
        SceneTitle _title;

        void Start()
        {

        }
    
#if false

        // Update is called once per frame
        public override void MUpdate()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SEManager.Instance.SEPlay(SETag.Shoot);
            }
        }
#endif
    }
}
