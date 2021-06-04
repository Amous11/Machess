using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace BrokenMugStudioSDK
{
    public class MenuScreensRefs: MonoBehaviour
    {
        #region Custom Editor
        [BoxGroup("Screens Toggler"), ListDrawerSettings(OnBeginListElementGUI = "BeginGUI", OnEndListElementGUI = "EndGUI", ShowPaging = false)]
        public List<MenuScreenBase> ScreensList = new List<MenuScreenBase>();
        private MenuScreenBase m_Screen;

#if UNITY_EDITOR
        private void BeginGUI(int i_Index)
        {
            GUILayout.BeginHorizontal();
        }

        private void EndGUI(int i_Index)
        {
            if (i_Index < ScreensList.Count && ScreensList[i_Index] != null && ScreensList[i_Index].gameObject != null)
            {
                var icon = ScreensList[i_Index].gameObject.activeSelf ? Sirenix.Utilities.Editor.EditorIcons.Minus : Sirenix.Utilities.Editor.EditorIcons.Plus;

                if (Sirenix.Utilities.Editor.SirenixEditorGUI.ToolbarButton(icon))
                {
                    //HideAllScreens();

                    m_Screen = ScreensList[i_Index];
                    m_Screen.gameObject.SetActive(!m_Screen.gameObject.activeSelf);
                }
            }

            GUILayout.EndHorizontal();
        }
#endif
        [BoxGroup("Screens Toggler")]
        private void HideAllScreens()
        {
            ScreensList.ForEach(x => x.gameObject.SetActive(false));
        }


        [OnInspectorGUI]
        private void reloadScreensList()
        {
            ScreensList.Clear();
            m_DummyMenues = GetComponentsInChildren<MenuScreenBase>(true);

            for (int i = 0; i < m_DummyMenues.Length; i++)
            {
                ScreensList.Add(m_DummyMenues[i]);
            }
        }

        #endregion

        [ReadOnly, ShowInInspector] public Dictionary<string, MenuScreenBase> MenuScreenList = new Dictionary<string, MenuScreenBase>();
        MenuScreenBase[] m_DummyMenues;
        public void Awake()
        {
            //MenuManager.Instance.MenuScreensRefs = this;

            m_DummyMenues = GetComponentsInChildren<MenuScreenBase>(true);
            for(int i=0;i< m_DummyMenues.Length;i++)
            {
                AddScreen(m_DummyMenues[i].GetType().Name, m_DummyMenues[i]);
                if (m_DummyMenues[i].gameObject.activeSelf)
                    Debug.LogFormat("{0} Screen is Opened. Is this correct?", m_DummyMenues[i].name);
            }
                
        }

        public void AddScreen(string i_Name, MenuScreenBase i_MenuScreenBase)
        {
            if (MenuScreenList.ContainsKey(i_Name))
            {
                Debug.LogErrorFormat("Menu Screen already in the game. Please remove duplicates. Menu Name - '{0}' ({1})", i_MenuScreenBase.name, i_Name);
                return;
            }

            MenuScreenList.Add(i_Name, i_MenuScreenBase);
        }
    }
}