using System;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class PlayerSaveData
    {
        public Vector3 Position;
        public int Health;
        public float Mana;
    }
}