using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 执行行为管理器
/// </summary>
public partial class DoActMgr
{
    ///// <summary>
    ///// 当evt事件发生时，执行act行为
    ///// </summary>
    //public void DoActByEvt(EvtItemData evt, DoActData act)
    //{
    //    System.Action<object> evtDeleg = delegate(object val)
    //    {
    //        DoAct(act);
    //    };
    //    AppEvtMgr.Instance.Register(evt, evtDeleg);
    //}


    ////根据行为信息，进行对应的行为。。注意异常反馈
    //public void DoAct(DoActData actData)
    //{
    //    switch (actData.m_Type)
    //    {
    //        case DoActType.OpenWin:
    //            DoAct_OpenWin(actData);
    //            break;
    //        case DoActType.CloseWin:
    //            break;
    //        case DoActType.ActiveTask:
    //            break;
    //        case DoActType.AcceptTask:
    //            break;
    //        case DoActType.FinishTask:
    //            break;
    //        case DoActType.RejectTask:
    //            break;
    //        case DoActType.ShowChat:
    //            break;
    //        case DoActType.Touch:
    //            break;
    //    }
    //}

    //public void DoTouch(DoActData actData)
    //{
    //    string val = actData.m_Val;
    //    try
    //    {
    //        string funcName = val.Remove(val.IndexOf('['));
    //        string paramVal = val.Replace(funcName, "").Replace("[", "").Replace("]", "");
    //        TouchParamType touchInfo = (TouchParamType)System.Enum.Parse(typeof(TouchParamType), funcName);
    //        Vector2 centerPos;   //canvas子物体的坐标系
    //        Vector2 rectSize;
    //        string[] paramSplit;
    //        Transform findObj = null;
    //        /////////////////////////////////////////TODO: 获取坐标和尺寸和，根据类型执行对应的引导遮罩
    //        switch (touchInfo)
    //        {
    //            case TouchParamType.SizeObj:
    //                findObj = FindObj(paramVal);
    //                if (findObj.GetComponent<RectTransform>() != null) 
    //                { rectSize = findObj.GetComponent<RectTransform>().sizeDelta; }
    //                break;
    //            case TouchParamType.RectObj:
    //                findObj = FindObj(paramVal);
    //                if (findObj.GetComponent<RectTransform>() != null) 
    //                { rectSize = findObj.GetComponent<RectTransform>().sizeDelta; }
    //                centerPos = UIRootMgr.Instance.m_Canvas.transform.InverseTransformPoint(findObj.position);
    //                break;
    //            case TouchParamType.PosObj://都转为Canvas子物体的位置
    //                findObj = FindObj(paramVal);
    //                centerPos = UIRootMgr.Instance.m_Canvas.transform.InverseTransformPoint(findObj.position);
    //                break;
    //            case TouchParamType.SizeBtw:
    //                paramSplit = paramVal.Split(',');
    //                findObj = FindObj(paramSplit[0]);
    //                Transform findObj2 = FindObj(paramSplit[1]);
    //                Vector3 posInRoot1 = UIRootMgr.Instance.m_Canvas.transform.InverseTransformPoint(findObj.position);
    //                Vector3 posInRoot2 = UIRootMgr.Instance.m_Canvas.transform.InverseTransformPoint(findObj2.position);
    //                rectSize = new Vector2(posInRoot1.x - posInRoot2.x, posInRoot1.y - posInRoot2.y);
    //                break;
    //            case TouchParamType.SizeXy:
    //                paramSplit = paramVal.Split(',');
    //                rectSize = new Vector2(int.Parse(paramSplit[0]), int.Parse(paramSplit[1]));
    //                break;
    //            case TouchParamType.PosXy:
    //                paramSplit = paramVal.Split(',');
    //                centerPos = new Vector2(int.Parse(paramSplit[0]), int.Parse(paramSplit[1]));
    //                break;
    //            case TouchParamType.Rect:
    //                paramSplit = paramVal.Split(',');
    //                centerPos = new Vector2(int.Parse(paramSplit[0]), int.Parse(paramSplit[1]));
    //                rectSize = new Vector2(int.Parse(paramSplit[2]), int.Parse(paramSplit[3]));
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    catch (System.Exception e)
    //    {
    //        TDebug.LogError(actData.m_Val + e.Message);
    //        throw;
    //    }
    //}

    //Transform FindObj(string objName , bool debugError = true)
    //{
    //    GameObject obj = GameObject.Find(objName);
    //    if (obj == null) 
    //    {
    //        if (debugError) { TDebug.LogError("没有找到物体" + objName); }
    //        return null; 
    //    }
    //    return obj.transform;
    //}
}

public enum TouchParamType
{
    //SizeObj(Btn_OpenBag)//
    //SizeXy(100,0)//xoffset,yoffset
    //SizeBtw(Btn_Bag,Part_Slot)//两个物体的相差距离
    //PosObj(Btn_OpenBag)
    //PosXy(100,100)
    //Rect(500,500,100,90)//x,y,width,height
    //RectObj(Btn_OpenBag)

    SizeObj, //物体尺寸
    RectObj,
    PosObj,
    SizeBtw, //两个物体的相差距离
    SizeXy,  //xoffset,yoffset
    PosXy,
    Rect,    //x,y,width,height
}