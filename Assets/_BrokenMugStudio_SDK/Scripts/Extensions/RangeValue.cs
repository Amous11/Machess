using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace BrokenMugStudioSDK.Extentions
{
    [System.Serializable]
    public class RangeValue
    {
        [HorizontalGroup]
        public float Min;
        [HorizontalGroup]
        public float Max;

        public float Value
        {
            get { return Random.Range(Min, Max); }
        }

        public RangeValue() { }
        public RangeValue(float i_Value)
        {
            this.Min = i_Value;
            this.Max = i_Value;
        }
        public RangeValue(float i_Min, float i_Max)
        {
            this.Min = i_Min;
            this.Max = i_Max;
        }
        public RangeValue Clone()
        {
            return new RangeValue(Min, Max);
        }
        public override string ToString()
        {
            return string.Format("RangeValue min = {0} max = {1}", Min, Max);
        }

        
    }
}
