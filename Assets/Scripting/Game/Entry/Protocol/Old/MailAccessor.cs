using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MailAccessor :DescObject
{
    public bool Inited = false;
    public bool IsHaveNewMail;
    public int  MailAmount;
    public List<Mail> MailList = new List<Mail>();



    public MailAccessor() { }

    public MailAccessor(MailAccessor origin)
    {
        this.IsHaveNewMail = origin.IsHaveNewMail;

    }

}

