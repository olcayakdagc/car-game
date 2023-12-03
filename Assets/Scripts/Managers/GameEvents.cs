using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public static class GameEvents
    {
        public static UnityAction onGameStart;
        public static UnityAction onFail;
        public static UnityAction onRestart;
        public static UnityAction onWin;
        public static UnityAction onLoad;
        public static UnityAction onLoadEnd;

    }
}
