using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeen.Extentions
{
    public class JointScaler : MonoBehaviour
    {
        public Joint Joint;
        private Vector3 m_ConnectedAnchor;
        private Vector3 m_Anchor;

        [Button]
        public void FindJoint()
        {
            Joint = GetComponent<Joint>();
        }


        private void Awake()
        {
            if (Joint == null)
            {
                FindJoint();
            }

            if (Joint != null)
            {
                m_ConnectedAnchor = Joint.connectedAnchor;
                m_Anchor = Joint.anchor;
            }

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Joint != null)
            {
                Joint.connectedAnchor = m_ConnectedAnchor;
                Joint.anchor = m_Anchor;
            }
        }
    }


}