using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MindTreeNode
{
    public string CacheString;  //为便于差错，将节点的原字符串储存
    public MindTreeNodeType Type;
    public List<string> Values = new List<string>();
    public List<MindTreeNode> ChildNode = new List<MindTreeNode>();


    public static MindTreeNode GetChildNode(MindTreeNode parentNode, List<string> strList, int lastTabCount)
    {
        if (strList == null || strList.Count == 0 || parentNode == null)
        {
            return null;
        }
        int curTabCount = YamlParse.GetTabSpaceCount(strList[0]);
        if (curTabCount > lastTabCount) //如果tab数量更多，则是子节点
        {
            string curStr = strList[0];
            strList.RemoveAt(0);
            MindTreeNode node = new MindTreeNode(curStr, false, parentNode.Type, parentNode == null ? "" : parentNode.CacheString);
            while (strList.Count > 0)
            {
                //遍历获得子节点
                MindTreeNode childNode = GetChildNode(node, strList, curTabCount);
                if (childNode == null) break;
                else
                {
                    node.ChildNode.Add(childNode);
                }
            }
            return node;
        }
        else
        {
            return null;
        }
    }

    public MindTreeNode(string str, bool isRoot, MindTreeNodeType parentType , string parentStr="")
    {
        str = YamlParse.RemoveTab(str);
        CacheString = str;
        if (isRoot)
        {
            Type = MindTreeNodeType.map;
            Values.Add(str);
        }
        else if (parentType.GetBaseType() == MindTreeNodeBaseType.Get || parentType == MindTreeNodeType.setBattle || parentType == MindTreeNodeType.setTalk) //后面肯定接条件
        {
            if (str.TryGetInt(-1) != -1)
            {
                Type = MindTreeNodeType.condition;
                Values.Add(str);
            }
            else
            {
                TDebug.LogError(string.Format("秘境节点条件参数错误，后面应该接条件节点[{0}],父节点[{1}]", str, parentStr));
            }
        }
        else
        {
            string[] strs = str.Split(',');

            if (strs.Length > 0 && strs[0] != "") //第一个参数为类型
            {
                Type = (MindTreeNodeType)System.Enum.Parse(typeof(MindTreeNodeType), strs[0]);
                if (Type == MindTreeNodeType.none)
                {
                    TDebug.LogError(string.Format("秘境节点字符串错误[{0}],内容[{1}],父节点[{2}]", strs[0], CacheString, parentStr));
                }
            }
            else
            {
                TDebug.LogError(string.Format("秘境节点字符串错误[{0}],内容[{1}],父节点[{2}]", strs[0], CacheString, parentStr));
            }

            //添加参数
            Values = new List<string>();
            for (int i = 1; i < strs.Length; i++)
            {
                if (strs[i] != "")
                {
                    Values.Add(strs[i]);
                }
            }
        }
    }

    #region 获取属性
    
    //得到子节点中某个类型的所有节点
    public List<MindTreeNode> GetChild(MindTreeNodeType childTy)
    {
        List<MindTreeNode> nodeList = new List<MindTreeNode>();
        for (int i = 0; i < ChildNode.Count; i++)
        {
            if (ChildNode[i].Type == childTy)
            {
                nodeList.Add(ChildNode[i]);
            }
        }
        return nodeList;
    }
    public List<MindTreeNode> GetChild(MindTreeNodeBaseType childTy)
    {
        List<MindTreeNode> nodeList = new List<MindTreeNode>();
        for (int i = 0; i < ChildNode.Count; i++)
        {
            if (ChildNode[i].Type.GetBaseType() == childTy)
            {
                nodeList.Add(ChildNode[i]);
            }
        }
        return nodeList;
    }


    public int TryGetInt(int _index)
    {
        if (_index >= 0 && _index < Values.Count)
        {
            if (Values[_index] != null)
            {
                int temp = 0;
                if (!int.TryParse(Values[_index], out temp))
                {
                    TDebug.LogError(string.Format("转换npcId失败:[{0}]", Values[_index]));
                }
                return temp;
            }
        }
        return -1;
    }


    public string TryGetString(int _index)
    {
        if (_index >= 0 && _index < Values.Count)
        {
            if (Values[_index] != null)
            {
                return Values[_index];
            }
        }
        return "";
    }

    public int GetNpcId()
    {
        if (Type == MindTreeNodeType.npc || Type == MindTreeNodeType.setNPC)
            return TryGetInt(0);
        else
        {
            TDebug.LogError(string.Format("只有npc节点可以获取npcid:[{0}]", Type));
            return -1;
        }
    }

    #endregion

}


