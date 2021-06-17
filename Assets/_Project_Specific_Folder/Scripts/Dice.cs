using BrokenMugStudioSDK;
using DG.Tweening;
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
    [SerializeField]
    private GameObject m_DiceMesh;
    public bool HasRolledDice;
    private bool m_IsThrown;
    private DiceSettings m_DiceSettings { get => GameConfig.Instance.GamePlay.DiceSettings; }
    [SerializeField]
    private ParticleSystem m_PoofFx;
    private void OnEnable()
    {
        OnTurnEnds();
        m_Rigidbody.maxAngularVelocity = Mathf.Infinity;
        GameManager.OnTurnEnds += OnTurnEnds;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnTurnEnds -= OnTurnEnds;

    }
    public void OnTurnEnds()
    {
        HasRolledDice = false;

    }
#if UNITY_EDITOR
    [Button]
#endif
    public void ThrowDice()
    {
        m_IsThrown = true;
        m_Rigidbody.position = m_ThrowPosition.position;
        m_Rigidbody.angularVelocity = GetRandomVector() * m_DiceSettings.ThrowAngularVelocity;
        m_Rigidbody.AddForce(m_Rigidbody.angularVelocity);
        Invoke(nameof(ShowDice),.05f);

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
        HasRolledDice = true;

        Debug.Log("DiceValue" + GetValue());
        OnDiceStopped?.Invoke(GetValue());
        PopupManager.Instance.ShowPopupText("+" + GetValue() + " AP",transform);
        HideDice();
    }
    public void ShowDice()
    {
        if (m_PoofFx != null)
        {
            m_PoofFx?.Play();
        }
        m_DiceMesh.transform.localScale = Vector3.zero;
        m_DiceMesh.transform.DOScale(Vector3.one, .25f).SetEase(Ease.OutQuad);
    }
    public void HideDice()
    {
        if(m_PoofFx!=null)
        {
            m_PoofFx?.Play();
        }
        m_DiceMesh.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InQuad);

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
