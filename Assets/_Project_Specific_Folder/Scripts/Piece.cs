using BrokenMugStudioSDK;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private PlayerBase m_Player;
    public bool IsPlayerTurn { get { return PlayerIndex == GameManager.Instance.CurrentPlayerIndex; } }

    public bool IsDead;
    public int PlayerIndex; 
    public ePieceTypes Type;
    [SerializeField]
    private Board m_Board;
    public Tile CurrentTile;
    private PieceData m_Settings { get => GameConfig.Instance.GamePlay.GetPieceData(Type); }
    private GamePlayVariablesEditor m_GamePlay { get => GameConfig.Instance.GamePlay; }
    [SerializeField]
    private Collider m_Collider;
    [ShowInInspector]
    private List<Vector3Int> m_PossibleMoves;
    [SerializeField]
    private LayerMask m_TileLayer;
    [SerializeField]
    private Skin m_Skin;
    [SerializeField]
    private Animator m_CharacterAnimator;
    private Piece m_TargetPiece;
    [SerializeField]
    private GameObject m_PiecesHighlight;
    [SerializeField]
    private FxColorSetter m_FxColorSetter;
    [SerializeField]
    private ParticleSystem m_DeathFX;
    private void OnEnable()
    {
        SetCurrentTile();
        GameManager.OnTurnEnds += OnTurnEnds;
    }
    private void OnDisable()
    {
        GameManager.OnTurnEnds -= OnTurnEnds;

    }
    public void OnTurnEnds()
    {
        if(IsPlayerTurn)
        {
            if(m_PiecesHighlight!=null)
            {
                m_PiecesHighlight.SetActive(true);
            }
        }else
        {
            if (m_PiecesHighlight != null)
            {
                m_PiecesHighlight.SetActive(false);
            }
        }
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

    public void SetSkin(PlayerBase i_Player)
    {
        m_Player = i_Player;
        PlayerIndex = i_Player.PlayerIndex;
        m_Skin.SetSkin(Type, PlayerIndex);
        m_FxColorSetter.SetColor(GameConfig.Instance.GamePlay.PlayerColors[PlayerIndex]);
        OnTurnEnds();

    }

    public void Move(Tile i_TilePosition,int i_Range,Piece i_PieceToKill)
    {
        Debug.Log("Move X");
        m_TargetPiece = i_PieceToKill;
        if (TargetPositionIsValid(i_TilePosition, i_Range))
        {
            Debug.Log("Move >TargetPositionIsValid X");
            CurrentTile = i_TilePosition;
            Vector3 targetPosition = Vector3.zero;
            if(m_TargetPiece!=null)
            {
                targetPosition = i_TilePosition.transform.position + m_TargetPiece.transform.forward;

            }else
            {
                targetPosition = i_TilePosition.transform.position ;

            }
            transform.DOLookAt(targetPosition, .1f);
            transform.DOMove(targetPosition, m_GamePlay.MoveTime).OnComplete(OnMoveComplete);

            m_Board.SelectionChanged(); //highlights tiles
            float speed = Vector3.Distance(targetPosition, transform.position)* m_GamePlay.RunSpeed;
            m_CharacterAnimator.SetBool(eAnimator.Run.ToString(), true);
            m_CharacterAnimator.SetFloat(eAnimator.RunSpeed.ToString(), speed);

        }
    }
    public void OnMoveComplete()
    {
        m_CharacterAnimator.SetBool(eAnimator.Run.ToString(), false);
        if(m_TargetPiece!=null)
        {
            transform.DOLookAt(m_TargetPiece.transform.position, .1f);

            m_CharacterAnimator.SetTrigger(eAnimator.Attack.ToString());
            m_Player.KilledEnemyPiece();
            m_TargetPiece.Die();
            transform.DOMove(m_TargetPiece.CurrentTile.transform.position, m_GamePlay.MoveTime).SetDelay(1);

        }

    }
    public void Die()
    {
        IsDead = true;
        m_CharacterAnimator.SetTrigger(eAnimator.Die.ToString());
        m_Collider.enabled = false;
        Invoke(nameof(DeathFx), .5f);
    }
    public void DeathFx()
    {
        //transform.GetChild(0).DOScale(Vector3.zero, .25f).SetEase(Ease.OutQuad);
        if(m_DeathFX!=null)
        {
            m_DeathFX.Play();
        }
        Invoke(nameof(HidePiece), 2.5f);

    }
    public void HidePiece()
    {
        gameObject.SetActive(false);
    }
    public bool TargetPositionIsValid(Tile i_TilePosition, int i_Range)
    {
        return m_PossibleMoves.Contains(i_TilePosition.Position);
    }

    public void Selected(int i_Range)
    {
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
