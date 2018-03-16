using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;



public class BattleRecordStr 
{
    public enum RecordType
    {
        ConstainStart,  //包含初始化信息
        OnlyAction,     //只有过程信息
        ContainEnd,     //包含结束信息
    }

    //public long Time;
    public static int ByteLengthNoString;

    public BattleType MyBattleType;
    public int ChallengerIdx;
    public Sect.SectType ChallengerSect;
    public int ChallengerHeadIconIdx;
    public int ChallengerHp;
    public int ChallengerMp;
    public List<Pet> ChallengerPets = new List<Pet>();
    public string ChallengerName;

    public int DefierIdx;
    public Sect.SectType DefierSect;
    public int DefierHeadIconIdx;
    public int DefierHp;
    public int DefierMp;
    public string DefierName;
    public List<Pet> DefierPets = new List<Pet>();
    public bool? Winnder;       //防守方胜利=false，挑战方胜利=true

    public List<RecordActionStr> ActionList = new List<RecordActionStr>();
    public void ReadFrom(BinaryReader ios , RecordType recordTy = RecordType.ConstainStart)
    {
        long amountLength = ios.BaseStream.Length;
        ByteLengthNoString = 0;
        if (recordTy == RecordType.ConstainStart)
        {
            MyBattleType = (BattleType)(int)ios.ReadByte();

            ChallengerSect = (Sect.SectType) ios.ReadByte();
            ChallengerHeadIconIdx = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            ChallengerIdx = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            ChallengerHp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            ChallengerMp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            ChallengerName = NetUtils.ReadUTF(ios);
            ChallengerPets = new List<Pet>();
            int length = ios.ReadByte();
            for (int i = 0; i < length; i++)
            {
                Pet pet = Pet.PetFetcher.GetPetByCopy(ios.ReadInt32());
                pet.CurLevel = ios.ReadByte();
                ChallengerPets.Add(pet);
            }

            DefierSect = (Sect.SectType)ios.ReadByte();
            DefierHeadIconIdx = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            DefierIdx = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            DefierHp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            DefierMp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            DefierName = NetUtils.ReadUTF(ios);
            DefierPets = new List<Pet>();
            length = ios.ReadByte();
            for (int i = 0; i < length; i++)
            {
                Pet pet = Pet.PetFetcher.GetPetByCopy(ios.ReadInt32());
                pet.CurLevel = ios.ReadByte();
                DefierPets.Add(pet);
            }


            TDebug.Log(string.Format("战斗初始：ChallengerHp:{0}|ChallengerMp:{1}|DefierHp:{2}|DefierMp:{3}|MyBattleType:{4}", ChallengerHp, ChallengerMp, DefierHp, DefierMp, MyBattleType));
        }
        else
        {
            int challengerHp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            int challengerMp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            int defierHp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            int defierMp = ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4;
            int length = ios.ReadByte(); BattleRecordStr.ByteLengthNoString += 1;
            try
            {
                for (int i = 0; i < length; i++)
                {
                    RecordActionStr action = new RecordActionStr();
                    action.ReadFrom(ios);
                    if (i == length-1)
                    {
                        action.ChallengerHp = challengerHp;
                        action.ChallengerMp = challengerMp;
                        action.DefierHp = defierHp;
                        action.DefierMp = defierMp;
                    }
                    ActionList.Add(action);
                }
            }
            catch{ }
        }
        
        TDebug.Log(string.Format("总数据:{0} |非字符串长度:{1} | actionNum:{2}",amountLength, BattleRecordStr.ByteLengthNoString, ActionList.Count));
    }

    public string ToStr()
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < ActionList.Count; i++)
        {
            str.Append(ActionList[i].ToStr());
            
        }
        return str.ToString();
    }
    public string ToStr2()
    {
        string str = "";
        for (int i = 0; i < ActionList.Count; i++)
        {
            str += string.Format("[{0}]{1}{2}", ActionList[i].Type, ActionList[i].ToOriginStr(), "\r\n");
        }
        return str;
    }

    public RecordActionStr GetNext()
    {
        if (ActionList.Count > 0)
        {
            RecordActionStr act = ActionList[0];
            return act;
        }
        return null;
    }
}


