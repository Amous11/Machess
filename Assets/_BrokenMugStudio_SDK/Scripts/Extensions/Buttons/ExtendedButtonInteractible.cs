using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace BrokenMugStudioSDK
{
    public class ExtendedButtonInteractible : MonoBehaviour
    {
        private Image m_Image;
        private TextMeshProUGUI m_TextTMP;
        private Text m_Text;

        [SerializeField]
        private Color m_InteractibleColor = Color.white;
        [SerializeField]
        private Color m_NonInteractibleColor = Color.gray;

        public void SetRefs()
        {
            m_Image = GetComponent<Image>();
            m_TextTMP = GetComponent<TextMeshProUGUI>();
            m_Text = GetComponent<Text>();
            if (m_TextTMP != null)
            {
                m_InteractibleColor= m_TextTMP.color;
            }
            if (m_Text != null)
            {
                m_InteractibleColor = m_Text.color;
            }
            if (m_Image != null)
            {
                m_InteractibleColor= m_Image.color;
            }
        }


        public void SetInteractable(bool i_Interactable = true)
        {
            if (i_Interactable)
            {
                if (m_TextTMP != null)
                {
                    m_TextTMP.color = m_InteractibleColor;
                }
                if (m_Image != null)
                {
                    m_Image.color = m_InteractibleColor;
                }
                if (m_Text != null)
                {
                    m_Text.color = m_InteractibleColor;
                }
            }
            else
            {
                if (m_TextTMP != null)
                {
                    m_TextTMP.color = m_NonInteractibleColor;
                }
                if (m_Image != null)
                {
                    m_Image.color = m_NonInteractibleColor;
                }
                if (m_Text != null)
                {
                    m_Text.color = m_NonInteractibleColor;
                }
            }
        }

    }

}
