using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int Position;
    public bool IsOccupied;
    private void OnEnable()
    {
        Position = new Vector3Int(transform.GetSiblingIndex(), 0, transform.parent.GetSiblingIndex());

    }
#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        Position = new Vector3Int(transform.GetSiblingIndex(),0,transform.parent.GetSiblingIndex());
    }


#endif
}
