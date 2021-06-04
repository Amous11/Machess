using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class Board_Tile : MonoBehaviour
{
    [SerializeField]
    private Board m_Board;
    public Vector2Int Coordinates;

    [SerializeField]
    private BlinkAnimation m_BlinkAnimation;

    private void OnMouseDown()
    {
        
    }

#if UNITY_EDITOR

    [Button]
    public void EditorEnit(Board i_Board)
    {
        m_Board = i_Board;
       
        Coordinates = new Vector2Int(transform.parent.GetSiblingIndex(), transform.GetSiblingIndex());
        gameObject.name = Coordinates.ToString();
    }
    
#endif
}
