﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 1、setBattle-getBattle
/// 2、不同秘境之间是否有交互
/// </summary>
public class MindTreeMapCtrl
{
    public static MindTreeMapCtrl Instance { get; private set; }

    public int MapIdx = 1002100001;
    public Dictionary<int, MindTreeNode> mMapRootDict = new Dictionary<int, MindTreeNode>();  //每个根节点坐标的信息
    public MapData.MapType MyMapType;

    #region 进行处理

    //执行保存节点，将一层的保存节点都合并起来执行，以免特殊情况只保存了一部分
    public bool DoSaveList(List<MindTreeNode> nodeList)
    {
        List<MindTreeNode> setList = new List<MindTreeNode>();
        List<NetMapSaveItem> saveList = new List<NetMapSaveItem>();
        for (int i = 0; i < nodeList.Count; i++)
        {
            setList.Add(nodeList[i]);
        }
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Type == MindTreeNodeType.setEnter || nodeList[i].Type == MindTreeNodeType.setNPC
                || nodeList[i].Type == MindTreeNodeType.setEvent || nodeList[i].Type == MindTreeNodeType.setItem)
            {
                MindTreeNode node = nodeList[i];
                switch (node.Type)
                {
                    case MindTreeNodeType.setEnter:
                        int closeEnterPos = node.TryGetInt(0);
                        if (closeEnterPos >= 0)
                        {
                            saveList.Add(new NetMapSaveItem(DungeonMapAccessor.MapSaveType.EnterClose, 0, closeEnterPos));
                        }
                        else { TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString)); }
                        break;
                    case MindTreeNodeType.setEvent:
                        int evnetId = node.TryGetInt(0);
                        int eventStatus = node.TryGetInt(1);
                        if (evnetId > 0)
                        {
                            saveList.Add(new NetMapSaveItem(DungeonMapAccessor.MapSaveType.Event, evnetId, eventStatus));
                        }
                        else { TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString)); }
                        break;
                    case MindTreeNodeType.setNPC:
                        int npcId = node.TryGetInt(0);
                        int npcStatus = node.TryGetInt(1);
                        OldHero hero = OldHero.HeroFetcher.GetHeroByCopy(npcId);
                        if (hero != null)
                        {
                            saveList.Add(new NetMapSaveItem(DungeonMapAccessor.MapSaveType.Npc, npcId, npcStatus));
                        }
                        else { TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString)); }
                        break;
                    case MindTreeNodeType.setItem:
                        int itemId = node.TryGetInt(0);
                        int itemValue = node.TryGetInt(1);
                        Item item = Item.Fetcher.GetItemCopy(itemId);
                        if (item != null)
                        {
                            saveList.Add(new NetMapSaveItem(DungeonMapAccessor.MapSaveType.Item, itemId, itemValue));
                        }
                        else { TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString)); }
                        break;
                }
                nodeList.RemoveAt(i);
                i--;
            }
        }
        if (saveList.Count > 0)
        {
            //PlayerPrefsBridge.Instance.SetMapSave(saveList, delegate() { DoNodeList(setList); }, MyMapType);
            return true;
        }
        return false;
    }

    public void StartDoList(List<MindTreeNode> doList)
    {
        if (doList == null || doList.Count == 0) return;
        if (!DoSaveList(doList)) //先执行保存性节点
        {
            DoNodeList(doList);
        }
    }

    //遍历执行
    void DoNodeList(List<MindTreeNode> nodeList) 
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            DoNode(nodeList[i]);
        }
    }

    public void DoNode(MindTreeNode node)
    {
        //if (MyMapType == MapData.MapType.SingleMap)
        {
        }
        if (node == null) return;
        TDebug.LogInEditorF("执行 [{0}]", node.CacheString);
        if (DoGetNode(node, DoNode))//如果是get节点，进行执行查询回调,  有处理，则不向下执行
        {
            return;
        }
        if (DoSetNode(node))//有处理，则不向下执行
        {
            return;
        }
        switch (node.Type)
        {
            case MindTreeNodeType.setTalk:  
            {
                Window_Chat setTalkWin = UIRootMgr.Instance.OpenWindow<Window_Chat>(WinName.Window_Chat);
                List<string> idList = new List<string>();
                for (int i = 0; i < node.Values.Count; i++) //获取对话列表
                {
                    idList.Add(node.TryGetString(i));
                }
                System.Action<int> del = delegate(int result)
                {
                    bool haveTalk = CheckLegal_HaveTalkAnyBranch(node.ChildNode);
                    DoNextByResult(node, result);
                };
                setTalkWin.OpenWindow(idList, del);
                break;
            }
            case MindTreeNodeType.condition:
            {
                StartDoList(node.ChildNode);
                break;
            }
            //case MindTreeNodeType.setBattle:
            //{
            //    Window_NpcInteract setBattleNpcWin = UIRootMgr.Instance.GetOpenListWindow<Window_NpcInteract>(WinName.Window_NpcInteract);
            //    Window_DungeonMap setBattleMapWin = UIRootMgr.Instance.GetOpenListWindow<Window_DungeonMap>(WinName.Window_DungeonMap);
            //    if (setBattleNpcWin != null)
            //    {
            //        setBattleNpcWin.OpenBattle(node);
            //    }
            //    else if (setBattleMapWin != null)
            //    {
            //        setBattleMapWin.AppendNewLineDecs("你被偷袭了");
            //        setBattleMapWin.OpenBattle(node);
            //    }
            //    else
            //    {
            //        TDebug.LogError(string.Format("执行{0}时，窗口未打开", node.CacheString));
            //    }
            //    break;
            //}
            //case MindTreeNodeType.setObtain:
            //{
            //    Window_NpcInteract setBattleNpcWin = UIRootMgr.Instance.GetOpenListWindow<Window_NpcInteract>(WinName.Window_NpcInteract);
            //    if (setBattleNpcWin != null)
            //    {
            //        setBattleNpcWin.OpenObtainSpell(node);
            //    }
            //    break;
            //}
            //case MindTreeNodeType.setShop:
            //{
            //    int storeId = node.TryGetInt(0);
            //    string msg  = "";
            //    if (!Shop.GetShopOpen(storeId, out msg) && UIRootMgr.Instance.MessageBox.ShowStatus(msg))
            //        return;
            //    UIRootMgr.Instance.IsLoading = true;
            //    break;
            //}
            //case MindTreeNodeType.modEnter:
            //case MindTreeNodeType.modLeave://暂时不做
            //    break;
            default:
                TDebug.LogError("错误处理的类型" + node.CacheString);
                break;
        }
    }

    //处理set节点，只表现
    private bool DoSetNode(MindTreeNode node)
    {
        switch (node.Type)
        {
            case MindTreeNodeType.setPlace:
            {
                break;
            }
            case MindTreeNodeType.setEnd: //触发结局
            {
                Window_DungeonMap mapWin = UIRootMgr.Instance.GetOpenListWindow<Window_DungeonMap>(WinName.Window_DungeonMap);
                mapWin.SendMapEnd(MapEndType.TriggerEnd, node);
                break;
            }
            case MindTreeNodeType.setEnter:
            {
                int closeEnterPos = node.TryGetInt(0);
                if (closeEnterPos < 0) { TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString)); }
                break;
            }
            case MindTreeNodeType.setEvent:
            {
                int evnetId = node.TryGetInt(0);
                int eventStatus = node.TryGetInt(1);
                if (evnetId <= 0) { TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString)); }
                break;
            }
            case MindTreeNodeType.setNPC:
            {
                int npcId = node.TryGetInt(0);
                int npcStatus = node.TryGetInt(1);
                OldHero hero = OldHero.HeroFetcher.GetHeroByCopy(npcId);
                if (hero != null)
                {
                    Window_DungeonMap setNPCWin = UIRootMgr.Instance.GetOpenListWindow<Window_DungeonMap>(WinName.Window_DungeonMap);
                    if (setNPCWin != null)
                    {
                        string descStr = "";
                        if (npcStatus == (int)NpcStatus.Dead)
                        {
                            descStr = LobbyDialogue.GetDescStr("npcDead", hero.name);
                            setNPCWin.AppendNewLineDecs(descStr);
                        }
                        setNPCWin.FreshNpc();
                    }
                }
                else
                {
                    TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString));
                }
                break;
            }
            case MindTreeNodeType.setItem:
            {
                int itemId = node.TryGetInt(0);
                int itemValue = node.TryGetInt(1);
                Item item = Item.Fetcher.GetItemCopy(itemId);
                if (item != null)
                {
                    Window_DungeonMap setItemWin = UIRootMgr.Instance.GetOpenListWindow<Window_DungeonMap>(WinName.Window_DungeonMap);
                    if (setItemWin != null)
                    {
                        string descStr = "";
                        if (itemValue > 0)
                        {
                            descStr = LobbyDialogue.GetDescStr("addItem", itemValue.ToString(), item.name);
                        }
                        else
                        {
                            descStr = LobbyDialogue.GetDescStr("lostItem", itemValue.ToString(), item.name);
                        }
                        setItemWin.AppendNewLineDecs(descStr);
                    }
                }
                else
                {
                    TDebug.LogError(string.Format("节点信息错误:[{0}]", node.CacheString));
                }
                break;
            }
            case MindTreeNodeType.setMsg:
            {
                TDebug.Log("setMsg");
                Window_DungeonMap setMsgWin = UIRootMgr.Instance.GetOpenListWindow<Window_DungeonMap>(WinName.Window_DungeonMap);

                if (setMsgWin != null)
                {
                    string msgId = node.TryGetString(0);
                    LobbyDialogue dialog = LobbyDialogue.LobbyDialogueFetcher.GetLobbyDialogueByCopy(msgId);
                    if (dialog != null)
                        setMsgWin.AppendNewLineDecs(dialog.ch);
                }
                break;
            }
            case MindTreeNodeType.setDesc:
            {
                break;
            }
            case MindTreeNodeType.setSex:
            {
                int itemValue = node.TryGetInt(0);
                PlayerPrefsBridge.Instance.PartnerAcce.selectSex = (PartnerData.Sex)(itemValue);
                PlayerPrefsBridge.Instance.savePartnerModule();
                break;
            }
            case MindTreeNodeType.setCharac:
            {
                int itemValue = node.TryGetInt(0);
                PlayerPrefsBridge.Instance.PartnerAcce.selectCharacType = (PartnerData.CharacType)(itemValue);
                PlayerPrefsBridge.Instance.savePartnerModule();
                break;
            }
            case MindTreeNodeType.setHairColor:
            {
                int itemValue = node.TryGetInt(0);
                PlayerPrefsBridge.Instance.PartnerAcce.selectHairColor = (PartnerData.HairColor)(itemValue);
                PlayerPrefsBridge.Instance.savePartnerModule();
                break;
            }
            case MindTreeNodeType.setHobby:
            {
                int itemValue = node.TryGetInt(0);
                PlayerPrefsBridge.Instance.PartnerAcce.selectHobbyType = (PartnerData.HobbyType)(itemValue);
                PlayerPrefsBridge.Instance.savePartnerModule();
                break;
            }
            case MindTreeNodeType.setPartner:
            {
                PlayerPrefsBridge.Instance.PartnerAcce.InitCurPartnerBySelect();
                PlayerPrefsBridge.Instance.savePartnerModule();
                break;
            }
            case MindTreeNodeType.setStarRotate:
            {
                LobbySceneMainUIMgr.Instance.ChangeStarRotate();
                break;
            }
            case MindTreeNodeType.setGuideStep:
            {
                int itemValue = node.TryGetInt(0);
                PlayerPrefsBridge.Instance.PlayerData.GuideStepIndex = itemValue;
                PlayerPrefsBridge.Instance.savePlayerModule();
                break;
            }
            default: return false;
        }
        return true;
    }


    //处理get节点
    private bool DoGetNode(MindTreeNode node, System.Action<MindTreeNode> nextCallback)
    {
        if (node.Type.GetBaseType() != MindTreeNodeBaseType.Get)
        {
            //TDebug.LogError(string.Format("此方法只支持get型节点，此节点:[{0}]", node.CacheString));
            return false;
        }
        System.Action<int> resultCallback = delegate(int result)
        {
            DoNextByResult(node, result);
        };
        return GetResult(node, resultCallback);
    }

    private bool GetResult(MindTreeNode node, Action<int> callBack)
    {
        switch (node.Type)
        {
            case MindTreeNodeType.getItem:
                int itemId = node.TryGetInt(0);
                int itemNum = node.TryGetInt(1);
                int curItemNum = 0;
                if (PlayerPrefsBridge.Instance.GetMapSaveCondition(DungeonMapAccessor.MapSaveType.Item, itemId, out curItemNum) &&
                    curItemNum >= itemNum)
                {
                    callBack(1);
                }
                else{ callBack(0); }
                break;
            case MindTreeNodeType.getSpell:
                TDebug.LogError("判断技能，应该服务器验证。需要判断基础id相同的技能？");
                if (PlayerPrefsBridge.Instance.GetSpellByIdx(node.TryGetInt(0))!=null)
                {
                    callBack(0);
                }
                else { callBack(1); }
                break;
            case MindTreeNodeType.getGuideStep:  //只要大于等于就返回1
            {
                int tempMisc = node.TryGetInt(0);
                if (PlayerPrefsBridge.Instance.PlayerData.GuideStepIndex >= tempMisc) { callBack(1); }
                else { callBack(0); }
                break;
            }
            case MindTreeNodeType.getMapLevel:  //只要大于等于就返回1
            {
                int tempMisc = node.TryGetInt(0);
                if (PlayerPrefsBridge.Instance.PlayerData.UnlockMapLevel >= tempMisc) { callBack(1); }
                else { callBack(0); }
                break;
            }
            case MindTreeNodeType.getSex: 
            {
                int tempMisc = (int)PlayerPrefsBridge.Instance.PartnerAcce.selectSex;
                if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectSex == tempMisc) { callBack(tempMisc); }
                else { callBack(tempMisc); }
                break;
            }
            case MindTreeNodeType.getCharac:
            {
                int tempMisc = (int)PlayerPrefsBridge.Instance.PartnerAcce.selectCharacType;
                if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectCharacType == tempMisc) { callBack(tempMisc); }
                else { callBack(tempMisc); }
                break;
            }
            case MindTreeNodeType.getLevel:
            {
                int level = node.TryGetInt(0);
                callBack(PlayerPrefsBridge.Instance.PlayerData.Level >= level ? 1 : 0);
                break;
            }
            case MindTreeNodeType.getAtt:
            {
                AttrType promType = (AttrType)node.TryGetInt(0);
                int promNum = node.TryGetInt(1);
                int num = PlayerPrefsBridge.Instance.PlayerData.GetPromTypeNum(promType);
                callBack(num >= promNum ? 1 : 0);
                break;
            }
            case MindTreeNodeType.getRandom:
                int prop = node.TryGetInt(0);
                if (prop > UnityEngine.Random.Range(0, 10000))
                {
                    callBack(1);
                }
                else { callBack(0); }
                break;
            case MindTreeNodeType.getEvent:
                int eventId = node.TryGetInt(0);
                int needEventStatus = node.TryGetInt(1);
                int curEventStatus = -1; //事件未触发状态为-1
                
                break;
            default:
                TDebug.LogError("错误的类型" + node.Type);
                return false;
        }
        return true;
    }

    #endregion



    #region 获取

    //根据结果，执行下一个节点
    public MindTreeNode DoNextByResult(MindTreeNode node, int result)
    {
        MindTreeNode nextNode = GetNodeByResult(node, result);
        if (nextNode != null)
        {
            DoNode(nextNode);
        }
        return nextNode;
    }


    private MindTreeNode GetNodeByResult(MindTreeNode node, int result)
    {
        for (int i = 0; i < node.ChildNode.Count; i++)
        {
            if (node.ChildNode[i].Type == MindTreeNodeType.condition)
            {
                if (node.ChildNode[i].TryGetInt(0) == result)
                {
                    return node.ChildNode[i];
                }
            }
            else
            {
                TDebug.LogError(string.Format("condition节点有错误的类型，父节点:[{0}]，本节点", node.Type, node.ChildNode[i].Type));
            }
        }
        return null;
    }


    //找到对应的npc
    public MindTreeNode GetNpcNode(int nodePos, int npc)
    {
        if (mMapRootDict.ContainsKey(nodePos) && mMapRootDict[nodePos]!=null)
        {
            List<MindTreeNode> temp = mMapRootDict[nodePos].ChildNode;
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Type == MindTreeNodeType.npc && temp[i].GetNpcId() ==npc)
                {
                    return temp[i];
                }
            }
        }
        return null;
    }

    public void DoMapEnd(MapEndType endType, MindTreeNode node)
    {
        TDebug.LogError("废弃");
        //ServPacketHander hander = delegate(BinaryReader ios)
        //{
        //    UIRootMgr.Instance.IsLoading = false;
        //    NetPacket.S2C_MapEnd msg = MessageBridge.Instance.S2C_MapEnd(ios);
        //    Window_DungeonMap setEndWin = UIRootMgr.Instance.GetOpenListWindow<Window_DungeonMap>(WinName.Window_DungeonMap);
        //    UIRootMgr.LobbyUI.ShowDropInfo(msg.Drops);
        //    if (setEndWin != null)
        //    {
        //        setEndWin.MapEnd(endType, node);
        //    }
        //};
        //if (MyMapType == MapData.MapType.SingleMap)
        //{
        //    int endId = node == null ? 0 : node.TryGetInt(0);
        //    UIRootMgr.Instance.IsLoading = true;
        //    GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MapEnd, hander);
        //    GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_MapEnd(endId));
        //}
        //else
        //{
        //    TDebug.LogError(string.Format("非秘境地图触发了:[{0}]", node == null ? "无节点信息" : node.CacheString));
        //}
    }

    //获取此位置的npcId、npc状态集合
    public Dictionary<int , NpcStatus> GetCanShowNpcs(int nodePos)
    {
        Dictionary<int, NpcStatus> npcDict = new Dictionary<int, NpcStatus>();
        DungeonMapAccessor saveMap =null;
        if (MyMapType == MapData.MapType.SingleMap || MyMapType == MapData.MapType.NewerMap)
        {
            saveMap = PlayerPrefsBridge.Instance.GetDungeonMapCopy();
        }

        if (mMapRootDict.ContainsKey(nodePos) && mMapRootDict[nodePos] != null)
        {
            List<MindTreeNode> npcList = mMapRootDict[nodePos].GetChild(MindTreeNodeType.npc);
            for (int i = 0; i < npcList.Count; i++)
            {
                int npcId = npcList[i].GetNpcId();
                NpcStatus npcStatus = (NpcStatus)npcList[i].TryGetInt(1);
                if (npcId == 0 || npcDict.ContainsKey(npcId))
                {
                    TDebug.LogError(string.Format("地图中有重复的NpcId，pos[{0}]，npcId[{1}]", nodePos, npcId));
                    continue;
                }
                if (saveMap != null)
                {
                    if (saveMap.NpcDic.ContainsKey(npcId))
                    {
                        npcStatus = (NpcStatus)(int)saveMap.NpcDic[npcId];
                    }
                }
                if (npcStatus != NpcStatus.Disable)//添加可显示的
                {
                    npcDict.Add(npcId, npcStatus);
                }
            }
        }
        return npcDict;
    }

    public MindTreeNode GetNodeByPos(int nodePos)
    {
        if (mMapRootDict.ContainsKey(nodePos) && mMapRootDict[nodePos] != null)
        {
            return mMapRootDict[nodePos];
        }
        return null;
    }

    public MindTreeNode GetPosChild(int nodePos, MindTreeNodeType nodeTy)
    {
        if (mMapRootDict.ContainsKey(nodePos) && mMapRootDict[nodePos] != null)
        {
            List<MindTreeNode> temp = mMapRootDict[nodePos].ChildNode;
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Type == nodeTy)
                {
                    return temp[i];
                }
            }
        }
        return null;
    }

    //获取值相同的，子根节点
    public MindTreeNode GetRootGuideStep(int guideStep)
    {
        if (mMapRootDict.ContainsKey(guideStep) && mMapRootDict[guideStep] != null)
        {
            DungeonMapAccessor map = PlayerPrefsBridge.Instance.GetDungeonMapCopy();
            for (int i = 0; i < map.EnterClosePos.Count; i++)
            {
                if (map.EnterClosePos[i] == guideStep) //如果已经关闭，直接返回null
                {
                    return null;
                }
            }
            List<MindTreeNode> temp = mMapRootDict[guideStep].ChildNode;
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Type == MindTreeNodeType.modEnter)
                {
                    return temp[i];
                }
            }
        }
        return null;
    }

    public void DoRootGuideStep(int guideStep)
    {
        if (mMapRootDict.ContainsKey(guideStep) && mMapRootDict[guideStep] != null)
        {
            List<MindTreeNode> temp = mMapRootDict[guideStep].ChildNode;
            DoNodeList(temp);
        }
        return;
    }




    #endregion



    #region 生成树

    public static bool CheckLegal(MindTreeMapCtrl treeMap)
    {
        bool isLegal = true;
        foreach (var item in treeMap.mMapRootDict) //检查是否合法
        {
            if (!CheckLegal(item.Value.ChildNode))
            {
                isLegal = false;
            }
            if (!CheckLegal_SetIsNoChild(item.Value.ChildNode))
            {
                isLegal = false;
            }
            for (int i = 0; i < item.Value.ChildNode.Count; i++)//地图pos层
            {
                for (int j = 0; j < item.Value.ChildNode[i].ChildNode.Count; j++)//npc层
                {
                    if (item.Value.ChildNode[i].ChildNode[j].Type == MindTreeNodeType.modTalk)
                    {
                        if (!CheckLegal_HaveTalkEventBranch(item.Value.ChildNode[i].ChildNode[j].ChildNode))
                        {
                            isLegal = false;
                            int npcId = item.Value.ChildNode[i].GetNpcId();
                            TDebug.LogError(string.Format("modTalk后有分支没有setTalk , Pos[{0}]|Npc[{1}]",
                                item.Value.TryGetInt(0), npcId));
                        }
                    }
                }
            }
            
        }

        return isLegal;
    }

    //检查信息是否合法
    private static bool CheckLegal(List<MindTreeNode> nodeList)
    {
        //一层如果有get，则没有同级节点
        int getNodeCount = 0;
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Type.GetBaseType() == MindTreeNodeBaseType.Get)
            {
                getNodeCount++;
            }
        }
        if (getNodeCount > 0 && nodeList.Count>1)
        {
            StringBuilder error = new StringBuilder();
            for (int i = 0; i < nodeList.Count; i++)
            {
                error.Append(string.Format(" 节点{0}:[{1}]", i, nodeList[i].CacheString));
            }
            TDebug.LogError("一层如果有get，则没有同级节点| " + error );
            return false;
        }

        for (int i = 0; i < nodeList.Count; i++) //递归子节点
        {
            if (nodeList[i].ChildNode.Count > 0)
            {
                if (!CheckLegal(nodeList[i].ChildNode))
                    return false;
            }
        }

        return true;
    }

    //modTalk后面，所有逻辑中间肯定有setTalk
    public static bool CheckLegal_HaveTalkEventBranch(List<MindTreeNode> nodeList)
    {
        bool isLegal = true;
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Type == MindTreeNodeType.setTalk)
            {
                return true;
            }
            else
            {
                if (nodeList[i].ChildNode.Count > 0)
                {
                    if (!CheckLegal_HaveTalkEventBranch(nodeList[i].ChildNode))
                        isLegal = false;
                }
                else { isLegal = false; }
            }
        }
        return isLegal;
    }

    public static bool CheckLegal_HaveTalkAnyBranch(List<MindTreeNode> nodeList)
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Type == MindTreeNodeType.setTalk)
            {
                return true;
            }
            else
            {
                if (nodeList[i].ChildNode.Count > 0)
                {
                    if (CheckLegal_HaveTalkEventBranch(nodeList[i].ChildNode))
                        return true;
                }
            }
        }
        return false;
    }

    //set节点，不能有子节点
    private static bool CheckLegal_SetIsNoChild(List<MindTreeNode> nodeList)
    {
        bool isLegal = true;
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Type.GetBaseType() == MindTreeNodeBaseType.Set)
            {
                if (nodeList[i].ChildNode != null && nodeList[i].ChildNode.Count > 0)
                {
                    TDebug.LogError(string.Format("Set不能有子节点:[{0}] ， 子节点数量:[{1}]", nodeList[i].CacheString, nodeList[i].ChildNode.Count));
                    return false;
                }
            }
            else if (nodeList[i].ChildNode.Count > 0)
            {
                isLegal = CheckLegal_SetIsNoChild(nodeList[i].ChildNode);
            }
        }
        return isLegal;
    }

    public MindTreeMapCtrl(MindTreeMap treeMap, MapData.MapType mapType)
    {
        if (treeMap == null)
        {
            TDebug.LogError(string.Format("树信息为空"));
            return;
        }
        MyMapType = mapType;
        string str = treeMap.content;
        str.Replace("\r", "");
        string[] strs = str.Split('\n');
        List<string> strList = new List<string>(strs);
        if (strList.Count > 0) //获得树id
        {
            MapIdx = strList[0].TryGetInt(0);
            strList.RemoveAt(0);
        }
        for (int i = 0; i < strList.Count; i++) //移除空回车
        {
            int tabCount = YamlParse.GetTabSpaceCount(strList[i]);
            if (tabCount == 0){
                strList.RemoveAt(i);
                i--;
            }
        }

        Dictionary<int, MindTreeNode> nodeDict = new Dictionary<int, MindTreeNode>();
        while (strList.Count > 0)
        {
            string curStr = strList[0];
            strList.RemoveAt(0);
            if (curStr != "")
            {
                int tabCount = YamlParse.GetTabSpaceCount(curStr);
                if (tabCount == 0) continue;
                if (tabCount == 1) //根节点，为地图位置
                {
                    MindTreeNode node = new MindTreeNode(curStr, true, MindTreeNodeType.map, "根节点");
                    int mapPos = node.TryGetInt(0);
                    if (mapPos >= 0 && !nodeDict.ContainsKey(mapPos))
                    {
                        while (strList.Count > 0)
                        {
                            //遍历获得子节点
                            MindTreeNode childNode = MindTreeNode.GetChildNode(node, strList, 1);
                            if (childNode == null) break;
                            else
                            {
                                node.ChildNode.Add(childNode);
                            }
                        }
                        nodeDict.Add(mapPos, node);
                    }
                    else
                    {
                        TDebug.LogError(string.Format("数据错误:{0}", curStr.Replace("	", "")));
                    }
                }
                else
                {
                    TDebug.LogError(string.Format("数据错误:{0}", curStr.Replace("	", "")));
                }
            }
        }
        mMapRootDict = nodeDict;
        Instance = this;
    }
    

    #endregion


}




public enum MapEndType:byte
{
    PlayerExit,
    TriggerEnd,
    Dead
}

public enum TreeMapType:byte
{
    Dungeon,
    Sect
}