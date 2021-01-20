using Anima2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using OilMagnate.StageScene;

namespace OilMagnate.Player
{
    /// <summary>
    /// Debugger.
    /// </summary>
    public partial class Player 
    {
        private void DrawDebugLine(TouchGestureDetector.TouchInfo touchinfo)
        {
            if (touchinfo.Diff.magnitude > _playerData.Status.SwipeThreshold)
            {
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(touchinfo.Positions.First()),
                    Camera.main.ScreenToWorldPoint(touchinfo.LastPosition), Color.red);
            }
            else
            {
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(touchinfo.Positions.First()),
                    Camera.main.ScreenToWorldPoint(touchinfo.LastPosition), Color.blue);
            }
        }
    }
}
