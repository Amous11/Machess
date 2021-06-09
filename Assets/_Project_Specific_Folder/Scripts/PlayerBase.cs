using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public bool IsMyTurn { get { return PlayerIndex == GameManager.Instance.CurrentPlayerIndex; } }
    public int PlayerIndex;
    [SerializeField]
    private Piece[] m_Pieces;
    private Piece m_SelectedPiece;
    private int m_InitialPiecesCount { get { return m_Pieces.Length; } }
    private int m_KillCount;
    private int m_Range { get => ActionPoints; }
    public int ActionPoints;

    public virtual void OnEnable()
    {
        GameManager.OnDiceRoll += RollDice;
        for(int i=0;i< m_Pieces.Length;i++)
        {
            m_Pieces[i].SetSkin(PlayerIndex);
        }
    }
    public virtual void OnDisable()
    {
        GameManager.OnDiceRoll -= RollDice;

    }
    public void RollDice()
    {
        if(IsMyTurn)
        {
            Dice.OnDiceStopped += DiceRolled;
            Dice.Instance.ThrowDice();
        }
    }

    private void DiceRolled(int Result)
    {
        SetActionPoints(Result);
        Dice.OnDiceStopped -= DiceRolled;
    }

    public void SetActionPoints(int i_DiceValue)
    {
        ActionPoints = Mathf.Clamp(ActionPoints + i_DiceValue, 0, GameConfig.Instance.GamePlay.ActionPointsCap);
        MenuManager.Instance.GetInGameScreen().UpdateActionPoints(ActionPoints);
    }
    public void SelectPiece(Piece i_SelectedPiece)
    {
        Debug.Log("Select");

        for (int i = 0; i < m_Pieces.Length; i++)
        {
            if (i_SelectedPiece == m_Pieces[i])
            {
                m_SelectedPiece = i_SelectedPiece;
                m_SelectedPiece.Selected(m_Range);
                return;
            }
        }
        
        if ((null != m_SelectedPiece) && (m_SelectedPiece.TargetPositionIsValid(i_SelectedPiece.CurrentTile, m_Range)))
        {
            MovePiece(i_SelectedPiece.CurrentTile);
            Destroy(i_SelectedPiece.gameObject);
            m_KillCount++;
            CheckNumberOfEnemyPieces();
        }   
    }

    public void MovePiece(Tile i_TargetPostion)
    {
        if(m_SelectedPiece==null)
        {
            return;
        }

        if (m_SelectedPiece.TargetPositionIsValid(i_TargetPostion, m_Range) && (Dice.Instance.gameObject.IsActive() == false))
        {
            Debug.Log("Move");
            int usedPoints = Mathf.Abs(m_SelectedPiece.CalculateUsedActionPoints(m_SelectedPiece.CurrentTile, i_TargetPostion));
            SetActionPoints(-usedPoints);
            m_SelectedPiece.Move(i_TargetPostion, m_Range);
            SelectPiece(m_SelectedPiece);
        }
        else
        {
            m_SelectedPiece.Selected(0);
        }
           
    }

    public void UseAbility(Piece m_SelectedPiece)
    {

    }

    public void CheckNumberOfEnemyPieces()
    {
        if (m_KillCount >= m_InitialPiecesCount)
        {
            GameManager.Instance.WinCondition = true;
        }
    }

#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_Pieces = GetComponentsInChildren<Piece>(true);
    }
#endif
}
