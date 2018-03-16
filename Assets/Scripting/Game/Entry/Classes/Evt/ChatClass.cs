using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatElem
{
    public int m_RoleId;
    public FaceType m_Face;
    public string m_Chat;
}

public class ChatData
{
    public int m_ChatId;
    public List<ChatElem> m_List;

    public ChatData(int chatId)
    {
        m_List = new List<ChatElem>();
    }
}


public enum FaceType
{
    no,
    happy,
    serious,
    sad
}

