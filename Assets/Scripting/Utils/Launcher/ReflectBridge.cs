using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ReflectBridge : MonoBehaviour {
    static bool mInited = false;
    static MethodInfo mLauncherMethod;
    public void Init()
    {
        if (mInited)
        {
            mLauncherMethod.Invoke(null, new object[] { new GameObject("AppLauncher") });
            return;
        }
        gameObject.AddComponent<LoadDllHelper>().LoadDll(InitAssembly);

    }

    void InitAssembly(byte[] dllBytes, byte[] pdbBytes)
    {
        Debug.Log(string.Format("dllByte is null : {0}", dllBytes == null));
        Assembly aly = null;
        if (dllBytes == null)
        {
            aly = System.Reflection.Assembly.Load("Assembly-CSharp");
        }
        else
        {
            Debug.Log("InitAssembly" + dllBytes.Length.ToString());
            dllBytes = TUtilityBase.EncryptDll(dllBytes);
            aly = System.Reflection.Assembly.Load(dllBytes);
        }
        GameObject obj = new GameObject("AppLauncher");
        var gameLauncher = aly.GetType("GameLauncher");
        mLauncherMethod = gameLauncher.GetMethod("AddGameLauncher");
        mLauncherMethod.Invoke(null, new object[] { obj });
        mInited = true;
    }
}
