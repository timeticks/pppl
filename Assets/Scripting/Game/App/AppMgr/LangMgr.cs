using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 语言管理器
/// </summary>
public static class LangMgr
{
    public static LanguageType LangType;  //当前语言

    #region 文本
    public static string ServerTipsText(int id)//服务器消息文本
    {
        Hashtable tab = GameData.Instance.GetData(DataName.ServerTipsText, id.ToString());
        if (tab != null)
        {
            switch (LangMgr.LangType)
            {
                case LanguageType.ZHCN: return TUtility.TryGetValueStr(tab, "zhcn", "");
                case LanguageType.EN: return TUtility.TryGetValueStr(tab, "en", "");
            }
        }
        TDebug.LogError("服务器语言文本里没有此ServerTipsText: " + id);
        return "";
    }

    public static string GetText(DataName dataName, string textIdx)
    {
        Hashtable hash = GameData.Instance.GetData(dataName, textIdx);
        return TUtility.TryGetValueStr(hash, LangType.ToString().ToLower(), "");
    }

    public static string GetText(string textKey, params string[] st)  //根据语言读取文本
    {
        return LobbyDialogue.GetDescStr(textKey, st);
        //return textKey;
        string pre = textKey.Remove(textKey.IndexOf("_"));
        Hashtable tab = null;
        switch (pre)
        {
            //case "Goods": case "GoodsText": tab = GameData.Instance.GetData(DataName.GoodsText, textKey); break;
            //case "Skill": case "SkillText": tab = GameData.Instance.GetData(DataName.SkillText, textKey); break;
            //case "City": case "CityText": tab = GameData.Instance.GetData(DataName.CityText, textKey); break;
            //case "ChatText": case "InvestChatText": tab = GameData.Instance.GetData(DataName.InvestChatText, textKey); break;
            default: break;
        }
        if (tab != null)
        {
            switch (LangMgr.LangType)
            {
                case LanguageType.ZHCN: return TUtility.TryGetValueStr(tab, "zhcn", "");
                case LanguageType.EN: return TUtility.TryGetValueStr(tab, "en", "");
            }
        }
        TDebug.LogError("语言文本里没有此textKey: " + textKey);
        return "";
    }
    #endregion



}
public static class TextExtension
{
    public static void SetText(this Text text, string str)
    {
        text.text = str;
    }
    public static string GetText(this Text text)
    {
        return text.text;
    }
    public static void SetValue(this Text text, string key)
    {
        text.text = LangMgr.GetText(key);
    }

}

public enum LanguageType
{
    ZHCN,  //简体中文
    ZHHK,  //繁体中文
    EN,    //英语
}