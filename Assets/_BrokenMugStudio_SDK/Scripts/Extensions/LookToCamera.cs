using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenMugStudioSDK.Extentions
{
    public class LookToCamera : MonoBehaviour
    {
        
        void Update()
        {
            transform.forward= -(CameraBehaviour.Instance.MainCamera.transform.position - transform.position).normalized;
            //transform.LookAt((CameraBehaviour.Instance.MainCamera.transform.position - transform.position).normalized);
        }
    }

}
