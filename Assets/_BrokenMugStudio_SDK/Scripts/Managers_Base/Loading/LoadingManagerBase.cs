using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
#if USING_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
#endif
namespace BrokenMugStudioSDK
{
    public class LoadingManagerBase : Singleton<LoadingManager>
    {
#if USING_ADDRESSABLES
        public bool InstantiateAsync = false;
        [ShowInInspector]
        private Dictionary<AssetReference, LoadingData> m_LoadingQueue;

        [SerializeField]
        private GameObject m_LoadingScreenCanvas;
        [SerializeField]
        private Image m_LoadingBar;
        [SerializeField]
        private TextMeshProUGUI m_LoadingText;
        private bool m_IsLoading = false;
        private float m_Progress;
        [SerializeField]
        private float m_FillSpeed = 1;

        public virtual void PreLoadStuff(AssetReference[] i_Assets)
        {
            if (GameConfig.Instance.Debug.IsDebugMode)
            {
                Debug.LogError("LoadManager>PreLoadStuff");
            }
            if (i_Assets == null || i_Assets.Length < 1)
            {
                return;
            }
            if (m_LoadingQueue == null)
            {
                m_LoadingQueue = new Dictionary<AssetReference, LoadingData>();
            }
            for (int i = 0; i < i_Assets.Length; i++)
            {
                if (!m_LoadingQueue.ContainsKey(i_Assets[i]))
                {

                    m_LoadingQueue.Add(i_Assets[i], new LoadingData(i_Assets[i], false));
                    m_LoadingQueue[i_Assets[i]].Load();
                }

            }
        }
        public virtual void RemoveInstances(AssetReference[] i_Assets)
        {
            if (i_Assets == null)
            {
                return;
            }
            for (int i = 0; i < i_Assets.Length; i++)
            {
                if (m_LoadingQueue.ContainsKey(i_Assets[i]))
                {
                    m_LoadingQueue[i_Assets[i]].RemoveInstances();

                }

            }
        }
        public virtual void Instantiate(AssetReference[] i_Assets)
        {
            if (i_Assets == null)
            {
                return;
            }
            if (m_LoadingQueue == null)
            {
                m_LoadingQueue = new Dictionary<AssetReference, LoadingData>();
            }
            for (int i = 0; i < i_Assets.Length; i++)
            {
                if (m_LoadingQueue.ContainsKey(i_Assets[i]))
                {
                    m_LoadingQueue[i_Assets[i]].InstantiateRef();

                }

            }
        }
        public virtual void Instantiate(AssetReference i_Asset)
        {
            if (i_Asset == null)
            {
                return;
            }
            if (m_LoadingQueue == null)
            {
                m_LoadingQueue = new Dictionary<AssetReference, LoadingData>();
            }
            if (m_LoadingQueue.ContainsKey(i_Asset))
            {
                m_LoadingQueue[i_Asset].InstantiateRef();
            }
        }
        public virtual void Instantiate(AssetReference i_Asset, Vector3 i_Position, Quaternion i_Rotation, Transform i_Parent = null)
        {
            if (m_LoadingQueue == null)
            {
                m_LoadingQueue = new Dictionary<AssetReference, LoadingData>();
            }
            if (i_Asset == null)
            {
                return;
            }
            if (m_LoadingQueue.ContainsKey(i_Asset))
            {
                if (GameConfig.Instance.Debug.IsDebugMode)
                {
                    Debug.LogError("LoadManager>Instantiate>ContainsKey=True");
                }
                m_LoadingQueue[i_Asset].InstantiateRef(i_Position, i_Rotation, i_Parent);

            }
            else
            {
                if (GameConfig.Instance.Debug.IsDebugMode)
                {
                    Debug.LogError("LoadManager>Instantiate>ContainsKey=false");
                }
            }
        }
        public virtual void Instantiate(AssetReference i_Asset, Vector3 i_Position, Quaternion i_Rotation, Action i_Callback, Transform i_Parent = null)
        {
            if (m_LoadingQueue == null)
            {
                m_LoadingQueue = new Dictionary<AssetReference, LoadingData>();
            }
            if (i_Asset == null)
            {
                return;
            }
            if (m_LoadingQueue.ContainsKey(i_Asset))
            {
                m_LoadingQueue[i_Asset].InstantiateRef(i_Position, i_Rotation, i_Parent, i_Callback);

            }
        }
        public virtual void LoadLevel()
        {
            if (GameConfig.Instance.Debug.IsDebugMode)
            {
                Debug.LogError("LoadManager>LoadLevel");
            }
            LoadAndInstantiateLevel(GameConfig.Instance.Levels.CurrentLevel, GameConfig.Instance.Levels.PreviousLevel);
        }
        private void LoadAndInstantiateLevel(AssetReference i_NewLevel, AssetReference i_PreviousLevel)
        {

            if (m_LoadingQueue == null)
            {
                m_LoadingQueue = new Dictionary<AssetReference, LoadingData>();
            }
            if (i_NewLevel == null)
            {
                return;
            }
            if (!m_LoadingQueue.ContainsKey(i_NewLevel))
            {
                m_LoadingQueue.Add(i_NewLevel, new LoadingData(i_NewLevel));
                m_LoadingQueue[i_NewLevel].Load();

            }
            else
            {
                if (m_LoadingQueue[i_NewLevel].IsLoading)
                {
                    m_LoadingQueue[i_NewLevel].InstantiateOnLoad = true;

                }
                else if (m_LoadingQueue[i_NewLevel].IsLoaded && !m_LoadingQueue[i_NewLevel].IsInstanciating)
                {
                    m_LoadingQueue[i_NewLevel].InstantiateRef();
                }
                else if (!m_LoadingQueue[i_NewLevel].IsLoaded)
                {
                    m_LoadingQueue[i_NewLevel].InstantiateOnLoad = true;
                    m_LoadingQueue[i_NewLevel].Load();

                }
            }
            if (i_PreviousLevel != null)
            {
                if (m_LoadingQueue.ContainsKey(i_PreviousLevel))
                {
                    if (GameConfig.Instance.Levels.KeepLevelsLoaded)
                    {
                        m_LoadingQueue[i_PreviousLevel].RemoveInstances();
                    }
                    else
                    {
                        m_LoadingQueue[i_PreviousLevel].RemoveCompletetly();
                        m_LoadingQueue.Remove(i_PreviousLevel);
                    }

                }

            }
        }
        public List<T> GetInstancesComponents<T>(AssetReference i_Asset)
        {
            if (m_LoadingQueue.ContainsKey(i_Asset))
            {
                return m_LoadingQueue[i_Asset].GetInstancesComponents<T>();
            }
            return null;
        }
        public T GetInstanceComponent<T>(AssetReference i_Asset)
        {
            if (m_LoadingQueue.ContainsKey(i_Asset))
            {
                return m_LoadingQueue[i_Asset].GetInstanceComponent<T>();
            }
            return default(T);
        }
        public void AssetLoadedCallBack(LoadingData i_Data)
        {

        }
        private float m_FakeProgress = 0;
        private float m_TargetProgress = 0;

