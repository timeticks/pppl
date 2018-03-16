using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MessageBridge 
{

    private Dictionary<NetCode_C, IPacket> mNCodePool   = new Dictionary<NetCode_C, IPacket>(128);
    private Dictionary<NetCode_S, IPacket> mNSCodePool  = new Dictionary<NetCode_S, IPacket>(128);
    private static MessageBridge mInstance;
    public static MessageBridge Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new MessageBridge();
            }
            return mInstance;
        }
    }
    
    #region 服务器同步模块
    public NetPacket.S2C_SnapshotRole S2C_SnapshotRole(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotRole, out packet))
        {
            packet = new NetPacket.S2C_SnapshotRole();
            mNSCodePool.Add(NetCode_S.SnapshotRole, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotRole)packet;
    }
    public NetPacket.S2C_SnapshotEnterBotting S2C_SnapshotEnterBotting(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotEnterBotting, out packet))
        {
            packet = new NetPacket.S2C_SnapshotEnterBotting();
            mNSCodePool.Add(NetCode_S.SnapshotEnterBotting, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotEnterBotting)packet;
    }

    public NetPacket.S2C_SnapshotPlayerAttribute S2C_SnapshotPlayerAttribute(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotPlayerAttribute, out packet))
        {
            packet = new NetPacket.S2C_SnapshotPlayerAttribute();
            mNSCodePool.Add(NetCode_S.SnapshotPlayerAttribute, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotPlayerAttribute)packet;
    }
    public NetPacket.S2C_SnapshotPlayerAttributeLong S2C_SnapshotPlayerAttributeLong(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotPlayerAttributeLong, out packet))
        {
            packet = new NetPacket.S2C_SnapshotPlayerAttributeLong();
            mNSCodePool.Add(NetCode_S.SnapshotPlayerAttributeLong, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotPlayerAttributeLong)packet;
    }

    public NetPacket.S2C_SnapshotInventory S2C_SnapshotInventory(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotInventory, out packet))
        {
            packet = new NetPacket.S2C_SnapshotInventory();
            mNSCodePool.Add(NetCode_S.SnapshotInventory, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotInventory)packet;
    }

    public NetPacket.S2C_SnapshotSpells S2C_SnapshotSpells(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotSpells, out packet))
        {
            packet = new NetPacket.S2C_SnapshotSpells();
            mNSCodePool.Add(NetCode_S.SnapshotSpells, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotSpells)packet;
    }
    public NetPacket.S2C_SnapshotEquips S2C_SnapshotEquips(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotEquips, out packet))
        {
            packet = new NetPacket.S2C_SnapshotEquips();
            mNSCodePool.Add(NetCode_S.SnapshotEquips, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotEquips)packet;
    }
    public NetPacket.S2C_SnapshotItem S2C_SnapshotItem(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotItem, out packet))
        {
            packet = new NetPacket.S2C_SnapshotItem();
            mNSCodePool.Add(NetCode_S.SnapshotItem, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotItem)packet;
    }
    public NetPacket.S2C_SnapshotAuxSkill S2C_SnapshotAuxSkill(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotAuxSkill, out packet))
        {
            packet = new NetPacket.S2C_SnapshotAuxSkill();
            mNSCodePool.Add(NetCode_S.SnapshotAuxSkill, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotAuxSkill)packet;
    }
    public NetPacket.S2C_SnapshotAddEquip S2C_SnapshotAddEquip(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotAddEquip, out packet))
        {
            packet = new NetPacket.S2C_SnapshotAddEquip();
            mNSCodePool.Add(NetCode_S.SnapshotAddEquip, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotAddEquip)packet;
    }

    public NetPacket.S2C_SnapshotAddRecipe S2C_SnapshotAddRecipe(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotAddRecipe, out packet))
        {
            packet = new NetPacket.S2C_SnapshotAddRecipe();
            mNSCodePool.Add(NetCode_S.SnapshotAddRecipe, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotAddRecipe)packet;
    }

    public NetPacket.S2C_SnapshotSpellObtain S2C_SnapshotSpellObtain(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotSpellObtain, out packet))
        {
            packet = new NetPacket.S2C_SnapshotSpellObtain();
            mNSCodePool.Add(NetCode_S.SnapshotSpellObtain, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotSpellObtain)packet;
    }

    public NetPacket.S2C_SnapshotBattleRecord S2C_SnapshotBattleRecord(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotBattleRecord, out packet))
        {
            packet = new NetPacket.S2C_SnapshotBattleRecord();
            mNSCodePool.Add(NetCode_S.SnapshotBattleRecord, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotBattleRecord)packet;
    }

    public NetPacket.S2C_BattleReport S2C_BattleReport(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.BattleReport, out packet))
        {
            packet = new NetPacket.S2C_BattleReport();
            mNSCodePool.Add(NetCode_S.BattleReport, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_BattleReport)packet;
    }

    public NetPacket.S2C_GetLoot S2C_GetLoot(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.GetLoot, out packet))
        {
            packet = new NetPacket.S2C_GetLoot();
            mNSCodePool.Add(NetCode_S.GetLoot, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_GetLoot)packet;
    }

    public NetPacket.S2C_SnapshotExp S2C_SnapshotExp(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotExp, out packet))
        {
            packet = new NetPacket.S2C_SnapshotExp();
            mNSCodePool.Add(NetCode_S.SnapshotExp, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotExp)packet;
    }
#endregion

    #region 游戏系统模块
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IPacket C2S_SnapshotTime()
    {
        IPacket packet = null;

        if (mNCodePool.TryGetValue(NetCode_C.SnapshotTime, out packet))
        {
            NetPacket.C2S_SnapshotTime msg = (NetPacket.C2S_SnapshotTime)packet;
            return msg;
        }
        else
        {
            packet = new NetPacket.C2S_SnapshotTime();
            mNCodePool.Add(NetCode_C.SnapshotTime, packet);
            return packet;
        }
    }
    /// <summary>
    /// 服务器时间同步
    /// </summary>
    /// <param name="ios"></param>
    /// <returns></returns>
    public NetPacket.S2C_SnapshotTime S2C_SnapshotTime(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SnapshotTime, out packet))
        {
            NetPacket.S2C_SnapshotTime msg = (NetPacket.S2C_SnapshotTime)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SnapshotTime();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SnapshotTime, packet);
            return (NetPacket.S2C_SnapshotTime)packet;
        }
    }

    /// <summary>
    /// 离线收益
    /// </summary>
    /// <param name="ios"></param>
    /// <returns></returns>
    public NetPacket.S2C_SnapshotOffLine S2C_SnapshotOffLine(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotOffLine, out packet))
        {
            packet = new NetPacket.S2C_SnapshotOffLine();
            mNSCodePool.Add(NetCode_S.SnapshotOffLine, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotOffLine)packet;
    }
