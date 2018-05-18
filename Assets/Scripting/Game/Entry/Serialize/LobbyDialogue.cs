using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ILobbyDialogueFetcher
{
    LobbyDialogue GetLobbyDialogueByCopy(string key);
}
public class LobbyDialogue :BaseObject
{
    private static ILobbyDialogueFetcher mFetcher;

    public static ILobbyDialogueFetcher LobbyDialogueFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public string ch;
    public string eg;


    public LobbyDialogue() : base() { }


    public LobbyDialogue Clone()
    {
        return (LobbyDialogue) this.MemberwiseClone();
    }




    public static string GetDescStr(string key, params object[] st)
    {
        LobbyDialogue temp = LobbyDialogue.LobbyDialogueFetcher.GetLobbyDialogueByCopy(key);
        if (temp != null)
        {
            try { return string.Format(temp.ch, st).Replace(@"\n", "\n").Replace(@"\f", "\f").Replace(@"\u3000","\u3000"); }
            catch { return string.Format("文本参数个数不匹配:{0}|{1}", key, st.Length); }
        }
        else
        {
            return string.Format(string.Format("{0}", key), st);
            //return string.Format(string.Format("nul:{0}", key), st);
        }
    }

}

