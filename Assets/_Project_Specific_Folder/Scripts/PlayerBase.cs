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
        GameManager.OnTurnEnds += OnTurnEnds;
        for(int i=0;i< m_Pieces.Length;i++)
        {
            m_Pieces[i].SetSkin(this);
        }
    }
    public virtual void OnDisable()
    {
        GameManager.OnDiceRoll -= RollDice;
        GameManager.OnTurnEnds -= OnTurnEnds;

    }
    public void RollDice()
    {
        if(IsMyTurn)
        {
            Dice.OnDiceStopped += DiceRolled;
            Dice.Instance.ThrowDice();
        }
    }
    public void OnTurnEnds()
    {
        if (IsMyTurn)
        {
            MenuManager.Instance.GetInGameScreen().UpdateActionPoints(ActionPoints);

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
            MovePiece(i_SelectedPiece.CurrentTile, i_SelectedPiece);
        }   
    }

    public void KilledEnemyPiece()
    {
        m_KillCount++;
        CheckNumberOfEnemyPieces();
    }
    public void MovePiece(Tile i_TargetPostion,Piece i_PieceToKill=null)
    {
        if(m_SelectedPiece==null)
        {
            Debug.LogError("Selected Piece Is NULL");
            return;
        }

        if (m_SelectedPiece.TargetPositionIsValid(i_TargetPostion, m_Range) && (Dice.Instance.HasRolledDice == true))
        {
            Debug.Log("Move");
            int usedPoints = Mathf.Abs(m_SelectedPiece.CalculateUsedActionPoints(m_SelectedPiece.CurrentTile, i_TargetPostion));
            SetActionPoints(-usedPoints);
            m_SelectedPiece.Move(i_TargetPostion, m_Range, i_PieceToKill);
            SelectPiece(m_SelectedPiece);
        }
        else
        {
            m_SelectedPiece.Selected(0);
        }
        GameManager.Instance.PlayerMoved();
           
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
