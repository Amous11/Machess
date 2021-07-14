using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSetter : MonoBehaviour
{
    public eItemType ItemType;
    [SerializeField]
    private SkinnedMeshRenderer[] m_SkinnedMeshes; 
    public void SetSkin(int i_Index)
    {
        DisableAll();
        if (i_Index >= 0&& i_Index<transform.childCount)
        {
            transform.GetChild(i_Index).gameObject.SetActive(true);
        }
    }

    public void DisableAll()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void SetMaterial(int i_Index)
    {
        for(int i=0;i< m_SkinnedMeshes.Length;i++)
        {
            m_SkinnedMeshes[i].material = GameConfig.Instance.Skins.PlayersMaterials[i_Index];
        }
    }

#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_SkinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>(true);
    }
#endif 
}
