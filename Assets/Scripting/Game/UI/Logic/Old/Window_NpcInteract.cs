using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Window_NpcInteract :WindowBase
{
    #region View
    public class ViewObj
    {
        public Button Mask;
        public Text NameText;
        public Text StateText;
        public Text DescText;
        public Transform BtnRoot;
        public Transform InteractRoot;
        public GameObject Part_InteractBtnItem;
        public Transform TalkRoot;
        public Transform TalkBtnRoot;
        public Button TalkMask;
        public Text TalkConditionText;
       // public Panel_ObtainSpell Panel_ObtainSpell;
        public ViewObj(UIViewBase view)
        {
            Mask = view.GetCommon<Button>("Mask");
            NameText = view.GetCommon<Text>("NameText");
            StateText = view.GetCommon<Text>("StateText");
            DescText = view.GetCommon<Text>("DescText");
            BtnRoot = view.GetCommon<Transform>("BtnRoot");
            TalkRoot = view.GetCommon<Transform>("TalkRoot");
            TalkBtnRoot = view.GetCommon<Transform>("TalkBtnRoot");
            InteractRoot = view.GetCommon<Transform>("InteractRoot");
            Part_InteractBtnItem = view.GetCommon<GameObject>("Part_InteractBtnItem");
            TalkMask = view.GetCommon<Button>("TalkMask");
            TalkConditionText = view.GetCommon<Text>("TalkConditionText");
        //    Panel_ObtainSpell = view.GetCommon<Transform>("Panel_ObtainSpell").gameObject.CheckAddComponent<Panel_ObtainSpell>();
        }
    }
    public class InteractBtnObj : SmallViewObj
    {
        public Text NameText;
        public Button Btn;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (Btn == null) Btn = view.GetCommon<Button>("Btn");
        }
    }

    #endregion


    public enum TabType:byte
    {
        Interact,
        Talk,
      //  SpellObtain,
    }

    public TabType CurTab;

    private ViewObj mViewObj;
    private MindTreeNode mNpcNode;
    private List<InteractBtnObj> mBtnList = new List<InteractBtnObj>();
    private List<InteractBtnObj> mTalkBtnList = new List<InteractBtnObj>();
     
    /// <summary>
    /// 如果传空，则沿用上一个npcNode
    /// </summary>
    /// <param name="npcNode"></param>
    public void OpenWindow(MindTreeNode npcNode)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Init(npcNode);
        mViewObj.Mask.SetOnClick(delegate() { CloseWindow(CloseActionType.OpenHide); });
    }
    //显示交互按钮
    private void Init(MindTreeNode npcNode)
    {
        SwitchTab(TabType.Interact);
        if (npcNode != null)
        {
            mNpcNode = npcNode;
        }
        else
        {
            if (mNpcNode == null) TDebug.LogError("mNpcNode == null");
        }
        int npcId = mNpcNode.GetNpcId();
        OldHero npc = OldHero.HeroFetcher.GetHeroByCopy(npcId);  //获取npc的信息，进行显示
        if (npc != null)
        {
            if (npc.Title == "null" || npc.Race == OldHero.RaceType.Thing) mViewObj.NameText.text = string.Format("{0}", npc.name);
            else mViewObj.NameText.text = string.Format("{0}【{1}】", npc.name, npc.Title);
            if (npc.Level == 1 && npc.Race != OldHero.RaceType.Thing)
            {
                mViewObj.StateText.text = "凡人";
            }
            else if (npc.Race == OldHero.RaceType.Thing)
            {
                mViewObj.StateText.text = "";
            }
            else
            {
                HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(npc.Level);
                mViewObj.StateText.text = levelUp == null ? "无" : levelUp.name;
            }
            if (npc.Desc == "null") mViewObj.DescText.text = "";
            else mViewObj.DescText.text = npc.Desc;
        }

        mBtnList = TAppUtility.Instance.AddViewInstantiate<InteractBtnObj>(mBtnList, mViewObj.Part_InteractBtnItem, mViewObj.BtnRoot,
            mNpcNode.ChildNode.Count);
        for (int i = 0; i < mBtnList.Count; i++) //清空之前的回调
        {
            mBtnList[i].Btn.SetOnClick(null);
        }
        for (int i = 0; i < mNpcNode.ChildNode.Count; i++)
        {
            mBtnList[i].View.gameObject.SetActive(true);
            if (mNpcNode.ChildNode[i].Type.GetBaseType() == MindTreeNodeBaseType.InteractMod) //显示按钮
            {
                mBtnList[i].Init(mBtnList[i].View);
                MindTreeNodeType itemTy = mNpcNode.ChildNode[i].Type;
                if (mNpcNode.ChildNode[i].Type == MindTreeNodeType.modTeach)
                {
                    mNpcNode.ChildNode[i].Type = MindTreeNodeType.modObtain;
                }
                mBtnList[i].NameText.text = mNpcNode.ChildNode[i].Type.GetDesc();
                mBtnList[i].Btn.SetOnClick(delegate() { BtnEvt_Interact(itemTy); });
            }
        }
    }

    //切换多mod按钮交互面板与谈话面板
    public void SwitchTab(TabType ty)
    {
        //if (!OpenState)
        //{
        //    OpenWindow(mNpcNode);
        //}
        CurTab = ty;
        mViewObj.DescText.gameObject.SetActive(true);
        mViewObj.InteractRoot.gameObject.SetActive(ty == TabType.Interact);
        mViewObj.TalkRoot.gameObject.SetActive(ty == TabType.Talk);
      //  mViewObj.Panel_ObtainSpell.gameObject.SetActive(ty == TabType.SpellObtain);
        switch (ty)
        {
            case TabType.Interact:
                for (int i = 0; i < mTalkBtnList.Count; i++)
                {
                    mTalkBtnList[i].Btn.SetOnClick(null);
                }
                break;
            case TabType.Talk:
                break;         
        }
    }

    //打开对话界面
    public void OpenTalk(MindTreeNode talkNode)
    {
        SwitchTab(TabType.Talk);
        if (talkNode.Type == MindTreeNodeType.setTalk)
        {
            int dialogId = talkNode.TryGetInt(0);
            if (dialogId > 0)
            {
                Dialog dialog = Dialog.DialogFetcher.GetDialogByCopy(dialogId);
                
                System.Action<int> callBack = delegate(int result) //按钮回调
                {
                    SwitchTab(TabType.Interact);
                    bool haveTalk = MindTreeMapCtrl.CheckLegal_HaveTalkAnyBranch(talkNode.ChildNode);
                    MindTreeNode childNode = MindTreeMapCtrl.Instance.DoNextByResult(talkNode, result);
                    if (childNode == null || !MindTreeMapCtrl.CheckLegal_HaveTalkAnyBranch(childNode.ChildNode))//回调时，如果子节点没有对话了则关闭对话
                    {
                        CloseWindow(CloseActionType.OpenHide);
                    }
                };
                int npcId = mNpcNode.GetNpcId();
                ShowTalk(string.Format("{0}", dialog.Desc), dialog.Button, callBack);
            }
            else
            {
                TDebug.LogError(string.Format("对话信息错误:[{0}]", talkNode.CacheString));
            }
        }
        else
        {
            TDebug.LogError(string.Format("节点类型不符:[{0}]", talkNode.CacheString));
        }
    }

    //显示对话，设置监听
    void ShowTalk(string content , string[] btns , System.Action<int> callBack)
    {
        mViewObj.TalkConditionText.text = content;
        mTalkBtnList = TAppUtility.Instance.AddViewInstantiate<InteractBtnObj>(mTalkBtnList, mViewObj.Part_InteractBtnItem, mViewObj.TalkBtnRoot,
            btns.Length);
        for (int i = 0; i < mTalkBtnList.Count; i++)
        {
            mTalkBtnList[i].Btn.SetOnClick(null);  //清空之前的回调
        }
        if (btns.Length > 0)
        {
            mViewObj.TalkMask.gameObject.SetActive(true);  //如果按钮数量大于0，则不能点击任意地方关闭对话
            mViewObj.TalkMask.SetOnClick(null);
            for (int i = 0; i < btns.Length; i++)
            {
                mTalkBtnList[i].Init(mTalkBtnList[i].View);
                mTalkBtnList[i].NameText.text = btns[i];
                int curIndex = i;
                mTalkBtnList[i].Btn.SetOnClick(delegate() { callBack(curIndex); });
            }
        }
        else
        {
            mViewObj.TalkMask.gameObject.SetActive(true);
            mViewObj.TalkMask.SetOnClick(delegate() { CloseWindow(CloseActionType.OpenHide); });
        }
    }

    //点击决斗、切磋后，开启战斗
    public void OpenBattle(MindTreeNode battleNode)
    {
        int npcId = battleNode.TryGetInt(0);
        SwitchTab(TabType.Interact);

        if (MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.SingleMap || MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.NewerMap)//秘境中
        {
            Window_DungeonMap dungeonMap = UIRootMgr.Instance.GetOpenListWindow<Window_DungeonMap>(WinName.Window_DungeonMap);
            if (dungeonMap != null && dungeonMap.OpenBattle(battleNode))
            {
                CloseWindow(CloseActionType.OpenHide);
            }
            else
            {
                TDebug.LogError(string.Format("错误的setBattle:[{0}]", battleNode.CacheString));
            }
        }
        else if (MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.SectMap)//门派地图中
        {
            BattleType battleType = (BattleType)battleNode.TryGetInt(1);
            System.Action<int> callBack = delegate(int result) //战斗回调
            {
                string descStr = "";
                OldHero hero = OldHero.HeroFetcher.GetHeroByCopy(npcId);
                if (result == 0)
                {
                    descStr = LobbyDialogue.GetDescStr("fightFail", hero.name);
                }
                else
                {
                    descStr = LobbyDialogue.GetDescStr("fightWin", hero.name);
                }
                if (UIRootMgr.LobbyUI != null) UIRootMgr.LobbyUI.AppendTextNewLine(descStr);
            };
            if (npcId > 0 && battleType > 0)
            {
                BattleMgr.Instance.EnterPVE(battleType, npcId, PVESceneType.SectWithNpc, callBack);
            }
            else
            {
                TDebug.LogError(string.Format("错误的setBattle:[{0}]", battleNode.CacheString));
                callBack(0);
            }
            CloseWindow(CloseActionType.OpenHide);
        }
    }

    public void OpenObtainSpell(MindTreeNode obtainSpellNode)
    {
       
        int obtainSpellId = 0;
        if (obtainSpellNode.Type == MindTreeNodeType.setObtain)
        {
            obtainSpellId = obtainSpellNode.TryGetInt(0);
        }
        if (obtainSpellId > 0)
        {
            SpellObtain spellObtain = SpellObtain.SpellObtainFetcher.GetSpellObtainByCopy(obtainSpellId);
            if (spellObtain == null)
            {
                TDebug.LogError(string.Format("不存在的SpellObtain, ID:{0}",obtainSpellId));
                return;
            }
            //UIRootMgr.Instance.OpenWindowWithHide<Window_ObtainSpell>(WinName.Window_ObtainSpell,WinName.Window_NpcInteract).OpenWindow(obtainSpellId);
        }    
    }

    //点击交谈、搜索、切磋等交互按钮。执行其子节点内容
    public void BtnEvt_Interact(MindTreeNodeType ty)
    {
        //TDebug.Log("点击交互按钮" + ty);
        MindTreeNode node = null;
        for (int i = 0; i < mNpcNode.ChildNode.Count; i++)
        {
            if (mNpcNode.ChildNode[i].Type == ty)
            {
                node = mNpcNode.ChildNode[i];
                break;
            }
        }
        if (node != null)
        {
            MindTreeMapCtrl.Instance.StartDoList(node.ChildNode);
        }
        else
        {
            TDebug.LogError(string.Format("错误的交互类型:[{0}] ", ty));
        }
    }



    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    void OnDestroy()
    {
        for (int i = 0; i < mBtnList.Count; i++)
        {
            mBtnList[i].Btn.SetOnClick(null);
        }
        for (int i = 0; i < mTalkBtnList.Count; i++)
        {
            mTalkBtnList[i].Btn.SetOnClick(null);
        }
    }
}
