using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrokenMugStudioSDK
{
    public class HapticManager : Singleton<HapticManager>
    {
        public void Haptic(HapticTypes i_Haptic, bool defaultToRegularVibrate = false, bool allowVibrationOnLegacyDevices = true)
        {
            if (Managers.Instance != null && StorageManager.Instance.IsVibrationOn)
            {
                if (InputManager.Instance.IsUsingGamePad)
                {
                    MMVibrationManager.Haptic(i_Haptic, defaultToRegularVibrate, true, this);
                }
                else
                {
                    MMVibrationManager.Haptic(i_Haptic, defaultToRegularVibrate, allowVibrationOnLegacyDevices);

                }

            }
        }
    }
}

