using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class YamlParse
{
    //public static Regex ex = new Regex("\t");
    //public static int GetTabCount(string str1) //得到换行符数量
    //{
    //    return ex.Matches(str1).Count;
    //}

    public const char yamlChar = '\t';
    public static int GetTabCount(string str1) //得到换行符数量
    {
        int count = 0;
        for (int k = 0; k < str1.Length; k++)  
        {
            if (str1[k].Equals(yamlChar)) 
            {  
                count ++;
            }  
        }
        return count;
    }

    public const char enterStr = '\r';
    public static int GetKeyCount(string str1) //得到换行符数量
    {
        int count = 0;
        int length = str1.Length - 1;
        for (int k = 0; k < length; k++)
        {
            if (str1[k].Equals(enterStr))
            {
                if(str1[k+1].Equals('\n'))
                    count++;
            }
        }
        return count;
    }

    public static int GetCharCount(string str , char ch)
    {
        int count = 0;
        for (int k = 0; k < str.Length; k++)
        {
            if (str[k].Equals(ch))
            {
                count++;
            }
        }
        return count;
    }
}


