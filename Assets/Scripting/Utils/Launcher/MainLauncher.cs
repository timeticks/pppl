using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLauncher : MonoBehaviour
{
    public AssemblyType AssemblyTy;
    static bool mInited = false;

    void Awake()
    {
        Debug.Log(string.Format("alyType:[0]", AssemblyTy.ToString()));
        GameObject go = GameObject.Find("GameLauncher");
        if (go == null)
        {
            go = new GameObject("GameLauncher");
        }
        if (AssemblyTy == AssemblyType.None)
        {
            System.Reflection.Assembly aly = System.Reflection.Assembly.Load("Assembly-CSharp");
            var gameLauncher = aly.GetType("GameLauncher");
            var launcherMethod = gameLauncher.GetMethod("AddGameLauncher");
            launcherMethod.Invoke(null, new object[] { go });
        }
        else if (AssemblyTy == AssemblyType.Reflect)
        {
            ReflectBridge t = go.GetComponent<ReflectBridge>();
            if (t == null) t = go.AddComponent<ReflectBridge>();
            t.Init();
            mInited = true;
        }
        //else
        //{
        //    ILRBridge t = go.GetComponent<ILRBridge>();
        //    if (t == null) t = go.AddComponent<ILRBridge>();
        //    t.Init();
        //    mInited = true;
        //}
    }

}


public enum AssemblyType
{
    None,
    Reflect,
    Ilr,
}