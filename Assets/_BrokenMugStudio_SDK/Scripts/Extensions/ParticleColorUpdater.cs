using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeen.Extentions
{
    public class ParticleColorUpdater : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] m_Particles;

        public void SetColor(Color i_Color)
        {
            foreach (ParticleSystem colorParticle in m_Particles)
            {
                ParticleSystem.MainModule mainModule = colorParticle.main;
                i_Color.a = mainModule.startColor.color.a;
                mainModule.startColor = i_Color;
            }
        }
    }


}