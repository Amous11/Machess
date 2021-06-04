    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenMugStudioSDK.Extentions
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField]
        private float m_Delay = -1;
        public void DestroyGO()
        {
            Destroy(gameObject);
        }
        public void DisableGO()
        {
            gameObject.SetActive(false);
        }
        // Start is called before the first frame update
        void Start()
        {
            if (m_Delay > 0)
            {
                Destroy(gameObject, m_Delay);
            }
        }
    }
}