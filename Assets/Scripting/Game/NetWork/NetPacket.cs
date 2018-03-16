using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using ICSharpCode.SharpZipLib.Core;
using JetBrains.Annotations;
using UnityEngine;

public class NetPacket
{

    #region 同步时间
    public sealed class C2S_SnapshotTime : Packet
    {

        public C2S_SnapshotTime() // 初始化
        {

        }

        public override short NetCode
        {
            get { return (short)NetCode_C.SnapshotTime; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {

        }


    }

    public sealed class S2C_SnapshotTime : Packet
    {
        public long time;
        public S2C_SnapshotTime() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotTime; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            time = ios.ReadInt64();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    #endregion

    #region 初始进入
    
    public sealed class C2S_VerifyLogin : Packet
    {

        public C2S_VerifyLogin() // 初始化
        {

        }

        public override short NetCode
        {
            get { return (short)NetCode_C.VerifyLogin; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {

        }


    }

    public sealed class S2C_VerifyLogin : Packet
    {
        public S2C_VerifyLogin() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.VerifyLogin; }
        }

        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_Register : Packet
    {
        public string Username;
        public string Password;
        public string Agent;
        public PlatformType Platform;

        public C2S_Register() // 初始化
        {
        }

        public override short NetCode
        {
            get { return (short)NetCode_C.Register; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte)Platform);
            NetUtils.WriteUTF(Agent, ref ios);
            NetUtils.WriteUTF(Username, ref ios);
            NetUtils.WriteUTF(Password, ref ios);
        }

    }

    public sealed class S2C_Register : Packet
    {
        public S2C_Register() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.Register; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    #region 登录
    
    public sealed class C2S_Login : Packet
    {
        public string Username;
        public string Password;

        public C2S_Login() // 初始化
        {
        }

        public override short NetCode
        {
            get { return (short)NetCode_C.Login; }
        }

        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {
            NetUtils.WriteUTF(Username, ref ios);
            NetUtils.WriteUTF(Password, ref ios);
            //NetUtils.WriteUTF(SystemInfo.deviceUniqueIdentifier, ref ios);
            NetUtils.WriteUTF("", ref ios);
        }

    }


    public sealed class S2C_Login : Packet
    {
        public NetMiniPlayer[] PlayerList = new NetMiniPlayer[3];
        public S2C_Login() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.Login; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            int count = ios.ReadByte();

            for (int i = 0,length =  PlayerList.Length; i < length; i++)
            {
                NetMiniPlayer miniPlayer = new NetMiniPlayer();
                if(i < count)miniPlayer.Serialize(ios);
                PlayerList[i] =  miniPlayer;
            }
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    #endregion


    #region 快速登录
    public sealed class C2S_FastRegister : Packet
    {
        public string       Agent;
        public PlatformType Platform;

        public C2S_FastRegister() { }
        
        public override short NetCode
        {
            get { return (short)NetCode_C.FastRegister; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte) Platform);
            NetUtils.WriteUTF(Agent, ref ios);
        }
    }

    public sealed class S2C_FastRegister : Packet
    {
        public string   Username;
        public string   Password;
        public S2C_FastRegister() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.FastRegister; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            Username = NetUtils.ReadUTF(ios);
            Password = NetUtils.ReadUTF(ios);
        }

        public override void WriteTo(BinaryWriter ios) { }
    }
    #endregion


    #region 角色存档操作

    public sealed class C2S_EnterGame : Packet
    {
        public int RoleIdx;    //在存档中的序号

        public C2S_EnterGame() // 初始化
        { }
        public override short NetCode
        {
            get { return (short)NetCode_C.EnterGame; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(RoleIdx);
        }
    }
    public sealed class S2C_EnterGame : Packet
    {
        public S2C_EnterGame() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.EnterGame; }
        }

        public override void ReadFrom(BinaryReader ios) { }
        public override void WriteTo(BinaryWriter ios) { }
    }

    public sealed class C2S_RemoveRoleSave : Packet
    {
        public int RoleIdx;    //在存档中的序号

        public C2S_RemoveRoleSave() // 初始化
        { }
        public override short NetCode
        {
            get { return (short)NetCode_C.RemoveSaveRole; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(RoleIdx);
        }
    }
    public sealed class S2C_RemoveRoleSave : Packet
    {
        public S2C_RemoveRoleSave() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.RemoveSaveRole; }
        }

        public override void ReadFrom(BinaryReader ios) { }
        public override void WriteTo(BinaryWriter ios) { }
    }

    #endregion


    #region 创建角色
    public sealed class C2S_CreateRole : Packet
    {

        public OldHero.Sex Sex;
            
        public C2S_CreateRole() // 初始化
        {
        }

        public override short NetCode
        {
            get { return (short)NetCode_C.CreateRole; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte)Sex);
        }
    }

    public sealed class S2C_CreateRole : Packet
    {
        public int RoleIdx;
        public S2C_CreateRole() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.CreateRole; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            RoleIdx = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    #endregion


    #endregion

    #region 起名

