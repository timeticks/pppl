using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Window_MapDetail : WindowBase
{

    public class ViewObj
    {
        public Button BtnExit;
        public Transform RootNpc;
        public Text TextMapName;
        public GameObject Part_MapNpcItem;
        public TextScrollView BattleScrollView;
        public TextScrollView MapScrollView;

        public ViewObj(UIViewBase view)
        {
            if (BtnExit != null) return;
            BtnExit = view.GetCommon<Button>("BtnExit");
            RootNpc = view.GetCommon<Transform>("RootNpc");
            TextMapName = view.GetCommon<Text>("TextMapName");
            Part_MapNpcItem = view.GetCommon<GameObject>("Part_MapNpcItem");
            BattleScrollView = view.GetCommon<TextScrollView>("BattleScrollView");
            MapScrollView = view.GetCommon<TextScrollView>("MapScrollView");

        }
    }

    private ViewObj mViewObj;
    private int mCurSelectIndex;

    public void OpenWindow(int mapId)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        OpenWin();
        Init(mapId);

    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }
    public void Init(int mapId)
    {
        mCurSelectIndex = 0;
        //mViewObj.SellBtn.SetOnClick(BtnEvt_Sell);
        //mViewObj.UseBtn.SetOnClick(BtnEvt_Use);
    }

    void Fresh(BallMap map)
    {
        mViewObj.TextMapName.text = map.name;

    }

    void BtnEvt_BigMap()
    {
        
    }

}
