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


    public void StartEnterRole()
    {
        mStatus = 1;
    }

    private int mStatus = 0; //加载的状态
    void Update()
    {
        if (mStatus == 1)
        {
            mStatus = 2;
            AppBridge.Instance.LoadSceneAsync(SceneType.LobbyScene);
        }
        else if (mStatus == 2)
        {
            if (AppBridge.Instance.AppScene.SceneAsyncData!=null &&
                !AppBridge.Instance.AppScene.SceneAsyncData.isDone)
            {
                Window_LoadBar.Instance.Fresh(AppBridge.Instance.AppScene.SceneAsyncData.progress, "加载游戏场景");
            }
            else
            {
                UIRootMgr.Instance.Window_LoadBar.SetFalse();
                mStatus = 3;
            }
        }
    }
    

   
}
