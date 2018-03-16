using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadSceneMainUIMgr : BaseMainUIMgr
{
    public Scrollbar m_LoadingBar;
    public override void _Init()
    {
        base.init();
    }
    public void ShowLoadingAsync(AsyncOperation sceneAsync, System.Action<object> del, SceneType sceneType)
    {
        StartCoroutine(ShowLoadingCor(sceneAsync, del, sceneType));
    }
    IEnumerator ShowLoadingCor(AsyncOperation sceneAsync, System.Action<object> del, SceneType sceneType)
    {
        if (sceneAsync != null)//取值范围在0.1 - 1
        {
            while (!sceneAsync.isDone)
            {
                m_LoadingBar.value = Mathf.Max(0f, sceneAsync.progress);
                yield return null;
            }
        }
        del(sceneType);
    }
}
