using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_Achieve : WindowBase,IScrollWindow
{

    public class ViewObj
    {
        public GameObject Part_ItemAchieve;
        public Text TextPoint;
        public GameObject Part_ItemAchType;
        public NumScrollTool ProScrollAll;
        public NumScrollTool ProScroll0;
        public NumScrollTool ProScroll1;
        public NumScrollTool ProScroll2;
        public NumScrollTool ProScroll3;
        public NumScrollTool ProScroll4;
        public NumScrollTool ProScroll5;
        public Transform RootItem;
        public GameObject Panel_AchGen;
        public GameObject Panel_AchList;
        public Transform RootItemAch;
        public Transform RootAchBtn;
        public UIScroller Scroller;
        public Button BtnGetAward;
        public List<NumScrollTool> ScrollList;
        public ViewObj(UIViewBase view)
        {       
            if (Part_ItemAchieve == null) Part_ItemAchieve = view.GetCommon<GameObject>("Part_ItemAchieve");
            if (TextPoint == null) TextPoint = view.GetCommon<Text>("TextPoint");
            if (Part_ItemAchType == null) Part_ItemAchType = view.GetCommon<GameObject>("Part_ItemAchType");
            if (ProScrollAll == null) ProScrollAll = view.GetCommon<NumScrollTool>("ProScrollAll");
            if (ProScroll0 == null) ProScroll0 = view.GetCommon<NumScrollTool>("ProScroll0");
            if (ProScroll1 == null) ProScroll1 = view.GetCommon<NumScrollTool>("ProScroll1");
            if (ProScroll2 == null) ProScroll2 = view.GetCommon<NumScrollTool>("ProScroll2");
            if (ProScroll3 == null) ProScroll3 = view.GetCommon<NumScrollTool>("ProScroll3");
            if (ProScroll4 == null) ProScroll4 = view.GetCommon<NumScrollTool>("ProScroll4");
            if (ProScroll5 == null) ProScroll5 = view.GetCommon<NumScrollTool>("ProScroll5");
            if (RootItem == null) RootItem = view.GetCommon<Transform>("RootItem");
            if (Panel_AchGen == null) Panel_AchGen = view.GetCommon<GameObject>("Panel_AchGen");
            if (Panel_AchList == null) Panel_AchList = view.GetCommon<GameObject>("Panel_AchList");
            if (RootItemAch == null) RootItemAch = view.GetCommon<Transform>("RootItemAch");
            if (RootAchBtn == null) RootAchBtn = view.GetCommon<Transform>("RootAchBtn");
            if (Scroller == null) Scroller = view.GetCommon<UIScroller>("Scroller");
            if (BtnGetAward == null) BtnGetAward = view.GetCommon<Button>("BtnGetAward");
            if (ScrollList == null)
            {
                ScrollList = new List<NumScrollTool>();
                ScrollList.Add(ProScrollAll);
                ScrollList.Add(ProScroll0);
                ScrollList.Add(ProScroll1);
                ScrollList.Add(ProScroll2);
                ScrollList.Add(ProScroll3);
                ScrollList.Add(ProScroll4);
                ScrollList.Add(ProScroll5);
            }

        }
        public void ResetSiteScroll()
        {
            Scroller.ScrollView.StopMovement();
            Vector3 tempPos = RootItemAch.localPosition;
            RootItemAch.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
        }
    }
    //成就类型标签Item
    public class AchTypeObj
    {
        public Button BtnAchType;
        public Text TextType;
        public Transform Mark;
        public Transform RootItem;
        public GameObject Part_ItemAchBtn;
        public GameObject Part_ItemAchType;
        public GameObject gameobject;
        public List<Achievement.AchieveSubType> SubTypeList;
        public List<Achievement> AchieveList;
        public List<AchBtnObj> AchBtnList = new List<AchBtnObj>();
        public bool isOpen = false;

        public ContentParentSizeFit contentSizeFit;
        public AchTypeObj(UIViewBase view)
        {
            if (BtnAchType == null) BtnAchType = view.GetCommon<Button>("BtnAchType");
            if (TextType == null) TextType = view.GetCommon<Text>("TextType");
            if (Mark == null) Mark = view.GetCommon<Transform>("Mark");
            if (RootItem == null) RootItem = view.GetCommon<Transform>("RootItem");
            if (Part_ItemAchBtn == null) Part_ItemAchBtn = view.GetCommon<GameObject>("Part_ItemAchBtn");
            if (Part_ItemAchType == null) Part_ItemAchType = view.GetCommon<GameObject>("Part_ItemAchType");
            if (contentSizeFit == null) contentSizeFit = Part_ItemAchType.GetComponent<ContentParentSizeFit>();
            if (gameobject == null) gameobject = view.gameObject;

        }
        /// <summary>
        /// 关闭下拉列表
        /// </summary>
        public void CloseAchItemList()
        {
            isOpen = false;
            Part_ItemAchType.GetComponent<Image>().enabled = false;
            Mark.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            RootItem.gameObject.SetActive(false);
            contentSizeFit.ResetSize();
        }
        public void OpenAchItemList()
        {
            isOpen = true;
            Part_ItemAchType.GetComponent<Image>().enabled = true;
            Mark.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            RootItem.gameObject.SetActive(true);
            contentSizeFit.ResetSize();
        }
       
    }
    //成就按钮Item
    public class AchBtnObj
    {
        public Button BtnAch;
        public Text TextTitle;
        public Sprite Bg_kuang_04;
        public Sprite Bg_kuang_06;
        public GameObject gameobject;
        public Achievement.AchieveSubType SubType;
        public AchBtnObj(UIViewBase view)
        {
            if (BtnAch == null) BtnAch = view.GetCommon<Button>("BtnAch");
            if (TextTitle == null) TextTitle = view.GetCommon<Text>("TextTitle");
            if (Bg_kuang_04 == null) Bg_kuang_04 = view.GetCommon<Sprite>("Bg_kuang_04");
            if (Bg_kuang_06 == null) Bg_kuang_06 = view.GetCommon<Sprite>("Bg_kuang_06");
            if (gameobject == null) gameobject = view.gameObject;
        }
        public void SelectItem(bool select)
        {
            BtnAch.image.overrideSprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BtnAch.image.sprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BtnAch.enabled = !select;
        }
    }

    public class AchtItemObj : ItemIndexObj
    {
        public Text TextName;
        public Text TextDesc;
        public Text TextPoints;
        public GameObject GotMark;

        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (TextDesc == null) TextDesc = view.GetCommon<Text>("TextDesc");
            if (TextPoints == null) TextPoints = view.GetCommon<Text>("TextPoints");
            if (GotMark == null) GotMark = view.GetCommon<GameObject>("GotMark");
        }
        public void InitItem(Achieve ach)
        {
            TextName.text = ach.Name;
            TextDesc.text = ach.Desc;
            TextPoints.text = ach.Point.ToString();
            GotMark.SetActive(ach.IsGot);
        }


    }

    private ViewObj mViewObj;
    private List<AchTypeObj> mAchTypelist = new List<AchTypeObj>();
    private List<AchBtnObj> mAchBtnList = new List<AchBtnObj>();
    private List<AchtItemObj> mRecentAchList = new List<AchtItemObj>();


    private Dictionary<int, AchtItemObj> mAchtItemList = new Dictionary<int, AchtItemObj>();
    private List<Achieve> mAchList = new List<Achieve>();

    private AchTypeObj mSelectTypeObj;//当前选中的成就类型
    private Achievement.AchieveSubType mSelectAchSubType = Achievement.AchieveSubType.None; // 当前选中成就

    public void OpenWindow()
    {
        //gameObject.SetActive(false);
        //if (!AchievementAccessor.IsInitAccessor)
        //{
        //    UIRootMgr.Instance.IsLoading = true;
        //    RegisterNetCodeHandler(NetCode_S.PullInfo, S2C_PullInfo);
        //    GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.Achievement, 0));
        //    return;
        //}
        OpenAchieve();  
    }

    void OpenAchieve()
    {
        gameObject.SetActive(true);
        if (mViewObj == null) { mViewObj = new ViewObj(mViewBase); }
        OpenWin();
        Init();
    }

    public void S2C_PullInfo(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_PullInfo msg = MessageBridge.Instance.S2C_PullInfo(ios);
        OpenAchieve();
    }

    void Init()
    {
        mViewObj.TextPoint.text = string.Format("成就积分：{0}", PlayerPrefsBridge.Instance.GetAchieveAccessor().AchievePoint);
        FreshAchTypeList();
        BtnEvt_AchTypeClick(Achievement.AchieveType.None);
        mViewObj.BtnGetAward.SetOnClick(delegate() { BtnEvt_OpenAchieveReward(); });
    }

    public void Reset()
    {
        mAchtItemList.Clear();
    }
    public void FreshScrollItem(int index)
    {
        if (index > mAchList.Count || mAchList[index] == null)
        {
            TDebug.LogError(string.Format("{ Ach不存在；index:{0}}", index));
            return;
        }
        AchtItemObj item;
        if (mViewObj.Scroller._unUsedQueue.Count > 0)
        {
            item = (AchtItemObj)mViewObj.Scroller._unUsedQueue.Dequeue();
        }
        else
        {
            item = new AchtItemObj();
            item.Init(mViewObj.Scroller.GetNewObj(mViewObj.RootItemAch, mViewObj.Part_ItemAchieve));
        }
        item.Scroller = mViewObj.Scroller;
        item.Index = index;
        mViewObj.Scroller._itemList.Add(item);

        item.InitItem(mAchList[index]);

        if (!mAchtItemList.ContainsKey(index))
            mAchtItemList.Add(index, item);
        else
            mAchtItemList[index] = item;
         
    }

    //成就总览刷新
    void FreshAchGen()
    {
        mViewObj.Panel_AchGen.SetActive(true);
        mViewObj.Panel_AchList.SetActive(false);
        //最近两个完成成就
        List <Achieve> recentAch = PlayerPrefsBridge.Instance.GetAchievementLastFinish();
        //进度总览
        //TAppUtility.Instance.AddViewInstantiate<AchtItemObj>(mRecentAchList, mViewObj.Part_ItemAchieve, mViewObj.RootItem, recentAch.Count);
        for (int i = 0; i < recentAch.Count; i++)
        {
            mRecentAchList[i].InitItem(recentAch[i]);
        }

        for (int i = 0,length = mViewObj.ScrollList.Count; i < length; i++)
        {
            int progress = PlayerPrefsBridge.Instance.GetAchievementProgess((Achievement.AchieveType)i);
            int totalCount = Achievement.AchieveCout[i];
            string percentStr = string.Format("{0}%",(progress*100/(float)totalCount).ToString("f0"));
            if (progress > 0)
               // mViewObj.ScrollList[i].Fresh(Mathf.Max(0.1f, progress / (float)totalCount), percentStr, ((Achievement.AchieveType)i).GetDesc());
                mViewObj.ScrollList[i].Fresh(progress / (float)totalCount, percentStr, ((Achievement.AchieveType)i).GetDesc());
            else
                mViewObj.ScrollList[i].Fresh(0, "0%", ((Achievement.AchieveType)i).GetDesc());
        }
    }

    //点击成就总分类
    public void BtnEvt_AchTypeClick(Achievement.AchieveType type)
    {      
        if (type == Achievement.AchieveType.None) //成就总览
        {
            FreshAchGen();
            if (mSelectTypeObj != null)
            {
                mSelectTypeObj.CloseAchItemList();//关闭上一个展开的下拉列表
                mSelectTypeObj = null;
            }
            mSelectAchSubType = Achievement.AchieveSubType.None;
            mAchTypelist[0].BtnAchType.enabled = false;
            return;
        }  
    
        int index = (int)type;
        AchTypeObj obj = mAchTypelist[index];
        mAchTypelist[0].BtnAchType.enabled = true;
        if (obj.isOpen)//关闭下拉列表
        {
            obj.CloseAchItemList();
            mSelectTypeObj = null;
            return;
        }
        if (mSelectTypeObj != null) mSelectTypeObj.CloseAchItemList();//关闭上一个展开的下拉列表
        mSelectTypeObj = obj;
        for (int i = mAchBtnList.Count; i < obj.SubTypeList.Count; i++)
        {
            GameObject g = Instantiate(obj.Part_ItemAchBtn);
            TUtility.SetParent(g.transform, obj.RootItem, false);
            AchBtnObj item = new AchBtnObj(g.GetComponent<UIViewBase>());
            mAchBtnList.Add(item);
        }
        ////重赋值
        obj.AchBtnList.Clear();
        for (int i = 0, length = mAchBtnList.Count; i < length; i++)
        {
            TUtility.SetParent(mAchBtnList[i].gameobject.transform, obj.RootItem, false);
            mAchBtnList[i].gameobject.transform.SetSiblingIndex(i);
            obj.AchBtnList.Add(mAchBtnList[i]);
        }
        ///初始化
        for (int i = 0, length = obj.SubTypeList.Count; i < length; i++)
        {
            Achievement.AchieveSubType subType = obj.SubTypeList[i];
            obj.AchBtnList[i].gameobject.SetActive(true);
            obj.AchBtnList[i].TextTitle.text = subType.GetDesc();
            obj.AchBtnList[i].SubType = subType;

            obj.AchBtnList[i].BtnAch.SetOnClick(delegate() { BtnEvt_AchBtnClick(subType); });
            mAchBtnList[i].SelectItem(mSelectAchSubType == subType);
        }
        for (int i = obj.SubTypeList.Count; i < obj.AchBtnList.Count; i++)
        {
            obj.AchBtnList[i].gameobject.SetActive(false);
        }
        obj.OpenAchItemList();
        mViewObj.Panel_AchGen.SetActive(false);
        mViewObj.Panel_AchList.SetActive(true);
        if (mSelectAchSubType == Achievement.AchieveSubType.None)
            BtnEvt_AchBtnClick(obj.SubTypeList[0]);
    }
    //点击具体成就
    public void BtnEvt_AchBtnClick(Achievement.AchieveSubType subType)
    {
        if (mSelectAchSubType == subType)
            return;
        Reset();
        mAchList = Achievement.GetAchievementsByCopy(mSelectTypeObj.AchieveList,subType);
        if (mAchList == null)
        {
            TDebug.LogError(string.Format("获取成就信息失败：type:{0}",subType));
            return;
        }
        mAchList.Sort((x, y) =>
        {
            if (x.IsGot || y.IsGot)
            {
                if (x.IsGot && y.IsGot)
                {
                    return x.Order.CompareTo(y.Order);
                }
                return x.IsGot ? 1 : -1;
            }
            else
            {
                return x.Order.CompareTo(y.Order);
            }
        });
        for (int i = 0; i < mAchBtnList.Count; i++)
        {
            mAchBtnList[i].SelectItem(mAchBtnList[i].SubType == subType);
        }
        mSelectAchSubType = subType;
        mViewObj.ResetSiteScroll();
        mViewObj.Scroller.Init(this, mAchList.Count);
    }  
    ///初始化成就类型标签
    void FreshAchTypeList()
    {
        int count = (int)Achievement.AchieveType.Max;
        for (int i = mAchTypelist.Count; i < count; i++)
        {
            GameObject g = Instantiate(mViewObj.Part_ItemAchType);
            TUtility.SetParent(g.transform, mViewObj.RootAchBtn, false);
            AchTypeObj item = new AchTypeObj(g.GetComponent<UIViewBase>());
            mAchTypelist.Add(item);
        }
        for (int i = 0; i < mAchTypelist.Count; i++)
        {
            AchTypeObj obj = mAchTypelist[i];
            Achievement.AchieveType type = (Achievement.AchieveType)i;
            obj.TextType.text = type.GetDesc();
            if (count-1 < i)
            {
                obj.gameobject.SetActive(false);
            }
            else
            {
                obj.gameobject.SetActive(true);
                obj.isOpen = false;
                obj.BtnAchType.SetOnClick(delegate() { BtnEvt_AchTypeClick(type); });
                obj.Mark.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                obj.RootItem.gameObject.SetActive(false);
                obj.contentSizeFit.ResetSize();
                obj.Mark.gameObject.SetActive(type != Achievement.AchieveType.None);
                obj.AchieveList = Achievement.AchievementFetcher.GetAchievementsByCopy(type);
                obj.SubTypeList = new List<Achievement.AchieveSubType>();
                for (int j = 0,length = obj.AchieveList.Count; j < length; j++)
                {
                    if (!obj.SubTypeList.Contains(obj.AchieveList[j].SubType))
                    {
                        obj.SubTypeList.Add(obj.AchieveList[j].SubType);
                    }
                }
            }
        }       
    }


    public void BtnEvt_OpenAchieveReward()
    {
        UIRootMgr.Instance.OpenWindow<Window_AchieveReward>(WinName.Window_AchieveReward, CloseUIEvent.None).OpenWindow();
    }


}
