using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class Window_ChooseRole : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_RoleSaveItem;
        public Text SexChooseText;
        public GameObject SaveItemRoot;     
        public ViewObj(UIViewBase view)
        {
            Part_RoleSaveItem = view.GetCommon<GameObject>("Part_RoleSaveItem"); 
            SexChooseText = view.GetCommon<Text>("SexChooseText");
            SaveItemRoot = view.GetCommon<GameObject>("SaveItemRoot"); 
        }
    }
    private ViewObj mViewObj;
    public class RoleSaveItemObj
    {
        public Button BtnSelect;
        public Text TextName;
        public Text TextSect;
        public Text TextYearOld;
        public Text TextLevel;
        public GameObject NullRoot;
        public Text TextNull;
        public NetMiniPlayer MyRole;
        public GameObject gameobject;
        public Button BtnRemove;
        public RoleSaveItemObj(UIViewBase view)
        {
            BtnSelect = view.GetCommon<Button>("BtnSelect");
            TextName = view.GetCommon<Text>("TextName");
            TextSect = view.GetCommon<Text>("TextSect");
            TextYearOld = view.GetCommon<Text>("TextYearOld");
            TextLevel = view.GetCommon<Text>("TextLevel");
            NullRoot = view.GetCommon<GameObject>("NullRoot");
            TextNull = view.GetCommon<Text>("TextNull");
            BtnRemove = view.GetCommon<Button>("BtnRemove");
            gameobject = view.gameObject;
        }
        public void Init(NetMiniPlayer role)
        {          
            MyRole = role;      
            if (MyRole.idx == 0)
            {
                NullRoot.gameObject.SetActive(true);
                TextNull.text = LangMgr.GetText("创建新角色");
            }
            else
            {
                long curTime = AppTimer.CurTimeStampMsSecond;
                long birthTime = MyRole.BirthSecond;
                long LoginTime = (curTime - birthTime) / 1000 / 24 / 60 / 60;

                NullRoot.gameObject.SetActive(false);
                TextName.text = LangMgr.GetText("姓名") + ": " + MyRole.name;
                HeroLevelUp level = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(role.Level);
                TextLevel.text = LangMgr.GetText("境界") + ": " + level.name;
                TextSect.text = LangMgr.GetText("门派") + ": " + Sect.GetSectName((Sect.SectType)MyRole.SectId);
                TextYearOld.text = LangMgr.GetText("年龄") + ": " + (LoginTime+16)+"岁";
                TDebug.Log("英雄信息,idx:" + MyRole.idx + "|name:" + MyRole.name);
            }          
        }
    }
    private RoleSaveItemObj[] mRoleSaveList = new RoleSaveItemObj[3];
    public void OpenWindow(NetMiniPlayer[] roleDataList)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init(roleDataList);
    }

    private void Init(NetMiniPlayer[] roleDataList)
    {
        for (int i = 0, length = mRoleSaveList.Length; i < length; i++)
        {
            GameObject g = Instantiate(mViewObj.Part_RoleSaveItem) as GameObject;
            TUtility.SetParent(g.transform, mViewObj.SaveItemRoot.transform, false, true);
            mRoleSaveList[i] = new RoleSaveItemObj(g.GetComponent<UIViewBase>());
            mRoleSaveList[i].gameobject.SetActive(true);
            mRoleSaveList[i].Init(roleDataList[i]);
            int idx = roleDataList[i].idx;
            mRoleSaveList[i].BtnSelect.SetOnClick(delegate() { ChooseRoleSave(idx); });
        }
        RegisterNetCodeHandler(NetCode_S.EnterGame, S2C_EnterRole);  //进行注册
    }

    public void RemoveRoleSave(int roldID)
    {
        if (roldID == 0) return;
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_RemoveRoleSave(roldID));   
    }
    public void S2C_RemoveSaveRole(BinaryReader ios)
    {
        NetPacket.S2C_RemoveRoleSave msg = MessageBridge.Instance.S2C_RemoveRoleSave(ios);
        UIRootMgr.Instance.IsLoading = false;
    }



    public void ChooseRoleSave(int idx)//选择了某个存档
    {

    }
    public void S2C_EnterRole(BinaryReader ios)
    {
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.EnterGame, null); //移除注册
        NetPacket.S2C_EnterGame msg = MessageBridge.Instance.S2C_EnterGame(ios);
        //UIRootMgr.Instance.IsLoading = false;
        TDebug.Log("等待所有snapshot完成之后关闭");//TODO:
        CloseWindow();
        UIRootMgr.StartUI.StartEnterRole();
    }

}
