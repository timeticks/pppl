using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



public class UIViewBase : MonoBehaviour
{
    [HideInInspector]
    public List<ViewBindItem> ObjList;
    [HideInInspector]
    public int ObjCount;

    private Dictionary<string, Object> mObjDic;

    public T GetCommon<T>(string nameKey) where T : Object
    {
        if (mObjDic == null) InitObjDic();
        if (!mObjDic.ContainsKey(nameKey))
        {
            TDebug.LogError(string.Format("没有绑定此物体:[{0}]", nameKey));
            return null;
        }
        try
        {
            if (typeof(T) == typeof(Transform))
            {
                return ((GameObject)mObjDic[nameKey]).transform as T;
            }
            else if (typeof(T) == typeof(GameObject))
            {
                return (T)mObjDic[nameKey];
            }
            else if (typeof(T) == typeof(Sprite))
            {
                return (T)mObjDic[nameKey];
            }
            else if (typeof(T) == typeof(Mesh))
            {
                return (T)mObjDic[nameKey];
            }
            else if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                return ((GameObject)mObjDic[nameKey]).GetComponent<T>();
            }
            else if (typeof(Behaviour).IsAssignableFrom(typeof(T)))
            {
                return ((GameObject)mObjDic[nameKey]).GetComponent<T>();
            }
            else if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                return ((GameObject)mObjDic[nameKey]).GetComponent<T>();
            }
            else if (typeof(T) == typeof(RectTransform))
            {
                return ((GameObject)mObjDic[nameKey]).GetComponent<T>();
            }
            else if (typeof(T) == typeof(AudioSource))
            {
                return ((GameObject)mObjDic[nameKey]).GetComponent<T>();
            }
            else
            {
                return (T)mObjDic[nameKey];
            }
        }
        catch
        {
            TDebug.LogError(string.Format("强制转换错误,name:{0}| type:{1}", nameKey, typeof(T)));
        }
        return null;
    }

    private void InitObjDic() //初始化
    {
        //TDebug.Log("仅用于测试绑定脚本的更新1111");
        mObjDic = new Dictionary<string, Object>();
        for (int i = 0; i < ObjList.Count; i++)
        {
            if (!mObjDic.ContainsKey(ObjList[i].Name))
            {
                mObjDic.Add(ObjList[i].Name, ObjList[i].Obj);
            }
            else
            {
                TDebug.LogErrorFormat("ViewObj有重复的Name:{0}" , ObjList[i].Name);
            }
        }
    }
}


[System.Serializable]
public class ViewBindItem
{
    public string Name;
    public Object Obj;
}


#if UNITY_EDITOR

[CanEditMultipleObjects()]
[CustomEditor(typeof(UIViewBase), true)]
public class ViewBaseEditor : Editor
{
    bool mIsFoldout = true;

    void OnEnable()
    {
    }

