using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    private RaycastHit m_DummyHit;
    private Ray m_DummyRay;
    private Piece m_SelectedPiece;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScreenRaycast();
        }
    }

    private void ScreenRaycast()
    {
        m_DummyRay = m_Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(m_DummyRay, out m_DummyHit))
        {
            if (m_DummyHit.collider.gameObject.CompareTag(eTags.Piece.ToString()))
            {
                m_SelectedPiece = m_DummyHit.collider.gameObject.GetComponent<Piece>();
            }
            else if (m_DummyHit.collider.gameObject.CompareTag(eTags.Tile.ToString()))
            {
                if (m_SelectedPiece != null)
                {
                    m_SelectedPiece.Move(m_DummyHit.collider.gameObject.GetComponent<Board_Tile>());
                }
            }
        }
    }
}
