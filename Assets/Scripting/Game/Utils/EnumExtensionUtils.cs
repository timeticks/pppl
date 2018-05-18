using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 枚举转中文意思，不用EnumDesc
/// </summary>
public static class EnumExtensionUtils
{
    public static string GetDescEx(this PartnerData.HairColor enu)
    {
        switch (enu)
        {
            case PartnerData.HairColor.Black:
                return "desc_hair_black";
            case PartnerData.HairColor.Blue:
                return "desc_hair_blue";
            case PartnerData.HairColor.Gold:
                return "desc_hair_gold";
            case PartnerData.HairColor.White:
                return "desc_hair_white";
            default:
                return "unknown";
        }
    }

    public static string GetDescEx(this PartnerData.Sex enu)
    {
        switch (enu)
        {
            case PartnerData.Sex.Male:
                return "desc_male";
            case PartnerData.Sex.Female:
                return "desc_female";
            default:
                return "unknown";
        }
    }

    public static string GetDescEx(this PartnerData.SkinColor enu)
    {
        switch (enu)
        {
            case PartnerData.SkinColor.Black:
                return "desc_skin_black";
            case PartnerData.SkinColor.BlackYellow:
                return "desc_skin_blue";
            case PartnerData.SkinColor.White:
                return "desc_skin_gold";
            case PartnerData.SkinColor.Yellow:
                return "desc_skin_white";
            default:
                return "unknown";
        }
    }

    public static string GetDescEx(this PartnerData.CharacType enu)
    {
        switch (enu)
        {
            case PartnerData.CharacType.Cute:
                return "desc_charac_cute";
            case PartnerData.CharacType.Mature:
                return "desc_charac_mature";
            default:
                return "unknown";
        }
    }

    public static string GetDescEx(this PartnerData.HappyMemory enu)
    {
        switch (enu)
        {
            case PartnerData.HappyMemory.SeeStar:
                return "desc_happy_memory_0";
            case PartnerData.HappyMemory.BuyFoot:
                return "desc_happy_memory_1";
            default:
                return "unknown";
        }
    }

    public static string GetDescEx(this NatureType enu)
    {
        switch (enu)
        {
            case NatureType.ScoreLoot:
                return "desc_nature_score_loot";
            case NatureType.MapEndLoot:
                return "desc_nature_map_end_loot";
            default:
                return "unknown";
        }
    }
}
