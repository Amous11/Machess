using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapticsToggleButton : MonoBehaviour
{
    [SerializeField]
    private Button m_HapticsButton;
    [SerializeField]
    private GameObject m_HapticsON;
    [SerializeField]
    private GameObject m_HapticsOFF;
    private void OnEnable()
    {
        UpdateVisuals();
        m_HapticsButton.onClick.AddListener(ToggleHaptics);
    }

    private void OnDisable()
    {
        m_HapticsButton.onClick.RemoveAllListeners();

    }

    public void UpdateVisuals()
    {
        m_HapticsON.SetActive(StorageManager.Instance.IsVibrationOn);
        m_HapticsOFF.SetActive(!StorageManager.Instance.IsVibrationOn);

    }
    public void ToggleHaptics()
    {
        StorageManager.Instance.IsVibrationOn = !StorageManager.Instance.IsVibrationOn;
        UpdateVisuals();
    }

}
