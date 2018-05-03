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
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            StarImage = view.GetCommon<Image>("StarImage");
            StarBtn = view.GetCommon<Button>("StarBtn");
            MyTrans = view.transform;
        }
    }


    private ViewObj mViewObj;
    private List<Part_StarMapItem> mStarMapItemList = new List<Part_StarMapItem>();
    private int mCurMapIdx;

    public void Init()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());

        mViewObj.TBtnEnter.TextBtn.text = LangMgr.GetText("btn_enter");
        mViewObj.TBtnEnter.SetOnAduioClick(BtnEvt_EnterMap);

        List<BallMap> mapList = BallMap.Fetcher.GetAllBallMapCopy(BallMap.MapType.Normal);
        TAppUtility.Instance.AddViewInstantiate<Part_StarMapItem>(mStarMapItemList, mViewObj.Part_StarMapItem,
            mViewObj.ItemRoot, mapList.Count);

        for (int i = 0; i < mapList.Count; i++)
        {
            BallMap ballMap = mapList[i];
            mStarMapItemList[i].MyTrans.localPosition = new Vector3(ballMap.pos[0],ballMap.pos[1] , ballMap.pos[2]);
            mStarMapItemList[i].MapIdx = ballMap.idx;
            int mapIdx = ballMap.idx;
            mStarMapItemList[i].StarBtn.SetOnAduioClick(delegate() { BtnEvt_SelectMap(mapIdx); });
        }
        BtnEvt_SelectMap(mapList[0].idx);
    }

    void BtnEvt_SelectMap(int mapIdx)
    {
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
