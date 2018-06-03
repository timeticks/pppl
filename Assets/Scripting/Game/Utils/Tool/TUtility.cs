using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Runtime.Serialization;

/// <summary>
/// 功能类。。。
/// 所有的float -> int ，都是四舍五入取整
/// </summary>
public class TUtility:TUtilityBase
{
    public const int LevelUpConfigId = 1204000000;//加上level得到当前level在技能表中的ID

    public static string[] PlayerNames = new string[]
    {"丽璐·阿歌特","佐伯·杏太郎","耶纳斯·帕沙","胡里奥·埃涅科","希恩·杨","切萨雷·托尼","艾米利奥·费龙","安吉洛·普契尼","卡洛·西诺","伊恩·杜可夫","梨花·薛",
"朱利安·洛佩斯","令·谢","法蒂玛·哈涅","哈希姆·阿迪勒","略克·西萨","马丁·斯派尔","宗甚·来岛","瑜·文","罗伯特·斯托克","查尔斯·格万","保罗·莫雷斯","西蒙·利纳雷斯","恩佐·莱季奥尼","萨利多·阿加尔","伊本·尼亚迪","大卫·格雷文","长庆·神室","胡安·布兰克","杰克斯·布鲁姆","朱里安·弗美尔","尤里斯·胡格恩","布莱特·佩罗","约翰·拉穆吉奥","雅各伯·波图昂","贝拉索·阿戈雷","萨迦诺斯·比伊","威廉·克莱维","埃尔南·贝利奥","伊斯尔·亚塞尔","里查德·汤森德","汉斯·莱切尔","阿卡布·达迈","谢乌德·埃米","米瓦奥尔·根茨","夏洛特·米列","相铉·韩","克莱尔·波弗","宗九郎·泷田","比安卡·普契尼","弗朗西斯卡","玛丽","玛格丽特","汪达","朱丽叶","唐娜","玛蒂尔达","玛丽娜","法提希亚","娜丽","哈特拉","萨菲亚","贝娜齐尔","露西亚","泰塞斯","美华","丽姬","樱","维利萨","希尔维亚","伊莎贝尔","杰克·布鲁姆","里安·弗美尔","胡格恩","佩罗","拉穆吉奥","约翰","雅各伯","阿戈雷","波图昂","威廉·比伊","克莱维","贝利奥",
       };


    public static string GetPlayerNames(int playerId)
    {
        if (playerId >= PlayerNames.Length)
        {
            int repeatPlayerId = playerId%(PlayerNames.Length - 1);
            return PlayerNames[repeatPlayerId] + playerId.ToString();
        }
        playerId = Mathf.Clamp(playerId, 0, PlayerNames.Length - 1);
        return PlayerNames[playerId];
    }



    public static Color Switch16ToColor(string colorString)//16进制转颜色
    {
        if (colorString.Length != 6)
            return Color.black;
        try
        {
            Color returnCol = new Color();
            returnCol.r = (float) Convert.ToInt32(colorString.Substring(0, 2), 16)/256f;
            returnCol.g = (float) Convert.ToInt32(colorString.Substring(2, 2), 16)/256f;
            returnCol.b = (float) Convert.ToInt32(colorString.Substring(4, 2), 16)/256f;
            returnCol.a = 1;
            return returnCol;
        }
        catch
        {
            return Color.black;
        }
    }

    public static Texture2D CreatRgbTexture(float r, float g, float b)
    {
        Color fillColor = new Color(r, g, b);
        return CreatColorTexture(fillColor);
    }

    public static Texture2D CreatColorTexture(Color fillColor) //创建纯色图片
    {
        var texture = new Texture2D(32, 32);
        var pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = fillColor;
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }


