using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxColorSetter : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] m_Particles;

    public void SetColor(Color i_Color)
    {
        for(int i=0;i< m_Particles.Length;i++)
        {
            m_Particles[i].startColor = i_Color;
        }
    }
#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_Particles = GetComponentsInChildren<ParticleSystem>(true);
    }
#endif 
}