public class RecordActionStr
{
    public int Idx;
    public int? ChallengerHp;
    public int? ChallengerMp;
    public int? DefierHp;
    public int? DefierMp;

    public PVELoggerType Type;
    public List<object> ItemValue = new List<object>();
    public List<BattleDescType> ItemType = new List<BattleDescType>();

    public void ReadFrom(BinaryReader ios)
    {
        Type = (PVELoggerType)ios.ReadByte();BattleRecordStr.ByteLengthNoString += 1;

        //读表，获得顺序
        List<PVEDialogue> tempList = PVEDialogue.PVEDialogueFetcher.GetPVEDialogueByTypeByCopy(Type);
        if (tempList == null || tempList.Count==0) //如果为空，读取之后跳过
        {
            int tempLength = ios.ReadByte(); BattleRecordStr.ByteLengthNoString += 1;
            for (int i = 0; i < tempLength; i++)
            {
                sbyte le = ios.ReadSByte(); BattleRecordStr.ByteLengthNoString += 1;
                if (le == 1) { ios.ReadSByte(); BattleRecordStr.ByteLengthNoString += 1; }
                else if (le == 2) { ios.ReadInt16(); BattleRecordStr.ByteLengthNoString += 2; }
                else if (le == 4) { ios.ReadInt32(); BattleRecordStr.ByteLengthNoString += 4; }
                else { NetUtils.ReadUTF(ios); }
            }
            return;
        }
        
        if (tempList == null) return;
        PVEDialogue dialogue = tempList[GameUtils.GetRandom(0, tempList.Count)];
        Idx = dialogue.idx;
        ItemType = GetItemList(dialogue.Sequence);

        int length = ios.ReadByte(); BattleRecordStr.ByteLengthNoString += 1;
        for (int i = 0; i < length; i++)
        {
            sbyte le = ios.ReadSByte(); BattleRecordStr.ByteLengthNoString += 1;
            if (le == 1) { ItemValue.Add(ios.ReadSByte()); BattleRecordStr.ByteLengthNoString += 1; }
            else if (le == 2) { ItemValue.Add(ios.ReadInt16()); BattleRecordStr.ByteLengthNoString += 2; }
            else if (le == 4) { ItemValue.Add(ios.ReadInt32()); BattleRecordStr.ByteLengthNoString += 4; }
            else { ItemValue.Add(NetUtils.ReadUTF(ios)); }
        }
    }

    public object GetValue(BattleDescType itemTy)//得到某个项的值
    {
        for (int i = 0; i < ItemType.Count; i++)
        {
            if (ItemType[i] == itemTy)
            {
                if (i < ItemValue.Count) return ItemValue[i];
            }
        }
        TDebug.Log(string.Format("{0}:没有{1}", ToStr(), itemTy.ToString()));
        return null;
    }
    public int GetValueTryInt(BattleDescType itemTy)
    {
        int val = 0;
        object obj = GetValue(itemTy);
        if (obj == null) return val;
        else
        {
            int.TryParse(obj.ToString(), out val);
            return val;
        }
    }

    public string ToOriginStr()
    {
        //读表，得到描述
        PVEDialogue dialogue = PVEDialogue.PVEDialogueFetcher.GetPVEDialogueByCopy(Idx);
        if (dialogue == null)
        {
            return "";
        }
        string objStrs = "";
        for (int i = 0; i < ItemValue.Count; i++)
        {
            objStrs += string.Format("|{0}:{1}   ",ItemType[i], ItemValue[i].ToString());
        }
        string returnStr = string.Format("{0}:{1}{2}", Type, dialogue.Describe, objStrs);
        return returnStr;
    }

