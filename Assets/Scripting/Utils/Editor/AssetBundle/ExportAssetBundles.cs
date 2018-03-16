using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class ExportAssetBundles
{
    public static string BundleOutputPath;

    static ExportAssetBundles()
    {
        BundleOutputPath = string.Format("{0}/{1}/{2}", "AssetBundles", PlatformUtils.PlatformTy, "Res");
    }
    
    static void BatchSetAssetName(string head)  //分别各不同的资源设置ab值
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        List<string> resourcesAssets = new List<string>();
        for (int i = 0; i < selection.Length; i++)
        {
            string source = AssetDatabase.GetAssetPath(selection[i]);
            if (!source.Contains("."))continue;   //如果是文件夹

            string _source = (source).Replace("\\", "/");
            string _assetPath = _source;
            //在代码中给资源设置AssetBundleName  
            AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath);
            string assetName = _assetPath.Substring(_assetPath.LastIndexOf("/") + 1);
            assetName = assetName.Remove(assetName.IndexOf('.') == -1 ? assetName.Length - 1 : assetName.IndexOf('.')); //删除后缀
            //Debug.Log (assetName);  
            assetImporter.assetBundleName = (head+assetName).ToLower();
        }
    }

    [MenuItem("Export/Export All One Key Auto(FreshStreamVersion And Copy )", priority = 1)]
    static void ExportResource_AutoCopy()
    {
        ExportResource_Auto();
        CopyInStreamingPath();
    }
    [MenuItem("Export/Export All One Key Auto(FreshStreamVersion And Copy Force Delete )", priority = 2)]
    static void ExportResource_AutoCopy_ForceDelete()
    {
        if (Directory.Exists(BundleOutputPath))
        {
            DirectoryInfo di = new DirectoryInfo(BundleOutputPath);
            di.Delete(true);
        }
        ExportResource_Auto();
        CopyInStreamingPath();
    }

    [MenuItem("Export/Export All One Key Auto(FreshStreamVersion)", priority = 3)]
    static void ExportResource_Auto()
    {
        //BuildScript.BuildAssetBundles();
        // 检查文件夹
        //if (!Directory.Exists(AssetBundles.Utility.OutputPath))
        //    Directory.CreateDirectory(AssetBundles.Utility.OutputPath);
        //BuildPipeline.BuildAssetBundles(AssetBundles.Utility.OutputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        //MD5Utility.CheckAndCreateVersion(AssetBundles.Utility.OutputPath + "/");

        if (!Directory.Exists(BundleOutputPath))
            Directory.CreateDirectory(BundleOutputPath);
        BuildPipeline.BuildAssetBundles(BundleOutputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        MD5Utils.CheckAndCreateVersion(BundleOutputPath + "/");
    }

    

    [MenuItem("Export/SetBundleName")]
    public static void BuildAssetBundles()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            //得到指定资源路径  
            string path = AssetDatabase.GetAssetPath(Selection.objects[i]);
            string abName = AssetImporter.GetAtPath(path).assetBundleName;
            abName = FileBaseUtils.GetDirectoryPath(abName);
            //得到指定资源的bundle名字  
            
            //得到指定资源的依赖资源路径  
            //string[] depends = AssetDatabase.GetDependencies(path);
            //修改所有依赖的bundle名  
            if (path.EndsWith(".cs") || path.EndsWith(".js")) continue;

            AssetImporter ai = AssetImporter.GetAtPath(path);
            ai.assetBundleName = abName+"/"+Selection.objects[i].name.ToLower();
        }
    }


    public static bool SimulateAssetBundleInEditor
    {
        get { return PlayerPrefs.GetInt("SimulateAssetBundleInEditor", 0) == 1; }
        set { PlayerPrefs.SetInt("SimulateAssetBundleInEditor", value ? 1 : 0); }
    }

    const string kSimulationMode = "Export/Simulation Mode";
    [MenuItem(kSimulationMode)]
    public static void ToggleSimulationMode()
    {
        SimulateAssetBundleInEditor = !SimulateAssetBundleInEditor;
        Debug.Log(SimulateAssetBundleInEditor);
    }

    [MenuItem(kSimulationMode, true)]
    public static bool ToggleSimulationModeValidate()
    {
        Menu.SetChecked(kSimulationMode, SimulateAssetBundleInEditor);
        return true;
    }

    [MenuItem("Export/Copy In StreamingPath")]
    static public void CopyInStreamingPath()//自动化构建时，自动调用此接口
    {
        CopyFileTo(Path.Combine(System.Environment.CurrentDirectory, BundleOutputPath), FileBaseUtils.StreamingAssetsPath + "/" + FileBaseUtils.GameResPath);
        AssetDatabase.Refresh();
    }

    public static void CopyFileTo(string source, string outputPath)
    {
        //清空之前的
        if (Directory.Exists(outputPath))
            FileUtil.DeleteFileOrDirectory(outputPath);
        else
        {
            Directory.CreateDirectory(outputPath);
            AssetDatabase.Refresh();
            Debug.LogError(string.Format("目录不存在,进行了自动创建,请再点一次: {0}", outputPath));
            return;
        }

        if (!System.IO.Directory.Exists(source))
        {
            Debug.LogError("No assetBundle output folder, try to build the assetBundles first.");
            return;
        }
        FileUtil.CopyFileOrDirectory(source, outputPath);
    }
}