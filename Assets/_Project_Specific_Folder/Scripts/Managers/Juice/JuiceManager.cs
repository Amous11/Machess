using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenMugStudioSDK
{
    [System.Serializable]
    public class DictionaryJuiceTypeListJuiceConfig : UnitySerializedDictionary<eJuice, JuiceData>
    {

    }
    public class JuiceManager : Singleton<JuiceManager>
    {

        [SerializeField]
        public DictionaryJuiceTypeListJuiceConfig JuiceItemDefinition = new DictionaryJuiceTypeListJuiceConfig();

        private JuiceData m_DummyJuice;
        private Coroutine m_SlowMoCoroutine;
        [SerializeField, ReadOnly]
        private float m_NormalFixedTimeStep;
        private void OnEnable()
        {
            m_NormalFixedTimeStep = Time.fixedDeltaTime;
        }
        public void JuiceIt(eJuice i_JuiceEvent)
        {
            if (!JuiceItemDefinition.ContainsKey(i_JuiceEvent))
            {
                return;
            }
            m_DummyJuice = JuiceItemDefinition[i_JuiceEvent];
            if (m_DummyJuice.DoHaptics)
            {
                HapticManager.Instance.Haptic(m_DummyJuice.HapticType);
            }
            if (m_DummyJuice.DoShake)
            {
                CameraBehaviour.Instance.DoShakeCoroutine(m_DummyJuice.ShakeData);
            }
            if (m_DummyJuice.DoSlowMo)
            {
                if (m_SlowMoCoroutine != null)
                {
                    StopCoroutine(m_SlowMoCoroutine);
                }
                m_SlowMoCoroutine = StartCoroutine(IESlowMo(m_DummyJuice.SlowMoData));
            }
        }

        private float m_SlowMoTime;
        private float m_DummyMultiplier;
        IEnumerator IESlowMo(SlowMoData i_Data)
        {
            m_SlowMoTime = i_Data.SlowMoDurration;
            float t = 0;

            while (m_SlowMoTime > 0)
            {
                t = 1f - (m_SlowMoTime / i_Data.SlowMoDurration);
                m_DummyMultiplier = i_Data.SlowMoCurve.Evaluate(t);
                Time.timeScale = m_DummyMultiplier;
                Time.fixedDeltaTime = m_NormalFixedTimeStep * m_DummyMultiplier;
                yield return new WaitForEndOfFrame();
                m_SlowMoTime -= Time.unscaledDeltaTime;
            }
            yield return null;
            Time.timeScale = 1;
            Time.fixedDeltaTime = m_NormalFixedTimeStep;
        }

    }

    [Serializable]
    public class JuiceData
    {
        public bool DoShake;
        [ShowIf(nameof(DoShake))]
        public CameraShakeData ShakeData;
        public bool DoHaptics;
        [ShowIf(nameof(DoHaptics))]
        public HapticTypes HapticType;
        public bool DoSlowMo;
        [ShowIf(nameof(DoSlowMo))]
        public SlowMoData SlowMoData;

    }
    [Serializable]
    public class SlowMoData
    {
        public float SlowMoDurration = .5f;
        public AnimationCurve SlowMoCurve;
    }
}