public enum MindTreeNodeType : byte
{
    none,
    map,
    npc,
    [EnumDesc("交谈")]
    modTalk,
    [EnumDesc("切磋")]
    modFight,
    [EnumDesc("决斗")]
    modBattle,
    [EnumDesc("查看")]
    modSearch,
    [EnumDesc("商店")]
    modShop,
    [EnumDesc("进入事件")]
    modEnter,
    [EnumDesc("离开事件")]
    modLeave,
    [EnumDesc("学习")]
    modObtain,
    [EnumDesc("拜师")]
    modTeach,

    getLevel,
    getItem,
    getSpell,
    getAtt,
    getRandom,
    getEvent,
    setBattle,
    ////////////
    getMapLevel,
    getGuideStep,


    setDesc,   //描述信息
    setObtain, //学习
    setPlace,  //将主角移动到
    setItem,
    setEvent,
    setEnd,    //设置结局
    setNPC,
    setEnter,   //关闭某进入事件
    setMsg,
    setShop,
    //////
    setTalk,    //setTalk的后续节点，必须是condition节点。若没有按钮，condition=0
    setGuideStep,
    setSex,
    setHairColor,
    setMemory,

    condition,
}


public enum MindTreeNodeBaseType  //节点的基本类型
{
    Object,              //人物、物体节点
    InteractMod,         //物体交互事件，交谈、对战
    EnterOrLevelMod,     //进入或离开事件
    Get,                 //获取某事件，都用异步
    Set,                 //设置某事件
    SetWithCondition,    //设置某事件，并且后面可能接子节点
    Condition,           //某事件的结果条件
}

public static class MindTreeNodeTypeExtend
{
    public static MindTreeNodeBaseType GetBaseType(this MindTreeNodeType ty)
    {
        switch (ty)
        {
            case MindTreeNodeType.map:
            case MindTreeNodeType.npc:
                return MindTreeNodeBaseType.Object;
            case MindTreeNodeType.modTalk:
            case MindTreeNodeType.modFight:
            case MindTreeNodeType.modBattle:
            case MindTreeNodeType.modSearch:
            case MindTreeNodeType.modShop:
            case MindTreeNodeType.modObtain:
            case MindTreeNodeType.modTeach:
                return MindTreeNodeBaseType.InteractMod;
            case MindTreeNodeType.modEnter:
            case MindTreeNodeType.modLeave:
                return MindTreeNodeBaseType.EnterOrLevelMod;
            case MindTreeNodeType.getItem:
            case MindTreeNodeType.getSpell:
            case MindTreeNodeType.getLevel:
            case MindTreeNodeType.getMapLevel:
            case MindTreeNodeType.getGuideStep:
            case MindTreeNodeType.getAtt:
            case MindTreeNodeType.getRandom:
            case MindTreeNodeType.getEvent:
                return MindTreeNodeBaseType.Get;
            case MindTreeNodeType.setDesc:
            case MindTreeNodeType.setItem:
            case MindTreeNodeType.setEvent:
            case MindTreeNodeType.setEnd:
            case MindTreeNodeType.setNPC:
            case MindTreeNodeType.setEnter:
            case MindTreeNodeType.setMsg:
            case MindTreeNodeType.setPlace:
                return MindTreeNodeBaseType.Set;
            case MindTreeNodeType.setTalk:   //setTalk与setBattle后面可能有选项
            case MindTreeNodeType.setBattle:   
                return MindTreeNodeBaseType.SetWithCondition;
            case MindTreeNodeType.condition:
                return MindTreeNodeBaseType.Condition;
            default:
                return MindTreeNodeBaseType.Condition;

        }
    }
}
