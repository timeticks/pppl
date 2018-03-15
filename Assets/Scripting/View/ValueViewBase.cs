using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ValueViewBase : MonoBehaviour {

    [HideInInspector]
    public List<ViewValueItem> ObjList;
    [HideInInspector]
    public int ObjCount;

    private Dictionary<string, string> mObjDic;

    public int Getint(string nameKey)
    {
        if (mObjDic == null) InitObjDic();
        if (!mObjDic.ContainsKey(nameKey))
        {
            TDebug.LogError(string.Format("没有绑定此物体:[{0}]", nameKey));
            return 0;
        }
        return (ToInt(mObjDic[nameKey]));
    }
    public string Getstring(string nameKey)
    {
        if (mObjDic == null) InitObjDic();
        if (!mObjDic.ContainsKey(nameKey))
        {
            TDebug.LogError(string.Format("没有绑定此物体:[{0}]", nameKey));
            return "";
        }
        return mObjDic[nameKey];
    }
    public float Getfloat(string nameKey)
    {
        if (mObjDic == null) InitObjDic();
        if (!mObjDic.ContainsKey(nameKey))
        {
            TDebug.LogError(string.Format("没有绑定此物体:[{0}]", nameKey));
            return 0;
        }
        return (ToFloat(mObjDic[nameKey]));
    }
    public bool Getbool(string nameKey)
    {
        if (mObjDic == null) InitObjDic();
        if (!mObjDic.ContainsKey(nameKey))
        {
            TDebug.LogError(string.Format("没有绑定此物体:[{0}]", nameKey));
            return false;
        }
        if (mObjDic[nameKey].Equals("true") || mObjDic[nameKey].Equals("1"))
            return true;
        return false;
    }


    private void InitObjDic() //初始化物体字典
    {
        mObjDic = new Dictionary<string, string>();
        for (int i = 0; i < ObjList.Count; i++)
        {
            if (!mObjDic.ContainsKey(ObjList[i].Name))
            {
                mObjDic.Add(ObjList[i].Name, ObjList[i].Val);
            }
            else
            {
                TDebug.LogErrorFormat("ViewObj有重复的Name:{0}" , ObjList[i].Name);
            }
        }
    }


    static float ToFloat(string str)
    {
        float i = 0;
        float.TryParse(str, out i);
        return i;
    }
    static int ToInt(string str)
    {
        int i = 0;
        int.TryParse(str, out i);
        return i;
    }
}


[System.Serializable]
public class ViewValueItem
{
    public string Name;
    public string Val;
}


#if UNITY_EDITOR

[CanEditMultipleObjects()]
[CustomEditor(typeof(ValueViewBase), true)]
public class ValueViewBaseEditor : Editor
{
    bool mIsFoldout = true;

    void OnDisable()
    {
        //将比ObjCount大的元素，在BindList中删除
        ValueViewBase targetIns = (ValueViewBase)target;
        if (targetIns.ObjList.Count > targetIns.ObjCount)
        {
            targetIns.ObjList.RemoveRange(targetIns.ObjCount, targetIns.ObjList.Count - targetIns.ObjCount);
        }
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        ValueViewBase targetIns = (ValueViewBase)target;
        targetIns.ObjCount = EditorGUILayout.IntField("ObjCount ", targetIns.ObjCount);
        if (targetIns.ObjCount < 0) targetIns.ObjCount = 0;
        if (targetIns.ObjList == null)
        {
            targetIns.ObjList = new List<ViewValueItem>();
        }

        for (int i = targetIns.ObjList.Count; i < targetIns.ObjCount; i++)
        {
            targetIns.ObjList.Add(new ViewValueItem());
        }
        mIsFoldout = EditorGUILayout.Foldout(mIsFoldout, "ObjList", true);//是否展开
        if (mIsFoldout)
        {
            for (int i = 0; i < targetIns.ObjCount; i++)//按ObjCount，显示文本
            {
                EditorGUILayout.BeginHorizontal();
                targetIns.ObjList[i].Name =
                    EditorGUILayout.TextField("    " + i, targetIns.ObjList[i].Name);
                if (targetIns.ObjList[i] != null && targetIns.ObjList[i].Name != null && targetIns.ObjList[i].Name.Contains(" "))
                {
                    Debug.LogError(string.Format("取名不要含有空格 [{0}]", targetIns.ObjList[i].Name));
                    targetIns.ObjList[i].Name = targetIns.ObjList[i].Name.Replace(" ", "");
                }
                string lastObj = targetIns.ObjList[i].Val;
                targetIns.ObjList[i].Val =
                    EditorGUILayout.TextField("", targetIns.ObjList[i].Val, GUILayout.MaxWidth(100));

                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Debug所有ViewValueName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Name == null || targetIns.ObjList[i].Name.Length==0)
                {
                    Debug.LogError("请检查是否有空的Name");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            debugStr += "public class ViewValue";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                {
                    nameDic.Add(targetIns.ObjList[i].Name, false);
                    string typeStr = GetNameType(targetIns.ObjList[i].Name);
                    debugStr += string.Format("public {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                }
                else
                {
                    Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                }
            }
            debugStr += "public ViewValue(ValueViewBase view)";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                if (i == 0)
                {
                    debugStr += string.Format("    if({0}!=null)return;\r\n", targetIns.ObjList[i].Name);
                }
                debugStr += string.Format("{0} = view.Getstring(\"{0}\");\r\n", targetIns.ObjList[i].Name);
            }
            debugStr += "}\r\n";
            debugStr += "}\r\n";
            Debug.Log(debugStr);
        }
        else if (GUILayout.Button("Debug所有ValueName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Val.Length==0)
                {
                    Debug.LogError("请检查是否有空的Object");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                {
                    nameDic.Add(targetIns.ObjList[i].Name, false);
                    string typeStr = GetNameType(targetIns.ObjList[i].Name);
                    debugStr += string.Format("public {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                }
                else
                {
                    Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                }
            }
            debugStr += "\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                debugStr += string.Format("{0} = view.Get{1}(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            Debug.Log(debugStr);
        }


    }

    public static string GetNameType(string name)
    {
        string typeStr = "string";
        if (name.Contains("I_")) typeStr = "int";
        else if (name.Contains("F_")) typeStr = "float";
        else if (name.Contains("S_")) typeStr = "string";
        else if (name.Contains("B_")) typeStr = "bool";
        return typeStr;
    }

}
#endif

