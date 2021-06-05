using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    
    [SerializeField]
    private Piece[] m_Pieces;
    private Piece m_SelectedPiece;
    private int m_Range = 3;
    private int m_actionPoint = 0;

    public void SelectPiece(Piece i_SelectedPiece)
    {
        Debug.Log("Select");
        m_SelectedPiece = i_SelectedPiece;
        m_SelectedPiece.Selected(m_Range);
    }

    public void MovePiece(Tile i_TargetPostion)
    {
        Debug.Log("Move");
        m_SelectedPiece.Move(i_TargetPostion, m_Range);
    }

    public void UseAbility(Piece m_SelectedPiece)
    {

    }

    public int RollDice(int i_DiceType)
    {
        return m_actionPoint += Random.Range(0, i_DiceType);
    }

    public void DistributeMana()
    {

    }

    

#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_Pieces = GetComponentsInChildren<Piece>(true);
    }
#endif
}
