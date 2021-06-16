using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : Singleton<Dice>
{
    public delegate void DiceEvent(int Result);
    public static event DiceEvent OnDiceStopped = delegate {};
    [SerializeField]
    private Transform m_ThrowPosition;
    [SerializeField]
    private Rigidbody m_Rigidbody;
    [SerializeField]
    private Transform m_DiceSides;
    private bool m_IsThrown;
    private DiceSettings m_DiceSettings { get => GameConfig.Instance.GamePlay.DiceSettings; }
#if UNITY_EDITOR
    [Button]
#endif
    public void ThrowDice()
    {
        m_IsThrown = true;
        m_Rigidbody.position = m_ThrowPosition.position;
        m_Rigidbody.angularVelocity = GetRandomVector() * m_DiceSettings.ThrowAngularVelocity;
        m_Rigidbody.AddForce(m_Rigidbody.angularVelocity);
    }

    private void FixedUpdate()
    {
        if(m_IsThrown)
        {
            if((m_Rigidbody.angularVelocity+m_Rigidbody.velocity).sqrMagnitude == 0f)
            {
                StopDice();
            }
        }
    }
    public void StopDice()
    {
        m_IsThrown = false;
        m_Rigidbody.angularVelocity = Vector3.zero;

        Debug.Log("DiceValue" + GetValue());
        OnDiceStopped?.Invoke(GetValue());

        gameObject.SetActive(false);
    }
    public int GetValue()
    {
        int closestValue = 1;
        float highestY = -999;
        for (int i = 0; i < m_DiceSides.childCount; i++)
        {
            if (m_DiceSides.GetChild(i).position.y > highestY)
            {
                highestY = m_DiceSides.GetChild(i).position.y;
                closestValue = i + 1;
            }
        }


        return closestValue;
    }
    public Vector3 GetRandomVector()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

#if UNITY_EDITOR
    [Button]
    public void SetRefrences()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_DiceSides = transform.GetChild(1);
    }
#endif
}
