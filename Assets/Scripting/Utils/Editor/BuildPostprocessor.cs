using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class BuildPostprocessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log(string.Format("OnPostprocessBuild:{0}|{1} ", target.ToString(), pathToBuiltProject));
        if (target == BuildTarget.Android && (!pathToBuiltProject.EndsWith(".apk")))
        {
            Debug.Log("target: " + target.ToString());
            Debug.Log("pathToBuiltProject: " + pathToBuiltProject);
            Debug.Log("productName: " + PlayerSettings.productName);

            string monoPath = Application.dataPath.Remove(Application.dataPath.LastIndexOf("/Assets")) + "/ThirdPlugins/MyMono";
            Debug.Log("monoPath: " + monoPath);

            string dllPath = pathToBuiltProject + "/" + PlayerSettings.productName + "/" + "assets/bin/Data/Managed/Assembly-CSharp.dll";

            if (File.Exists(dllPath))
            {
                //加密 Assembly-CSharp.dll;  
                //Debug.Log("Encrypt assets/bin/Data/Managed/Assembly-CSharp.dll Start");  

                //byte[] bytes = File.ReadAllBytes(dllPath);  
                //bytes[0] += 1;  
                //File.WriteAllBytes(dllPath, bytes);  

                //Debug.Log("Encrypt assets/bin/Data/Managed/Assembly-CSharp.dll Success");  

                //Debug.Log("Encrypt libmono.so Start !!");  

                //Debug.Log("Current is : " + EditorUserBuildSettings.development.ToString());  

                //替换 libmono.so;  
                if (EditorUserBuildSettings.development)
                {
                    //string armv7a_so_path = pathToBuiltProject + "/" + PlayerSettings.productName + "/" + "libs/armeabi-v7a/libmono.so";
                    //Debug.LogError("armv7a_so_path:"+armv7a_so_path); 
                    ////File.Copy(Application.dataPath + "/MonoEncrypt/Editor/libs/development/armeabi-v7a/libmono.so", armv7a_so_path, true);  

                    //string x86_so_path = pathToBuiltProject + "/" + PlayerSettings.productName + "/" + "libs/x86/libmono.so";  
                    // Debug.LogError("armv7a_so_path:"+x86_so_path); 
                    //File.Copy(Application.dataPath + "/MonoEncrypt/Editor/libs/development/x86/libmono.so", x86_so_path, true);  
                }
                else
                {
                    string armv7a_so_path = pathToBuiltProject + "/" + PlayerSettings.productName + "/" + "libs/armeabi-v7a/libmono.so";
                    File.Copy(monoPath + "/armv7a/libmono.so", armv7a_so_path, true);

                    string x86_so_path = pathToBuiltProject + "/" + PlayerSettings.productName + "/" + "libs/x86/libmono.so";
                    File.Copy(monoPath + "/x86/libmono.so", x86_so_path, true);
                }

                Debug.Log("Encrypt libmono.so Success !!");
            }
            else
            {
                Debug.LogError(dllPath + "  Not Found!!");
            }
        }
    }
}
