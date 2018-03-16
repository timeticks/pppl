using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class YamlParse
{
    public static int GetTabCount(string str1) //得到换行符数量
    {
        Regex ex = new Regex("\t");
        return ex.Matches(str1).Count;
    }  
}


