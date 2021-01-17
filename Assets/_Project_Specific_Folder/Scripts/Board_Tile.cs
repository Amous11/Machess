using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class Board_Tile : MonoBehaviour
{
    [SerializeField]
    private Board m_Board;
    public Vector2Int Cordinates;
    [SerializeField]
    private Board_Tile[] m_ConnectedTiles;
    [SerializeField]
    private BlinkAnimation m_BlinkAnimation;
    [SerializeField]
    private bool m_ForceBlink = true;
    private void OnMouseDown()
    {
        OnClickBlink();
    }

    public void OnClickBlink(int i_AffectedDegree=1)
    {
        if(m_BlinkAnimation!=null)
        m_BlinkAnimation.StartAnimation((i_AffectedDegree - 1) * .2f, m_ForceBlink);
        for(int i=0;i< m_ConnectedTiles.Length;i++)
        {
            if(m_ConnectedTiles[i]!=null)
            {
                m_ConnectedTiles[i].Blink(i_AffectedDegree + 1, ( m_ConnectedTiles[i].transform.position- transform.position).normalized);

            }
        }
    }
    public void Blink(int i_AffectedDegree)
    {
        if (i_AffectedDegree > 3) return;
        if (m_BlinkAnimation != null)
        {
            m_BlinkAnimation.StartAnimation((i_AffectedDegree - 1) * .2f, m_ForceBlink);

        }
        for (int i = 0; i < m_ConnectedTiles.Length; i++)
        {
            if (m_ConnectedTiles[i] != null)
            {
                m_ConnectedTiles[i].Blink(i_AffectedDegree + 1);

            }
        }
    }
    public void Blink(int i_AffectedDegree,Vector3 i_EffectDirection)
    {
        //if (i_AffectedDegree > 3) return;
        if (m_BlinkAnimation != null)
        {
            m_BlinkAnimation.StartAnimation((i_AffectedDegree - 1) * .2f, m_ForceBlink);

        }
        for (int i = 0; i < m_ConnectedTiles.Length; i++)
        {
            if (m_ConnectedTiles[i] != null)
            {
                if((m_ConnectedTiles[i].transform.position-transform.position).normalized== i_EffectDirection)
                {
                    m_ConnectedTiles[i].Blink(i_AffectedDegree + 1, i_EffectDirection);

                }

            }
        }
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Selection.activeGameObject==gameObject)
        {
            for (int i=0;i< m_ConnectedTiles.Length;i++)
            {
                if(m_ConnectedTiles[i]!=null)
                Gizmos.DrawLine(transform.position, m_ConnectedTiles[i].transform.position);
            }
                
        }
        
          
    }

    [Button]
    public void EditorEnit(Board i_Board)
    {
        m_Board = i_Board;
        int x = 0;
        if(transform.parent.childCount%2==0 && transform.GetSiblingIndex()>= transform.parent.childCount / 2)
        {
            x = 1;
        }
        Cordinates = new Vector2Int(transform.parent.GetSiblingIndex()- (transform.parent.parent.childCount/2), ((transform.GetSiblingIndex()+ x) - (transform.parent.childCount / 2)));
        gameObject.name = Cordinates.ToString();
        List<Board_Tile>  ConnectedTiles = new List<Board_Tile>();
        if(GetComponent<MeshCollider>()==null)
        {
            gameObject.tag = eTags.Tile.ToString();
            MeshCollider meshCo=gameObject.AddComponent<MeshCollider>();
            meshCo.sharedMesh = GetComponentInChildren<MeshFilter>(true).mesh;
            meshCo.convex = true;
        }
        m_BlinkAnimation = GetComponentInChildren<BlinkAnimation>(true);
        for (int i=0; i<GameConfig.Instance.GamePlay.CastDirection.Length;i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, GameConfig.Instance.GamePlay.CastDirection[i], out hit, 20))
            {
                if(hit.collider.gameObject.CompareTag(eTags.Tile.ToString()))
                {
                    ConnectedTiles.Add(hit.collider.gameObject.GetComponent<Board_Tile>());
                }
            }
            /*
             * if(m_Board.GetTileByCoordonate(Cordinates+ GameConfig.Instance.GamePlay.TileHelperUneven[i])!=null)
            {
                m_ConnectedTiles.Add(m_Board.GetTileByCoordonate(Cordinates + GameConfig.Instance.GamePlay.TileHelperUneven[i]));
            }
            */
        }
        m_ConnectedTiles = ConnectedTiles.ToArray();


    }
#endif
}
