using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Hawkeen.Extentions
{
    [System.Serializable]
    public class RangeIntValue
    {
        [HorizontalGroup]
        public int Min;
        [HorizontalGroup]
        public int Max;

        public int Value
        {
            get { return Random.Range(Min, Max+1); }
        }

        public RangeIntValue() { }
        public RangeIntValue(int i_Value)
        {
            this.Min = i_Value;
            this.Max = i_Value;
        }
        public RangeIntValue(int i_Min, int i_Max)
        {
            this.Min = i_Min;
            this.Max = i_Max;
        }
        public RangeIntValue Clone()
        {
            return new RangeIntValue(Min, Max);
        }
        public override string ToString()
        {
            return string.Format("RangeIntValue min = {0} max = {1}", Min, Max);
        }
    }
}
