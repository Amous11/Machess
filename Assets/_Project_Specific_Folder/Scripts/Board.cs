using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Board_Tile[] m_BoardTiles;

    public Board_Tile GetTileByCoordonate(Vector2 i_Coordonate)
    {
        for (int i = 0; i < m_BoardTiles.Length; i++)
        {
            if(m_BoardTiles[i].Cordinates==i_Coordonate)
            {
                return m_BoardTiles[i];
            }
        }

        return null;
    }

#if UNITY_EDITOR
    [Button]
    private void editorEnit()
    {
        m_BoardTiles = GetComponentsInChildren<Board_Tile>(true);
        for(int i=0;i< m_BoardTiles.Length;i++)
        {
            m_BoardTiles[i].EditorEnit(this);
        }
    }
#endif
}
