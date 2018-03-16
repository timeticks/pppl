using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class BuilderWindow : EditorWindow
{
    public const string BuilderConfigPath = "Assets/Game/TResources/Resources";  //配置文件路径
    public static BuilderData m_BuilderData;

    List<string> m_allBuilderFile = new List<string>();  //文件名
    string m_curSelectFile = "";   //当前选中的配置文件
    int m_curSelectIndex = 0;
    int m_frameCount = 0;

    [MenuItem("Tools/OpenBuilder" , priority=1)]
    static void OpenSendWin()
    {
        BuilderWindow win = EditorWindow.GetWindow<BuilderWindow>(false, "Builder", true);
        win.Show();
    }
    
    //[MenuItem("Tools/CreateBuilderData")]
    //static void CreateBuilderData()
    //{
    //    BuilderData temp = new BuilderData();
    //    AssetDatabase.CreateAsset(temp, BuilderConfigPath + "/BuilderData.asset");
    //    AssetDatabase.Refresh();
    //}

    void OnGUI()
    {
        if (m_allBuilderFile.Count == 0)  //初始化builder信息
        {
            if (Directory.Exists(BuilderConfigPath))
            {
                DirectoryInfo direction = new DirectoryInfo(BuilderConfigPath);
                FileInfo[] files = direction.GetFiles("*.asset", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    string localPath = files[i].FullName.Substring(files[i].FullName.IndexOf("Assets")).Replace("\\", "/");
                    if (!files[i].Name.Contains("Builder")) continue; //如果没有包含builder，则跳过
                    m_allBuilderFile.Add(files[i].Name);
                }
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("");
        GUILayout.EndHorizontal();
        m_curSelectIndex = EditorGUILayout.Popup(m_curSelectIndex, m_allBuilderFile.ToArray(), GUILayout.MinWidth(150f));
        if (m_curSelectFile != m_allBuilderFile[m_curSelectIndex]) //如果改变了选中的文件
        {
            m_curSelectFile = m_allBuilderFile[m_curSelectIndex].Remove(m_allBuilderFile[m_curSelectIndex].IndexOf("."));
            m_BuilderData = Resources.Load(m_curSelectFile, typeof(BuilderData)) as BuilderData;
        }

        for (int i = 0; i < m_BuilderData.m_ItemList.Count; i++)
        {
            ClientSettingData dataItem = m_BuilderData.m_ItemList[i];
            //if (NGUIEditorTools.DrawHeader("" + i, true))
            {
                //版本，路径
                //dataItem.m_PackageVersion = EditorGUILayout.IntField("PackageVersion", dataItem.m_PackageVersion, GUILayout.MinWidth(150f));
                //if (string.IsNullOrEmpty(dataItem.m_BuildPath)) dataItem.m_BuildPath = "";
                //dataItem.m_BuildPath = EditorGUILayout.TextField("BuildPath", dataItem.m_BuildPath, GUILayout.MinWidth(150f));
                ////if (!m_Data.m_BuildPath.Contains("AssetBundles/")) m_Data.m_BuildPath = "AssetBundles/sailage";

                //dataItem.m_PackageName = EditorGUILayout.TextField("PackageName", dataItem.m_PackageName, GUILayout.MinWidth(150f));

                //短连
                //GUILayout.BeginHorizontal();
                //dataItem.m_HttpIpType = (IpType)EditorGUILayout.EnumPopup("HttpIpType", dataItem.m_HttpIpType, GUILayout.MinWidth(150f));
                ////GUILayout.Label(AssetUpdate.HttpIPTable[(int)dataItem.m_HttpIpType]);
                //GUILayout.EndHorizontal();

                //长连
                //GUILayout.BeginHorizontal();
                //dataItem.m_NetIpType = (IpType)EditorGUILayout.EnumPopup("NetIpType", dataItem.m_NetIpType, GUILayout.MinWidth(150f));
                ////GUILayout.Label(AssetUpdate.NetIPTable[(int)dataItem.m_NetIpType]);
                //GUILayout.EndHorizontal();

                ////资源
                //GUILayout.BeginHorizontal();
                //dataItem.m_ResType = (ResType)EditorGUILayout.EnumPopup("ResType", dataItem.m_ResType, GUILayout.MinWidth(150f));
                ////GUILayout.Label(AssetUpdate.ResTable[(int)dataItem.m_ResType]);
                //GUILayout.EndHorizontal();

                //资源包
                dataItem.m_IsPure = EditorGUILayout.Toggle("Is Pure", dataItem.m_IsPure);
            }
        }

        
        if (GUI.Button(new Rect(6, this.position.size.y - 90,this.position.size.x - 12, 82) , "Do Build"))
        {
            FreshSave();
            for (int i = 0; i < m_BuilderData.m_ItemList.Count; i++)
            {
                if (m_BuilderData.m_ItemList[i].m_PackageName.Equals("")) { Debug.LogError("错误，PackageName为空"); return; }
                if (m_BuilderData.m_ItemList[i].m_IsPure) { BuildAndroidPure(m_BuilderData.m_ItemList[i]); }
                else { BuildAndroid(m_BuilderData.m_ItemList[i]); }
            }
        }

        //检查修改 ，并刷新保存
        m_frameCount++;
        if (m_frameCount % 30 == 0)
        {
            FreshSave();
        }
    }

    void FreshSave()
    {
        return;
        m_frameCount++;
        if (true)   //如果有修改
        {
            string localPath = string.Format("{0}/{1}.asset", BuilderConfigPath, m_allBuilderFile[m_curSelectIndex]);
            AssetDatabase.CreateAsset(m_BuilderData, localPath);
            AssetDatabase.Refresh();
        }
    }

    void OnInspectorUpdate()
    {
        this.Repaint();//这里开启窗口的重绘，不然窗口信息不会刷新
    }





    // 输出路径
    private static string BuildPath = "";//此项为你需要输出的安卓工程的路径

    public static void BuildAndroid(ClientSettingData builderItem)
    {
        ClientSetting temp = new ClientSetting(builderItem);
        AssetDatabase.CreateAsset(temp, BuilderConfigPath + "/ClientSetting.asset");

        BuilderWindow win = EditorWindow.GetWindow<BuilderWindow>(false, "Builder", true);
        BuildPath = Application.dataPath.Remove(Application.dataPath.IndexOf("/Assets"));
        if (!builderItem.m_PackageName.EndsWith(".apk")) { builderItem.m_PackageName += ".apk"; }
        BuildPath += string.Format("/{0}/{1}", builderItem.m_BuildPath, builderItem.m_PackageName);
        // 如果不是android平台,转为android平台
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        }

        // 允许读写SD卡
        PlayerSettings.Android.forceSDCardPermission = true;

        // 设置 keystore 信息根据自己的KEY设置
        //PlayerSettings.Android.keystoreName = "hai.keystore";
        //PlayerSettings.Android.keystorePass = "123";
        //PlayerSettings.Android.keyaliasName = "123";
        //PlayerSettings.Android.keyaliasPass = "123";

        // 充许调试 开发 外部修改
        //BuildOptions options = BuildOptions.AcceptExternalModificationsToPlayer;  //打包出java工程
        BuildOptions options = BuildOptions.None;     //打包apk

        // 添加一个叫Debug的宏
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");

        // 设置标识符
       // PlayerSettings.bundleIdentifier = "com.DaShen.SailAgePix2";//此项为当你导出安卓工程时的包名;
        PlayerSettings.companyName = "DaShen";
        PlayerSettings.productName = "SailAgePix2";

        // 检查输出路径存在则删除;
        if (System.IO.Directory.Exists(BuildPath))
        {
            System.IO.Directory.Delete(BuildPath);
        }

        if (EditorBuildSettings.scenes.Length > 1)
        {
            TDebug.LogError("场景数大于1，请检查"); return;
        }
        ClientSetting setData = Resources.Load(ClientSetting.fileName, typeof(ClientSetting)) as ClientSetting;
#if UNITY_STANDALONE_WIN
        TDebug.Log("PC");
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, BuildPath, BuildTarget.StandaloneWindows64, options);
#elif  UNITY_ANDROID
        TDebug.Log("安卓");
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, BuildPath, BuildTarget.Android, options);
#elif UNITY_IPHONE
        TDebug.Log("苹果");
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, BuildPath, BuildTarget.iOS, options);
#else
        TDebug.Log("PC，不进行build");
#endif
        //string versionPath = BuildPath.Remove(BuildPath.LastIndexOf("/")) + "/" + AssetBundles.Utility.GetPlatformName() + "/" + SharedAsset.VersionPackageTxt;
        //FileStream stream = new FileStream(versionPath, FileMode.OpenOrCreate);
        //byte[] data = Encoding.UTF8.GetBytes(setData.m_Data.m_PackageVersion.ToString());
        //stream.Write(data, 0, data.Length);
        //stream.Flush();
        //stream.Close();
        //stream.Dispose();
        AssetDatabase.Refresh();
    }


    public static void BuildAndroidPure(ClientSettingData builderItem)
    {
        System.IO.Directory.Move(string.Format("{0}/StreamingAssets", Application.dataPath), string.Format("{0}/StreamingAssets2", Application.dataPath));
        BuildAndroid(builderItem);
        System.IO.Directory.Move(string.Format("{0}/StreamingAssets2", Application.dataPath), string.Format("{0}/StreamingAssets", Application.dataPath));
    }

}