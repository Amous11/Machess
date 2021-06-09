using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skin : MonoBehaviour
{
    [SerializeField]
    private SkinSetter[] m_SkinSetters;
    private PieceTypeSkin m_Settings;
    private void OnEnable()
    {
        DisableAll();
    }
    public void DisableAll()
    {
        for(int i=0;i< m_SkinSetters.Length;i++)
        {
            m_SkinSetters[i].DisableAll();
        }
    }

    public void SetSkin(ePieceTypes i_PieceType,int i_PlayerIndex)
    {
        DisableAll();
        m_Settings = GameConfig.Instance.Skins.GetSkinSettings(i_PieceType);
        
        for(int i=0;i< m_Settings.Skin.Length;i++)
        {
            if(GetSkinSetter(m_Settings.Skin[i].Type)!=null)
            {
                GetSkinSetter(m_Settings.Skin[i].Type).SetSkin(m_Settings.Skin[i].Index);
                GetSkinSetter(m_Settings.Skin[i].Type).SetMaterial(i_PlayerIndex);
            }
        }
    }

    public SkinSetter GetSkinSetter(eItemType i_Type)
    {
        for(int i=0;i< m_SkinSetters.Length;i++)
        {
            if(m_SkinSetters[i].ItemType==i_Type)
            {
                return m_SkinSetters[i];
            }
        }
        return null;
    }

#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_SkinSetters = GetComponentsInChildren<SkinSetter>(true);
        for(int i=0;i< m_SkinSetters.Length;i++)
        {
            m_SkinSetters[i].SetRefrences();
        }
    }

#endif
}
