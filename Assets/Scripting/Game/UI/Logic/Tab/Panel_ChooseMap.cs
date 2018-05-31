using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using DG.Tweening;
public class Panel_ChooseMap :MonoBehaviour {

    public class ViewObj
    {
        public GameObject Part_StarMapItem;
        public Transform ItemRoot;
        public Text DetailText;
        public TextButton TBtnEnter;
        public GameObject EnterBtnAni;
        public NumScrollTool RecallScrollTool;
        public Button BtnRotate;
        public ViewObj(UIViewBase view)
        {
            if (Part_StarMapItem != null) return;
            Part_StarMapItem = view.GetCommon<GameObject>("Part_StarMapItem");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            DetailText = view.GetCommon<Text>("DetailText");
            TBtnEnter = view.GetCommon<TextButton>("TBtnEnter");
            RecallScrollTool = view.GetCommon<NumScrollTool>("RecallScrollTool");
            EnterBtnAni = view.GetCommon<GameObject>("EnterBtnAni");
            BtnRotate = view.GetCommon<Button>("BtnRotate");
        }
    }

    public class Part_StarMapItem : SmallViewObj
    {
        public int MapIdx;
        public Transform MyTrans;
        public Image StarImage;
        public Button StarBtn;
        public ParticleSystem StarParticle;
        public ParticleSystem LineParticle;
        public ParticleSystem SelectParticle;
        public ParticleSystem UnlcokParticle;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            StarImage = view.GetCommon<Image>("StarImage");
            StarBtn = view.GetCommon<Button>("StarBtn");
            MyTrans = view.transform;
            StarParticle = view.GetCommon<ParticleSystem>("StarParticle");
            LineParticle = view.GetCommon<ParticleSystem>("LineParticle");
            SelectParticle = view.GetCommon<ParticleSystem>("SelectParticle");
            UnlcokParticle = view.GetCommon<ParticleSystem>("UnlcokParticle");
        }
    }


    private ViewObj mViewObj;
    private List<Part_StarMapItem> mStarMapItemList = new List<Part_StarMapItem>();
    private int mCurMapIdx;
    private int mMaxMapLevel;
    private bool mIsRotating=true;//是否在选择

    public void Init()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());

        List<BallMap> mapList = BallMap.Fetcher.GetAllBallMapCopy(BallMap.MapType.Normal);
        mapList.Sort((x, y) => { return x.level.CompareTo(y.level); });

        mMaxMapLevel = mapList[mapList.Count - 1].level;

        TAppUtility.Instance.AddViewInstantiate<Part_StarMapItem>(mStarMapItemList, mViewObj.Part_StarMapItem,
            mViewObj.ItemRoot, mapList.Count);

        mViewObj.BtnRotate.gameObject.SetActive(PlayerPrefsBridge.Instance.PartnerAcce.HavePartner());
        mViewObj.BtnRotate.SetOnAduioClick(BtnEvt_Rotate);

        for (int i = 0; i < mapList.Count; i++)
        {
            BallMap ballMap = mapList[i];
            mStarMapItemList[i].MyTrans.localPosition = new Vector3(ballMap.pos[0],ballMap.pos[1] , ballMap.pos[2]);
            mStarMapItemList[i].MapIdx = ballMap.idx;
            int mapIdx = ballMap.idx;
            int mapLevel = mapList[i].level;
            mStarMapItemList[i].StarBtn.SetOnAduioClick(delegate() { BtnEvt_SelectMap(mapLevel,mapIdx); });

        }
        for (int i = 0; i < mapList.Count; i++)
        {
            mStarMapItemList[i].UnlcokParticle.gameObject.SetActive(false);
            mStarMapItemList[i].StarImage.gameObject.SetActive(true);
            if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel >= i + 1)  //设置点亮和连线
            {
                mStarMapItemList[i].StarParticle.gameObject.SetActive(true);
                if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel >= i + 2
                    || i+1 <= mapList.Count)
                {
                    int nextStarIndex = i + 1;
                    float lineLengthPct = 1f; //连线长度，1为完全连接
                    if (i + 1 == mapList.Count) 
                        nextStarIndex = 0; 
                    else if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel == i + 1)
                    {
                        lineLengthPct = Mathf.Min(1f, PlayerPrefsBridge.Instance.GetItemNum(GameConstUtils.id_item_recall) / (float)mapList[i+1].unlockRecall);
                    }

                    float distance = Vector3.Distance(mStarMapItemList[i].MyTrans.localPosition, mStarMapItemList[nextStarIndex].MyTrans.localPosition);
                    Vector3 lookDir = mStarMapItemList[nextStarIndex].MyTrans.localPosition - mStarMapItemList[i].MyTrans.localPosition;
                    mStarMapItemList[i].LineParticle.transform.localRotation = Quaternion.LookRotation(lookDir);
                    mStarMapItemList[i].LineParticle.gameObject.SetActive(true);
                    //TDebug.LogInEditorF("连线：{0}   {1}   {2}", i, distance, lookDir.ToString());
                    ParticleSystem.MainModule main = mStarMapItemList[i].LineParticle.main;
                    main.startLifetime = new ParticleSystem.MinMaxCurve(distance / 40f * lineLengthPct + 1f);//根据距离，设置其存活长度
                    //var emitParams = new ParticleSystem.EmitParams();
                    //mStarMapItemList[i].LineParticle.Emit(emitParams, 0);
                    mStarMapItemList[i].LineParticle.Play();
                }
                else
                {
                    mStarMapItemList[i].LineParticle.gameObject.SetActive(false);
                }
            }
            else if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel == i) //当前是待解锁的地图
            {
                mStarMapItemList[i].StarParticle.gameObject.SetActive(false);
                mStarMapItemList[i].StarImage.gameObject.SetActive(false);
                mStarMapItemList[i].UnlcokParticle.gameObject.SetActive(true);
            }
            else
            {
                mStarMapItemList[i].StarParticle.gameObject.SetActive(false);
                mStarMapItemList[i].LineParticle.gameObject.SetActive(false);
            }
        }
        if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel > 0)
        {
            int mapIndex = PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel - 1;
            BtnEvt_SelectMap(mapList[mapIndex].level, mapList[mapIndex].idx);
        }
    }

    void Update()
    {
        if (mIsRotating)
        {
            for (int i = 0; i < mStarMapItemList.Count; i++)
            {
                mStarMapItemList[i].StarImage.transform.rotation = Quaternion.identity;
                mStarMapItemList[i].StarBtn.transform.rotation = Quaternion.identity;
            }
        }
    }

    void BtnEvt_SelectMap(int mapLevel , int mapIdx)
    {
        if (mapLevel > PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel+1)
        {
            UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("未开启"), Color.green);
            return;
        }
        mCurMapIdx = mapIdx;
        BallMap map = BallMap.Fetcher.GetBallMapCopy(mapIdx);
        mViewObj.DetailText.text = string.Format("{0}\n{1}",map.name , map.desc);
        mViewObj.EnterBtnAni.gameObject.SetActive(false);

        //选中效果
        for (int i = 0; i < mStarMapItemList.Count; i++)
        {
            mStarMapItemList[i].SelectParticle.gameObject.SetActive(mStarMapItemList[i].MapIdx == mapIdx);
        }

        if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel >= mapLevel)
        {
            mViewObj.RecallScrollTool.gameObject.SetActive(false);
            mViewObj.TBtnEnter.TextBtn.text = LangMgr.GetText("btn_enter");
            mViewObj.TBtnEnter.SetOnAduioClick(BtnEvt_EnterMap);
        }
        else if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel + 1 == mapLevel 
            && PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel < mMaxMapLevel)
        {
            mViewObj.RecallScrollTool.gameObject.SetActive(true);
            mViewObj.TBtnEnter.TextBtn.text = LangMgr.GetText("btn_unlock");
            mViewObj.TBtnEnter.SetOnAduioClick(BtnEvt_OpenMap);
            int itemNum = PlayerPrefsBridge.Instance.GetItemNum(GameConstUtils.id_item_recall);
            mViewObj.RecallScrollTool.Fresh((float)itemNum / (float)map.unlockRecall, string.Format("{0}/{1}", itemNum, map.unlockRecall),
                LangMgr.GetText("碎片进度"));
            if(itemNum>=map.unlockRecall)
                mViewObj.EnterBtnAni.gameObject.SetActive(true);
        }
        else
        {
            mViewObj.RecallScrollTool.gameObject.SetActive(false);
            mViewObj.TBtnEnter.TextBtn.text = LangMgr.GetText("none");
            mViewObj.TBtnEnter.SetOnAduioClick(null);
        }
    }

    void BtnEvt_OpenMap()
    {
        BallMap map = BallMap.Fetcher.GetBallMapCopy(mCurMapIdx);
        if (map.level - PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel != 1
            || PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel >= mMaxMapLevel)
        {
            UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("等级不对"), Color.red);
            return;
        }
        if (!PlayerPrefsBridge.Instance.checkItem(GameConstUtils.id_item_recall, map.unlockRecall, true))
        {
            return;
        }
        PlayerPrefsBridge.Instance.consumeItem(GameConstUtils.id_item_recall, map.unlockRecall, true, "");
        PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel++;
        PlayerPrefsBridge.Instance.savePlayerModule();
        LobbySceneMainUIMgr.Instance.ShowMainUI();  //刷新主界面
    }

    void BtnEvt_EnterMap()
    {
        //清空以前的
        PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx = 0;
        PlayerPrefsBridge.Instance.saveMapAccessor();

        UIRootMgr.Instance.OpenWindow<Window_BallBattle>(WinName.Window_BallBattle).OpenWindow(mCurMapIdx);
    }

    void BtnEvt_Rotate()
    {
        StarRotate(true, !IsRatated);
    }


    public static bool IsRatated
    {
        get{ return PlayerPrefs.GetInt("IsRotatedStar",0)==1;}
        set { PlayerPrefs.SetInt("IsRotatedStar", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public void StarRotate(Vector3 rot)
    {
        mViewObj.ItemRoot.rotation = Quaternion.Euler(mViewObj.ItemRoot.rotation.eulerAngles + rot);
    }
    public void StarRotate(bool doAni , bool isToRotated)
    {
        float useTime = doAni ? 3 : 0f;
        if (isToRotated)
        {
            mViewObj.ItemRoot.DOLocalMove(Vector3.zero, useTime);
            mViewObj.ItemRoot.DOLocalRotate(Vector3.zero, useTime, RotateMode.FastBeyond360);
        }
        else
        {
            //mViewObj.ItemRoot.DOLocalMove(new Vector3(-609, 38, 36), 3);
            //mViewObj.ItemRoot.DOLocalRotate(new Vector3(5f, 67.5f, 4.1f), 3); 
            //mViewObj.ItemRoot.DOLocalMove(new Vector3(-678, 32, 46), useTime);
            mViewObj.ItemRoot.DOLocalMove(Vector3.zero, useTime);
            mViewObj.ItemRoot.DOLocalRotate(new Vector3(9.7f, 79f, -33f), useTime, RotateMode.FastBeyond360);
        }
        IsRatated = isToRotated;
    }

    void OnGUI()
    {
        if (GUILayout.Button("                                               "))
        {
            StarRotate(true, !IsRatated);
        }
    }

}
