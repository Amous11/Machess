using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BrokenMugStudioSDK.Extentions
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class GridPositionHelper : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField]
        private float m_CellSize = 5;
        [Button]
        public void UpdateCells()
        {
            foreach (Transform child in transform)
            {
                child.localPosition = new Vector3(
                        Mathf.RoundToInt(child.localPosition.x / m_CellSize) * m_CellSize,
                        0,
                        Mathf.RoundToInt(child.localPosition.z / m_CellSize) * m_CellSize);
            }

        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                bool hasChanged = false;
                foreach (Transform child in transform)
                {
                    if (child.hasChanged && Selection.activeGameObject != child.gameObject)
                    {
                        child.localPosition = new Vector3(
                            Mathf.RoundToInt(child.localPosition.x / m_CellSize) * m_CellSize,
                            0,
                            Mathf.RoundToInt(child.localPosition.z / m_CellSize) * m_CellSize);
                        hasChanged = true;
                    }
                }
            }
        }
#endif

    }
}