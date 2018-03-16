using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Window_UpTips : MonoBehaviour 
{
    public class ViewObj
    {
        public GameObject Part_UpTips;
        public GameObject Part_TipsAchieve;
        public GameObject Part_BannerTips;
        public Transform Root;
        public ViewObj(UIViewBase view)
        {
            Part_UpTips = view.GetCommon<GameObject>("Part_UpTips");
            Part_TipsAchieve = view.GetCommon<GameObject>("Part_TipsAchieve");
            Part_BannerTips = view.GetCommon<GameObject>("Part_BannerTips");
            Root = view.GetCommon<Transform>("Root");        }
    }
    private ViewObj mViewObj;

    private List<Part_UpTips> m_Part_UpTipsList = new List<Part_UpTips>();
    private Part_AchieveTips mPart_AchieveTips;

    private bool showAchieveTip = false;

    public void Init()
    {
        mViewObj = new ViewObj(GetComponent<UIViewBase>());
    }

    public void InitTips(string tips , Color tipsColor, bool isAdd=true)
    {
        if (tips == string.Empty || tips == "") return;
        gameObject.SetActive(true);
        Part_UpTips curTip = GetNextTips(isAdd);
        curTip.Init(tips, tipsColor);
    }



    private Part_UpTips GetNextTips(bool isAdd)
    {
        Part_UpTips curTip = null;
        if (isAdd)
        {
            curTip = TAppUtility.Instance.GetEnableObj<Part_UpTips>(m_Part_UpTipsList, mViewObj.Part_UpTips, mViewObj.Root.transform);
        }
        else
        {
            if (m_Part_UpTipsList.Count < 1)
            {
                curTip = TAppUtility.Instance.GetEnableObj<Part_UpTips>(m_Part_UpTipsList, mViewObj.Part_UpTips, mViewObj.Root.transform);
            }
            curTip = m_Part_UpTipsList[0];
        }
        curTip.gameObject.SetActive(false);
        return curTip;
    }

   
    //private Part_UpTips GetNewTips()
    //{
    //    GameObject g = Instantiate(mViewObj.Part_UpTips) as GameObject;
    //    g.SetActive(false);
    //    Vector3 pos = mViewObj.Root.localPosition;
    //    g.transform.SetParent(mViewObj.Root);
    //    g.transform.localScale = Vector3.one;
    //    g.transform.localPosition = pos;
    //    Part_UpTips p = g.GetComponent<Part_UpTips>();
    //    return p;
    //}

    private Part_AchieveTips GetNewAchieveTips()
    {
        GameObject g = Instantiate(mViewObj.Part_TipsAchieve) as GameObject;
        g.SetActive(false);
        g.transform.SetParent(mViewObj.Root);
        g.transform.localScale = Vector3.one;
        g.transform.localPosition = Vector3.zero;
        Part_AchieveTips p = g.GetComponent<Part_AchieveTips>();
        return p;
    }

}