    /// <summary>
    /// 获取所有子节点 包括隐藏
    /// </summary>
    public static List<Transform> GetAllChild(GameObject go)
    {
        List<Transform> childlist = new List<Transform>();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            childlist.Add(go.transform.GetChild(i));
            List<Transform> list = GetAllChild(go.transform.GetChild(i).gameObject);
            for (int j = 0; j < list.Count; j++)
            {
                childlist.Add(list[j]);
            }
        }
        return childlist;
    }

    /// <summary>
    /// 将数字转为可排序的名字1——0001
    /// </summary>
    public static string PadZeroLeft(int num , int padNum)
    {
        string numString = num.ToString();
        numString = numString.PadLeft(padNum, '0');
        return numString;
    }

    /// <summary>
    /// 根据数字，将sprite赋在imageList中。isFirstIs10是否数字的第一位都显示第11个sprite，如：-10
    /// </summary>
    /// <param name="numValue"></param>
    /// <param name="numImageList"></param>
    public static void SetNumToSpriteList(int numValue, List<Image> numImageList, List<Sprite> numSpriteList, bool isFirstIs10)
    {
        if (numSpriteList == null || numImageList == null)
        {
            TDebug.LogError("numSpriteList == null || numImageList == null");
            return;
        }
        for (int i = 0; i < numImageList.Count; i++)
        {
            numImageList[i].gameObject.SetActive(true);
        }
        int curIndex = 0;
        if (isFirstIs10 && numSpriteList.Count >= 11)//如显示伤害时，第一个都为减号
        {
            numImageList[0].sprite = numSpriteList[10]; numImageList[0].SetNativeSize();
            curIndex++;
        }
        if (numImageList.Count < curIndex + 2) numValue = Mathf.Min(numValue, 9);
        else if (numImageList.Count < curIndex + 3) numValue = Mathf.Min(numValue, 99);
        else if (numImageList.Count < curIndex + 4) numValue = Mathf.Min(numValue, 999);

        if (numValue >= 1000)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue / 1000, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 1000 / 100, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        else if (numValue >= 100)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue / 100, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        else if (numValue >= 10)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].gameObject.SetActive(false);
        }
        else
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        for (int i = curIndex + 1; i < numImageList.Count; i++)
        {
            numImageList[i].gameObject.SetActive(false);
        }
    }


    public static bool CheckIsNullAndDebug(object obj)//判空，并输出错误信息
    {
        if (obj == null)
        {
            TDebug.LogError(obj.ToString() + "IsNull");
            return true;
        }
        return false;
    }


    static System.Random rand = new System.Random();
    public static List<int> GetRandomList(List<int> randomList)//将有序列表，转为随机列表
    {
        int count = randomList.Count;
        for (int i = 0; i < count; i++)
        {
            int r1 = rand.Next(0, randomList.Count);
            int r2 = rand.Next(0, randomList.Count);
            int temp = randomList[r1];
            randomList[r1] = randomList[r2];
            randomList[r2] = temp;
        }
        return randomList;
    }

    //生成0-numAmount的列表，其中元素值随机
    public static List<int> GetRandomList(int numAmount)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < numAmount; i++)
        {
            list.Add(i);
        }
        return GetRandomList(list);
    }

    /// <summary>
    /// 根据V4的限制，得到随机的V2
    /// </summary>
    /// <param name="limit">x左，y上，z右，w下</param>
    /// <returns></returns>
    public static Vector2 GetRandomV2InRect(List<Vector4> limit)
    {
        int randNum = rand.Next(0, limit.Count);
        return new Vector2(rand.Next((int)limit[randNum].x, (int)limit[randNum].z), rand.Next((int)limit[randNum].w, (int)limit[randNum].y));
    }

    public static string TryGetValOfAttr(AttrType type,int val)
    {
        if (type.ToInt() >=100)
            return val.ToFloat_100().ToString("f1") + "%";
        switch (type)
        {
            case AttrType.Hit:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.Dodge:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.CritDmg:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.CritPct:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.DoSkillPct:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.RevivePct:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.UpExp:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.UpGold:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.UpDrop:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.StrengthPct:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.DefCrit:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.PhyDmgInc:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.MagDmgInc:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.AllDmgDec:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.MagDmgDec:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.PhyDmgDec:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.DizzyPct:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.PoisonPct:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.FrozenPct:
                return val.ToFloat_100().ToString("f1") + "%";
            case AttrType.FirePct:
                return val.ToFloat_100().ToString("f1") + "%";
            default:
                //TDebug.LogError("不存在的属性类型");
                return val.ToString();
        }
    }
    public static string TryGetStateName(int state)
    {
        switch(state)
        {
            case 1:
                return "炼气期";
            case 2:
                return "筑基期";
            case 3:
                return "结丹期";
            case 4:
                return "元婴期";
            case 5:
                return "化神期";
            case 6:
                return "炼虚期";
            case 7:
                return "合体期";
            case 8:
                return "大乘期";
            case 9:
                return "渡劫期";
            default:
                return "";
        }
    }
    public static string TryGetEquipQualityString(int quality)
    {
        switch (quality)
        {
            case 0:
                return GetTextByQuality("白色", quality);
            case 1:
                return GetTextByQuality("绿色", quality);
            case 2:
                return GetTextByQuality("蓝色", quality);
            case 3:
                return GetTextByQuality("紫色", quality);
            case 4:
                return GetTextByQuality("橙色", quality);
            case 5:
                return GetTextByQuality("红色", quality);
            case 6:
                return GetTextByQuality("黑色", quality);
            default:
                return  GetTextByQuality("未知色", quality);;
        }
    }
    public static string GetWealthIcon(WealthType type)
    {
        switch (type)
        {
            case WealthType.Diamond:
                return "Icon_xianyu";
            case WealthType.Gold:
                return "Icon_lingshi";
            case WealthType.Potentail:
                return "Icon_qianneng";
            default :
                return "";
        }
    }


    public static string GetTextByQuality(string content , int quality)
    {
        switch (quality)
        {
            case 0:
                return string.Format("<color=#EEEEEEFF>{0}</color>", content);
            case 1:
                return string.Format("<color=#11CC44FF>{0}</color>", content);
            case 2:
                return string.Format("<color=#00BBFFFF>{0}</color>", content);
            case 3:
                return string.Format("<color=#FF44CCFF>{0}</color>", content);
            case 4:
                return string.Format("<color=#EF5F09FF>{0}</color>", content);
            case 5:
                return string.Format("<color=#FF0000FF>{0}</color>", content);
            default:
                return string.Format("<color=#EEEEEEFF>{0}</color>", content); 
        }
    }
    /// <summary>
    /// 2个字中间间隔2个空格，3个及以上不间隔
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static string GetTagStr(string tag)
    {
        return tag.Length == 2?tag.Insert(1, "  "):tag;
    }

    public static string TryGetLootGoodsName(LootItemType type,int id)
    {
        switch(type)
        {
            case LootItemType.Recipe:
                Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(id);
                return recipe == null ? "无效图纸" : recipe.name;
            case LootItemType.Equip:
                Equip equip = Equip.Fetcher.GetEquipCopy(id);
                return equip == null ? "无效法宝" : GetTextByQuality(equip.name, equip.curQuality);
            case LootItemType.Money:
            {
                WealthType wealth = (WealthType) id;
                return wealth.GetDesc();
            }
            case LootItemType.Item:
                Item item = Item.Fetcher.GetItemCopy(id);
                return item == null ? "无效道具" : GetTextByQuality(item.name, item.quality);
            case LootItemType.Pet:
                Pet pet = Pet.PetFetcher.GetPetByCopy(id);
                return pet == null ? "无效宠物" : pet.name;
            //case LootType.spell:
            //    OldSpell spell = OldSpell.SpellFetcher.GetSpellByCopy(id);
            //    return spell == null ? "无效功法" : spell.name;
            case LootItemType.Prestige:
            {
                PrestigeLevel.PrestigeType ty = (PrestigeLevel.PrestigeType)id;
                return string.Format("{0}声望", ty.GetDesc());
            }
            default:
                return null;
        }
    }
    public static string TryGetPetIcon(Pet.PetTypeEnum? type)
    {
        switch (type)
        {
            case Pet.PetTypeEnum.Animal:
                return "Icon_lingshou";
            case Pet.PetTypeEnum.Ghost:
                return "Icon_yinhun";
            case Pet.PetTypeEnum.Puppet:
                return "Icon_kuilei";
            default :
                return "";
        }
    }
    public static string TryGetPetTypeString(Pet.PetTypeEnum? pos)
    {
        switch (pos)
        {
            case Pet.PetTypeEnum.None:
                return "";
            case Pet.PetTypeEnum.Animal:
                return "Icon_lingshou";
            case Pet.PetTypeEnum.Puppet:
                return "Icon_kuilei";
            case Pet.PetTypeEnum.Ghost:
                return "Icon_yinhun";
            default:
                return "";
        }
    }
    public static string TryGetAuxSkillTypeStr(AuxSkillLevel.SkillType type)
    {
        switch (type)
        {
            case AuxSkillLevel.SkillType.Forge:
                return "炼器";
            case AuxSkillLevel.SkillType.MakeDrug:
                return "炼丹";
            case AuxSkillLevel.SkillType.Mine:
                return "挖矿";
            case AuxSkillLevel.SkillType.GatherHerb:
                return "采药";
            default:
                return "";
        }
    }

    public static string TryGetAttrTypeStr(AttrType type)
    {
        switch (type)
        {
            case AttrType.Hp:
                return "生命";
                  case AttrType.Mp:
                return "法力";
                  case AttrType.Luk:
                return "机缘";
                  case AttrType.Hit:
                return "命中";
                  case AttrType.Dodge:
                return "躲闪";
                  default:
                TDebug.LogError("不存在的属性类型");
                return "";
        }
    }

    public static string GetMoneyString(int num)
    {
        string tempStr;
        if (num > 99999 && num< 10000 * 10000)
        {
            tempStr = string.Format("{0}万", num / 10000);
        }
        else if (num > 9999 * 10000)
        {
            tempStr = string.Format("{0}千万", num / 10000000);
        }
        else
            tempStr = num.ToString();
        return tempStr;
    }

    public static string GetRankNumStr(int num)
    {
        switch (num)
        {
            case 0:
                return "零";
            case 1:
                return "壹";
            case 2:
                return "贰";
            case 3:
                return "叁";
            case 4:
                return "肆";
            case 5:
                return "伍";
            case 6:
                return "陆";
            case 7:
                return "柒";
            case 8:
                return "捌";
            case 9:
                return "玖";
            case 10:
                return "拾";
            default :
                return num.ToString();
        }
    }


    /// <summary>
    /// 在形如“111010;111012”的字符串中，获得数字列表
    /// </summary>
    public static List<int> TryGetIntByStr(string str, char splitChar = ';', bool ignoreZero = true)
    {
        List<int> l = new List<int>();
        string[] s = str.Split(splitChar);
        if (s != null && s.Length > 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int temp = 0;
                if (int.TryParse(s[i], out temp))
                {
                    if (ignoreZero && temp == 0) continue;
                    l.Add(temp);
                }
            }
        }
        return l;
    }
    /// <summary>
    /// 在形如“111010;111012”的字符串中，获得数字列表
    /// </summary>
    public static List<float> TryGetFloatListByStr(string str, char splitChar = ';', bool ignoreZero = true)
    {
        List<float> l = new List<float>();
        string[] s = str.Split(splitChar);
        if (s != null && s.Length > 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                float temp = 0;
                if (float.TryParse(s[i], out temp))
                {
                    if (ignoreZero && temp == 0) continue;
                    l.Add(temp);
                }
            }
        }
        return l;
    }


    public static Color GetColorByQuality(int quality)
    {
        quality = Mathf.Clamp(quality, 0, 4);
        switch (quality)
        {
            case 0:
                return new Color(42f,229f,90f);
            case 1:
                return new Color(0f, 171f, 255f);
            case 2:
                return new Color(255f, 142f, 0f);
            case 3:
                return new Color(149f, 88f, 255f);
            case 4:
                return new Color(182f, 0f, 0f);
        }
        return new Color(42f, 229f, 90f);
    }
    public static string GetColorStringByQuality(int quality)
    {
        quality = Mathf.Clamp(quality, 0, 4);
        switch (quality)
        {
            case 0:
                return "2AE459";
            case 1:
                return "00AAFF";
            case 2:
                return "9558FF";
            case 3:
                return "FF8E00";
            case 4:
                return "B50000";
        }
        return "2AE459";
    }


    public static Vector2 GetDirByDisAndCenter(Vector2 curPos, Vector2 forward, float forwardDis, Vector2 centerPos, float radius)//用于绕某圆心运动
    {
        Vector2 v2 = Vector2.zero;
        Vector2 forwardPos = curPos + forward * forwardDis;
        v2 = forwardPos - centerPos;
        v2 = v2.normalized * radius + centerPos;
        return v2;
    }
    public static Vector3 GetDirByCenter(Vector3 curPos, float forwardDis, Vector3 centerPos, float radius)
    {
        Vector3 targetPos = Vector3.zero;
        Vector3 tanDir = Vector3.Cross(curPos - centerPos, Vector3.up);
        Vector2 tanDir2 = new Vector2(tanDir.x, tanDir.z);
        Vector2 v2 = GetDirByDisAndCenter(new Vector2(curPos.x, curPos.z), tanDir2.normalized, forwardDis, new Vector2(centerPos.x, centerPos.z), radius);
        targetPos = new Vector3(v2.x, curPos.y, v2.y);
        return targetPos;
    }

    


    public static void SetParent(Transform childTran, Transform parentTran, bool isRectTransformZero=false,bool isRotationZero=true)  //设置子物体后，重置位置信息
    {
        childTran.SetParent(parentTran);
        childTran.localPosition = Vector3.zero;
        childTran.localScale = Vector3.one;
        if (isRotationZero) childTran.localRotation = Quaternion.identity;
        if (isRectTransformZero)
        {
            childTran.GetComponent<RectTransform>().localPosition = Vector3.zero;
            childTran.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
    }
    public static void SetParent(Transform childTran, Transform parentTran, Vector3 localPosition, Vector2 sizeDetla) //设置子物体后，重置位置信息
    {
        childTran.SetParent(parentTran);
        childTran.localScale = Vector3.one;
        childTran.localRotation = Quaternion.identity;
        childTran.GetComponent<RectTransform>().localPosition = localPosition;
        childTran.GetComponent<RectTransform>().sizeDelta = sizeDetla;
    }

    public static int[] SplitToIntArray(string str, char splitChar = '|')
    {
        string[] s = str.Split(splitChar);
        int[] l = new int[s.Length];
        if (s != null && s.Length > 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int temp = 0;
                int.TryParse(s[i], out temp);
                l[i] = temp;
            }
        }
        return l;
    }


    #region 时间相关
    /// <summary>
    /// 秒数转化时:分:秒string返回
    /// </summary>
    public static string TimeSecondsToDayStr_LCD( int total )
    {
        // TDebug.LogWarning("----------------" + total);
        int sec = total % 60;
        int min = total / 60 % 60;
        int hour = total / 60 / 60;
        return ( hour < 10 ? "0" + hour : "" + hour ) + ":" + ( min < 10 ? "0" + min : "" + min ) + ":" + ( sec < 10 ? "0" + sec : "" + sec );
    }
    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    public static DateTime stampToDateTime(long timeStamp)
    {
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        try
        {
            return dateTimeStart.AddSeconds(timeStamp);
        }
        catch
        {
            return dateTimeStart;
        }
    }
    public static DateTime ConvertToDateTime(long timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp.ToString() + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public static int GetLocalDayHour()
    {
        return DateTime.Now.Hour;
    }

    /// <summary>
    /// 将时间戳转化为与现在的倒计时,转化时:分:秒string 返回
    /// </summary>
    public static string GetUnixTimeToDayStr(int total)
    {
        //得到当前时间的时间戳
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        int curStamp = Convert.ToInt32(ts.TotalSeconds);
        //时间戳相减,得到剩余秒数
        int remainTime = total - curStamp;
        //转化为时分秒返回
        return TimeSecondsToDayStr_LCD(remainTime);
    }

    /// <summary>
    /// C#格式时间转为时间戳
    /// </summary>
    public static long DateTimeToStamp(DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (long)(time - startTime).TotalMilliseconds;
    }

    /// <summary>
    /// 时间戳（毫秒）转为C#时间，返回豪秒
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static long GetDiffToNow(long timeStamp)
    {
        DateTime stampDataTime = ConvertToDateTime(timeStamp);
        TimeSpan diff = DateTime.Now.Subtract(stampDataTime);
        long r = Convert.ToInt64(diff.TotalMilliseconds);
        return r;
    }

    /// <summary>
    /// 和当前时间的差值 字符串 X年/月/天/小时/分钟
    /// </summary>
    public static string GetStringDiffToNow(long timeStamp)
    {
        long second = GetDiffToNow(timeStamp);
        return GetStringDiff(second);
    }    
    
    /// <summary>
    /// 差值转换时间字符串 X年/月/天/小时/分钟
    /// </summary>
    public static string GetStringDiff(long second)
    {
        string str = "";
        int minNum = 60 * 1000;
        int hourNum = minNum * 60 * 1000;
        int dayNum = hourNum * 24 * 1000;
        int monthNum = dayNum * 30*1000;
        int yearNum = dayNum * 365*1000;

        if ((second / yearNum) > 0)
        {
            str = (second / yearNum) + "年";
        }
        else if ((second / monthNum) > 0)
        {
            str = (second / monthNum) + "月";
        }
        else if ((second / dayNum) > 0)
        {
            str = (second / dayNum) + "天";
        }
        else if ((second / hourNum) > 0)
        {
            str = (second / hourNum) + "小时";
        }
        else if ((second / minNum) > 0)
        {
            str = (second / minNum) + "分钟";
        }
        else
        {
            str = "1分钟";
        }
        return str;
    }
    /// <summary>
    /// 转换时间字符串 天/小时/分钟
    /// </summary>
    public static string GetStringTime(long second)
    {
        string str = "";
        long minNum = second / 60 % 60;
        long hourNum = second / 60 / 60 % 24;
        long dayNum = second / 60 / 60 / 24;
        long secNum = second % 60;
        if (dayNum > 0)
        {
            str += dayNum+"天";
        }
        if (hourNum > 0)
        {
            str += hourNum + "小时";
        }
        if (minNum > 0)
        {
            str += minNum + "分";
        }
        str += Math.Max(0,secNum) + "秒";
        return str;
    }
    /// <summary>
    /// 只显示最大的时间单位
    /// </summary>
    public static string GetStringTime2(long second)
    {
        string str = "";
        long minNum = second / 60 % 60;
        long hourNum = second / 60 / 60 % 24;
        long dayNum = second / 60 / 60 / 24;
        long secNum = second % 60;
        if (dayNum > 0)
        {
            return dayNum + "天";
        }
        if (hourNum > 0)
        {
            return hourNum + "小时";
        }
        if (minNum > 0)
        {
            return minNum + "分";
        }
        return Math.Max(0, secNum) + "秒";
    }
    public static void WatchPerformance(Action del)
    {
        float startTime = Time.realtimeSinceStartup;
        if (del != null) { del(); }
        TDebug.Log(Time.realtimeSinceStartup - startTime);
    }

    #endregion





    /// <summary>
    /// 缓存当前角色游历挂机中某大事件选择完成的小事件，ID小于等于0移除键值
    /// </summary>
    /// <param name="eventIdx"></param>
    /// <param name="smallTravelEventID"></param>
    public static void SetTravelChooseSmallEventCache(int eventIdx, int smallTravelEventID)
    {
        if (smallTravelEventID<=0)
            PlayerPrefs.DeleteKey(PlayerPrefsBridge.Instance.PlayerData.PlayerUid + eventIdx.ToString());
        else
            PlayerPrefs.SetInt(PlayerPrefsBridge.Instance.PlayerData.PlayerUid+eventIdx.ToString(), smallTravelEventID);
    }
    /// <summary>
    /// 获取当前角色游历挂机中某大事件选择完成的小事件
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public static int GetTravelChooseSmallEventCach(int idx)
    {
        return PlayerPrefs.GetInt(PlayerPrefsBridge.Instance.PlayerData.PlayerUid + idx.ToString());
    }



    /// <summary>
    /// 返回curValue是否包含二次幂的enumValue
    /// </summary>
    public static bool IsPowNumHave(int curValue, int enumValue)
    {
        if (enumValue == 0 || !Mathf.IsPowerOfTwo(enumValue))
        {
            return false;
        }
        for (int i = 0; i < 32; i++)
        {
            if (((enumValue >> i) & 1) == 1)
            {
                if (((curValue >> i) & 1) == 1)
                    return true;
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 返回curValue包含的所有二次幂值
    /// </summary>
    public static List<int> GetPowList(int curValue)
    {
        List<int> l = new List<int>();
        int i = 0;
        while (curValue != 0)
        {
            if (((curValue >> i) & 1) == 1)
            {
                curValue -= (1 << i);
                l.Add(1 << i);
            }
            i++;
        }
        return l;
    }
}
 