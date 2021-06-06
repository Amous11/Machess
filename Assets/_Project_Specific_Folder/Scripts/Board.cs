using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Row[] m_Rows;
    public Tile GetPositionTile(Vector3Int i_Position)
    {
        if(i_Position.z>=0 && i_Position.z<m_Rows.Length)
        {
            return m_Rows[i_Position.z].GetTile(i_Position.x);
        }
        return null;
    }
    public void SelectionChanged()
    {
        for (int i = 0; i < m_Rows.Length; i++)
        {
            m_Rows[i].SelectionChanged();
        }
    }
#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_Rows = GetComponentsInChildren<Row>(true);
        for(int i=0;i<m_Rows.Length;i++)
        {
            m_Rows[i].SetRefrences();
        }
    }
#endif
}
