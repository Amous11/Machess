using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeen.Extentions
{
    public class LookToCamera : MonoBehaviour
    {
        private Transform m_Camera;
        // Start is called before the first frame update
        void Start()
        {
            m_Camera = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(m_Camera);
        }
    }

}
