using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour
{
    public void Move(Board_Tile i_BoardTile)
    {
        transform.DOKill();
        transform.DOMove(i_BoardTile.transform.position, 0.2f);
    }

    
}
