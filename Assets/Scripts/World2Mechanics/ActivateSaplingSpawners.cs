﻿using System.Collections.Generic;
using UnityEngine;

// For List<T>

namespace Assets.Scripts.World2Mechanics {
    /// <summary>
    /// 
    /// ActivateSaplingSpawners
    /// 
    /// Used in World 2 Waterfall scene.
    /// When obstacle button is pressed, all sapling spawners become pressable.
    /// 
    /// </summary>

    public class ActivateSaplingSpawners : MonoBehaviour {

        public List<SaplingSpawner> saplingSpawners = new List<SaplingSpawner>();

        public void Activate()
        {
            foreach(SaplingSpawner saplingSpawner in saplingSpawners)
            {
                saplingSpawner.setPressable(true);
            }
        }
    }
}
