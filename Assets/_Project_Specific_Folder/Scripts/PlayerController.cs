using BrokenMugStudioSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerBase
{
    private Camera m_Camera { get { return CameraBehaviour.Instance.MainCamera; } }
    private void OnEnable()
    {
        InputManager.OnPointerDown += onPointerDown;
        InputManager.OnPointerUp += onPointerUp;

    }
    private void OnDisable()
    {
        InputManager.OnPointerDown -= onPointerDown;
        InputManager.OnPointerUp -= onPointerUp;
    }
    private void onPointerDown()
    {
        Raycast();
    }
    private void onPointerUp()
    {

    }
    //on mouse click in runtime
    public void Raycast()
    {
        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
      
        if ((Physics.Raycast(ray, out hit)))
        {
            if((hit.collider.gameObject.CompareTag(eTags.Tile.ToString())))
            {
                MovePiece(hit.collider.GetComponent<Tile>());
            }
            else if((hit.collider.gameObject.CompareTag(eTags.Piece.ToString())))
            {
                SelectPiece(hit.collider.gameObject.GetComponent<Piece>());
            }
        }
    }   

    

    
}
