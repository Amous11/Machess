using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public int PlayerIndex;
    [SerializeField]
    private Piece[] m_Pieces;
    private Piece m_SelectedPiece;
    private int m_Range { get => ActionPoints; }
    public int ActionPoints;

    public virtual void OnEnable()
    {
        GameManager.OnDiceRoll += RollDice;
    }
    public virtual void OnDisable()
    {
        GameManager.OnDiceRoll -= RollDice;

    }
    public void RollDice()
    {
        Dice.OnDiceStopped += DiceRolled;
        Dice.Instance.ThrowDice();
    }

    private void DiceRolled(int Result)
    {
        SetActionPoints(Result);
        Dice.OnDiceStopped -= DiceRolled;

    }

    public void SetActionPoints(int i_DiceValue)
    {
        ActionPoints=Mathf.Clamp(ActionPoints + i_DiceValue, 0, GameConfig.Instance.GamePlay.ActionPointsCap);
        MenuManager.Instance.GetInGameScreen().UpdateActionPoints(ActionPoints);
    }
    public void SelectPiece(Piece i_SelectedPiece)
    {
        Debug.Log("Select");
        m_SelectedPiece = i_SelectedPiece;
        m_SelectedPiece.Selected(m_Range);
    }

    public void MovePiece(Tile i_TargetPostion)
    {
        if(m_SelectedPiece==null)
        {
            return;
        }
        Debug.Log("Move");
        int usedPoints = Mathf.Abs(m_SelectedPiece.CalculateUsedActionPoints(m_SelectedPiece.CurrentTile, i_TargetPostion));
        SetActionPoints(-usedPoints);
        m_SelectedPiece.Move(i_TargetPostion, m_Range);
       //m_SelectedPiece = null;
        SelectPiece(m_SelectedPiece);
    }

    public void UseAbility(Piece m_SelectedPiece)
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
