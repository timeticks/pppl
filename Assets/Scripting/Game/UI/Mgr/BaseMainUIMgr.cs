using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseMainUIMgr : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> m_RefList = new List<GameObject>();//在主界面被删掉时，删掉关联的物体

    public virtual void _Init() { }
    
    protected void init()
    {
        GetComponent<RectTransform>().offsetMax = Vector2.zero;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
    }

    public virtual void ShowMainUI()//上层窗口关闭，重新显示主界面
    {
    }
}
