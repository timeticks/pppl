using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class NetRankBotting : DescObject {

    public class RankItem
    {
        public Sect.SectType sectType;
        public int playerIcon;
        public int heroIdx;
        public int level;
        public int num;   //不同排行榜，数值含义不同。等级排行榜没有含义
        public string playerName;
        public int playerUid;
        public RankItem() { }

        public void readFrom(BinaryReader ios, RankType rankType)
        {
            sectType = (Sect.SectType)ios.ReadByte();
            level = ios.ReadInt16();
            if (rankType != RankType.Level0 && rankType != RankType.Level1)                              
                num = ios.ReadInt32();
            heroIdx = ios.ReadInt32();
            playerIcon = ios.ReadInt32();
            playerUid = ios.ReadInt32();
            playerName = NetUtils.ReadUTF(ios);
        }

    }

    public class OtherRoleInfo
    {
        public string Name;
        public int IconHead;
        public OldHero.Sex Sex;
        public Sect.SectType Sect;
        public int Level;
        public int Str;
        public int Mana;
        public int Mind;
        public int Con;
        public int Vit;
        public int Luk;
        public int Hp;
        public int Mp;
        public long Exp;
        public long BirthTime;

        public int[] SpellList;
        public int[] EquipList;
        public int[] PetList;
        public OtherRoleInfo() { }

        public void ReadFrom(BinaryReader ios)
        {
     
            Sex = (OldHero.Sex)ios.ReadByte();
            Sect = (Sect.SectType)ios.ReadByte();

            Level = ios.ReadInt16();
            Str = ios.ReadInt16();
            Mana = ios.ReadInt16();
            Mind = ios.ReadInt16();
            Con = ios.ReadInt16();
            Vit = ios.ReadInt16();
            Luk = ios.ReadInt16();
            IconHead = ios.ReadInt32();
           
            Hp = ios.ReadInt32();
            Mp = ios.ReadInt32();

            Exp = ios.ReadInt64();
            BirthTime = ios.ReadInt64();

            Name = NetUtils.ReadUTF(ios);
           
          

            int length = ios.ReadByte();
            SpellList = new int[length];
            for (int i = 0; i < length; i++)
                SpellList[i] = ios.ReadInt32();

            length = ios.ReadByte();
            EquipList = new int[length];
            for (int i = 0; i < length; i++)
                EquipList[i] = ios.ReadInt32();

            length = ios.ReadByte();
            PetList = new int[length];
            for (int i = 0; i < length; i++)
                PetList[i] = ios.ReadInt32();
        }
      
    }

}
public enum RankType
{
    Level0,
    Level1,
    Sect0,
    Sect1,
    Gold0, 
    Gold1,
    Achieve0,
    Achieve1,
    Tower,
    Max,
   
}