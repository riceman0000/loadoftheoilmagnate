using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    /// <summary>
    /// GameObject.tagのenum
    /// </summary>
    public enum ObjTag
    {
        /// <summary>プレイヤー</summary>
        Player,
        /// <summary>敵</summary>
        Enemy,
        /// <summary>地面</summary>
        FieldMap,
        /// <summary>石油の間欠泉</summary>
        OilGeyser,
        /// <summary>木箱</summary>
        WoodenBox,
        /// <summary>ドラム缶</summary>
        Drum,
        /// <summary>炎</summary>
        Fire,
        /// <summary>転がるドラム缶</summary>
        DrumRoll,
    }

    /// <summary>
    /// 使用するScene名のenum
    /// </summary>
    [Flags]
    public enum SceneTitle
    {
        /// <summary>Invalid value</summary>        
        None = 0,
        Menu = 1,
        Title = 1 << 1,
        StageSelect = 1 << 2,
        HighScore = 1 << 3,
        Opening = 1 << 4,
        Scenario = 1 << 5,
        StageOne = 1 << 6,
        StageTwo = 1 << 7,
        LastBoss = 1 << 8,
        Continue = 1 << 9,
        GameOver = 1 << 10,
        Ending = 1 << 11,
        StaffRoll = 1 << 12,
        StageOneTest = 1 << 13,
        StageThree = 1 << 14,
        StageFour = 1 << 15,
        All = Menu | Title | StageSelect | HighScore | Opening | Scenario
            | StageOne | StageTwo | LastBoss | Continue | GameOver | Ending
            | StaffRoll | StageOneTest | StageThree | StageFour,
    }
}