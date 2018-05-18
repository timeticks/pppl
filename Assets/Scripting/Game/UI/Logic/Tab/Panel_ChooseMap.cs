using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class Panel_ChooseMap :MonoBehaviour {

    public class ViewObj
    {
        public GameObject Part_StarMapItem;
        public Transform ItemRoot;
        public Text DetailText;
        public TextButton TBtnEnter;
        public ViewObj(UIViewBase view)
        {
            if (Part_StarMapItem != null) return;
            Part_StarMapItem = view.GetCommon<GameObject>("Part_StarMapItem");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            DetailText = view.GetCommon<Text>("DetailText");
            TBtnEnter = view.GetCommon<TextButton>("TBtnEnter");
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
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            StarImage = view.GetCommon<Image>("StarImage");
            StarBtn = view.GetCommon<Button>("StarBtn");
            MyTrans = view.transform;
            StarParticle = view.GetCommon<ParticleSystem>("StarParticle");
            LineParticle = view.GetCommon<ParticleSystem>("LineParticle");
        }
    }


    private ViewObj mViewObj;
    private List<Part_StarMapItem> mStarMapItemList = new List<Part_StarMapItem>();
    private int mCurMapIdx;
    private int mMaxMapLevel;
    public void Init()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());

        mViewObj.TBtnEnter.TextBtn.text = LangMgr.GetText("btn_enter");
        mViewObj.TBtnEnter.SetOnAduioClick(BtnEvt_EnterMap);

        List<BallMap> mapList = BallMap.Fetcher.GetAllBallMapCopy(BallMap.MapType.Normal);
        mapList.Sort((x, y) => { return x.level.CompareTo(y.level); });

        mMaxMapLevel = mapList[mapList.Count - 1].level;

        TAppUtility.Instance.AddViewInstantiate<Part_StarMapItem>(mStarMapItemList, mViewObj.Part_StarMapItem,
            mViewObj.ItemRoot, mapList.Count);
        
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
                        lineLengthPct = Mathf.Min(1f, PlayerPrefsBridge.Instance.PlayerData.RecallUnlockNum/(float)mapList[i].unlockRecall);
                    }

                    float distance = Vector3.Distance(mStarMapItemList[i].MyTrans.localPosition, mStarMapItemList[nextStarIndex].MyTrans.localPosition);
                    Vector3 lookDir = mStarMapItemList[nextStarIndex].MyTrans.localPosition - mStarMapItemList[i].MyTrans.localPosition;
                    mStarMapItemList[i].LineParticle.transform.localRotation = Quaternion.LookRotation(lookDir);
                    mStarMapItemList[i].LineParticle.gameObject.SetActive(true);
                    TDebug.LogInEditorF("连线：{0}   {1}   {2}", i, distance, lookDir.ToString());
                    ParticleSystem.MainModule main = mStarMapItemList[i].LineParticle.main;
                    main.startLifetime = new ParticleSystem.MinMaxCurve(distance / 40f * lineLengthPct + 0.5f);//根据距离，设置其存活长度
                    //var emitParams = new ParticleSystem.EmitParams();
                    //mStarMapItemList[i].LineParticle.Emit(emitParams, 0);
                    mStarMapItemList[i].LineParticle.Play();
                }
                else if (i + 1 < mapList.Count) //正在解锁
                {

                }
                else
                {
                    mStarMapItemList[i].LineParticle.gameObject.SetActive(false);
                }
            }
            else
            {
                mStarMapItemList[i].StarParticle.gameObject.SetActive(false);
                mStarMapItemList[i].LineParticle.gameObject.SetActive(false);
            }
        }
        if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel>0)
            BtnEvt_SelectMap(mapList[0].level, mapList[0].idx);
    }

    void BtnEvt_SelectMap(int mapLevel , int mapIdx)
    {
        if (mapLevel == PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel+1)//下一等级
        {
            UIRootMgr.Instance.OpenWindow<Window_ItemInventory>(WinName.Window_ItemInventory, CloseUIEvent.None).OpenWindow(Item.ItemType.Recall);
            return;
        }
        else if (mapLevel > PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel+1)
        {
            UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("未开启"), Color.green);
            return;
        }
        mCurMapIdx = mapIdx;
        BallMap map = BallMap.Fetcher.GetBallMapCopy(mapIdx);
        mViewObj.DetailText.text = string.Format("{0}\n{1}",map.name , map.desc);
    }

    void BtnEvt_EnterMap()
    {
        //清空以前的
        PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx = 0;
        PlayerPrefsBridge.Instance.saveMapAccessor();

        UIRootMgr.Instance.OpenWindow<Window_BallBattle>(WinName.Window_BallBattle).OpenWindow(mCurMapIdx);
    }





}