        [SerializeField]
        private float m_FakeProgressTime = 5;
        public virtual void Update()
        {
            if (IsLoadingInProgrees())
            {
                if (!m_IsLoading)
                {
                    m_FakeProgress = 0;
                    LoadingStarts();
                    m_IsLoading = true;
                    m_Progress = 0;
                }
                m_FakeProgress += Time.deltaTime;
                m_FakeProgress = Mathf.Clamp(m_FakeProgress, 0, m_FakeProgressTime);
                m_TargetProgress = (QueueProgrees() * .5f) + (m_FakeProgress / m_FakeProgressTime) * .5f;

                m_Progress = Mathf.Lerp(m_Progress, m_TargetProgress, Time.deltaTime * m_FillSpeed);
                UpdateLoadingBar();
            }
            else
            {
                if (m_IsLoading)
                {
                    m_Progress = Mathf.Lerp(m_Progress, 1f, Time.deltaTime * m_FillSpeed);
                    if (m_Progress > .95f)
                    {
                        AllDone();
                        m_IsLoading = false;
                    }

                }
            }
        }
        private LoadingData m_DummyLoadingData;
        private bool IsLoadingInProgrees()
        {
            if (m_LoadingQueue == null)
            {
                return false;
            }
            if (m_LoadingQueue.Count > 0)
            {
                for (int i = 0; i < m_LoadingQueue.Count; i++)
                {
                    m_DummyLoadingData = m_LoadingQueue.ElementAt(i).Value;
                    if (m_DummyLoadingData.IsLoading || (m_DummyLoadingData.IsInstanciating))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private float QueueProgrees()
        {
            float progress = 0;
            int count = 0;
            if (m_LoadingQueue.Count > 0)
            {
                for (int i = 0; i < m_LoadingQueue.Count; i++)
                {
                    if (m_LoadingQueue.ElementAt(i).Value.IsLoading || m_LoadingQueue.ElementAt(i).Value.IsInstanciating)
                    {
                        count++;
                        if (m_LoadingQueue.ElementAt(i).Value.IsLoading)
                        {
                            progress += m_LoadingQueue.ElementAt(i).Value.LoadHandle.PercentComplete;
                        }
                        else if (m_LoadingQueue.ElementAt(i).Value.IsInstanciating)
                        {
                            progress += m_LoadingQueue.ElementAt(i).Value.InstantiationHandle.PercentComplete;

                        }
                    }
                }
            }

            return (((float)progress / count) + ((float)(m_LoadingQueue.Count - count) / m_LoadingQueue.Count)) / 2f;
        }
        private void LoadingStarts()
        {
            m_LoadingScreenCanvas.SetActive(true);
        }
        private void AllDone()
        {
            m_LoadingBar.DOFillAmount(1, .5f).OnComplete(() => m_LoadingScreenCanvas.SetActive(false));
        }
        private void UpdateLoadingBar()
        {
            m_LoadingBar.fillAmount = m_Progress;
        }

        [Serializable]
        public class LoadingData
        {
            public LoadingData(AssetReference i_AssetRefrence, bool i_InstantiateOnLoad = true, Action i_LoadCallback = null, Action i_InstantiationCallback = null)
            {
                AssetRef = i_AssetRefrence;
                InstantiateOnLoad = i_InstantiateOnLoad;
                LoadCallback = i_LoadCallback;
                InstantiationCallback = i_InstantiationCallback;

            }
            public LoadingData(AssetReference i_AssetRefrence, bool i_InstantiateOnLoad, Vector3 i_Position, Quaternion i_Rotation, Transform i_Parent, Action i_LoadCallback = null, Action i_InstantiationCallback = null)
            {
                AssetRef = i_AssetRefrence;
                InstantiateOnLoad = i_InstantiateOnLoad;
                InstancePos = i_Position;
                InstanceRot = i_Rotation;
                Parent = i_Parent;
                LoadCallback = i_LoadCallback;
                InstantiationCallback = i_InstantiationCallback;

            }
            public AssetReference AssetRef;
            public AsyncOperationHandle<GameObject> LoadHandle;
            public AsyncOperationHandle<GameObject> InstantiationHandle;
            public Action LoadCallback;
            public Action InstantiationCallback;


            public GameObject AssetResult;
            public List<GameObject> AssetInstances;
            public Vector3 InstancePos;
            public Quaternion InstanceRot;
            public Transform Parent;
            public bool InstantiateOnLoad = false;

            public bool IsLoaded = false;
            public bool IsLoading { get { return m_IsLoading && LoadHandle.IsValid(); } }
            private bool m_IsLoading = false;
            public bool IsInstanciating
            {
                get
                {
                    if (LoadingManager.Instance.InstantiateAsync)
                    {
                        return m_IsInstanciating && InstantiationHandle.IsValid();
                    }
                    else
                    {
                        return m_IsInstanciating;
                    }
                }
            }
            private bool m_IsInstanciating = false;

            public bool IsInstanciated = false;


            private float m_LoadStartTime;
            public void Load()
            {
                if (GameConfig.Instance.Debug.IsDebugMode)
                {
                    Debug.LogError("LoadingData>Load>" + AssetRef.AssetGUID);
                }
                m_IsLoading = true;
                m_LoadStartTime = Time.time;
                LoadHandle = Addressables.LoadAssetAsync<GameObject>(AssetRef);
                LoadHandle.Completed += (operation) =>
                {
                    IsLoaded = true;
                    m_IsLoading = false;
                    if (GameConfig.Instance.Debug.IsDebugMode)
                    {
                        Debug.Log(AssetRef.AssetGUID + " Asset Load Time = " + (Time.time - m_LoadStartTime));
                    }
                    AssetResult = operation.Result;
                    if (InstantiateOnLoad)
                    {
                        InstantiateRef();
                    }
                    if (LoadCallback != null)
                    {
                        LoadCallback?.Invoke();
                    }

                };
            }
            public void InstantiateRef(Action i_Callback = null)
            {
                if (GameConfig.Instance.Debug.IsDebugMode)
                {
                    Debug.LogError("LoadingData>InstantiateRef>" + AssetRef.AssetGUID);
                }
                InstantiationCallback = i_Callback;
                if (AssetInstances == null)
                {
                    AssetInstances = new List<GameObject>();
                }
                if (!LoadHandle.IsValid() || !LoadHandle.IsDone)
                {
                    if (GameConfig.Instance.Debug.IsDebugMode)
                    {
                        Debug.LogError(AssetRef.AssetGUID + "-You Are Trying To Instantiate an unloaded Refrence");
                    }
                    return;
                }
                m_LoadStartTime = Time.time;
                m_IsInstanciating = true;
                if (LoadingManager.Instance.InstantiateAsync)
                {
                    InstantiationHandle = AssetRef.InstantiateAsync(InstancePos, InstanceRot, Parent);
                    InstantiationHandle.Completed += (asyncOperationHandle) =>
                    {
                        if (AssetInstances == null)
                        {
                            AssetInstances = new List<GameObject>();
                        }
                        AssetInstances.Add(asyncOperationHandle.Result);
                        IsInstanciated = true;
                        if (GameConfig.Instance.Debug.IsDebugMode)
                        {
                            Debug.Log(AssetRef.AssetGUID + "-Asset InstantiateAsync Time = " + (Time.time - m_LoadStartTime));
                        }
                        if (InstantiationCallback != null)
                        {
                            InstantiationCallback?.Invoke();
                        }
                        m_IsInstanciating = false;


                    };
                }
                else
                {
                    AssetInstances.Add(Instantiate(AssetResult, InstancePos, InstanceRot, Parent));
                    IsInstanciated = true;
                    if (GameConfig.Instance.Debug.IsDebugMode)
                    {
                        Debug.Log(AssetRef.AssetGUID + "-Asset Instantiate Sync Time = " + (Time.time - m_LoadStartTime));
                    }
                    if (InstantiationCallback != null)
                    {
                        InstantiationCallback?.Invoke();
                    }
                    m_IsInstanciating = false;

                }
            }
            public void InstantiateRef(Vector3 i_Position, Quaternion i_Rotation, Transform i_Parent, Action i_Callback = null)
            {
                if (GameConfig.Instance.Debug.IsDebugMode)
                {
                    Debug.LogError("LoadingData>InstantiateRef>" + AssetRef.AssetGUID);
                }
                InstantiationCallback = i_Callback;
                if (AssetInstances == null)
                {
                    AssetInstances = new List<GameObject>();
                }
                if (!LoadHandle.IsValid() || !LoadHandle.IsDone)
                {
                    if (GameConfig.Instance.Debug.IsDebugMode)
                    {
                        Debug.LogError(AssetRef.AssetGUID + "-You Are Trying To Instantiate an unloaded Refrence");

                    }
                    InstancePos = i_Position;
                    InstanceRot = i_Rotation;
                    Parent = i_Parent;
                    InstantiateOnLoad = true;

                    return;
                }
                m_LoadStartTime = Time.time;
                m_IsInstanciating = true;
                if (LoadingManager.Instance.InstantiateAsync)
                {
                    InstantiationHandle = AssetRef.InstantiateAsync(i_Position, i_Rotation, i_Parent);
                    InstantiationHandle.Completed += (asyncOperationHandle) =>
                    {
                        if (AssetInstances == null)
                        {
                            AssetInstances = new List<GameObject>();
                        }
                        AssetInstances.Add(asyncOperationHandle.Result);
                        IsInstanciated = true;
                        if (GameConfig.Instance.Debug.IsDebugMode)
                        {
                            Debug.Log(AssetRef.AssetGUID + "-Asset InstantiateAsync Time = " + (Time.time - m_LoadStartTime));
                        }
                        if (InstantiationCallback != null)
                        {
                            InstantiationCallback?.Invoke();
                        }
                        m_IsInstanciating = false;


                    };
                }
                else
                {
                    AssetInstances.Add(Instantiate(AssetResult, i_Position, i_Rotation, i_Parent));
                    IsInstanciated = true;
                    if (GameConfig.Instance.Debug.IsDebugMode)
                    {
                        Debug.Log(AssetRef.AssetGUID + "-Asset Instantiate Sync Time = " + (Time.time - m_LoadStartTime));
                    }
                    if (InstantiationCallback != null)
                    {
                        InstantiationCallback?.Invoke();
                    }
                    m_IsInstanciating = false;

                }
            }
            public List<T> GetInstancesComponents<T>()
            {
                if (AssetInstances != null)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < AssetInstances.Count; i++)
                    {
                        list.Add(AssetInstances[i].GetComponent<T>());
                    }
                    return list;
                }
                return null;
            }
            public T GetInstanceComponent<T>()
            {
                if (AssetInstances != null && AssetInstances.Count > 0)
                {
                    return AssetInstances[0].GetComponent<T>();
                }
                return default(T);
            }
            public void RemoveInstances()
            {
                if (AssetInstances == null)
                {
                    AssetInstances = new List<GameObject>();
                }
                if (AssetInstances != null)
                {
                    for (int i = 0; i < AssetInstances.Count; i++)
                    {
                        Destroy(AssetInstances[i]);
                        Addressables.ReleaseInstance(AssetInstances[i]);
                    }
                    AssetInstances.Clear();
                    IsInstanciated = false;

                }

            }
            public void RemoveCompletetly()
            {
                if (AssetInstances == null)
                {
                    AssetInstances = new List<GameObject>();
                }
                if (AssetInstances != null)
                {
                    for (int i = 0; i < AssetInstances.Count; i++)
                    {
                        Destroy(AssetInstances[i]);
                        Addressables.ReleaseInstance(AssetInstances[i]);
                    }

                    IsInstanciated = false;

                }

                if (LoadHandle.IsValid())
                {
                    Addressables.Release(LoadHandle);
                    IsLoaded = false;

                }

            }
        }

#endif
    }
}
