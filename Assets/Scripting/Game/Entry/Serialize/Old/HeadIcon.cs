using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface IHeadIconFetcher
{
    HeadIcon GetHeadIconByCopy(int idx);
}
public class HeadIcon : DescObject
{
    private static IHeadIconFetcher mFetcher;
    public static IHeadIconFetcher HeadIconFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public string mDesc = "";


    public string mMHead = "";  //男头像
    public string mMTex = "";
    public string mFHead = "";  //女头像
    public string mFTex = "";

    public HeadIcon():base()
    {
        
    }
    public HeadIcon(HeadIcon origin): base(origin)
    {
        this.mDesc = origin.mDesc;
        this.mMHead = origin.mMHead;
        this.mMTex = origin.mMTex;
        this.mFHead = origin.mFHead;
        this.mFTex = origin.mFTex;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mDesc = NetUtils.ReadUTF(ios);
        this.mMHead = NetUtils.ReadUTF(ios);
        this.mMTex = NetUtils.ReadUTF(ios);
        this.mFHead = NetUtils.ReadUTF(ios);
        this.mFTex = NetUtils.ReadUTF(ios);
    }




    public string Desc
    {
        get { return mDesc; }
    }

    public string MHead
    {
        get { return mMHead; }
    }

    public string MTex
    {
        get { return mMTex; }
    }

    public string FHead
    {
        get { return mFHead; }
    }

    public string FTex
    {
        get { return mFTex; }
    }


    public static string GetHeadIcon(int iconIdx, int heroIdx)
    {
        OldHero hero = OldHero.HeroFetcher.GetHeroByCopy(heroIdx);
        if (hero != null)
            return GetHeadIcon(iconIdx, hero.MySex);
        return GetHeadIcon(iconIdx, OldHero.Sex.Male);
    }
    
    public static string GetHeadIcon(int iconIdx , OldHero.Sex sex)
    {
        if (iconIdx <= 0) return GameConstUtils.MonsterIcon;
        HeadIcon icon = HeadIcon.HeadIconFetcher.GetHeadIconByCopy(iconIdx);
        if (icon == null) return GameConstUtils.MonsterIcon;
        if (sex == OldHero.Sex.Female)
        {
            return icon.FHead;
        }
        else
        {
            return icon.MHead;
        }
    }



    public static string GetTexIcon(int iconIdx, OldHero.Sex sex)
    {
        HeadIcon icon = HeadIcon.HeadIconFetcher.GetHeadIconByCopy(iconIdx);
        if (icon == null) return "Npc_001";
        if (sex == OldHero.Sex.Female)
        {
            return icon.FTex;
        }
        else
        {
            return icon.MTex;
        }
    }
}
