using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawkeen.Extentions
{
    public class SimpleSpawner : MonoBehaviour
    {
        public GameObject Prefab;
        public int Count;
        public Vector3 Delta;

        [Button]
        public void Spawn()
        {
            ExtensionMethodsV2.RemoveAllChild(transform);
            for(int i = 0;i< Count; i++)
            {
                GameObject go = null;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    go = PrefabUtility.InstantiatePrefab(Prefab) as GameObject;
                    go.transform.parent = transform;
                }
#endif
                if (go == null)
                {
                    go = Instantiate(Prefab, transform);
                }
                
                go.transform.localPosition = Delta * i;
                go.transform.localRotation = Quaternion.identity;

            }

        }
    }


}