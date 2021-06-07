using BrokenMugStudioSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerBase
{
    private Camera m_Camera { get { return CameraBehaviour.Instance.MainCamera; } }
    [SerializeField]
    private LayerMask m_Layer;
    public override void OnEnable()
    {
        base.OnEnable();
        InputManager.OnPointerDown += onPointerDown;
        InputManager.OnPointerUp += onPointerUp;

    }
    public override void OnDisable()
    {
        base.OnDisable();

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
    
    public void Raycast()
    {
        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
      
        if (Physics.Raycast(ray, out hit,Mathf.Infinity, m_Layer))
        {
            Debug.DrawRay(m_Camera.transform.position, ray.direction*hit.distance, Color.red);

            if((hit.collider.gameObject.CompareTag(eTags.Tile.ToString())))
            { 
                MovePiece(hit.collider.GetComponent<Tile>());
            }
            else if ((hit.collider.gameObject.CompareTag(eTags.Piece.ToString())))
            {
                SelectPiece(hit.collider.gameObject.GetComponent<Piece>());
            }

        }
       
    }   

    

    
}
