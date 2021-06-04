using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenMugStudioSDK.Extentions
{
    [System.Serializable]
    public class ChanceValue
    {
        [Range(-0.01f, 1.01f)]
        public float Chance;
        public bool Value
        {
            get { return Random.value <= Chance; }
        }

        public ChanceValue() { }
        public ChanceValue(float i_Chance)
        {
            this.Chance = i_Chance;
        }

        public ChanceValue Clone()
        {
            return new ChanceValue(Chance);
        }
    }
}