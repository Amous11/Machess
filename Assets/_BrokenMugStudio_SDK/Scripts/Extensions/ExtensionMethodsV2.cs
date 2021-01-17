using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeen.Extentions
{
    public class ExtensionMethodsV2 : MonoBehaviour
    {
        public static void RemoveAllChild(Transform i_Transform)
        {
            if (!Application.isPlaying)
            {
                while (i_Transform.childCount > 0)
                {
                    Object.DestroyImmediate(i_Transform.GetChild(0).gameObject);
                }
            }
            else
            {
                int numChild = i_Transform.childCount;
                for (int i = 0; i < numChild; i++)
                {
                    Object.Destroy(i_Transform.GetChild(i).gameObject);
                }
            }
        }

        public static float DistanceSqr(Vector3 i_Pos01, Vector3 i_Pos02)
        {
            return (i_Pos01 - i_Pos02).sqrMagnitude;
        }

        public static Vector3 RotatePointAroundPivot(Vector3 i_Point, Vector3 i_Pivot, Vector3 i_Angles)
        {
            Vector3  dir = i_Point - i_Pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(i_Angles) * dir; // rotate it
            i_Point = dir + i_Pivot; // calculate rotated point
            return i_Point;
        }
    }
}