using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int Position;
    public bool IsOccupied;
    [SerializeField]
    private GameObject m_PossibleMoveIndicator;
    [SerializeField]
    private GameObject m_OriginalTile;
    private void OnEnable()
    {
        SetRefrences();

    }

    public void HighlightTile(bool i_Highlight)
    {
        m_OriginalTile.SetActive(!i_Highlight);
        m_PossibleMoveIndicator.SetActive(i_Highlight);
    }
    

    public void SetRefrences()
    {
        Position = new Vector3Int(transform.GetSiblingIndex(),0,transform.parent.GetSiblingIndex());
        m_PossibleMoveIndicator = transform.GetChild(1).gameObject;
        m_OriginalTile = transform.GetChild(0).gameObject;
    }


}