    //public string ToOriginStr()
    //{
    //    string str = "";
    //    for (int i = 0; i < ItemValue.Count; i++)
    //    {
    //        str += ItemValue[i] + "|";
    //    }
    //    for (int i = 0; i < ItemType.Count; i++)
    //    {
    //        str += ItemType[i]+"|";
    //    }
    //    return str;
    //}
    public string ToStr() //获取行动的描述
    {
        PVEDialogue dialogue = PVEDialogue.PVEDialogueFetcher.GetPVEDialogueByCopy(Idx);
        if (dialogue == null)
        {
            return "没有此id" + Type;
        }
        //TDebug.Log(ToStr2());
        List<object> shwoObjs = new List<object>();
        string selfName = "";
        for (int i = 0; i < ItemType.Count && i<ItemValue.Count; i++)   //获得显示参数
        {
            if (ItemType[i] == BattleDescType.self) selfName = ItemValue[i].ToString();//如果施放者与目标相同，则设为自己
            if (ItemType[i] == BattleDescType.aim && selfName == ItemValue[i].ToString()) ItemValue[i] = "自己";

            shwoObjs.Add(GetShowObj(ItemType[i], ItemValue[i]));
        }
        
        
        string returnStr = "";
        try
        {
            returnStr = string.Format(dialogue.Describe, shwoObjs.ToArray());  //用参数替换
        }
        catch
        {
            string errorStr = string.Format("参数错误:{0} |[数量:{1}]", dialogue.Describe, shwoObjs.Count);
            return errorStr + "\r\n";
        }
        switch (Type)   
        {
            case PVELoggerType.RoundStart:  //某些类型，添加回车
            case PVELoggerType.DoIdle:
            case PVELoggerType.HotDoing:
            case PVELoggerType.DotDoing:
            case PVELoggerType.EndFail:
            case PVELoggerType.EndDead:
            case PVELoggerType.DoHealSpell:
            case PVELoggerType.DoStateSpell:
            case PVELoggerType.DoSignstSpell:
            case PVELoggerType.DoAttackSpell:
            case PVELoggerType.DoLifeRemoval:
                returnStr = "\r\n" + returnStr;
                break;
            case PVELoggerType.DmgCommon:
            case PVELoggerType.DmgMiss:
            case PVELoggerType.DmgCrit:
            case PVELoggerType.DmgCritBlock:
            case PVELoggerType.DmgBlock:
            case PVELoggerType.DmgBlockDead:
            case PVELoggerType.DoStateSubSpell:
            case PVELoggerType.DoHealSubSpell:
            case PVELoggerType.DoAttackSubSpell:
            case PVELoggerType.DoLifeRemovalSub:
            case PVELoggerType.DoSignstSubSpell:
                break;
        }


        return returnStr;
    }

    public object GetShowObj(BattleDescType ty , object val)//得到显示的值，比如将SpellId
    {
        switch (ty)
        {
            case BattleDescType.backDmg: 
            case BattleDescType.dmg: 
            case BattleDescType.duration: 
            case BattleDescType.max: 
            case BattleDescType.num:
            case BattleDescType.prom:
                return val;
            case BattleDescType.aim:  //TODO:在这里，将byte转为角色名字
            case BattleDescType.self: 
            case BattleDescType.loser: 
            case BattleDescType.winner:
                string roleName = val.ToString();
                if (roleName == PlayerPrefsBridge.Instance.PlayerData.name)
                {
                    //TDebug.Log("这里先强制将特殊名字设为玩家自身");
                    return "你";
                }
                return val;
            case BattleDescType.spell:
            {
                //int id = (int)val;
                //Skill spell = Skill.SkillFetcher.GetSpellByCopy(id);
                //if (spell != null)
                //{
                //    return (object) spell.name;
                //}
                return null;
            }

        }
        return -1;
    }

    public static List<BattleDescType> GetItemList(string str)
    {
        List<BattleDescType> tempList = new List<BattleDescType>();
        string[] strs = str.Split(',');
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i] != "")
            {
                for (int j = 0; j < (int)BattleDescType.max; j++)
                {
                    if ((((BattleDescType)j).ToString()) == strs[i].Replace("{", "").Replace("}", ""))
                    {
                        tempList.Add((BattleDescType) j);
                        break;
                    }
                }
            }
        }
        return tempList;
    }
}


public enum BattleStatisType : byte
{
    EnemyInLive,
    Round,
    MyDead,
    MyInLive,
    Max,
}