#endregion

    #region 登陆模块

    /// <summary>
    /// 获取登陆状态
    /// </summary>
    /// <returns></returns>
    public IPacket C2S_VerifyLogin()
    {
        IPacket packet = null;

        if (mNCodePool.TryGetValue(NetCode_C.VerifyLogin, out packet))
        {
            NetPacket.C2S_VerifyLogin msg = (NetPacket.C2S_VerifyLogin)packet;
            return msg;
        }
        else
        {
            packet = new NetPacket.C2S_VerifyLogin();
            mNCodePool.Add(NetCode_C.VerifyLogin, packet);
            return packet;
        }
    }
    /// <summary>
    /// 获取登陆状态
    /// </summary>
    /// <returns></returns>
    public IPacket C2S_Register(string username,string passwd ,string agent,PlatformType platform)
    {
        IPacket                     packet  = null;
        NetPacket.C2S_Register    msg     = null;
        if (mNCodePool.TryGetValue(NetCode_C.Register, out packet))
        {
            msg = (NetPacket.C2S_Register)packet;
        }
        else
        {
            msg = new NetPacket.C2S_Register();
            mNCodePool.Add(NetCode_C.Register, msg);
        }

        msg.Platform= platform;
        msg.Agent   = agent;
        msg.Username= username;
        msg.Password= passwd;
        return msg;
    }

    /// <summary>
    /// 获取登陆状态
    /// </summary>
    /// <returns></returns>
    public IPacket C2S_Login(string username, string passwd)
    {
        IPacket             packet  = null;
        NetPacket.C2S_Login msg     = null;
        if (mNCodePool.TryGetValue(NetCode_C.Login, out packet))
        {
            msg = (NetPacket.C2S_Login)packet;
        }
        else
        {
            msg = new NetPacket.C2S_Login();
            mNCodePool.Add(NetCode_C.Login, msg);
        }

        msg.Username    = username;
        msg.Password    = passwd;
        return msg;
    }

    /// <summary>
    /// 快速登录
    /// </summary>
    public IPacket C2S_FastRegister(string agent, PlatformType platform)
    {
        IPacket packet = null;
        NetPacket.C2S_FastRegister msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.FastRegister, out packet))
        {
            msg = (NetPacket.C2S_FastRegister)packet;
        }
        else
        {
            msg = new NetPacket.C2S_FastRegister();
            mNCodePool.Add(NetCode_C.FastRegister, msg);
        }

        msg.Platform    = platform;
        msg.Agent       = agent;
        return msg;
    }
    
    /// <summary>
    /// 获取登陆状态
    /// </summary>
    /// <returns></returns>
    public IPacket C2S_CreateRole(OldHero.Sex sex)
    {
        IPacket                     packet  = null;
        NetPacket.C2S_CreateRole    msg     = null;
        if (mNCodePool.TryGetValue(NetCode_C.CreateRole, out packet))
        {
            msg = (NetPacket.C2S_CreateRole)packet;
        }
        else
        {
            msg = new NetPacket.C2S_CreateRole();
            mNCodePool.Add(NetCode_C.CreateRole, msg);
        }

        msg.Sex         = sex;
        return msg;
    }
    public IPacket C2S_EnterGame(int roleIdx)
    {
        IPacket packet = null;
        NetPacket.C2S_EnterGame msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.EnterGame, out packet))
        {
            msg = (NetPacket.C2S_EnterGame)packet;
        }
        else
        {
            msg = new NetPacket.C2S_EnterGame();
            mNCodePool.Add(NetCode_C.EnterGame, msg);
        }

        msg.RoleIdx = roleIdx;
        return msg;
    }

    public IPacket C2S_RemoveRoleSave(int roleIdx)
    {
        IPacket packet = null;
        NetPacket.C2S_RemoveRoleSave msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.RemoveSaveRole, out packet))
        {
            msg = (NetPacket.C2S_RemoveRoleSave)packet;
        }
        else
        {
            msg = new NetPacket.C2S_RemoveRoleSave();
            mNCodePool.Add(NetCode_C.RemoveSaveRole, msg);
        }

        msg.RoleIdx = roleIdx;
        return msg;
    }
    public NetPacket.S2C_RemoveRoleSave S2C_RemoveRoleSave(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.RemoveSaveRole, out packet))
        {
            packet = new NetPacket.S2C_Login();
            mNSCodePool.Add(NetCode_S.RemoveSaveRole, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_RemoveRoleSave)packet;
    }
    /// <summary>
    /// 服务器登陆回调
    /// </summary>
    /// <returns></returns>
    public NetPacket.S2C_Login S2C_Login(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.Login, out packet))
        {
            packet = new NetPacket.S2C_Login();
            mNSCodePool.Add(NetCode_S.Login, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_Login)packet;
    }

    /// <summary>
    /// 快速登录回调
    /// </summary>
    public NetPacket.S2C_FastRegister S2C_FastRegister(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.FastRegister, out packet))
        {
            packet = new NetPacket.S2C_FastRegister();
            mNSCodePool.Add(NetCode_S.FastRegister, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_FastRegister)packet;
    }
    public NetPacket.S2C_EnterGame S2C_EnterGame(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.EnterGame, out packet))
        {
            packet = new NetPacket.S2C_EnterGame();
            mNSCodePool.Add(NetCode_S.EnterGame, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_EnterGame)packet;
    }
    public NetPacket.S2C_CreateRole S2C_CreateRole(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.CreateRole, out packet))
        {
            packet = new NetPacket.S2C_CreateRole();
            mNSCodePool.Add(NetCode_S.CreateRole, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_CreateRole)packet;
    }
    #endregion

    #region 纳戒

    #region 背包
    public IPacket C2S_SellItem(int idx, int num)
    {
        IPacket packet = null;
        NetPacket.C2S_SellItem msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.SellItem, out packet))
        {
            msg = (NetPacket.C2S_SellItem)packet;
        }
        else
        {
            msg = new NetPacket.C2S_SellItem();
            mNCodePool.Add(NetCode_C.SellItem, msg);
        }
        msg.Idx = idx;
        msg.Num = num;

        return msg;
    }
    public NetPacket.S2C_SellItem S2C_SellItem(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SellItem, out packet))
        {
            NetPacket.S2C_SellItem msg = (NetPacket.S2C_SellItem)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SellItem();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SellItem, packet);
            return (NetPacket.S2C_SellItem)packet;
        }
    }

    public IPacket C2S_UseItem(int idx, int num)
    {
        IPacket packet = null;
        NetPacket.C2S_UseItem msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.UseItem, out packet))
        {
            msg = (NetPacket.C2S_UseItem)packet;
        }
        else
        {
            msg = new NetPacket.C2S_UseItem();
            mNCodePool.Add(NetCode_C.UseItem, msg);
        }
        msg.Idx = idx;
        msg.Num = num;

        return msg;
    }
    public NetPacket.S2C_UseItem S2C_UseItem(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.UseItem, out packet))
        {
            NetPacket.S2C_UseItem msg = (NetPacket.S2C_UseItem)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_UseItem();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.UseItem, packet);
            return (NetPacket.S2C_UseItem)packet;
        }
    }
    #endregion

    #region 功法
    public IPacket C2S_EquipSpell(sbyte equipsPos, sbyte inventoryPos)
    {
        IPacket packet = null;
        NetPacket.C2S_EquipSpell msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.EquipSpell, out packet))
        {
            msg = (NetPacket.C2S_EquipSpell)packet;
        }
        else
        {
            msg = new NetPacket.C2S_EquipSpell();
            mNCodePool.Add(NetCode_C.EquipSpell, msg);
        }
        msg.InventoryPos = inventoryPos;
        msg.EquipPos = equipsPos;
        return msg;
    }  
    public IPacket C2S_StudySpell(sbyte equipsPos,bool fastStudy)
    {
        IPacket packet = null;
        NetPacket.C2S_StudySpell msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.StudySpell, out packet))
        {
            msg = (NetPacket.C2S_StudySpell)packet;
        }
        else
        {
            msg = new NetPacket.C2S_StudySpell();
            mNCodePool.Add(NetCode_C.StudySpell, msg);
        }
        //    msg.SpellIdx = spellIdx;
        msg.EquipPos = equipsPos;
        msg.IsFastStudy = fastStudy;
        return msg;
    }
    public NetPacket.S2C_EquipSpell S2C_EquipSpell(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.EquipSpell, out packet))
        {
            packet = new NetPacket.S2C_EquipSpell();
            mNSCodePool.Add(NetCode_S.EquipSpell, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_EquipSpell)packet;
    }
    public NetPacket.S2C_StudySpell S2C_StudySpell(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.StudySpell, out packet))
        {
            packet = new NetPacket.S2C_StudySpell();
            mNSCodePool.Add(NetCode_S.StudySpell, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_StudySpell)packet;
    }
   
  
    #endregion

    #region 法宝
    public IPacket C2S_EquipEquip(byte equipsPos, sbyte inventoryPos)
    {
        IPacket packet = null;
        NetPacket.C2S_EquipEquip msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.EquipEquip, out packet))
        {
            msg = (NetPacket.C2S_EquipEquip)packet;
        }
        else
        {
            msg = new NetPacket.C2S_EquipEquip();
            mNCodePool.Add(NetCode_C.EquipEquip, msg);
        }
        msg.InventoryPos = inventoryPos;
        msg.EquipPos = equipsPos;
        return msg;
    }
    public IPacket C2S_SellEquip(List<byte> sellEquipList)
    {
        IPacket packet = null;
        NetPacket.C2S_SellEquip msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.SellEquip, out packet))
        {
            msg = (NetPacket.C2S_SellEquip)packet;
        }
        else
        {
            msg = new NetPacket.C2S_SellEquip();
            mNCodePool.Add(NetCode_C.SellEquip, msg);
        }
        msg.SellEquipList = sellEquipList;
        return msg;
    }
    public NetPacket.S2C_EquipEquip S2C_EquipEquip(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.EquipEquip, out packet))
        {
            packet = new NetPacket.S2C_EquipEquip();
            mNSCodePool.Add(NetCode_S.EquipEquip, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_EquipEquip)packet;
    }
    public NetPacket.S2C_SellEquip S2C_SellEquip(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SellEquip, out packet))
        {
            packet = new NetPacket.S2C_SellEquip();
            mNSCodePool.Add(NetCode_S.SellEquip, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SellEquip)packet;
    }
    #endregion

    #region 宠物
    public NetPacket.S2C_SnapshotEquipPet S2C_SnapshotEquipPet(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SnapshotEquipPet, out packet))
        {
            NetPacket.S2C_SnapshotEquipPet msg = (NetPacket.S2C_SnapshotEquipPet)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SnapshotEquipPet();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SnapshotEquipPet, packet);
            return (NetPacket.S2C_SnapshotEquipPet)packet;
        }
    }
    public NetPacket.S2C_SnapshotPet S2C_SnapshotPet(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotPet, out packet))
        {
            packet = new NetPacket.S2C_SnapshotPet();
            mNSCodePool.Add(NetCode_S.SnapshotPet, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotPet)packet;
    }

    public IPacket C2S_PetLevelUp(int inventoryPos)
    {
        IPacket packet = null;
        NetPacket.C2S_PetLevelUp msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.PetLevelUp, out packet))
        {
            msg = (NetPacket.C2S_PetLevelUp)packet;
        }
        else
        {
            msg = new NetPacket.C2S_PetLevelUp();
            mNCodePool.Add(NetCode_C.PetLevelUp, msg);
        }
        msg.inventoryPos = inventoryPos;
        return msg;
    }
    public IPacket C2S_EquipPet(sbyte inventoryPos, byte equipPos)
    {
        IPacket packet = null;
        NetPacket.C2S_EquipPet msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.EuqipPet, out packet))
        {
            msg = (NetPacket.C2S_EquipPet)packet;
        }
        else
        {
            msg = new NetPacket.C2S_EquipPet();
            mNCodePool.Add(NetCode_C.EuqipPet, msg);
        }
        msg.InventoryPos = inventoryPos;
        msg.EquipPos = equipPos;
        return msg;
    }

    public NetPacket.S2C_PetLevelUp S2C_PetLevelUp(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.PetLevelUp, out packet))
        {
            NetPacket.S2C_PetLevelUp msg = (NetPacket.S2C_PetLevelUp)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_PetLevelUp();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.PetLevelUp, packet);
            return (NetPacket.S2C_PetLevelUp)packet;
        }
    }
    public NetPacket.S2C_EquipPet S2C_EquipPet(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.EquipPet, out packet))
        {
            NetPacket.S2C_EquipPet msg = (NetPacket.S2C_EquipPet)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_EquipPet();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.EquipPet, packet);
            return (NetPacket.S2C_EquipPet)packet;
        }
    }
    #endregion

    #endregion
   
    #region 秘境

    //////////////////////////////////////////秘境/////////////////////////////////////////////////
    public IPacket C2S_EnterDungeonMap(int mapIdx)
    {
        IPacket packet = null;
        NetPacket.C2S_EnterDungeonMap msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.EnterDungeonMap, out packet))
        {
            msg = (NetPacket.C2S_EnterDungeonMap)packet;
        }
        else
        {
            msg = new NetPacket.C2S_EnterDungeonMap();
            mNCodePool.Add(NetCode_C.EnterDungeonMap, msg);
        }
        msg.MapIdx = mapIdx;
        return msg;
    }
    public NetPacket.S2C_EnterDungeonMap S2C_EnterDungeonMap(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.EnterDungeonMap, out packet))
        {
            NetPacket.S2C_EnterDungeonMap msg = (NetPacket.S2C_EnterDungeonMap)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_EnterDungeonMap();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.EnterDungeonMap, packet);
            return (NetPacket.S2C_EnterDungeonMap)packet;
        }
    }
    public NetPacket.S2C_SnapshotDungeonMap S2C_SnapshotDungeonMap(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SnapshotDungeonMap, out packet))
        {
            NetPacket.S2C_SnapshotDungeonMap msg = (NetPacket.S2C_SnapshotDungeonMap)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SnapshotDungeonMap();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SnapshotDungeonMap, packet);
            return (NetPacket.S2C_SnapshotDungeonMap)packet;
        }
    }

    public IPacket C2S_CheckCondition(MapData.ConditionType conditionTy , int idx , int num)
    {
        IPacket packet = null;
        NetPacket.C2S_CheckCondition msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.CheckCondition, out packet))
        {
            msg = (NetPacket.C2S_CheckCondition)packet;
        }
        else
        {
            msg = new NetPacket.C2S_CheckCondition();
            mNCodePool.Add(NetCode_C.CheckCondition, msg);
        }
        msg.ConditionType = conditionTy;
        msg.ConditionIdx = idx;
        msg.ConditionValue = num;
        return msg;
    }
    public NetPacket.S2C_CheckCondition S2C_CheckCondition(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.CheckCondition, out packet))
        {
            NetPacket.S2C_CheckCondition msg = (NetPacket.S2C_CheckCondition)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_CheckCondition();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.CheckCondition, packet);
            return (NetPacket.S2C_CheckCondition)packet;
        }
    }

    public IPacket C2S_MapSave(List<NetMapSaveItem> saveList , int rolePos)
    {
        IPacket packet = null;
        NetPacket.C2S_MapSave msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.MapSave, out packet))
        {
            msg = (NetPacket.C2S_MapSave)packet;
        }
        else
        {
            msg = new NetPacket.C2S_MapSave();
            mNCodePool.Add(NetCode_C.MapSave, msg);
        }
        msg.RolePos = rolePos;
        msg.SaveList = saveList;

        return msg;
    }
    public NetPacket.S2C_MapSave S2C_MapSave(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.MapSave, out packet))
        {
            NetPacket.S2C_MapSave msg = (NetPacket.S2C_MapSave)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_MapSave();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.MapSave, packet);
            return (NetPacket.S2C_MapSave)packet;
        }
    }
    public NetPacket.S2C_SnapshotMapEvent S2C_SnapshotMapEvent(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SnapshotMapEvent, out packet))
        {
            NetPacket.S2C_SnapshotMapEvent msg = (NetPacket.S2C_SnapshotMapEvent)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SnapshotMapEvent();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SnapshotMapEvent, packet);
            return (NetPacket.S2C_SnapshotMapEvent)packet;
        }
    }

    public IPacket C2S_MapEventReward(int eventId)
    {
        IPacket packet = null;
        NetPacket.C2S_MapEventReward msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.MapEventReward, out packet))
        {
            msg = (NetPacket.C2S_MapEventReward)packet;
        }
        else
        {
            msg = new NetPacket.C2S_MapEventReward();
            mNCodePool.Add(NetCode_C.MapEventReward, msg);
        }
        msg.EventId = eventId;
        return msg;
    }
    public NetPacket.S2C_MapEventReward S2C_MapEventReward(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.MapEventReward, out packet))
        {
            NetPacket.S2C_MapEventReward msg = (NetPacket.S2C_MapEventReward)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_MapEventReward();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.MapEventReward, packet);
            return (NetPacket.S2C_MapEventReward)packet;
        }
    }

    public IPacket C2S_MapEnd(int endId , MapEndType endType)//-1为死亡退出
    {
        IPacket packet = null;
        NetPacket.C2S_MapEnd msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.MapEnd, out packet))
        {
            msg = (NetPacket.C2S_MapEnd)packet;
        }
        else
        {
            msg = new NetPacket.C2S_MapEnd();
            mNCodePool.Add(NetCode_C.MapEnd, msg);
        }
        msg.EndId = endId;
        msg.EndType = endType;
        return msg;
    }
    public NetPacket.S2C_MapEnd S2C_MapEnd(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.MapEnd, out packet))
        {
            NetPacket.S2C_MapEnd msg = (NetPacket.S2C_MapEnd)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_MapEnd();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.MapEnd, packet);
            return (NetPacket.S2C_MapEnd)packet;
        }
    }

    #endregion

    #region 宗门
    public IPacket C2S_SetSect(Sect.SectType ty)
    {
        IPacket packet = null;
        NetPacket.C2S_SetSect msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.SetSect, out packet))
        {
            msg = (NetPacket.C2S_SetSect)packet;
        }
        else
        {
            msg = new NetPacket.C2S_SetSect();
            mNCodePool.Add(NetCode_C.SetSect, msg);
        }
        msg.SectType = ty;
        return msg;
    }
    public NetPacket.S2C_SetSect S2C_SetSect(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SetSect, out packet))
        {
            NetPacket.S2C_SetSect msg = (NetPacket.S2C_SetSect)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SetSect();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SetSect, packet);
            return (NetPacket.S2C_SetSect)packet;
        }
    }
    public NetPacket.S2C_SnapshotSect S2C_SnapshotSect(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SnapshotSect, out packet))
        {
            NetPacket.S2C_SnapshotSect msg = (NetPacket.S2C_SnapshotSect)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SnapshotSect();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SnapshotSect, packet);
            return (NetPacket.S2C_SnapshotSect)packet;
        }
    }

    public IPacket C2S_SpellObtain(int spellObtainIdx , int spellIndex)
    {
        IPacket packet = null;
        NetPacket.C2S_SpellObtain msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.SpellObtain, out packet))
        {
            msg = (NetPacket.C2S_SpellObtain)packet;
        }
        else
        {
            msg = new NetPacket.C2S_SpellObtain();
            mNCodePool.Add(NetCode_C.SpellObtain, msg);
        }
        msg.SpellObtainIdx = spellObtainIdx;
        msg.SpellIndex = (byte)spellIndex;
        return msg;
    }
    public NetPacket.S2C_SpellObtain S2C_SpellObtain(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SpellObtain, out packet))
        {
            NetPacket.S2C_SpellObtain msg = (NetPacket.S2C_SpellObtain)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SpellObtain();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SpellObtain, packet);
            return (NetPacket.S2C_SpellObtain)packet;
        }
    }

    #endregion

    #region 起名
    public NetPacket.S2C_SnapshotPlayerName S2C_SnapshotPlayerName(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SnapshotPlayerName, out packet))
        {
            NetPacket.S2C_SnapshotPlayerName msg = (NetPacket.S2C_SnapshotPlayerName)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SnapshotPlayerName();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SnapshotPlayerName, packet);
            return (NetPacket.S2C_SnapshotPlayerName)packet;
        }
    }
    public IPacket C2S_SetPlayerName(string name)
    {
        IPacket packet = null;
        NetPacket.C2S_SetPlayerName msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.SetPlayerName, out packet))
        {
            msg = (NetPacket.C2S_SetPlayerName)packet;
        }
        else
        {
            msg = new NetPacket.C2S_SetPlayerName();
            mNCodePool.Add(NetCode_C.SetPlayerName, msg);
        }
        msg.Name = name;
        return msg;
    }
    public NetPacket.S2C_SetPlayerName S2C_SetPlayerName(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SetPlayerName, out packet))
        {
            NetPacket.S2C_SetPlayerName msg = (NetPacket.S2C_SetPlayerName)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SetPlayerName();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SetPlayerName, packet);
            return (NetPacket.S2C_SetPlayerName)packet;
        }
    }
    #endregion

    #region PVE
    public IPacket C2S_EnterPVE(BattleType battleType, int enemyIdx, PVESceneType sceneType)
    {
        IPacket packet = null;
        NetPacket.C2S_EnterPVE msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.EnterPVE, out packet))
        {
            msg = (NetPacket.C2S_EnterPVE)packet;
        }
        else
        {
            msg = new NetPacket.C2S_EnterPVE();
            mNCodePool.Add(NetCode_C.EnterPVE, msg);
        }
        msg.EnemyIdx = enemyIdx;
        msg.BattleType = battleType;
        msg.SceneType = sceneType;
        return msg;
    }
    public NetPacket.S2C_EnterPVE S2C_EnterPVE(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.EnterPVE, out packet))
        {
            NetPacket.S2C_EnterPVE msg = (NetPacket.S2C_EnterPVE)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_EnterPVE();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.EnterPVE, packet);
            return (NetPacket.S2C_EnterPVE)packet;
        }
    }
    public IPacket C2S_RetreatFinish(bool useDiamond =false)
    {
        IPacket packet = null;
        NetPacket.C2S_RetreatFinish msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.RetreatFinish, out packet))
        {
            msg = (NetPacket.C2S_RetreatFinish)packet;
        }
        else
        {
            msg = new NetPacket.C2S_RetreatFinish();
            mNCodePool.Add(NetCode_C.RetreatFinish, msg);
        }
        msg.UseDiamond = useDiamond;
        return msg;
    }
    #endregion

    #region 排行
    public IPacket C2S_PullRank(byte type)
    {
        IPacket packet = null;
        NetPacket.C2S_PullRank msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.PullRank, out packet))
        {
            msg = (NetPacket.C2S_PullRank)packet;
        }
        else
        {
            msg = new NetPacket.C2S_PullRank();
            mNCodePool.Add(NetCode_C.PullRank, msg);
        }
        msg.Type = type;
        return msg;
    }
    public NetPacket.S2C_PullRank S2C_PullRank(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.PullRank, out packet))
        {
            NetPacket.S2C_PullRank msg = (NetPacket.S2C_PullRank)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_PullRank();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.PullRank, packet);
            return (NetPacket.S2C_PullRank)packet;
        }
    }

    public IPacket C2S_PullOtherRoleInfo(int playerID)
    {
        IPacket packet = null;
        NetPacket.C2S_PullOtherRoleInfo msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.PullOtherRoleInfo, out packet))
        {
            msg = (NetPacket.C2S_PullOtherRoleInfo)packet;
        }
        else
        {
            msg = new NetPacket.C2S_PullOtherRoleInfo();
            mNCodePool.Add(NetCode_C.PullOtherRoleInfo, msg);
        }
        msg.PlayerID = playerID;
        return msg;
    }
    public NetPacket.S2C_PullOtherRoleInfo S2C_PullOtherRoleInfo(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.PullOtherRoleInfo, out packet))
        {
            NetPacket.S2C_PullOtherRoleInfo msg = (NetPacket.S2C_PullOtherRoleInfo)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_PullOtherRoleInfo();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.PullOtherRoleInfo, packet);
            return (NetPacket.S2C_PullOtherRoleInfo)packet;
        }
    }


    #endregion



    public IPacket C2S_BattleHand(int[] spells)
    {
        IPacket packet = null;
        NetPacket.C2S_BattleHand msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.BattleHand, out packet))
        {
            msg = (NetPacket.C2S_BattleHand)packet;
        }
        else
        {
            msg = new NetPacket.C2S_BattleHand();
            mNCodePool.Add(NetCode_C.BattleHand, msg);
        }
        msg.SpellList = spells;
        return msg;
    }
    public NetPacket.S2C_BattleLog S2C_BattleLog(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.BattleLog, out packet))
        {
            NetPacket.S2C_BattleLog msg = (NetPacket.S2C_BattleLog)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_BattleLog();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.BattleLog, packet);
            return (NetPacket.S2C_BattleLog)packet;
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 服务器注册消息
    /// </summary>
    /// <returns></returns>
    public NetPacket.S2C_Register S2C_Register(BinaryReader ios)
    {
        IPacket packet = null;
      
        if (!mNSCodePool.TryGetValue(NetCode_S.Register, out packet))
        {
            packet = new NetPacket.S2C_Register();
            mNSCodePool.Add(NetCode_S.Register, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_Register)packet;
    }


    #region 邮件
    public IPacket C2S_ReceiveMail(int mailIdx)
    {
        IPacket packet = null;
        NetPacket.C2S_ReceiveMail msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.ReceiveMail, out packet))
        {
            msg = (NetPacket.C2S_ReceiveMail)packet;
        }
        else
        {
            msg = new NetPacket.C2S_ReceiveMail();
            mNCodePool.Add(NetCode_C.ReceiveMail, msg);
        }
        msg.MailIdx = mailIdx;
        return msg;
    }

    public NetPacket.S2C_ReceiveMail S2C_ReceiveMail(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.ReceiveMail, out packet))
        {
            packet = new NetPacket.S2C_ReceiveMail();
            mNSCodePool.Add(NetCode_S.ReceiveMail, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_ReceiveMail)packet;
    }

    public IPacket C2S_MailDetail(int mailIdx)
    {
        IPacket packet = null;
        NetPacket.C2S_MailDetail msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.MailDetail, out packet))
        {
            msg = (NetPacket.C2S_MailDetail)packet;
        }
        else
        {
            msg = new NetPacket.C2S_MailDetail();
            mNCodePool.Add(NetCode_C.MailDetail, msg);
        }
        msg.MailIdx = mailIdx;
        return msg;
    }

    public NetPacket.S2C_MailDetail S2C_MailDetail(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.MailDetail, out packet))
        {
            packet = new NetPacket.S2C_MailDetail();
            mNSCodePool.Add(NetCode_S.MailDetail, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_MailDetail)packet;
    }

    public NetPacket.S2C_SnapshotMail S2C_SnapshotMail(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotMail, out packet))
        {
            packet = new NetPacket.S2C_SnapshotMail();
            mNSCodePool.Add(NetCode_S.SnapshotMail, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotMail)packet;
    }
    #endregion


    #region 商店
    public IPacket C2S_BuyProduct(int goodsId, int storeId)
    {
        IPacket packet = null;
        NetPacket.C2S_BuyProduct msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.BuyProduct, out packet))
        {
            msg = (NetPacket.C2S_BuyProduct)packet;
        }
        else
        {
            msg = new NetPacket.C2S_BuyProduct();
            mNCodePool.Add(NetCode_C.BuyProduct, msg);
        }
        msg.GoodsId = goodsId;
        msg.StoreId = storeId;
        return msg;
    }
    public NetPacket.S2C_BuyProduct S2C_BuyProduct(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.BuyProduct, out packet))
        {
            packet = new NetPacket.S2C_BuyProduct();
            mNSCodePool.Add(NetCode_S.BuyProduct, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_BuyProduct)packet;
    }
    public IPacket C2S_ShopInfo(int shopId)
    {
        IPacket packet = null;
        NetPacket.C2S_ShopInfo msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.ShopInfo, out packet))
        {
            msg = (NetPacket.C2S_ShopInfo)packet;
        }
        else
        {
            msg = new NetPacket.C2S_ShopInfo();
            mNCodePool.Add(NetCode_C.ShopInfo, msg);
        }
        msg.ShopId = shopId;
        return msg;
    }
    public NetPacket.S2C_ShopInfo S2C_ShopInfo(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.ShopInfo, out packet))
        {
            packet = new NetPacket.S2C_ShopInfo();
            mNSCodePool.Add(NetCode_S.ShopInfo, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_ShopInfo)packet;
    }

    public NetPacket.S2C_SnapshotBuyShopInfo S2C_SnapshotBuyShopInfo(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotBuyShopInfo, out packet))
        {
            packet = new NetPacket.S2C_SnapshotBuyShopInfo();
            mNSCodePool.Add(NetCode_S.SnapshotBuyShopInfo, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotBuyShopInfo)packet;
    }

    public NetPacket.S2C_SnapshotShopInfo S2C_SnapshotShopInfo(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotShopInfo, out packet))
        {
            packet = new NetPacket.S2C_SnapshotShopInfo();
            mNSCodePool.Add(NetCode_S.SnapshotShopInfo, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotShopInfo)packet;
    }
    #endregion


    #region 生产
    public IPacket C2S_Produce(byte inventoryPos, int num)
    {
        IPacket packet = null;
        NetPacket.C2S_Produce msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.Produce, out packet))
        {
            msg = (NetPacket.C2S_Produce)packet;
        }
        else
        {
            msg = new NetPacket.C2S_Produce();
            mNCodePool.Add(NetCode_C.Produce, msg);
        }
        msg.InventoryPos = inventoryPos;
        msg.Num = num;
        return msg;
    }
    public IPacket C2S_FinishProduce(bool useDiamond ,int recipeID)
    {
        IPacket packet = null;
        NetPacket.C2S_FinishProduce msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.FinishProduce, out packet))
        {
            msg = (NetPacket.C2S_FinishProduce)packet;
        }
        else
        {
            msg = new NetPacket.C2S_FinishProduce();
            mNCodePool.Add(NetCode_C.FinishProduce, msg);
        }
        msg.useDiamond = useDiamond;
        msg.recipeID = recipeID;
        return msg;
    }
    public IPacket C2S_ProduceAccelerate(int recipeID)
    {
        IPacket packet = null;
        NetPacket.C2S_ProduceAccelerate msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.ProduceAccelerate, out packet))
        {
            msg = (NetPacket.C2S_ProduceAccelerate)packet;
        }
        else
        {
            msg = new NetPacket.C2S_ProduceAccelerate();
            mNCodePool.Add(NetCode_C.ProduceAccelerate, msg);
        }
        msg.recipeID = recipeID;
        return msg;
    }
    public IPacket C2S_AuxSkillLevelUp(int skillPos)
    {
        IPacket packet = null;
        NetPacket.C2S_AuxSkillLevelUp msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.AuxSkillLevelUp, out packet))
        {
            msg = (NetPacket.C2S_AuxSkillLevelUp)packet;
        }
        else
        {
            msg = new NetPacket.C2S_AuxSkillLevelUp();
            mNCodePool.Add(NetCode_C.AuxSkillLevelUp, msg);
        }
        msg.skillPos = skillPos;
        return msg;
    }
   
    public NetPacket.S2C_SnapShotProduceBotting S2C_SnapShotProduceBotting(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapShotProduceBotting, out packet))
        {
            packet = new NetPacket.S2C_SnapShotProduceBotting();
            mNSCodePool.Add(NetCode_S.SnapShotProduceBotting, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapShotProduceBotting)packet;
    }
    public NetPacket.S2C_SnapShotProduce S2C_SnapShotProduce(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapShotProduce, out packet))
        {
            packet = new NetPacket.S2C_SnapShotProduce();
            mNSCodePool.Add(NetCode_S.SnapShotProduce, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapShotProduce)packet;
    }

    public NetPacket.S2C_Produce S2C_Produce(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.Produce, out packet))
        {
            packet = new NetPacket.S2C_Produce();
            mNSCodePool.Add(NetCode_S.Produce, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_Produce)packet;
    }
    public NetPacket.S2C_FinishProduce S2C_FinishProduce(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.FinishProduce, out packet))
        {
            packet = new NetPacket.S2C_FinishProduce();
            mNSCodePool.Add(NetCode_S.FinishProduce, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_FinishProduce)packet;
    }
    public NetPacket.S2C_ProduceAccelerate S2C_ProduceAccelerate(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.ProduceAccelerate, out packet))
        {
            packet = new NetPacket.S2C_ProduceAccelerate();
            mNSCodePool.Add(NetCode_S.ProduceAccelerate, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_ProduceAccelerate)packet;
    }
    public NetPacket.S2C_AuxSkillLevelUp S2C_AuxSkillLevelUp(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.AuxSkillLevelUp, out packet))
        {
            packet = new NetPacket.S2C_AuxSkillLevelUp();
            mNSCodePool.Add(NetCode_S.AuxSkillLevelUp, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_AuxSkillLevelUp)packet;
    }
