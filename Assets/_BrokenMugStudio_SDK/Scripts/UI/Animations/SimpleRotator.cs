using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class SimpleRotator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_Axis;
        [SerializeField]
        private float m_Speed;
        void Update()
        {
            transform.eulerAngles += m_Axis * m_Speed * Time.deltaTime;
        }
    }
}

