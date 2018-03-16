using UnityEngine;
using System.Collections;

//所有场景管理器的基类
public class SceneMgrBase : MonoBehaviour 
{
    public static SceneMgrBase InstanceBase { get; private set; }
    public CameraCtrl2D m_WorldCam;
    public SceneType m_SceneType;

    protected void _Init(SceneMgrBase instance)
    {
        InstanceBase = instance;
        if (UIRootMgr.Instance != null) UIRootMgr.Instance.TopBlackMask = false;
    }

}