#endregion 

    #region 洞府
    public IPacket C2S_CaveLevelUp(bool useDiamond)
    {
        IPacket packet = null;
        NetPacket.C2S_CaveLevelUp msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.CaveLevelUp, out packet))
        {
            msg = (NetPacket.C2S_CaveLevelUp)packet;
        }
        else
        {
            msg = new NetPacket.C2S_CaveLevelUp();
            mNCodePool.Add(NetCode_C.CaveLevelUp, msg);
        }
        msg.IsUseDiamond = useDiamond;
        return msg;
    }      
    public IPacket C2S_ZazenStart(int drugId)
    {
        IPacket packet = null;
        NetPacket.C2S_ZazenStart msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.ZazenStart, out packet))
        {
            msg = (NetPacket.C2S_ZazenStart)packet;
        }
        else
        {
            msg = new NetPacket.C2S_ZazenStart();
            mNCodePool.Add(NetCode_C.ZazenStart, msg);
        }
        msg.DrugId = drugId;
        return msg;
    }
    public IPacket C2S_ZazenFinish()
    {
        IPacket packet = null;
        NetPacket.C2S_ZazenFinish msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.ZazenFinish, out packet))
        {
            msg = (NetPacket.C2S_ZazenFinish)packet;
        }
        else
        {
            msg = new NetPacket.C2S_ZazenFinish();
            mNCodePool.Add(NetCode_C.ZazenFinish, msg);
        }
        return msg;
    }  
 
    public IPacket C2S_RetreatStart(int drugIdx)
    {
        IPacket packet = null;
        NetPacket.C2S_RetreatStart msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.RetreatStart, out packet))
        {
            msg = (NetPacket.C2S_RetreatStart)packet;
        }
        else
        {
            msg = new NetPacket.C2S_RetreatStart();
            mNCodePool.Add(NetCode_C.RetreatStart, msg);
        }
        msg.DrugIdx = drugIdx;
        return msg;
    }
    public IPacket C2S_AuxSkillToolLevelUp(int index)
    {
        IPacket packet = null;
        NetPacket.C2S_AuxSkillToolLevelUp msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.AuxSkillToolLevelUp, out packet))
        {
            msg = (NetPacket.C2S_AuxSkillToolLevelUp)packet;
        }
        else
        {
            msg = new NetPacket.C2S_AuxSkillToolLevelUp();
            mNCodePool.Add(NetCode_C.AuxSkillToolLevelUp, msg);
        }
        msg.Index = index;
        return msg;
    }
    public NetPacket.S2C_CaveLevelUp S2C_CaveLevelUp(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.CaveLevelUp, out packet))
        {
            packet = new NetPacket.S2C_CaveLevelUp();
            mNSCodePool.Add(NetCode_S.CaveLevelUp, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_CaveLevelUp)packet;
    }
    public NetPacket.S2C_ZazenStart S2C_ZazenStart(BinaryReader ios)
    {
        IPacket packet = null;

        if (!mNSCodePool.TryGetValue(NetCode_S.ZazenStart, out packet))
        {
            packet = new NetPacket.S2C_ZazenStart();
            mNSCodePool.Add(NetCode_S.ZazenStart, packet);
        }

        packet.ReadFrom(ios);
        return (NetPacket.S2C_ZazenStart)packet;
    }   
    public NetPacket.S2C_ZazenFinish S2C_ZazenFinish(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.ZazenFinish, out packet))
        {
            packet = new NetPacket.S2C_ZazenFinish();
            mNSCodePool.Add(NetCode_S.ZazenFinish, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_ZazenFinish)packet;
    }
    public NetPacket.S2C_RetreatStart S2C_RetreatStart(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.RetreatStart, out packet))
        {
            packet = new NetPacket.S2C_RetreatStart();
            mNSCodePool.Add(NetCode_S.RetreatStart, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_RetreatStart)packet;
    }
    public NetPacket.S2C_RetreatFinish S2C_RetreatFinish(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.RetreatFinish, out packet))
        {
            packet = new NetPacket.S2C_RetreatFinish();
            mNSCodePool.Add(NetCode_S.RetreatFinish, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_RetreatFinish)packet;
    }
    public NetPacket.S2C_AuxSkillToolLevelUp S2C_AuxSkillToolLevelUp(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.AuxSkillToolLevelUp, out packet))
        {
            packet = new NetPacket.S2C_AuxSkillToolLevelUp();
            mNSCodePool.Add(NetCode_S.AuxSkillToolLevelUp, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_AuxSkillToolLevelUp)packet;
    }

    #endregion

    #region 游历
    public IPacket C2S_StartTravel(int travelId)
    {
        IPacket packet = null;
        NetPacket.C2S_StartTravel msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.TravelStart, out packet))
        {
            msg = (NetPacket.C2S_StartTravel)packet;
        }
        else
        {
            msg = new NetPacket.C2S_StartTravel();
            mNCodePool.Add(NetCode_C.TravelStart, msg);
        }
        msg.index = travelId;
        return msg;
    }
    public IPacket C2S_FinishTravelEvent(int eventId)
    {
        IPacket packet = null;
        NetPacket.C2S_FinishTravelEvent msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.FinishTravelEvent, out packet))
        {
            msg = (NetPacket.C2S_FinishTravelEvent)packet;
        }
        else
        {
            msg = new NetPacket.C2S_FinishTravelEvent();
            mNCodePool.Add(NetCode_C.FinishTravelEvent, msg);
        }
        msg.EventId = eventId;
        return msg;
    }
    public IPacket C2S_PullInfo(PullInfoType type, int id)
    {
        IPacket packet = null;
        NetPacket.C2S_PullInfo msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.PullInfo, out packet))
        {
            msg = (NetPacket.C2S_PullInfo)packet;
        }
        else
        {
            msg = new NetPacket.C2S_PullInfo();
            mNCodePool.Add(NetCode_C.PullInfo, msg);
        }
        msg.PullType = type;
        msg.Idx = id;
        return msg;
    }
    public NetPacket.S2C_SnapshotTravelEvent S2C_SnapshotTravelEvent(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotTravelEvent, out packet))
        {
            packet = new NetPacket.S2C_SnapshotTravelEvent();
            mNSCodePool.Add(NetCode_S.SnapshotTravelEvent, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotTravelEvent)packet;
    }
    public NetPacket.S2C_SnapshotTravelBotting S2C_SnapshotTravelBotting(BinaryReader ios)
    {
        IPacket packet = null;
        if (mNSCodePool.TryGetValue(NetCode_S.SnapshotTravelBotting, out packet))
        {
            NetPacket.S2C_SnapshotTravelBotting msg = (NetPacket.S2C_SnapshotTravelBotting)packet;
            msg.ReadFrom(ios);
            return msg;
        }
        else
        {
            packet = new NetPacket.S2C_SnapshotTravelBotting();
            packet.ReadFrom(ios);
            mNSCodePool.Add(NetCode_S.SnapshotTravelBotting, packet);
            return (NetPacket.S2C_SnapshotTravelBotting)packet;
        }
    }
    public NetPacket.S2C_SnapshotTravelHistory S2C_SnapshotTravelHistory(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotTravelHistory, out packet))
        {
            packet = new NetPacket.S2C_SnapshotTravelHistory();
            mNSCodePool.Add(NetCode_S.SnapshotTravelHistory, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotTravelHistory)packet;
    }
    public NetPacket.S2C_StartTravel S2C_StartTravel(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.TravelStart, out packet))
        {
            packet = new NetPacket.S2C_StartTravel();
            mNSCodePool.Add(NetCode_S.TravelStart, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_StartTravel)packet;
    }
    public NetPacket.S2C_FinishTravelEvent S2C_FinishTravelEvent(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.FinishTravelEvent, out packet))
        {
            packet = new NetPacket.S2C_FinishTravelEvent();
            mNSCodePool.Add(NetCode_S.FinishTravelEvent, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_FinishTravelEvent)packet;
    }
    public NetPacket.S2C_PullInfo S2C_PullInfo(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.PullInfo, out packet))
        {
            packet = new NetPacket.S2C_PullInfo();
            mNSCodePool.Add(NetCode_S.PullInfo, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_PullInfo)packet;
    }
    #endregion

    #region 声望任务
    public IPacket C2S_FreshPrestigeTask(bool justPullInfo , bool useDiamond , PrestigeLevel.PrestigeType ty)
    {
        IPacket packet = null;
        NetPacket.C2S_FreshPrestigeTask msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.FreshPrestigeTask, out packet))
        {
            msg = (NetPacket.C2S_FreshPrestigeTask)packet;
        }
        else
        {
            msg = new NetPacket.C2S_FreshPrestigeTask();
            mNCodePool.Add(NetCode_C.FreshPrestigeTask, msg);
        }
        msg.Type = ty;
        msg.UseDiamond = useDiamond;
        msg.JustPullInfo = justPullInfo;
        return msg;
    }

    public NetPacket.S2C_FreshPrestigeTask S2C_FreshPrestigeTask(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.FreshPrestigeTask, out packet))
        {
            packet = new NetPacket.S2C_FreshPrestigeTask();
            mNSCodePool.Add(NetCode_S.FreshPrestigeTask, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_FreshPrestigeTask)packet;
    }

    public IPacket C2S_StartPrestigeTask(PrestigeLevel.PrestigeType ty , int taskId , int taskPos)
    {
        IPacket packet = null;
        NetPacket.C2S_StartPrestigeTask msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.StartPrestigeTask, out packet))
        {
            msg = (NetPacket.C2S_StartPrestigeTask)packet;
        }
        else
        {
            msg = new NetPacket.C2S_StartPrestigeTask();
            mNCodePool.Add(NetCode_C.StartPrestigeTask, msg);
        }
        msg.Type = ty;
        msg.TaskIdx = taskId;
        msg.TaskPos = taskPos;
        return msg;
    }

    public NetPacket.S2C_StartPrestigeTask S2C_StartPrestigeTask(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.StartPrestigeTask, out packet))
        {
            packet = new NetPacket.S2C_StartPrestigeTask();
            mNSCodePool.Add(NetCode_S.StartPrestigeTask, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_StartPrestigeTask)packet;
    }
    public IPacket C2S_FinishPrestigeTask(bool useDiamond)
    {
        IPacket packet = null;
        NetPacket.C2S_FinishPrestigeTask msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.FinishPrestigeTask, out packet))
        {
            msg = (NetPacket.C2S_FinishPrestigeTask)packet;
        }
        else
        {
            msg = new NetPacket.C2S_FinishPrestigeTask();
            mNCodePool.Add(NetCode_C.FinishPrestigeTask, msg);
        }
        
        msg.UseDiamond = useDiamond;
        return msg;
    }

    public NetPacket.S2C_FinishPrestigeTask S2C_FinishPrestigeTask(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.FinishPrestigeTask, out packet))
        {
            packet = new NetPacket.S2C_FinishPrestigeTask();
            mNSCodePool.Add(NetCode_S.FinishPrestigeTask, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_FinishPrestigeTask)packet;
    }

    public NetPacket.S2C_SnapshotPrestigeTask S2C_SnapshotPrestigeTask(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotPrestigeTask, out packet))
        {
            packet = new NetPacket.S2C_SnapshotPrestigeTask();
            mNSCodePool.Add(NetCode_S.SnapshotPrestigeTask, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotPrestigeTask)packet;
    }
    public NetPacket.S2C_SnapshotPrestigeTaskDetail S2C_SnapshotPrestigeTaskDetail(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotPrestigeTaskDetail, out packet))
        {
            packet = new NetPacket.S2C_SnapshotPrestigeTaskDetail();
            mNSCodePool.Add(NetCode_S.SnapshotPrestigeTaskDetail, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotPrestigeTaskDetail)packet;
    }
    #endregion

    #region 成就

    public NetPacket.S2C_SnapshotAchieve S2C_SnapshotAchieve(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotAchieve, out packet))
        {
            packet = new NetPacket.S2C_SnapshotAchieve();
            mNSCodePool.Add(NetCode_S.SnapshotAchieve, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotAchieve)packet;
    }

    public NetPacket.S2C_SnapshotAchieveFinish S2C_SnapshotAchieveFinish(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.SnapshotAchieveFinish, out packet))
        {
            packet = new NetPacket.S2C_SnapshotAchieveFinish();
            mNSCodePool.Add(NetCode_S.SnapshotAchieveFinish, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_SnapshotAchieveFinish)packet;
    }

    public IPacket C2S_GetAchieveReward(int rewardId)
    {
        IPacket packet = null;
        NetPacket.C2S_GetAchieveReward msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.GetAchieveReward, out packet))
        {
            msg = (NetPacket.C2S_GetAchieveReward)packet;
        }
        else
        {
            msg = new NetPacket.C2S_GetAchieveReward();
            mNCodePool.Add(NetCode_C.GetAchieveReward, msg);
        }

        msg.RewardId = rewardId;
        return msg;
    }
    public NetPacket.S2C_GetAchieveReward S2C_GetAchieveReward(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.GetAchieveReward, out packet))
        {
            packet = new NetPacket.S2C_GetAchieveReward();
            mNSCodePool.Add(NetCode_S.GetAchieveReward, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_GetAchieveReward)packet;
    }
    #endregion

    #region VIP
    public IPacket C2S_BuyVIP()
    {
        IPacket packet = null;
        NetPacket.C2S_BuyVIP msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.BuyVIP, out packet))
        {
            msg = (NetPacket.C2S_BuyVIP)packet;
        }
        else
        {
            msg = new NetPacket.C2S_BuyVIP();
            mNCodePool.Add(NetCode_C.BuyVIP, msg);
        }
        return msg;
    }
    public NetPacket.S2C_BuyVIP S2C_BuyVIP(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.BuyVIP, out packet))
        {
            packet = new NetPacket.S2C_BuyVIP();
            mNSCodePool.Add(NetCode_S.BuyVIP, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_BuyVIP)packet;
    }

    public IPacket C2S_GetVIPAward()
    {
        IPacket packet = null;
        NetPacket.C2S_GetVIPAward msg = null;
        if (mNCodePool.TryGetValue(NetCode_C.GetVIPDailyAward, out packet))
        {
            msg = (NetPacket.C2S_GetVIPAward)packet;
        }
        else
        {
            msg = new NetPacket.C2S_GetVIPAward();
            mNCodePool.Add(NetCode_C.GetVIPDailyAward, msg);
        }
        return msg;
    }
    public NetPacket.S2C_GetVIPAward S2C_GetVIPAward(BinaryReader ios)
    {
        IPacket packet = null;
        if (!mNSCodePool.TryGetValue(NetCode_S.GetVIPDailyAward, out packet))
        {
            packet = new NetPacket.S2C_GetVIPAward();
            mNSCodePool.Add(NetCode_S.GetVIPDailyAward, packet);
        }
        packet.ReadFrom(ios);
        return (NetPacket.S2C_GetVIPAward)packet;
    }
    #endregion
}
public enum PullInfoType
{
    None,
    TravelEventHistory,
    MapEvent,
    MailList,
    Achievement,
    PromAchieve,
}