    void OnDisable()
    {
        //将比ObjCount大的元素，在BindList中删除
        UIViewBase targetIns = (UIViewBase)target;
        if (targetIns.ObjList.Count > targetIns.ObjCount)
        {
            targetIns.ObjList.RemoveRange(targetIns.ObjCount, targetIns.ObjList.Count - targetIns.ObjCount);
        }
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        UIViewBase targetIns = (UIViewBase)target;
        targetIns.ObjCount = EditorGUILayout.IntField("ObjCount ", targetIns.ObjCount);
        if (targetIns.ObjCount < 0) targetIns.ObjCount = 0;
        if (targetIns.ObjList == null)
        {
            targetIns.ObjList = new List<ViewBindItem>();
        }

        for (int i = targetIns.ObjList.Count; i < targetIns.ObjCount; i++)
        {
            targetIns.ObjList.Add(new ViewBindItem());
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
                Object lastObj = targetIns.ObjList[i].Obj;
                targetIns.ObjList[i].Obj =
                    EditorGUILayout.ObjectField("", targetIns.ObjList[i].Obj, typeof(Object), GUILayout.MaxWidth(100)) as Object;

                //绑定后自动赋名字
                if (targetIns.ObjList[i].Obj != null &&
                    (targetIns.ObjList[i].Name == "" || targetIns.ObjList[i].Name == string.Empty || targetIns.ObjList[i].Name == null || lastObj != targetIns.ObjList[i].Obj))
                {
                    targetIns.ObjList[i].Name = targetIns.ObjList[i].Obj.name;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("重排"))
        {
            for (int i = 0, length = targetIns.ObjCount; i < length; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    targetIns.ObjList.RemoveAt(i);
                    i--;
                    length--;
                }
                if (targetIns.ObjList[i].Obj != null) targetIns.ObjList[i].Name = targetIns.ObjList[i].Obj.name;
            }
            targetIns.ObjCount = targetIns.ObjList.Count;
        }

        if (GUILayout.Button("Debug所有ViewObjName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    Debug.LogError("请检查是否有空的Object");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            debugStr += "public class ViewObj";
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
            debugStr += "public ViewObj(UIViewBase view)";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                if (i == 0)
                {
                    debugStr += string.Format("    if({0}!=null)return;\r\n", targetIns.ObjList[i].Name);
                }
                debugStr += string.Format("{0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            debugStr += "}\r\n";
            debugStr += "}\r\n";
            debugStr += " private ViewObj mViewObj;\r\n    public void OpenWindow()\r\n    {\r\n        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);\r\n        base.OpenWin();\r\n    }";
            Debug.Log(debugStr);
        }
        if (GUILayout.Button("Debug所有SmallItemObjName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    Debug.LogError("请检查是否有空的Object");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            debugStr += "public class SmallObj:SmallViewObj";
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
            debugStr += " public override void Init(UIViewBase view)";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                if (i == 0)
                {
                    debugStr +="     if (View != null) return;\r\n      base.Init(view);\r\n";
                }
                debugStr += string.Format("{0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            debugStr += "}\r\n";
            debugStr += "}\r\n";
            Debug.Log(debugStr);
        }
        if (GUILayout.Button("Debug所有ObjName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
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
                debugStr += string.Format("{0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            Debug.Log(debugStr);
        }


    }

    public static string GetNameType(string name)
    {
        string typeStr = "GameObject";
        if (name.Contains("Part_")) typeStr = "GameObject";
        else if (name.Contains("Texture")) typeStr = "RawImage";
        else if (name.Contains("Text")) typeStr = "Text";
        else if (name.Contains("Mat")) typeStr = "Material";
        else if (name.Contains("TBtn") || name.Contains("TextButton")) typeStr = "TextButton";
        else if (name.Contains("Btn") || name.Contains("Button")) typeStr = "Button";
        else if (name.Contains("Root") || name.Contains("Panel")) typeStr = "Transform";
        else if (name.Contains("Icon") || name.Contains("Image")) typeStr = "Image";

        return typeStr;
    }

    public static void MyViewInspector(UIViewBase targetIns, ref  bool isFoldout)
    {

    }
}
#endif





public class FitItemIndex
{
    private UIFitScroller _scroller;
    private int _index;
    public bool isMaxSize;
    public UIViewBase View;

    public int Index
    {
        get { return _index; }
        set
        {
            _index = value;
            View.transform.localPosition = _scroller.GetPosition(_index);
            View.gameObject.name = _index.ToString();
            //_textIndex.text = _index.ToString();
            //gameObject.name = "Scroll" + (_index < 10 ? "0" + _index : _index.ToString());
        }
    }

    public virtual void Init(UIViewBase view)
    {
        View = view;
    }
    public void SetFalse()
    {
        Index = -2;
        isMaxSize = false;
        View.transform.localPosition = new Vector3(-4000, 0, 0);
    }

    public UIFitScroller Scroller
    {
        set { _scroller = value; }
    }
}
public class ItemIndex
{
    private UIScroller _scroller;
    private int _index;
     public UIViewBase View;

    public ItemIndex()
    {
    }

    public int Index
    {
        get { return _index; }
        set
        {
            _index = value;
            View.transform.localPosition   = _scroller.GetPosition(_index);
            View.gameObject.name = _index.ToString();
            //_textIndex.text = _index.ToString();
            //gameObject.name = "Scroll" + (_index < 10 ? "0" + _index : _index.ToString());
        }
    }

    public void SetFalse()
    {
        Index = -2;
        View.transform.localPosition = new Vector3(-4000, 0, 0);
    }

    public UIScroller Scroller
    {
        set { _scroller = value; }
    }
}