using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BlinkAnimation : MonoBehaviour
{
    private const string k_BlinkName = "_Blink";
    private const string k_BlinkColor = "_ColorBlink";

    private Sequence m_AnimationTweens;

    [SerializeField]
    private Renderer m_Renderer;

    private float m_TimeToBlink { get { return GameConfig.Instance.Tweens.TimeToBlink; } }

    private float m_TimeToNormal { get { return GameConfig.Instance.Tweens.TimeToNormal; } }

    private bool IsAnimationPlaying = false;

    public void PingPongBlink(float i_Time)
    {
        if (m_Renderer != null)
        {
            m_Renderer.material.SetFloat(k_BlinkName, Mathf.PingPong(i_Time, 1));
        }
    }

    public void StartAnimation(bool i_ForcePlay=false)
    {
        if (IsAnimationPlaying && !i_ForcePlay)
        {
            return;
        }
        if (m_Renderer != null)
        {
            IsAnimationPlaying = true;
            m_AnimationTweens?.Kill();
            m_AnimationTweens = DOTween.Sequence();
            m_AnimationTweens.Append(
                m_Renderer.material.DOFloat(1, k_BlinkName, m_TimeToBlink).
                SetEase(Ease.OutSine)
                );

            m_AnimationTweens.Append(
                m_Renderer.material.DOFloat(0, k_BlinkName, m_TimeToNormal).
                SetEase(Ease.InOutSine));
            m_AnimationTweens.OnComplete(AnimationComplete);
        }
    }
    public void StartAnimation(float i_Delay, bool i_ForcePlay = false)
    {
        if(IsAnimationPlaying && !i_ForcePlay)
        {
            return;
        }

        if (m_Renderer != null)
        {
            IsAnimationPlaying = true;

            m_AnimationTweens?.Kill();
            m_AnimationTweens = DOTween.Sequence();
            m_AnimationTweens.SetDelay(i_Delay);
            m_AnimationTweens.Append(
               m_Renderer.material.DOFloat(1, k_BlinkName, m_TimeToBlink).
               SetEase(Ease.OutSine)
               );
            m_AnimationTweens.Append(
                m_Renderer.material.DOFloat(1, k_BlinkName, m_TimeToBlink).
                SetEase(Ease.OutSine));
            m_AnimationTweens.Append(
                m_Renderer.material.DOFloat(0, k_BlinkName, m_TimeToNormal).
                SetEase(Ease.InOutSine));
            m_AnimationTweens.OnComplete(AnimationComplete);

        }
    }

    public void AnimationComplete()
    {
        IsAnimationPlaying = false;

    }

#if UNITY_EDITOR
    [Button]
    public void EditorInit()
    {
        m_Renderer = GetComponent<Renderer>();
    }
#endif
}
