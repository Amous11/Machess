using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Welcome : MenuScreenBase
{
    [SerializeField] private Button m_TapToStart;
    [SerializeField] private Button m_ShopButton;

    [Button]
    private void SetRefs()
    {
        m_TapToStart = transform.GetComponentInChildren<Button>(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_TapToStart.onClick.AddListener(OnTapToStart);
        m_ShopButton.onClick.AddListener(OnShopButton);

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_TapToStart.onClick.RemoveAllListeners();
        m_ShopButton.onClick.RemoveAllListeners();


    }

    public override void Reset()
    {
        base.Reset();
    }

    public void OnShopButton()
    {
        GameManager.Instance.PauseGame();
        MenuManager.Instance.OpenShopScreen();
    }

    private void OnTapToStart()
    {
        Debug.Log(nameof(OnTapToStart));
        GameManager.Instance.StartGame();
        Close();
    }
}
