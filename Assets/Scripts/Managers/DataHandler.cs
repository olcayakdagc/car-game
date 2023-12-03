using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DataHandler : MonoSingleton<DataHandler>
    {
        public int level
        {
            get { return PlayerPrefs.GetInt("level", 0); }
        }

        public void Increaselevel()
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 0) + 1);
        }

    }
}

