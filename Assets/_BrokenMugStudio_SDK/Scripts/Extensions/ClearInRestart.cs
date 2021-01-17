using UnityEngine;

namespace Hawkeen.Extentions
{
    public class ClearInRestart : MonoBehaviour
    {
        void Start()
        {
            GameManagerBase.OnGameReset += gameRestartCallback;
        }

        private void OnDestroy()
        {
            GameManagerBase.OnGameReset -= gameRestartCallback;
        }

        private void gameRestartCallback()
        {
            ExtensionMethodsV2.RemoveAllChild(transform);
        }
    }
}