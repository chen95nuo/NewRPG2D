using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Script.UIManger
{
    #region define

    public enum UIType
    {
        Normal, //常规
        Fixed,  //固定
        PopUp,  //弹出
        None,   //独立的窗口
    }

    public enum UIMode
    {
        DoNothing,
        HideOther,     // 闭其他界面
        NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
        NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
    }

    public enum UICollider
    {
        None,      // 显示该界面不包含碰撞背景
        Normal,    // 碰撞透明背景
        WithBg,    // 碰撞非透明背景
    }
    #endregion
    public class UIPanelManager : TSingleton<UIPanelManager>
    {

        //all pages with the union type
        private  Dictionary<string, TTUIPage> m_allPages;
        public  Dictionary<string, TTUIPage> allPages
        { get { return m_allPages; } }

        //control 1>2>3>4>5 each page close will back show the previus page.
        private  List<TTUIPage> m_currentPageNodes;
        public  List<TTUIPage> currentPageNodes
        { get { return m_currentPageNodes; } }


        //delegate load ui function.
        public  Func<string, Object> delegateSyncLoadUI = null;
        public  Action<string, Action<Object>> delegateAsyncLoadUI = null;

        /// <summary>
        /// Sync Show UI Logic
        /// </summary>
        protected T ShowUIPage<T>(string uiPath, T UIPage, object data) where T : TTUIPage
        {
            //1:instance UI
            T page = UIPage;
            if (page == null && string.IsNullOrEmpty(uiPath) == false)
            {
                if (delegateSyncLoadUI != null)
                {
                    Object o = delegateSyncLoadUI(uiPath);
                    page = o != null ? GameObject.Instantiate(o) as T : null;
                }
                else
                {
                    page = GameObject.Instantiate(Resources.Load(uiPath)) as T;
                }

                //protected.
                if (page == null)
                {
                    Debug.LogError("[UI] Cant sync load your ui prefab.");
                    return null;
                }

                AnchorUIGameObject(page.gameObject);

                //mark this ui sync ui
                page.isAsyncUI = false;
            }
            return page;
        }

        /// <summary>
        /// Async Show UI Logic
        /// </summary>
        protected void ShowUIPage<T>(string uiPath, T UIPage, object data, Action callback) where T : TTUIPage
        {
            TTUIRoot.Instance.StartCoroutine(AsyncShow<T>(uiPath, UIPage, data, callback));
        }

        IEnumerator AsyncShow<T>(string uiPath, T UIPage, object data, Action callback) where T : TTUIPage
        {
            //1:Instance UI
            //FIX:support this is manager multi gameObject,instance by your self.
            T page = UIPage;
            if (page == null && string.IsNullOrEmpty(uiPath) == false)
            {
                bool _loading = true;
                delegateAsyncLoadUI(uiPath, (o) =>
                {
                    page = o != null ? GameObject.Instantiate(o) as T : null;
                    AnchorUIGameObject(page.gameObject);

                    page.isAsyncUI = true;
                    _loading = false;

                    //:refresh ui component.
                    page.Show(data);

                    //:popup this node to top if need back.
                    PopNode(page);

                    if (callback != null) callback();
                });

                float _t0 = Time.realtimeSinceStartup;
                while (_loading)
                {
                    if (Time.realtimeSinceStartup - _t0 >= 10.0f)
                    {
                        Debug.LogError("[UI] WTF async load your ui prefab timeout!");
                        yield break;
                    }
                    yield return null;
                }
            }
            else
            {

                //:refresh ui component.
                page.Show(data);

                //:popup this node to top if need back.
                PopNode(page);

                if (callback != null) callback();
            }
        }



        protected void AnchorUIGameObject(GameObject ui)
        {
            if (TTUIRoot.Instance == null || ui == null) return;

            //check if this is ugui or (ngui)?
            Vector3 anchorPos = Vector3.zero;
            Vector2 sizeDel = Vector2.zero;
            Vector3 scale = Vector3.one;
            if (ui.GetComponent<RectTransform>() != null)
            {
                anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
                sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
                scale = ui.GetComponent<RectTransform>().localScale;
            }
            else
            {
                anchorPos = ui.transform.localPosition;
                scale = ui.transform.localScale;
            }

            //Debug.Log("anchorPos:" + anchorPos + "|sizeDel:" + sizeDel);
            TTUIPage page = ui.GetComponent<TTUIPage>();
            if (page.type == UIType.Fixed)
            {
                ui.transform.SetParent(TTUIRoot.Instance.fixedRoot);
            }
            else if (page.type == UIType.Normal)
            {
                ui.transform.SetParent(TTUIRoot.Instance.normalRoot);
            }
            else if (page.type == UIType.PopUp)
            {
                ui.transform.SetParent(TTUIRoot.Instance.popupRoot);
            }

            if (ui.GetComponent<RectTransform>() != null)
            {
                ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
                ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
                ui.GetComponent<RectTransform>().localScale = scale;
            }
            else
            {
                ui.transform.localPosition = anchorPos;
                ui.transform.localScale = scale;
            }
        }

        #region  api

        private  bool CheckIfNeedBack(TTUIPage page)
        {
            return page != null && page.CheckIfNeedBack();
        }

        /// <summary>
        /// make the target node to the top.
        /// </summary>
        private  void PopNode(TTUIPage page)
        {
            if (m_currentPageNodes == null)
            {
                m_currentPageNodes = new List<TTUIPage>();
            }

            if (page == null)
            {
                Debug.LogError("[UI] page popup is null.");
                return;
            }

            //sub pages should not need back.
            if (CheckIfNeedBack(page) == false)
            {
                return;
            }

            bool _isFound = false;
            for (int i = 0; i < m_currentPageNodes.Count; i++)
            {
                if (m_currentPageNodes[i].Equals(page))
                {
                    m_currentPageNodes.RemoveAt(i);
                    m_currentPageNodes.Add(page);
                    _isFound = true;
                    break;
                }
            }

            //if dont found in old nodes
            //should add in nodelist.
            if (!_isFound)
            {
                m_currentPageNodes.Add(page);
            }

            //after pop should hide the old node if need.
            HideOldNodes();
        }

        private  void HideOldNodes()
        {
            if (m_currentPageNodes.Count < 0) return;
            TTUIPage topPage = m_currentPageNodes[m_currentPageNodes.Count - 1];
            if (topPage.mode == UIMode.HideOther)
            {
                //form bottm to top.
                for (int i = m_currentPageNodes.Count - 2; i >= 0; i--)
                {
                    if (m_currentPageNodes[i].isActive())
                        m_currentPageNodes[i].Hide();
                }
            }
        }

        public  void ClearNodes()
        {
            m_currentPageNodes.Clear();
        }

        private  void ShowPage<T>(string pagePath, Action callback, object pageData, bool isAsync) where T : TTUIPage
        {
            string pageName = pagePath;

            if (m_allPages != null && m_allPages.ContainsKey(pageName))
            {
                ShowPage(pageName, m_allPages[pageName], callback, pageData, isAsync);
            }
            else
            {
                T instance = ShowUIPage<T>(pageName,null, pageData);
                ShowPage(pageName, instance, callback, pageData, isAsync);
            }
        }

        private  void ShowPage(string pageName, TTUIPage pageInstance, Action callback, object pageData, bool isAsync)
        {
            if (string.IsNullOrEmpty(pageName) || pageInstance == null)
            {
                Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
                return;
            }

            if (m_allPages == null)
            {
                m_allPages = new Dictionary<string, TTUIPage>();
            }

            TTUIPage page = null;
            if (m_allPages.ContainsKey(pageName))
            {
                page = m_allPages[pageName];
            }
            else
            {
                m_allPages.Add(pageName, pageInstance);
                page = pageInstance;
            }

            //if active before,wont active again.
            //if (page.isActive() == false)
            {
                //before show should set this data if need. maybe.!!
                if (isAsync)
                    page.Show(pageData, callback);
                else
                    page.Show(pageData);

                PopNode(page);
            }
        }

        /// <summary>
        /// Sync Show Page
        /// </summary>
        public  void ShowPage<T>(string pagePath) where T : TTUIPage
        {
            ShowPage<T>(pagePath, null, null, false);
        }

        /// <summary>
        /// Sync Show Page With Page Data Input.
        /// </summary>
        public  void ShowPage<T>(string pagePath, object pageData) where T : TTUIPage
        {
            ShowPage<T>(pagePath, null, pageData, false);
        }

        public  void ShowPage(string pageName, TTUIPage pageInstance)
        {
            ShowPage(pageName, pageInstance, null, null, false);
        }

        public void ShowPage(string pageName, TTUIPage pageInstance, object pageData)
        {
            ShowPage(pageName, pageInstance, null, pageData, false);
        }

        /// <summary>
        /// Async Show Page with Async loader bind in 'TTUIBind.Bind()'
        /// </summary>
        public  void ShowPage<T>(string pagePath, Action callback) where T : TTUIPage
        {
            ShowPage<T>(pagePath, callback, null, true);
        }

        public  void ShowPage<T>(string pagePath, Action callback, object pageData) where T : TTUIPage
        {
            ShowPage<T>(pagePath, callback, pageData, true);
        }

        /// <summary>
        /// Async Show Page with Async loader bind in 'TTUIBind.Bind()'
        /// </summary>
        public  void ShowPage(string pageName, TTUIPage pageInstance, Action callback)
        {
            ShowPage(pageName, pageInstance, callback, null, true);
        }

        public  void ShowPage(string pageName, TTUIPage pageInstance, Action callback, object pageData)
        {
            ShowPage(pageName, pageInstance, callback, pageData, true);
        }

        /// <summary>
        /// close current page in the "top" node.
        /// </summary>
        public  void ClosePage()
        {
            //Debug.Log("Back&Close PageNodes Count:" + m_currentPageNodes.Count);

            if (m_currentPageNodes == null || m_currentPageNodes.Count <= 1) return;

            TTUIPage closePage = m_currentPageNodes[m_currentPageNodes.Count - 1];
            m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);

            //show older page.
            //TODO:Sub pages.belong to root node.
            if (m_currentPageNodes.Count > 0)
            {
                TTUIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
                if (page.isAsyncUI)
                    ShowPage(page.uiPath, page, () =>
                    {
                        closePage.Hide();
                    });
                else
                {
                    ShowPage(page.uiPath, page);

                    //after show to hide().
                    closePage.Hide();
                }
            }
        }

        /// <summary>
        /// Close target page
        /// </summary>
        public  void ClosePage(TTUIPage target)
        {
            if (target == null) return;
            if (target.isActive() == false)
            {
                if (m_currentPageNodes != null)
                {
                    for (int i = 0; i < m_currentPageNodes.Count; i++)
                    {
                        if (m_currentPageNodes[i] == target)
                        {
                            m_currentPageNodes.RemoveAt(i);
                            break;
                        }
                    }
                    return;
                }
            }

            if (m_currentPageNodes != null && m_currentPageNodes.Count >= 1 && m_currentPageNodes[m_currentPageNodes.Count - 1] == target)
            {
                m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);

                //show older page.
                //TODO:Sub pages.belong to root node.
                if (m_currentPageNodes.Count > 0)
                {
                    TTUIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
                    if (page.isAsyncUI)
                        ShowPage(page.uiPath, page, () =>
                        {
                            target.Hide();
                        });
                    else
                    {
                        ShowPage(page.uiPath, page);
                        target.Hide();
                    }

                    return;
                }
            }
            else if (target.CheckIfNeedBack())
            {
                for (int i = 0; i < m_currentPageNodes.Count; i++)
                {
                    if (m_currentPageNodes[i] == target)
                    {
                        m_currentPageNodes.RemoveAt(i);
                        target.Hide();
                        break;
                    }
                }
            }

            target.Hide();
        }

        public  void ClosePage<T>() where T : TTUIPage
        {
            Type t = typeof(T);
            string pageName = t.ToString();

            if (m_allPages != null && m_allPages.ContainsKey(pageName))
            {
                ClosePage(m_allPages[pageName]);
            }
            else
            {
                Debug.LogError(pageName + "havnt show yet!");
            }
        }

        public  void ClosePage(string pageName)
        {
            if (m_allPages != null && m_allPages.ContainsKey(pageName))
            {
                ClosePage(m_allPages[pageName]);
            }
            else
            {
                Debug.LogError(pageName + " havnt show yet!");
            }
        }

        #endregion

    }
}
