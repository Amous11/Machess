#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


namespace BrokenMugStudioSDK
{

    [CustomEditor(typeof(ExtendedButton))]
    public class ExtendedButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ExtendedButton target = (ExtendedButton)this.target;

            if (GUILayout.Button("SetRefs"))
                target.SetRefs();


            EditorGUILayout.LabelField("Debug");

            if (GUILayout.Button("Set Interactable"))
                target.SetInteractable(true);

            if (GUILayout.Button("Set Non Interactable"))
                target.SetInteractable(false);
        }
    }
}
#endif
