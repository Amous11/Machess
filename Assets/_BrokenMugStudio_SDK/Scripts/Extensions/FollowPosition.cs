using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeen.Extentions
{
    public class FollowPosition : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Follow;
        private void LateUpdate()
        {
            if (m_Follow != null)
            {
                transform.position = m_Follow.position;
            }
            
        }
    }
}