using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField]
    private Tile[] m_Tiles;
    public Tile GetTile(int x)
    {
        if(x>=0 && x< m_Tiles.Length)
        {
            return m_Tiles[x];
        }
        return null;
    }

#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_Tiles = GetComponentsInChildren<Tile>(true);
        for (int i = 0; i < m_Tiles.Length; i++)
        {
            m_Tiles[i].SetRefrences();
        }
    }

    
#endif
}
