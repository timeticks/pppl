using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BadgeStatus
{
    NoOpen,
    ShowBadge,
    Normal
}

public interface IBadgeUI
{
    void FreshBadge();
}

//红点提示
public class BadgeTips : MonoBehaviour
{
    public const string BadgeTag = "BadgeTag";
    public const string NormalTag = "Untagged";


    #region 静态方法
    /// <summary>
    /// 显示新模块特效，如果没有找到NewEffectRoot则显示小红点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="status">状态，0代表消失，1代表显示新模块特效，2代表正常</param>
    public static void ShowNewEffect(Transform parent, int status)
    {
        parent.gameObject.SetActive(true);
        Transform t = parent.Find("NewEffectRoot");
        if (t != null) t.gameObject.SetActive(false);

        if (status == 0)
        {
            parent.gameObject.SetActive(false);
        }
        else if (status == 1)
        {
            if (t != null) t.gameObject.SetActive(true);
            else   //如果没有NewEffectRoot，则显示小红点
            {
                SetBadgeView(parent);
            }
        }
        else
        {
        }
    }


    public static void FreshWindow(WinName winName)
    {
        WindowBase win = UIRootMgr.Instance.GetOpenListWindow(winName);
        if (win != null) win.FreshBadge();
    }

    /// <summary>
    /// 显示红点
    /// </summary>
    public static BadgeTips SetBadgeView(Transform parent, int xOffset = -15, int yOffset = -25)
    {
        if (parent == null) return null;
        BadgeTips temp = parent.gameObject.CheckAddComponent<BadgeTips>();
        temp.Init(xOffset, yOffset);
        return temp;
    }

    void OnBecameInvisible()
    {
        Debug.Log("OnBecameInvisible");
    }


    /// <summary>
    /// 隐藏红点
    /// </summary>
    public static void SetBadgeViewFalse(Transform parent)
    {
        if (!parent.gameObject.CompareTag(BadgeTag))
            return;
        BadgeTips badge = parent.GetComponent<BadgeTips>();
        if (badge != null && badge.mBadge != null)
            badge.Show(false);
    }

    /// <summary>
    /// 设置红点状态
    /// </summary>
    /// <param name="canSetDisable">是否可以隐藏，若为false，则是在外部进行设置其隐藏效果</param>
    public static void FreshByStatus(Transform parent, BadgeStatus status, bool canSetDisable = true)
    {
        if (parent == null) return;
        if (status == BadgeStatus.NoOpen)
        {
            if (canSetDisable)
                parent.gameObject.SetActive(false);
            BadgeTips.SetBadgeViewFalse(parent.transform);  //关闭红点
        }
        else if (status == BadgeStatus.ShowBadge)
        {
            parent.gameObject.SetActive(true);
            BadgeTips.SetBadgeView(parent);
        }
        else
        {
            parent.gameObject.SetActive(true);
            //BadgeTips.SetBadgeViewFlase(parent.transform);//除开每次刷新的开始和事件触发时，不主动隐藏小红点
        }
    }

    //如果Id由相符合的，显示红点
    public static bool CheckIdToBadge(Transform trans, int curId, List<int> badgeIdList)
    {
        if (trans == null) return false;
        BadgeTips.SetBadgeViewFalse(trans);
        for (int i = 0; i < badgeIdList.Count; i++) //显示红点
        {
            if (badgeIdList[i] == curId)
            {
                BadgeTips.SetBadgeView(trans);
                return true;
            }
        }
        return false;
    }
    public static bool CheckIdToBadge(Transform trans, int curId, int badgeId)
    {
        if (trans == null) return false;
        BadgeTips.SetBadgeViewFalse(trans);
        if (badgeId == curId)
        {
            BadgeTips.SetBadgeView(trans);
            return true;
        }
        return false;
    }
    #endregion
    


    private BadgeTipsView mBadge;
    private bool mIsOriginExist;   //是否prefab中就存在红点
    void Init(int xOffset, int yOffset)
    {
        if (mBadge == null)
        {
            Transform originTran = transform.Find("Part_Badge");
            if (originTran != null) mBadge = originTran.GetComponent<BadgeTipsView>();
            if (mBadge == null)
            {
                GameObject g = Instantiate(UIRootMgr.Instance.MyViewObj.Part_Badge) as GameObject;
                mBadge = g.CheckAddComponent<BadgeTipsView>();
                TUtility.SetParent(g.transform, transform);
                mIsOriginExist = false;
            }
            else
            {
                mIsOriginExist = true;
            }
        }
        if (!mIsOriginExist) //当mIsOriginExist为假时，才使用脚本设置的位置
        {
            RectTransform rectTran = transform.GetComponent<RectTransform>();
            mBadge.transform.localPosition = new Vector3(rectTran.rect.width / 2 + xOffset, rectTran.rect.height / 2 + yOffset, 0);//从右上角算起
        }

        Show(true);
    }

    public void ShowFalse()
    {
        Show(false);
    }

    //每次关闭后，自动将红点销毁。只有下次激活后再次调用Init才会打开
    void OnDisable()
    {
        if (mBadge != null && mBadge.gameObject != null)
        {
            Show(false);
        }
    }

    void Show(bool isActive)
    {
        mBadge.gameObject.SetActive(isActive);
        gameObject.tag = isActive ? BadgeTips.BadgeTag : BadgeTips.NormalTag; //也可以用InstanceID来判断
    }
}