    public sealed class S2C_SnapshotPlayerName : Packet
    {
        public string Name;
        public S2C_SnapshotPlayerName() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotPlayerName; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Name = NetUtils.ReadUTF(ios);
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_SetPlayerName : Packet
    {
        public string Name;

        public C2S_SetPlayerName() // 初始化
        {
        }

        public override short NetCode
        {
            get { return (short)NetCode_C.SetPlayerName; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            NetUtils.WriteUTF(Name, ref ios);
        }

    }

    public sealed class S2C_SetPlayerName : Packet
    {

        public S2C_SetPlayerName() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SetPlayerName; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }


    #endregion

    #region 功法、技能穿卸和学习

    //穿戴招式
    public sealed class C2S_EquipSpell : Packet
    {
        public int InventoryPos;   //
        public sbyte EquipPos;  //位置序号

        public override short NetCode
        {
            get { return (short) NetCode_C.EquipSpell; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(EquipPos);
            ios.Write(InventoryPos);
        }
    }

    public sealed class S2C_EquipSpell : Packet
    {
        public byte EquipPos;  //发生变动的装备槽   

        public S2C_EquipSpell() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.EquipSpell; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            EquipPos = ios.ReadByte();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_StudySpell : Packet
    {
     //   public int SpellIdx;   //技能id
        public sbyte EquipPos;  //位置序号
        public bool IsFastStudy;
        public override short NetCode
        {
            get { return (short)NetCode_C.StudySpell; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(EquipPos);
            ios.Write(IsFastStudy);
         //   ios.Write(SpellIdx);
        }
    }
    public sealed class S2C_StudySpell : Packet
    {
        public int NewSpellIdx;   //技能id
        public int CostGold;   //
        public int CostPotential;
        public S2C_StudySpell() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.StudySpell; }
        }


        public override void ReadFrom(BinaryReader ios)
        {   
            NewSpellIdx = ios.ReadInt32();
            CostGold = ios.ReadInt32();
            CostPotential = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
  
    
    public sealed class C2S_EnterPVE : Packet
    {
        public int EnemyIdx;
        public BattleType BattleType;
        public PVESceneType SceneType;
        
        public override short NetCode
        {
            get { return (short)NetCode_C.EnterPVE; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(EnemyIdx);
            ios.Write((byte)BattleType);
            ios.Write((byte)SceneType);

        }
    }

    public sealed class S2C_EnterPVE : Packet
    {
        public BattleRecordStr BattleStr;
        public override short NetCode
        {
            get { return (short)NetCode_S.EnterPVE; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            BattleStr = new BattleRecordStr();
            BattleStr.ReadFrom(ios , BattleRecordStr.RecordType.ConstainStart);
            //Round = ios.ReadByte();
            //EnemyIdx = ios.ReadInt32();
            //MyHp = ios.ReadInt32();
            //MyMp = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    //穿戴法宝
    public sealed class C2S_EquipEquip : Packet
    {
        public sbyte InventoryPos;   //背包位置（卸下的话是-1）
        public byte EquipPos;  //  穿戴的槽位 
        public C2S_EquipEquip() { }

        public override short NetCode
        {
            get { return (short)NetCode_C.EquipEquip; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(EquipPos);
            ios.Write(InventoryPos);      
        }
    }
    public sealed class S2C_EquipEquip : Packet
    {
        public byte EquipPos;  //发生变动的装备槽   
        public S2C_EquipEquip() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.EquipEquip; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            EquipPos = ios.ReadByte();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_SellEquip : Packet
    {
        public List<byte> SellEquipList =new List<byte>();
        public C2S_SellEquip() { }

        public override short NetCode
        {
            get { return (short)NetCode_C.SellEquip; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte)SellEquipList.Count);
            for (int i = 0; i < SellEquipList.Count; i++)
            {
                ios.Write(SellEquipList[i]);
            }
        }
    }

    public sealed class S2C_SellEquip : Packet
    {
        public int SellNum;  
        public int Gold;
        public S2C_SellEquip() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SellEquip; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            SellNum = ios.ReadByte();
            Gold = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_SpellObtain : Packet
    {
        public int SpellObtainIdx;
        public byte SpellIndex;
        public override short NetCode
        {
            get { return (short)NetCode_C.SpellObtain; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(SpellObtainIdx);
            ios.Write(SpellIndex);
        }
    }
    public sealed class S2C_SpellObtain : Packet 
    {
        public override short NetCode
        {
            get { return (short)NetCode_S.SpellObtain; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }


    public sealed class C2S_SetSect : Packet
    {
        public Sect.SectType SectType;
        public override short NetCode
        {
            get { return (short)NetCode_C.SetSect; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte) SectType);
        }
    }
    public sealed class S2C_SetSect : Packet
    {
        public Sect.SectType MySectType;
        public GoodsToDrop[] GoodsList;
        public override short NetCode
        {
            get { return (short)NetCode_S.SetSect; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            MySectType = (Sect.SectType) ios.ReadByte();
            GoodsList= GoodsToDrop.SerializeList(ios);
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

   
    #endregion

    #region 生产制作
    public sealed class C2S_Produce : Packet //生产制造
    {
        public byte InventoryPos;   //背包位置
        public int Num;
        //     public byte Type;
        public C2S_Produce() { }

        public override short NetCode
        {
            get { return (short)NetCode_C.Produce; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(InventoryPos);
            ios.Write(Num);
        }
    }
    public sealed class S2C_Produce : Packet
    {
        public int RecipeID;
        public S2C_Produce() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.Produce; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            RecipeID = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_FinishProduce : Packet //完成事件
    {
        public int recipeID;
        public bool useDiamond;
        public C2S_FinishProduce() { }
        public override short NetCode
        {
            get { return (short)NetCode_C.FinishProduce; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(useDiamond);
            ios.Write(recipeID);
        }
    }
    public sealed class S2C_FinishProduce : Packet
    {
        public int RecipeID;
        public int Prociency;
        public GoodsToDrop[] TranslateList;

        public S2C_FinishProduce() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.FinishProduce; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            RecipeID = ios.ReadInt32();
            Prociency = ios.ReadInt16();
            TranslateList = GoodsToDrop.SerializeList(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_ProduceAccelerate : Packet
    {
        public int recipeID;
        public C2S_ProduceAccelerate() { }
        public override short NetCode
        {
            get { return (short)NetCode_C.ProduceAccelerate; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(recipeID);
        }
    }
    public sealed class S2C_ProduceAccelerate : Packet
    {
        public int recipeID;
        public S2C_ProduceAccelerate() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.ProduceAccelerate; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            recipeID = ios.ReadInt32();
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    
    public sealed class S2C_SnapShotProduce : Packet
    {
        public ProduceItem ProduceItem;
        public S2C_SnapShotProduce() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapShotProduceBotting; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            ProduceItem = new ProduceItem();
            ProduceItem.ReadFrom(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class S2C_SnapShotProduceBotting : Packet
    {
        public List<ProduceItem> ProduceItemList;
        public S2C_SnapShotProduceBotting() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapShotProduceBotting; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            int length = ios.ReadByte();
            ProduceItemList = new List<ProduceItem>();
            for (int i = 0; i < length; i++)
            {
                ProduceItem item = new ProduceItem();
                item.ReadFrom(ios);
                ProduceItemList.Add(item);
            }
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_SnapshotMapEvent : Packet
    {
        public int MapId;
        public Dictionary<int,MapEvent.MapEventStatus> EventStatusPool;
        public S2C_SnapshotMapEvent() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotMapEvent; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            MapId = ios.ReadInt32();
            int length = ios.ReadByte();
            EventStatusPool = new Dictionary<int, MapEvent.MapEventStatus>();
            for (int i = 0; i < length; i++)
            {
                MapEvent.MapEventStatus status = (MapEvent.MapEventStatus) ios.ReadByte();
                int eventId = ios.ReadInt32();
                EventStatusPool.Add(eventId, status);
            }
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_AuxSkillLevelUp : Packet
    {
        public int skillPos;
        public C2S_AuxSkillLevelUp() { }
        public override short NetCode
        {
            get { return (short)NetCode_C.AuxSkillLevelUp; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(skillPos);
        }
    }
    public sealed class S2C_AuxSkillLevelUp : Packet
    {
       // public int RecipeID;
        public S2C_AuxSkillLevelUp() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.AuxSkillLevelUp; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
           // RecipeID = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    #endregion

    #region 排行

    public sealed class C2S_PullRank: Packet //
    {
        public byte Type;   //背包位置
        public C2S_PullRank() { }

        public override short NetCode
        {
            get { return (short)NetCode_C.PullRank; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(Type);
        }
    }
    public sealed class S2C_PullRank : Packet
    {
        public int  SelfRank;
        public NetRankBotting.RankItem[] RankList;
        public RankType RankType;
        public S2C_PullRank() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.PullRank; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            RankType = (RankType)ios.ReadByte();
            SelfRank = ios.ReadInt32();
            int length = ios.ReadByte();
            RankList = new NetRankBotting.RankItem[length];         
            for (int i = 0; i < length; i++)
            {
                NetRankBotting.RankItem rankItem = new NetRankBotting.RankItem();
                rankItem.readFrom(ios, RankType);
                RankList[i] = rankItem;
            }
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_PullOtherRoleInfo : Packet //
    {
        public int PlayerID;
        public C2S_PullOtherRoleInfo() { }

        public override short NetCode
        {
            get { return (short)NetCode_C.PullOtherRoleInfo; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(PlayerID);
        }
    }
    public sealed class S2C_PullOtherRoleInfo : Packet
    {
        public NetRankBotting.OtherRoleInfo OtherRole;
        public S2C_PullOtherRoleInfo() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.PullOtherRoleInfo; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            OtherRole = new NetRankBotting.OtherRoleInfo();
            OtherRole.ReadFrom(ios);
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }



    #endregion

    #region 洞府

    public sealed class C2S_ZazenStart : Packet
    {
        public int DrugId;
        public override short NetCode
        {
            get { return (short)NetCode_C.ZazenStart; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(DrugId);
        }
    }
    public sealed class S2C_ZazenStart : Packet
    {
        public long StartTime;
        public S2C_ZazenStart() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.ZazenStart; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            StartTime = ios.ReadInt64();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
   
    public sealed class C2S_CaveLevelUp : Packet
    {
        public bool IsUseDiamond;
        public override short NetCode 
        {
            get { return (short)NetCode_C.CaveLevelUp; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(IsUseDiamond);
        }
    }
    public sealed class S2C_CaveLevelUp : Packet
    {
        public int Level;
        public S2C_CaveLevelUp() { } 

        public override short NetCode
        {
            get { return (short)NetCode_S.CaveLevelUp; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Level = ios.ReadInt16();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

  
    public sealed class C2S_ZazenFinish : Packet
    {
        public override short NetCode
        {
            get { return (short)NetCode_C.ZazenFinish; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class S2C_ZazenFinish : Packet
    {
        public long StartTime;
        public S2C_ZazenFinish()
        {
        }
        public override short NetCode
        {
            get { return (short)NetCode_S.ZazenFinish; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            StartTime = ios.ReadInt64();

        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
   
    public sealed class C2S_RetreatStart : Packet
    {
        public int DrugIdx;
        public override short NetCode
        {
            get { return (short)NetCode_C.RetreatStart; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(DrugIdx);
        }
    }
    public sealed class S2C_RetreatStart  : Packet
    {
        public long StartTime;
        public S2C_RetreatStart() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.RetreatStart; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            StartTime = ios.ReadInt64();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_RetreatFinish : Packet
    {
        public bool UseDiamond;
        public override short NetCode
        {
            get { return (short)NetCode_C.RetreatFinish; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(UseDiamond);
        }
    }
    public sealed class S2C_RetreatFinish : Packet
    {
        public bool PctSuccess;//概率成功
        public bool MinSufferSuccess;
        public bool MaxSufferSuccess;
        public S2C_RetreatFinish() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.RetreatFinish; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            int result = ios.ReadByte();
            PctSuccess = result >= 1;
            MinSufferSuccess = result >= 2;
            MaxSufferSuccess = result >= 3;
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_AuxSkillToolLevelUp : Packet
    {
        public int Index;
        public override short NetCode
        {
            get { return (short)NetCode_C.AuxSkillToolLevelUp; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(Index);
        }
    }
    public sealed class S2C_AuxSkillToolLevelUp : Packet
    {
        public int Pos;
        public int Level;
        public S2C_AuxSkillToolLevelUp() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.AuxSkillToolLevelUp; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Pos = ios.ReadByte();
            Level = ios.ReadInt16();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
   
    #endregion

    #region 宠物
    public sealed class S2C_SnapshotEquipPet : Packet
    {
        public sbyte EquipPos;
        public sbyte InventoryPos;  //在背包中的位置(-1表示当前位置法宝为空)
        public sbyte InventoryPosOld;  //同步：装备槽位置、当前装备的背包位置、之前的装备背包位置
        public S2C_SnapshotEquipPet() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotEquipPet; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            EquipPos = ios.ReadSByte();
            InventoryPos = ios.ReadSByte();
            InventoryPosOld = ios.ReadSByte();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class S2C_SnapshotPet : Packet
    {
        public int Idx;
        public int Pos;
        public int Level;//强化次数
        public S2C_SnapshotPet() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotPet; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Pos = ios.ReadByte();
            Level = ios.ReadInt16();
            Idx = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }


    public sealed class C2S_PetLevelUp : Packet
    {
        public int inventoryPos;
        public override short NetCode
        {
            get { return (short)NetCode_C.PetLevelUp; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(inventoryPos);
        }
    }
    public sealed class S2C_PetLevelUp : Packet
    {
        public int OldPetIdx;
        public int NewPetIdx;
        public int Level;
        public S2C_PetLevelUp() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.PetLevelUp; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            OldPetIdx = ios.ReadInt32();
            NewPetIdx = ios.ReadInt32();
            Level = ios.ReadByte();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_EquipPet : Packet
    {
        public byte EquipPos;  //  穿戴的槽位 
        public sbyte InventoryPos;   //背包位置（卸下的话是-1）
       
        public override short NetCode
        {
            get { return (short)NetCode_C.EuqipPet; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(EquipPos);
            ios.Write(InventoryPos);
        }
    }
    public sealed class S2C_EquipPet : Packet
    {
        public byte EquipPos;
        public S2C_EquipPet() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.EquipPet; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            EquipPos = ios.ReadByte();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    #endregion

    #region 秘境
    public sealed class C2S_EnterDungeonMap : Packet
    {
        public int MapIdx;

        public override short NetCode
        {
            get { return (short)NetCode_C.EnterDungeonMap; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(MapIdx);
        }
    }
    public sealed class S2C_EnterDungeonMap : Packet
    {
        public S2C_EnterDungeonMap() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.EnterDungeonMap; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
   
    public sealed class S2C_SnapshotDungeonMap : Packet
    {
        public DungeonMapAccessor DungeonMap;
        public S2C_SnapshotDungeonMap() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotDungeonMap; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            DungeonMap = new DungeonMapAccessor();
            DungeonMap.ReadFrom(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
   
   
    public sealed class C2S_CheckCondition : Packet
    {
        public MapData.ConditionType ConditionType;
        public int ConditionIdx;
        public int ConditionValue;

        public override short NetCode
        {
            get { return (short)NetCode_C.CheckCondition; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte)ConditionType);
            ios.Write(ConditionIdx);
            ios.Write(ConditionValue);
        }
    }
    public sealed class S2C_CheckCondition : Packet
    {
        public bool IsStatisfy;

        public S2C_CheckCondition() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.CheckCondition; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            IsStatisfy = ios.ReadBoolean();
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_MapEventReward : Packet
    {
        public int MapEventId;
        public GoodsToDrop[] GoodsList = new GoodsToDrop[0];
        public S2C_MapEventReward() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.MapEventReward; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            MapEventId = ios.ReadInt32();
            GoodsList = GoodsToDrop.SerializeList(ios);
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_MapEventReward : Packet
    {
        public int EventId;
        public override short NetCode
        {
            get { return (short)NetCode_C.MapEventReward; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }
        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(EventId);
        }
    }
   
    public sealed class C2S_SellItem : Packet
    {
        public int Idx;
        public int Num;
        public override short NetCode
        {
            get { return (short)NetCode_C.SellItem; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(Idx);
            ios.Write((short)Num);
        }
    }
    public sealed class S2C_SellItem : Packet
    {
        public int ItemID;
        public int Num;
        public int Gold;
        public S2C_SellItem() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.SellItem; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            ItemID = ios.ReadInt32();
            Num = ios.ReadInt16();
            Gold = ios.ReadInt32();
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
  

    public sealed class C2S_UseItem : Packet
    {
        public int Idx;
        public int Num;
        public override short NetCode
        {
            get { return (short)NetCode_C.UseItem; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(Idx);
            ios.Write((byte)Num);
        }
    }
    public sealed class S2C_UseItem : Packet
    {
        public int ItemIdx;
        public GoodsToDrop[] GoodsList = new GoodsToDrop[0];
        public S2C_UseItem() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.UseItem; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            ItemIdx = ios.ReadInt32();
            GoodsList = GoodsToDrop.SerializeList(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class C2S_MapSave : Packet
    {
        public int RolePos;
        public List<NetMapSaveItem> SaveList;
        public override short NetCode
        {
            get { return (short)NetCode_C.MapSave; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte) RolePos);
            ios.Write((byte)SaveList.Count);
            for (int i = 0; i < SaveList.Count; i++)
            {
                ios.Write((byte)SaveList[i].SaveType);
                ios.Write(SaveList[i].SaveId);
                ios.Write(SaveList[i].SaveValue);
            }
        }
    }
    public sealed class S2C_MapSave : Packet
    {
        public S2C_MapSave() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.MapSave; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_MapEnd : Packet
    {
        public MapEndType EndType;
        public int EndId;

        public override short NetCode
        {
            get { return (short)NetCode_C.MapEnd; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte) EndType);
            ios.Write(EndId);
        }
    }
    public sealed class S2C_MapEnd : Packet
    {
        public MapEndType EndType;
        public int EndId;
        public List<MapEvent> FinishEvents = new List<MapEvent>();
        public S2C_MapEnd() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.MapEnd; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            EndType = (MapEndType) ios.ReadByte();
            EndId = ios.ReadInt32();
            int length = ios.ReadByte();
            FinishEvents = new List<MapEvent>();
            MapEvent mapEvent;
            for (int i = 0; i < length; i++)
            {
                mapEvent = MapEvent.MapEventFetcher.GetMapEventByCopy(ios.ReadInt32());
                bool firstFinish = ios.ReadBoolean();
                if (mapEvent != null)
                {
                    mapEvent.IsFirstFinish = firstFinish;
                    FinishEvents.Add(mapEvent);
                }
            }
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    #endregion

    #region 游历挂机

    public sealed class S2C_StartTravel : Packet
    {
        public int TravelId;
        public S2C_StartTravel() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.TravelStart; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            TravelId = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_StartTravel : Packet
    {
        public int index;  //

        public override short NetCode
        {
            get { return (short)NetCode_C.TravelStart; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(index);
        }
    }
  
    public sealed class S2C_SnapshotTravelBotting : Packet
    {
        /// <summary>
        /// 游历挂机场景ID
        /// </summary>
        public int TravelIdx;
        /// <summary>
        /// 游历挂机开始时间
        /// </summary>
        public long StartTime;
        public Dictionary<int, int> TravelEventProgress = new Dictionary<int,int>();
        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotTravelBotting; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            TravelEventProgress.Clear();
            TravelIdx = ios.ReadInt32();
            StartTime = ios.ReadInt64();
            int length = ios.ReadByte();
            for (int i = 0; i < length; i++)
            {
                int key = ios.ReadInt32();
                int value = ios.ReadByte();
                if (!TravelEventProgress.ContainsKey(key))
                {
                    TravelEvent travelEvent = TravelEvent.TravelEventFetcher.GetTravelEventByCopy(key);
                    if (travelEvent.EventList.Length - 1 >= value)
                    {
                         TravelEventProgress.Add(key, value);
                         TDebug.Log(string.Format("游历同步===={0}==小事件进度为{1}",travelEvent.name,value));
                    }
                     
                    else
                    {
                        TDebug.Log(string.Format("{0}==小事件进度为{1},超过最大长度{2}，移除",travelEvent.name,value,travelEvent.EventList.Length));
                    }
                }
             }
                   
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_SnapshotTravelEvent : Packet
    {
        public int TravelIdx;
        public int SmallEventIndex;
        public S2C_SnapshotTravelEvent() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotTravelEvent; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            TravelIdx = ios.ReadInt32();
            SmallEventIndex = ios.ReadByte();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class S2C_SnapshotTravelHistory : Packet
    {
        public int TravelId;
        public List<TravelEventProgress> TravelEventProgressList =new List<TravelEventProgress>(); //所有大事件的小事件完成进度
        public S2C_SnapshotTravelHistory() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.FinishTravelEvent; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            TravelId = ios.ReadInt32();
            int length = ios.ReadByte();
            TravelEventProgressList.Clear();
            for (int i = 0; i < length; i++)
            {
                TravelEventProgress progress = new TravelEventProgress();
                progress.TravelEventID = ios.ReadInt32();
                progress.FinishSmallEventNum = ios.ReadByte();
                TravelEventProgressList.Add(progress);
            }
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_FinishTravelEvent : Packet //完成挂机事件
    {
        public int EventId;
        public C2S_FinishTravelEvent() { }

        public override short NetCode
        {
            get { return (short)NetCode_C.FinishTravelEvent; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(EventId);
        }
    }
    public sealed class S2C_FinishTravelEvent : Packet
    {
        public int EventIndex;
        public bool IsSucessed = false;
        public GoodsToDrop[] GoodsDrops;
        public S2C_FinishTravelEvent() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.FinishTravelEvent; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            GoodsDrops = new GoodsToDrop[0];
            EventIndex = ios.ReadByte();
            IsSucessed = ios.ReadBoolean();
            GoodsDrops = GoodsToDrop.SerializeList(ios);
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class C2S_PullInfo : Packet //完成挂机事件
    {
        public PullInfoType PullType;
        public int Idx;
        public C2S_PullInfo() { }

        public override short NetCode
        {
            get { return (short)NetCode_C.PullInfo; }
        }


        public override void ReadFrom(BinaryReader ios)
        {

        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte)PullType);
            ios.Write(Idx);

        }
    }
    public sealed class S2C_PullInfo : Packet
    {
        public S2C_PullInfo() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.PullInfo; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    #endregion


    #region 商店

    public sealed class  S2C_SnapshotShopInfo : Packet
    {
        public long NextFreshTime;
        public Dictionary<int, int> EveryDayList = new Dictionary<int, int>();
        public Dictionary<int, int> HistoryList = new Dictionary<int, int>();
        public S2C_SnapshotShopInfo() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotShopInfo; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            NextFreshTime = ios.ReadInt64();
            int length = ios.ReadByte();
            for (int i = 0; i < length; i++)
            {
                int key = ios.ReadInt32();
                int value = ios.ReadInt16();
                if (EveryDayList.ContainsKey(key))
                    EveryDayList[key] = value;
                else
                    EveryDayList.Add(key,value);
            }
            length = ios.ReadByte();
            for (int i = 0; i < length; i++)
            {
                int key = ios.ReadInt32();
                int value = ios.ReadInt16();
                if (HistoryList.ContainsKey(key))
                    HistoryList[key] = value;
                else
                    HistoryList.Add(key,value);
            }
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_SnapshotBuyShopInfo : Packet
    {
        public int CommodityId;
        public int DayNum;
        public int HisNum;
        public S2C_SnapshotBuyShopInfo() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotBuyShopInfo; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            CommodityId = ios.ReadInt32();
            DayNum = ios.ReadInt16();
            HisNum = ios.ReadInt16();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    } 


    public sealed class S2C_BuyProduct : Packet
    {
        public GoodsToDrop[] TranslateList;
        public S2C_BuyProduct() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.BuyProduct; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
           TranslateList = GoodsToDrop.SerializeList(ios);
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_BuyProduct : Packet
    {
        public int StoreId;
        public int GoodsId;  //

        public override short NetCode
        {
            get { return (short)NetCode_C.BuyProduct; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(StoreId);
            ios.Write(GoodsId);
        }
    }
    public sealed class S2C_ShopInfo : Packet
    {
        public int ShopId;
        public S2C_ShopInfo() { }
        public override short NetCode
        {
            get { return (short)NetCode_S.ShopInfo; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            ShopId = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_ShopInfo : Packet
    {
        public int ShopId;  //

        public override short NetCode
        {
            get { return (short)NetCode_C.ShopInfo; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(ShopId);
        }
    }
  
    #endregion

    #region 战斗

    public sealed class C2S_BattleHand : Packet
    {
        public int[] SpellList;
        public override short NetCode
        {
            get { return (short)NetCode_C.BattleHand; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            for (int i = 0; i < SpellList.Length; i++)
            {
                ios.Write(SpellList[i]);
            }
        }
    }
    public sealed class S2C_BattleLog : Packet
    {
        public BattleRecordStr Record;
        public S2C_BattleLog() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.BattleLog; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            Record = new BattleRecordStr();
            Record.ReadFrom(ios,  BattleRecordStr.RecordType.OnlyAction);
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }


    #endregion

    #region 同步方法
    public sealed class S2C_SnapshotRole : Packet
    {
        public int    Idx;
        public string Name;
        public int Uid;

        public byte     Sex; //性别
        public Sect.SectType MySect;
        public short    GuideId;   //新手引导当前打点
        public byte     Vip;
        public short    Level;
        public int      CaveLevel;
        public long      CurExp;

        public int TeacherId; //师傅
        public int SectId;    //宗门
        public int Potential; //潜能
        public int WorldSec;  //秒，世界时间，从开服计算
        public int Diamond;
        public int  HeroIdx;
        public int  Silver; //银币
        public int  Gold;   //金币
        public long BirthTime; //毫秒 

        public int[] SpellList;
        public int[] EquipList;
        public int[] PetList;
        public AuxSkillLevel[] AuxSkillList;
        public PropLevelUp[] AuxToolList;
        public PrestigeLevel[] PrestigeList;
        public Dictionary<AttrType, int> AddProm;
        public List<int> IconList;
        public long ZazenStartTime;
        public int ZazenGoodsIdx;
        public long RetreatStartTime;
        public int RetreatGoodsIdx;
        public int InDungeonMapIdx;
        public bool IsInRank;
        public bool IsFinishNewerMap;
        public bool IsSetName;
        public int HeadIconIdx;
        public long NextVipDailyDiamond;
        public S2C_SnapshotRole() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotRole; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Idx     = ios.ReadInt32();
            Name    = NetUtils.ReadUTF(ios);
            IsInRank = ios.ReadBoolean();
            IsFinishNewerMap = ios.ReadBoolean();
            IsSetName = ios.ReadBoolean();

            Sex         = ios.ReadByte();
            CaveLevel   = ios.ReadByte();

            Level       = ios.ReadInt16();
            HeadIconIdx = ios.ReadInt32();
            Uid         = ios.ReadInt32();
            HeroIdx     = ios.ReadInt32();
            Gold        = ios.ReadInt32();
            CurExp      = ios.ReadInt64();
            Potential   = ios.ReadInt32();
            Diamond     = ios.ReadInt32();
            MySect      = (Sect.SectType)ios.ReadByte();
            BirthTime   = ios.ReadInt64();

            InDungeonMapIdx = ios.ReadInt32();

            ZazenStartTime = ios.ReadInt64();
            ZazenGoodsIdx = ios.ReadInt32();
            RetreatStartTime = ios.ReadInt64();
            RetreatGoodsIdx = ios.ReadInt32();

            NextVipDailyDiamond = ios.ReadInt64();

            int length = ios.ReadByte();
            SpellList = new int[length];
            for (int i = 0; i < length; i++)
                SpellList[i] = ios.ReadSByte();

            length = ios.ReadByte();
            EquipList = new int[length];
            for (int i = 0; i < length; i++)
                EquipList[i] = ios.ReadSByte();

            length = ios.ReadByte();
            PetList = new int[length];
            for (int i = 0; i < length; i++)
                PetList[i] = ios.ReadSByte();

            length = ios.ReadByte();
            PrestigeList = new PrestigeLevel[length];
            for (int i = 0; i < length; i++)
            {
                PrestigeList[i] = PrestigeLevel.PrestigeLevelFetcher.GetPrestigeLevelByCopy(ios.ReadByte(), (PrestigeLevel.PrestigeType)i);
                PrestigeList[i].CurPrestige = ios.ReadInt32();
            }

            length = ios.ReadByte();
            AddProm = new Dictionary<AttrType, int>();
            for (int i = 0; i < length; i++)
            {
                AddProm.Add((AttrType) ios.ReadByte(), ios.ReadInt32());
            }
            length = ios.ReadByte();
            IconList = new List<int>();
            for (int i = 0; i < length; i++)
            {
                IconList.Add(ios.ReadInt32());
            }


            //CaveAccessor的同步
            length = ios.ReadByte();
            AuxSkillList = new AuxSkillLevel[length];
            for (int i = 0; i < length; i++)
            {
                AuxSkillLevel skill = null;
                int level = ios.ReadByte();
                int curProficiency = ios.ReadInt32();
                AuxSkillLevel.SkillType type = (AuxSkillLevel.SkillType)i;
                if (level > 0) skill = AuxSkillLevel.AuxSkillFetcher.GetAuxSkilleByCopy(level,type);
                if (skill != null)
                {
                    skill.CurProficiency = curProficiency;
                    skill.mType = type;
                }
                else
                {
                    skill = new AuxSkillLevel();
                    skill.mType = type;
                }
                AuxSkillList[i] = skill;
            }

            length = ios.ReadByte();
            AuxToolList = new PropLevelUp[length];
            for (int i = 0; i < length; i++)
            {
                PropLevelUp tool = null;
                int level = ios.ReadByte();
                PropLevelUp.PropLevelType type = (PropLevelUp.PropLevelType)i;
                if (level > 0) tool = PropLevelUp.propLevelUpFetcher.GetPropLevelUpByCopy(level, type);
                if (tool != null)
                {
                    tool.mType = type;
                }
                else
                {
                    tool = new PropLevelUp();
                    tool.mType = type;
                }
                AuxToolList[i] = tool;
            }

          
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    public sealed class S2C_SnapshotEnterBotting : Packet
    {
        public List<int> CanRewardEventList = new List<int>();
        public List<ProduceItem> ProduceItemList = new List<ProduceItem>();
        public bool IsHaveNewMail;

        public int TaskFinishNum;
        public int TaskFreeFreshNum;
        public int CurTaskPos;
        public int CurTaskIdx;
        public long TaskStartTime;

        public int TowerFloorIndex;
        public int TowerFailNum;
        public S2C_SnapshotEnterBotting() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotEnterBotting; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            int length = ios.ReadByte();
            CanRewardEventList.Clear();
            ProduceItemList.Clear();
            for (int i = 0; i < length; i++)
            {
                CanRewardEventList.Add(ios.ReadInt32());
            }
            length = ios.ReadByte();
            for (int i = 0; i < length; i++)
            {
                ProduceItem item = new ProduceItem();
                item.ReadFrom(ios);
                ProduceItemList.Add(item);
            }
            IsHaveNewMail = ios.ReadBoolean();

            TaskFinishNum = ios.ReadByte();
            TaskFreeFreshNum = ios.ReadByte();
            CurTaskPos = ios.ReadSByte();
            CurTaskIdx = ios.ReadInt32();
            TaskStartTime = ios.ReadInt64();

            TowerFailNum = ios.ReadByte();
            TowerFloorIndex = ios.ReadInt16();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }

    }

    public sealed class S2C_SnapshotOffLine : Packet
    {
        public int Type; //0游历，1打坐
        public int Exp;
        public int Gold;
        public int Potential;
        public int ZazenUseItemIdx;
      //  public long OffLineTime;
        public long ZazenStartTime;
        public S2C_SnapshotOffLine() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotOffLine; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
          //  OffLineTime = ios.ReadInt64();
            Gold = ios.ReadInt32();
            Exp = ios.ReadInt32();
            Potential = ios.ReadInt32();
            Type = ios.ReadByte();

            //ZazenStartTime = ios.ReadInt64();
            //ZazenUseItemIdx = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_SnapshotPlayerAttribute : Packet
    {
        public PlayerAttribute Type;
        public int[] Attributes;

        public S2C_SnapshotPlayerAttribute() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotPlayerAttribute; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            Type    =   (PlayerAttribute)ios.ReadByte();

            int length = ios.ReadByte();
            Attributes = new int[length];
            for (int i = 0; i < length; i++)
                Attributes[i] = ios.ReadInt32();
           
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    public sealed class S2C_SnapshotPlayerAttributeLong : Packet
    {
        public PlayerAttribute Type;
        public long[] Attributes;

        public S2C_SnapshotPlayerAttributeLong() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotPlayerAttributeLong; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            Type = (PlayerAttribute)ios.ReadByte();

            int length = ios.ReadByte();
            Attributes = new long[length];
            for (int i = 0; i < length; i++)
                Attributes[i] = ios.ReadInt64();

        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }



    //同步已穿戴、未穿戴的技能信息
    public sealed class S2C_SnapshotInventory : Packet
    {
        public Spell[]  SpellInventory;    //招式
        public Equip[] EquipInventory;
        public Item[]   ItemInventory;      //功法
        public Recipe[] RecipeInventory;//配方
        public AuxSkillLevel[] AuxSkillList;
        public Pet[] PetList;
        public S2C_SnapshotInventory() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotInventory; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            int     length  = ios.ReadByte();
            Spell   spell   = null;
            SpellInventory  = new Spell[length];

            for (int i = 0; i < length; i++)
            {
                int idx = ios.ReadInt32();
                int curLevel = ios.ReadInt16();
                bool isEquip = ios.ReadBoolean();
                spell = Spell.Fetcher.GetSpellCopy(idx);
                if (spell != null)
                {
                    spell.curLevel = curLevel;
                    spell.curIsEquip = isEquip;
                    SpellInventory[i] = spell;
                }
            }
            //....equip
            length = ios.ReadByte();
            Equip equip = null;
            EquipInventory = new Equip[length];
            for (int i = 0; i < length; i++)
            {
                int idx = ios.ReadInt32();  //如果传0，当前位置为null
                bool isEquip = ios.ReadBoolean();
                if (idx != 0)
                {
                    equip = Equip.Fetcher.GetEquipCopy(idx);
                    if (equip != null)
                    {
                        equip.curIsEquip = isEquip;
                        EquipInventory[i] = equip;
                    }
                    else
                    {
                        TDebug.LogError(idx);
                    }
                }
                else
                {
                    EquipInventory[i] = new Equip();
                }
            }

            length = ios.ReadByte();
            Item item = null;
            ItemInventory = new Item[length];
            for (int i = 0; i < length; i++)
            {
                int idx = ios.ReadInt32();
                int curNum = ios.ReadInt32();
                item = Item.Fetcher.GetItemCopy(idx);
                if (item != null)
                {
                    item.num = curNum;
                    ItemInventory[i] = item;
                }
                else{ TDebug.LogError(idx); }
            }

            length = ios.ReadByte();
            Recipe recipe = null;
            RecipeInventory = new Recipe[length];
            for (int i = 0; i < length; i++)
            {
                int idx = ios.ReadInt32();
                recipe = Recipe.RecipeFetcher.GetRecipeByCopy(idx);
                if (recipe != null)
                {
                    RecipeInventory[i] = recipe;
                }
            }
            length = ios.ReadByte();
            Pet pet = null;
            PetList = new Pet[length];
            for (int i = 0; i < length; i++)
            {
                int idx = ios.ReadInt32();
                int curLevel = ios.ReadInt16();
                bool isEquiped = ios.ReadBoolean();
                pet = Pet.PetFetcher.GetPetByCopy(idx);
                if (pet != null)
                {
                    pet.CurLevel = curLevel;
                    pet.IsEquiped = isEquiped;
                    PetList[i] = pet;
                }
            }
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }


    //同步已穿戴、未穿戴的技能信息
    public sealed class S2C_SnapshotSpells : Packet
    {
        public int SpellId;
        public sbyte EquipPos;   //学习的是未装备的技能，为-1
        public sbyte InventoryPos;  //在背包中的位置(-1表示当前位置法宝为空)
        public sbyte InventoryPosOld;  //同步：装备槽位置、当前装备的背包位置、之前的装备背包位置
        public short curLevel;//当前强化数
        public S2C_SnapshotSpells() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotSpells; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            SpellId = ios.ReadInt32();
            EquipPos = ios.ReadSByte();
            InventoryPos = ios.ReadSByte();
            InventoryPosOld = ios.ReadSByte();
            curLevel = ios.ReadInt16();
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_SnapshotEquips : Packet
    {
        public sbyte EquipPos;     
        public sbyte InventoryPos;  //在背包中的位置(-1表示当前位置法宝为空)
        public sbyte InventoryPosOld;  //同步：装备槽位置、当前装备的背包位置、之前的装备背包位置
        public S2C_SnapshotEquips() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotEquips; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            EquipPos = ios.ReadSByte();
            InventoryPos = ios.ReadSByte();
            InventoryPosOld = ios.ReadSByte();
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_SnapshotAddEquip : Packet
    {
        public int Idx;
        public int Pos;

        public S2C_SnapshotAddEquip() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotAddEquip; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Pos = ios.ReadByte();
            Idx = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class S2C_SnapshotAddRecipe : Packet
    {
        public int Idx;
        public int Pos;

        public S2C_SnapshotAddRecipe() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotAddRecipe; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Pos = ios.ReadByte();
            Idx = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_SnapshotSpellObtain : Packet
    {
        public int Idx;
        public int Pos;

        public S2C_SnapshotSpellObtain() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotSpellObtain; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            Pos = ios.ReadByte();
            Idx = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_SnapshotBattleRecord : Packet
    {
        public string BattleRecordKey;
        public BattleRecordStr RecordStr;
        public S2C_SnapshotBattleRecord() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotBattleRecord; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            RecordStr = new BattleRecordStr();
            RecordStr.ReadFrom(ios);
        }

        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_BattleReport : Packet
    {
        public bool Winner;
        public S2C_BattleReport() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.BattleReport; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            Winner = ios.ReadBoolean();
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_GetLoot : Packet
    {
        public int NpcId;
        public Loot.LootActionType ActionType;
        public GoodsToDrop[] GoodsList;
        public S2C_GetLoot() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.GetLoot; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            ActionType = (Loot.LootActionType)ios.ReadByte();
            NpcId = ios.ReadInt32();
            GoodsList = GoodsToDrop.SerializeList(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    public sealed class S2C_SnapshotItem : Packet
    {
        public int Idx;
        public byte Pos;
        public int Num;

        public S2C_SnapshotItem() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotItem; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Pos = ios.ReadByte();
            Idx = ios.ReadInt32();
            Num = ios.ReadInt32();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_SnapshotExp : Packet
    {
        public long Num;

        public S2C_SnapshotExp() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotExp; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
            Num = ios.ReadInt64();
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    public sealed class S2C_SnapshotAuxSkill : Packet
    {
        public int Pos;
        public int Level;
        public int CurProficiency;

        public S2C_SnapshotAuxSkill() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotAuxSkill; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            Pos = ios.ReadByte();
            Level = ios.ReadInt16();
            CurProficiency = ios.ReadInt32();
            //isUnlock = ios.ReadBoolean();
        }

        public override void WriteTo(BinaryWriter ios)
        {

        }
    }



    public sealed class S2C_SnapshotSect : Packet
    {
        public Sect.SectType SectType;
        public S2C_SnapshotSect() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotSect; }
        }


        public override void ReadFrom(BinaryReader ios)
        {
            SectType = (Sect.SectType)ios.ReadByte();
        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    #endregion

    #region 邮件
    public sealed class C2S_ReceiveMail : Packet
    {
        public int MailIdx;
        public override short NetCode
        {
            get { return (short)NetCode_C.ReceiveMail; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(MailIdx);
        }
    }
    public sealed class S2C_ReceiveMail : Packet
    {
        public GoodsToDrop[] GoodsList;
        public S2C_ReceiveMail() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.ReceiveMail; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            GoodsList = GoodsToDrop.SerializeList(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    public sealed class C2S_MailDetail : Packet
    {
        public int MailIdx;
        public override short NetCode
        {
            get { return (short)NetCode_C.MailDetail; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(MailIdx);
        }
    }
    public sealed class S2C_MailDetail : Packet
    {
        public Mail DetailMail;
        public S2C_MailDetail() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.MailDetail; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            DetailMail = new Mail();
            DetailMail.SerializeDetail(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_SnapshotMail : Packet
    {
        public bool ReplaceOrAdd;
        public int MailAmount;
        public int RemoveMail;
        public List<Mail> MailList = new List<Mail>();
        public S2C_SnapshotMail() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotMail; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            ReplaceOrAdd = ios.ReadBoolean();
            MailAmount = ios.ReadByte();
            RemoveMail = ios.ReadInt32();
            MailList = new List<Mail>();
            int length = ios.ReadByte();
            Mail mail;
            for (int i = 0; i < length; i++)
            {
                mail = new Mail();
                mail.Serialize(ios);
                MailList.Add(mail);
            }
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    #endregion


    #region 声望任务
    public sealed class C2S_FreshPrestigeTask : Packet
    {
        public bool JustPullInfo;  //只是拉取信息，不进行主动刷新
        public bool UseDiamond;    //刷新是否使用钻石
        public PrestigeLevel.PrestigeType Type;
        public override short NetCode
        {
            get { return (short)NetCode_C.FreshPrestigeTask; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(JustPullInfo);
            ios.Write(UseDiamond);
            ios.Write((byte) Type);
        }
    }
    public sealed class S2C_FreshPrestigeTask : Packet
    {
        public S2C_FreshPrestigeTask() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.FreshPrestigeTask; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class C2S_StartPrestigeTask : Packet
    {
        public PrestigeLevel.PrestigeType Type;
        public int TaskIdx;
        public int TaskPos;
        public override short NetCode
        {
            get { return (short)NetCode_C.StartPrestigeTask; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write((byte)Type);
            ios.Write((byte) TaskPos);
            ios.Write(TaskIdx);
        }
    }
    public sealed class S2C_StartPrestigeTask : Packet
    {
        public S2C_StartPrestigeTask() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.StartPrestigeTask; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class C2S_FinishPrestigeTask : Packet
    {
        public bool UseDiamond;
        public override short NetCode
        {
            get { return (short)NetCode_C.FinishPrestigeTask; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(UseDiamond);
        }
    }
    public sealed class S2C_FinishPrestigeTask : Packet
    {
        public int FinishTask;
        public S2C_FinishPrestigeTask() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.FinishPrestigeTask; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            FinishTask = ios.ReadInt32();
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_SnapshotPrestigeTask : Packet
    {
        public PrestigeLevel.PrestigeType Type;
        public int FreeFreshNum;
        public List<int> TaskList;
        public S2C_SnapshotPrestigeTask() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotPrestigeTask; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            Type = (PrestigeLevel.PrestigeType)ios.ReadByte();
            FreeFreshNum = ios.ReadByte();
            int length = ios.ReadByte();
            TaskList = new List<int>();
            for (int i = 0; i < length; i++)
            {
                TaskList.Add(ios.ReadInt32());
            }
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class S2C_SnapshotPrestigeTaskDetail : Packet
    {
        public int FinishNum;
        public int FreeFreshNum;
        public int CurTask;
        public int CurTaskPos;
        public long StartTime;
        public S2C_SnapshotPrestigeTaskDetail() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotPrestigeTaskDetail; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            FinishNum = ios.ReadByte();
            FreeFreshNum = ios.ReadByte();
            CurTaskPos = ios.ReadSByte();
            CurTask = ios.ReadInt32();
            StartTime = ios.ReadInt64();
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    #endregion

    #region 成就
    public sealed class S2C_SnapshotAchieve : Packet
    {
        public int ZazenTime;
        public int AddGold;
        public int ConsumeGold;
        public int ConsumeDiamond;
        public int AdvertNum;
        public int MarketPurchase;
        public int ProduceDrug;
        public int GetMine;
        public int GetHerb;
        public int LevelUpPet;
        public int EvolvePet;

        public int[] ProduceEquip;//品质
        public int[] FinishPrestigeTask;//类型
        public Dictionary<int, int> AchieveMap; //

        public int LastFinish0;  //最近完成的成就
        public int LastFinishPos0;
        public int LastFinish1;  //上一个最近完成的成就
        public int LastFinishPos1;

        public int AchievePoint;
        public S2C_SnapshotAchieve() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotAchieve; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            this.AchievePoint = ios.ReadInt32();
            this.LastFinish0 = ios.ReadInt32();
            this.LastFinishPos0 = ios.ReadByte();
            this.LastFinish1 = ios.ReadInt32();
            this.LastFinishPos1 = ios.ReadByte();
  
            this.ZazenTime = ios.ReadInt32();
            this.AddGold = ios.ReadInt32();
            this.ConsumeGold = ios.ReadInt32();
            this.ConsumeDiamond = ios.ReadInt32();
            this.AdvertNum = ios.ReadInt32();
            this.MarketPurchase = ios.ReadInt32();
            this.ProduceDrug = ios.ReadInt32();
            this.GetMine = ios.ReadInt32();
            this.GetHerb = ios.ReadInt32();
            this.LevelUpPet = ios.ReadInt32();
            this.EvolvePet = ios.ReadInt32();

            int length = ios.ReadByte();
            this.ProduceEquip = new int[length];
            for (int i = 0; i < length; i++)
            {
                this.ProduceEquip[i] = ios.ReadInt32();
            }

            length = ios.ReadByte();
            this.FinishPrestigeTask = new int[length];
            for (int i = 0; i < length; i++)
            {
                this.FinishPrestigeTask[i] = ios.ReadInt32();
            }
            length = ios.ReadInt16();
            this.AchieveMap = new Dictionary<int, int>();
            for (int i = 0; i < length; i++)
            {
                int key = ios.ReadInt32();
                int value = ios.ReadByte();
                this.AchieveMap.Add(key, value);
            }
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    public sealed class S2C_SnapshotAchieveFinish : Packet
    {
        public int AchievePoint;
        public int SecondLastFinish;
        public int SecondLastFinishPos;
        public int LastFinish;
        public int LastFinishPos;

        public List<int> FinishPosList = new List<int>();

        public S2C_SnapshotAchieveFinish() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.SnapshotAchieveFinish; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            this.AchievePoint = ios.ReadInt32();
            this.SecondLastFinish = ios.ReadInt32();
            this.SecondLastFinishPos = ios.ReadByte();

            this.LastFinish = ios.ReadInt32();
            this.LastFinishPos = ios.ReadByte();
            int length = ios.ReadByte();
            FinishPosList.Clear();
            for (int i = 0; i < length; i++)
            {
                FinishPosList.Add(ios.ReadByte());
            }
    
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    public sealed class C2S_GetAchieveReward : Packet
    {
        public int RewardId;  
        public override short NetCode
        {
            get { return (short)NetCode_C.GetAchieveReward; }
        }
        public override void ReadFrom(BinaryReader ios)
        {
        }

        public override void WriteTo(BinaryWriter ios)
        {
            ios.Write(RewardId);
        }
    }
    public sealed class S2C_GetAchieveReward : Packet
    {
        public GoodsToDrop[] TranslateList;
        public int rewardId;

        public S2C_GetAchieveReward() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.GetAchieveReward; }
        }

        public override void ReadFrom(BinaryReader ios)
        {
            rewardId = ios.ReadInt32();
            TranslateList = GoodsToDrop.SerializeList(ios);
        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    #endregion

    #region VIP
    public sealed class C2S_BuyVIP : Packet
    {
        public override short NetCode
        {
            get { return (short)NetCode_C.BuyVIP; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }
    public sealed class C2S_GetVIPAward : Packet
    {
        public override short NetCode
        {
            get { return (short)NetCode_C.GetVIPDailyAward; }
        }
        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {

        }
    }

    public sealed class S2C_BuyVIP : Packet
    {
        public S2C_BuyVIP() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.BuyVIP; }
        }

        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }
    public sealed class S2C_GetVIPAward : Packet
    {
        public S2C_GetVIPAward() { }

        public override short NetCode
        {
            get { return (short)NetCode_S.GetVIPDailyAward; }
        }

        public override void ReadFrom(BinaryReader ios)
        {

        }
        public override void WriteTo(BinaryWriter ios)
        {
        }
    }

    #endregion

}
