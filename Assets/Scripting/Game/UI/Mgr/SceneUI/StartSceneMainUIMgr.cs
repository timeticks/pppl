using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartSceneMainUIMgr : BaseMainUIMgr 
{
    public class ViewObj
    {
        public ViewObj(UIViewBase view)
        {
        }
    }
    private ViewObj mViewObj;

    public override void _Init()
    {
        base.init();
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        
    }

    public void LoadInitDataOver()
    {
        UIRootMgr.Instance.Window_LoadBar.SetFalse();
    }

    public void StartEnterRole()
    {
        StartCoroutine(LoadBundleAndScene());
    }
    
    IEnumerator LoadBundleAndScene()
    {
        TDebug.Log("这里需要修改");
        yield return null;
        AppBridge.Instance.LoadSceneAsync(SceneType.LobbyScene);
        
        yield return null;

        while (!AppBridge.Instance.AppScene.SceneAsync.isDone)
        {
            UIRootMgr.Instance.Window_LoadBar.Fresh(AppBridge.Instance.AppScene.SceneAsync.progress, "加载游戏场景");
            yield return null;
        }
        LoadInitDataOver();
    }

   
}
