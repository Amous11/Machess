using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace BrokenMugStudioSDK
{
    public class ExtendedButton : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        // LEGACY
        public Action onDown = () => { };
        public Action onUp = () => { };
        // LEGACY

        public Action<PointerEventData> OnDownEvent = (i_PointerEventData) => { };
        public Action<PointerEventData> OnUpEvent = (i_PointerEventData) => { };
        public Action<PointerEventData> OnBeginDragEvent = (i_PointerEventData) => { };
        public Action<PointerEventData> OnDragEvent = (i_PointerEventDataevData) => { };
        public Action<PointerEventData> OnEndDragEvent = (i_PointerEventData) => { };

        [SerializeField, ReadOnly] private Image m_OriginalImage;

        [SerializeField, ReadOnly] private Image[] m_Images;
        [SerializeField, ReadOnly] private Color[] m_ImagesOriginalColors;

        [SerializeField, ReadOnly] private Text[] m_Texts;
        [SerializeField, ReadOnly] private Color[] m_TextsOriginalColors;

        [SerializeField, ReadOnly] private TextMeshProUGUI[] m_TextsTMP;
        [SerializeField, ReadOnly] private Color[] m_TextsTMPOriginalColors;
        [SerializeField, ReadOnly] private Color[] m_TextsTMPOutlineOriginalColors;

        public ButtonJuice BtnJuice { get { return GameConfig.Instance.HUD.MainButtonJuice; } }

#if UNITY_EDITOR
        //Needs to be public due to custom Editor
        [Button]
        public virtual void SetRefs()
        {
            m_OriginalImage = GetComponent<Image>();

            //TODO - can we simplify this?
            m_Images = GetComponentsInChildren<Image>();
            m_ImagesOriginalColors = new Color[m_Images.Length];
            for (int i = 0; i < m_ImagesOriginalColors.Length; i++)
            {
                m_ImagesOriginalColors[i] = m_Images[i].color;
            }

            m_Texts = GetComponentsInChildren<Text>();
            m_TextsOriginalColors = new Color[m_Texts.Length];
            for (int i = 0; i < m_TextsOriginalColors.Length; i++)
            {
                m_TextsOriginalColors[i] = m_Texts[i].color;
            }

            m_TextsTMP = GetComponentsInChildren<TextMeshProUGUI>();
            m_TextsTMPOriginalColors = new Color[m_TextsTMP.Length];
            for (int i = 0; i < m_TextsTMPOriginalColors.Length; i++)
            {
                m_TextsTMPOriginalColors[i] = m_TextsTMP[i].color;
            }

            m_TextsTMPOutlineOriginalColors = new Color[m_TextsTMP.Length];
            for (int i = 0; i < m_TextsTMPOutlineOriginalColors.Length; i++)
            {
                m_TextsTMPOutlineOriginalColors[i] = m_TextsTMP[i].fontSharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        public void SetInteractable(bool i_Value)
        {
            interactable = i_Value;

            //Debug.LogError("Set Interactable " + i_Value);

            //TODO - can we simplify this?
            for (int i = 0; i < m_Images.Length; i++)
            {
                if (i_Value)
                {
                    if (m_Images[i] != m_OriginalImage)
                        m_Images[i].color = m_ImagesOriginalColors[i];
                }
                else
                {
                    if (m_Images[i] != m_OriginalImage)
                        m_Images[i].color = m_ImagesOriginalColors[i] * colors.disabledColor;
                }
            }

            for (int i = 0; i < m_Texts.Length; i++)
            {
                if (i_Value)
                {
                    m_Texts[i].color = m_TextsOriginalColors[i];
                }
                else
                {
                    m_Texts[i].color = m_TextsOriginalColors[i] * colors.disabledColor;
                }
            }

            for (int i = 0; i < m_TextsTMP.Length; i++)
            {
                if (i_Value)
                {
                    m_TextsTMP[i].color = m_TextsTMPOriginalColors[i];
                    m_TextsTMP[i].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, m_TextsTMPOutlineOriginalColors[i]);
                }
                else
                {
                    m_TextsTMP[i].color = m_TextsTMPOriginalColors[i] * colors.disabledColor;
                    m_TextsTMP[i].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, m_TextsTMPOutlineOriginalColors[i] * colors.disabledColor);
                }


                //Debug.LogError(m_TextsTMP[i].name + "       " + m_TextsTMP[i].color);
            }
        }



        #region Events
        private Sequence m_BackToNormalSequence;

        public override void OnPointerDown(PointerEventData i_PointerEventData)
        {
            base.OnPointerDown(i_PointerEventData);
            /*m_BackToNormalSequence.Kill();

            transform.DOKill();
            transform.DOScale(Vector3.one * BtnJuice.ScaleDown, BtnJuice.DownDurration).SetEase(BtnJuice.DownEase);
            if(BtnJuice.DoVibrateDown)
            {
                HapticManager.Instance.Haptic(MoreMountains.NiceVibrations.HapticTypes.Selection);
            }*/
            OnPointerDownEffect();
            OnDownEvent.Invoke(i_PointerEventData);
            onDown.Invoke();
        }
        public void OnPointerDownEffect()
        {
            m_BackToNormalSequence.Kill();
            transform.localScale = Vector3.one;
            m_BackToNormalSequence = DOTween.Sequence();
            m_BackToNormalSequence.Append(transform.DOScale(Vector3.one * BtnJuice.ScaleDown, BtnJuice.DownDurration).SetEase(BtnJuice.DownEase));
            if (BtnJuice.DoVibrateDown)
            {
                HapticManager.Instance.Haptic(MoreMountains.NiceVibrations.HapticTypes.Selection);
            }
        }
        public void OnPointerUpEffect()
        {
            transform.localScale = Vector3.one * BtnJuice.ScaleDown;
            m_BackToNormalSequence = DOTween.Sequence();
            m_BackToNormalSequence.Append(transform.DOPunchScale(Vector3.one * BtnJuice.Strength, BtnJuice.UpShakeDurration, BtnJuice.Vibro, BtnJuice.Elasticity));
            m_BackToNormalSequence.Append(transform.DOScale(Vector3.one, BtnJuice.UpResetDurration).SetEase(BtnJuice.DownEase));

            if (BtnJuice.DoVibrateUp)
            {
                HapticManager.Instance.Haptic(MoreMountains.NiceVibrations.HapticTypes.Selection);
            }
        }
        public override void OnPointerUp(PointerEventData i_PointerEventData)
        {
            base.OnPointerUp(i_PointerEventData);
            OnPointerUpEffect();
            OnUpEvent.Invoke(i_PointerEventData);
            onUp.Invoke();
        }

        public virtual void OnBeginDrag(PointerEventData i_PointerEventData)
        {
            OnBeginDragEvent(i_PointerEventData);
        }

        public virtual void OnDrag(PointerEventData i_PointerEventData)
        {
            OnDragEvent(i_PointerEventData);
        }

        public virtual void OnEndDrag(PointerEventData i_PointerEventData)
        {
            OnEndDragEvent(i_PointerEventData);
        }
        #endregion


    }
}