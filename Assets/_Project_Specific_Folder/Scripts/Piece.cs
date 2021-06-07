using BrokenMugStudioSDK;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public ePieceTypes Type;
    [SerializeField]
    private Board m_Board;
    public Tile CurrentTile;
    public PieceData m_Settings { get => GameConfig.Instance.GamePlay.GetPieceData(Type); }
    [ShowInInspector]
    private List<Vector3Int> m_PossibleMoves;
    [SerializeField]
    private LayerMask m_TileLayer;
    private void OnEnable()
    {
        SetCurrentTile();
    }
    public void SetCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position+Vector3.up, Vector3.down, out hit, 20, m_TileLayer))
        {
            if ((hit.collider.gameObject.CompareTag(eTags.Tile.ToString())))
            {
                CurrentTile = hit.collider.GetComponent<Tile>();
            }
        }
        
    }
    public void Move(Tile i_TilePosition,int i_Range)
    {
        Debug.Log("Move X");

        if (TargetPositionIsValid(i_TilePosition, i_Range))
        {
            Debug.Log("Move >TargetPositionIsValid X");
            CurrentTile = i_TilePosition;
            transform.DOMove(i_TilePosition.transform.position, .5f);
            m_Board.SelectionChanged(); //highlights tiles

        }
    }
    public bool TargetPositionIsValid(Tile i_TilePosition, int i_Range)
    {
        return m_PossibleMoves.Contains(i_TilePosition.Position);
    }

    public void Selected(int i_Range)
    {
        Debug.Log("Se");
        m_Board.SelectionChanged();

        m_PossibleMoves = new List<Vector3Int>();
        for (int i=0; i<m_Settings.Moves.Length; i++)
        {
            for(int j=1;j<=i_Range;j++)
            {
                if(m_Board.GetPositionTile(CurrentTile.Position + (m_Settings.Moves[i] * j)) != null)
                {
                    m_PossibleMoves.Add(CurrentTile.Position + (m_Settings.Moves[i] * j));
                    m_Board.GetPositionTile(CurrentTile.Position + (m_Settings.Moves[i] * j)).HighlightTile(true);
                }
            }
        }
    }

    public int CalculateUsedActionPoints(Tile i_InitialTile, Tile i_ClickedTile)
    {
        Vector3 vector = i_ClickedTile.Position - i_InitialTile.Position;
        if ((Mathf.Abs(vector.x) >= (Mathf.Abs(vector.z))))
        {
            return (int)vector.x;
        }
        else
        {
            return (int)vector.z;
        }
    }
}
