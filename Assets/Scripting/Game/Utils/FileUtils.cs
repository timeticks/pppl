
using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System;
public class FileUtils
{

    public const string GameResPath = "/Game/Res";
    public const string GameResName = "Res.bytes";


    public const string GameDataPath = "/Game/Data";
    public const string GameDataName = "GameData";

    public const string GameLocalizaPath = "/Game/Localiza";

    public static PlatformType Platform { get { return mPlatform; } }



    private static ServerInfo _Instance;

    private static string mResFolder;
    private static string mDLCURL;
    private static string mNoticeURL;
    private static string mServerAreaURL;

    private static PlatformType mPlatform;
    private static string mBundleStoreURL;
    private static string mResURL;
    private static string mAppInfoURL;

    private static int mUpdateBundleVersion;
    private static uint mUpdateBundleCRC;

    private static string mGameDataURL;
    private static string mGameLocalizaURL;

    private static string mStreamingAssetsPath;
    private static string mPersistentDataPath;
    private static string mPersistentDataPurePath;


    public static string MindTreeMapUrl { get; private set; }
    public static string BundleOutputPath { get; private set; }

    static FileUtils()
    {
        //if (Application.platform == RuntimePlatform.Android)
        //    mPlatform = PlatformType.Android;
        //else if (Application.platform == RuntimePlatform.IPhonePlayer ||
        //        Application.platform == RuntimePlatform.OSXEditor)
        //    mPlatform = PlatformType.iOS;
        //else
        //    mPlatform = PlatformType.Standalone;
#if UNITY_ANDROID
        mPlatform = PlatformType.Android;
#elif UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
        mPlatform = PlatformType.iOS;
#else
        mPlatform = PlatformType.Standalone;
#endif

        mStreamingAssetsPath = string.Format("{0}/{1}", Application.streamingAssetsPath, mPlatform);
        mPersistentDataPath = string.Format("{0}/{1}", Application.persistentDataPath, mPlatform);
        BundleOutputPath = string.Format("{0}/{1}/{2}", "AssetBundles", mPlatform, "Res");

    }



    public static void Init()
    {
        mDLCURL = string.Format("http://{0}/{1}/{2}", AppSetting.AppHost, mPlatform, AppSetting.Version); ;
        mAppInfoURL = string.Format("{0}/AppInfo.html", mDLCURL);
        mGameDataURL = string.Format("{0}/Data.lib", mDLCURL);
        mGameLocalizaURL = string.Format("{0}/Localiza.lib", mDLCURL);
        mNoticeURL = string.Format("{0}/Notice.html", mDLCURL);
        mServerAreaURL = string.Format("{0}/ServerArea.html", mDLCURL);
        mResURL = string.Format("{0}/Res", mDLCURL);//"http://localhost/Res";
        MindTreeMapUrl = string.Format("{0}/{1}", mDLCURL, TUtils.MDEncode("MindTreeMap"));

        TDebug.LogWarning("↓↓↓↓↓↓↓↓↓↓↓游戏环境↓↓↓↓↓↓↓↓↓");
        TDebug.LogWarning(string.Format("DLCUrl:{0}", mDLCURL));
        TDebug.LogWarning(string.Format("AppInfoURL:{0}", mAppInfoURL));
        TDebug.LogWarning(string.Format("GameDataURL:{0}", mGameDataURL));
        TDebug.LogWarning(string.Format("GameLocalizaURL:{0}", mGameDataURL));
        TDebug.LogWarning(string.Format("NoticeUrl:{0}", mNoticeURL));
        TDebug.LogWarning("↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑");

    }

    //public AppServSetting AppServInfo
    //{
    //    get { return mServSetting; }
    //    set
    //    {
    //        mServSetting = value;
    //        //Debug.LogError("========================游戏环境===========================");
    //        //Debug.LogError("GameVersion=====>"      + mServSetting.GameVersion);
    //        //Debug.LogError("GateSrvIp=====>"        + mServSetting.GateSrvIp);
    //        //Debug.LogError("GateServerPort=====>"   + mServSetting.GateServPort);
    //        //Debug.LogError("BundleVersion=====>"    + mServSetting.BundleVersion);
    //    }
    //}

    public static int IsForceUpdate(double fromVer, double toVer)
    {
#if UNITY_EDITOR
        return 0;
#endif
        if (Mathf.Abs((float)(fromVer - toVer)) < 0.0001f) return 0;
        if (!ServerInfo.GameVersion.Equals(AppSetting.GameVersion))
        {
            if (FileUtils.Platform == PlatformType.Android)
            {
                if ((int)(fromVer / 100) - (int)(toVer / 100) > 0) //只有百位数改变，android才会强制更新包
                {
                    return 2;
                }
                return 1;
            }
            else
            {
                if (Mathf.Abs((int)fromVer - (int)toVer) >= 1)
                    return 2;
                return 0;
            }
            //mIAssetsLoader.OnChangeState("客户端版本已有新版本(Ver:"+ servSetting.GameVersion+ "),请重新更新游戏版本,W_Code:105");
        }
        return 0;
    }


    ///////////////////////////Get And Set Method//////////////////////////////////////////////////////////////////////////////
    //安卓  persistentDataPath  读取+ file:// 写 则不用 
    //安卓  streamingAssetsPath 读取+ file:// 写 则不用 
    public static string PersistentDataWritePath(string url, string filename)
    {
        //    //方法一致,没问题可以合并
#if UNITY_ANDROID && !UNITY_EDITOR
        return Path.Combine(mPersistentDataPath + "/" + AppSetting.Version + "/" + url, filename);
#else
        return Path.Combine(mPersistentDataPath + "/" + AppSetting.Version + "/" + url, filename);
#endif

    }

    public static string PersistentDataReadPath(string url, string filename)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        //return  mPersistentDataPath  +"/"+AppSetting.Version+"/"+ url + filename;
        return Path.Combine(mPersistentDataPath + "/" + AppSetting.Version + "/" + url, filename);
      //  return Path.Combine("file:///" + mPersistentDataPath  +"/"+mAppS/etting.AppVersion+"/"+ url, filename);
#elif UNITY_IPHONE
        return Path.Combine(mPersistentDataPath  +"/"+AppSetting.Version+"/"+ url, filename);
#else
        return Path.Combine(mPersistentDataPath + "/" + AppSetting.Version + "" + url, filename);
#endif
    }


    public static string StreamingAssetsPathReadPath(string url, string filename)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return Path.Combine(mStreamingAssetsPath + url, filename);
#elif UNITY_IPHONE
        return Path.Combine(mStreamingAssetsPath + url, filename);
#else
        return Path.Combine(mStreamingAssetsPath + url, filename);
#endif
    }

    //    public static string StreamingAssetsPathBundlePath(string url, string filename)
    //    {
    //#if UNITY_ANDROID && !UNITY_EDITOR
    //        return Path.Combine(mStreamingAssetsPath + url, filename);
    //#elif UNITY_IPHONE
    //        return "file://"  + Path.Combine(mStreamingAssetsPath + url, filename);
    //#else
    //        return Path.Combine(mStreamingAssetsPath + url, filename);
    //#endif
    //    }

    public static string StreamingAssetsPath
    {
        get
        {
            return mStreamingAssetsPath;
        }
    }

    public static string PersistentDataPath
    {
        get
        {
            return mPersistentDataPath;
        }
    }

    public static string PersistentDataPurePath
    {
        get
        {
            return mPersistentDataPurePath;
        }
    }


    public static string GameDataURL
    {
        get { return mGameDataURL; }
    }

    public static string GameLocalizaURL
    {
        get { return mGameLocalizaURL; }
    }



    public static string ResURL
    {
        get { return mResURL; }
    }

    public static string BundleStoreUrl
    {
        get { return mBundleStoreURL; }
    }

    public static int UpdateBundleVersion
    {
        get { return mUpdateBundleVersion; }
        set { mUpdateBundleVersion = value; }
    }



    public static uint UpdateBundleCRC
    {
        get { return mUpdateBundleCRC; }
        set { mUpdateBundleCRC = value; }
    }

    public static string AppInfoURL
    {
        get { return mAppInfoURL; }
    }

    public static string NoticeURL
    {
        get { return mNoticeURL; }
    }
    public static string ServerAreaURL
    {
        get { return mServerAreaURL; }
    }


    static public bool DeleteDirectory(string path)
    {
#if !UNITY_WEBPLAYER && !UNITY_FLASH && !UNITY_METRO && !UNITY_WP8 && !UNITY_WP_8_1
        try
        {

            if (Directory.Exists(path))
            {
                //DirectoryInfo directory = new DirectoryInfo(path);
                //directory.Delete(true);
                Directory.Delete(path, true);
            }
            return true;
        }
        catch (System.Exception) { }
#endif
        return false;
    }

    /// <summary>
    /// 使用GZIP解压文件的方法
    /// </summary>
    /// <param name="zipfilename">源文件路径+文件名</param>
    /// <param name="unzipfilename">解压缩文件路径+文件名</param>
    /// <returns>返回bool操作结果，成功true，失败 flase</returns>
    public static void UnGzipDirectoryFile(Stream zipData, string fileDir)//, string unzipfilename
    {
        // string encodeName = isEncode ? TUtils.MD5Encode(filename) : filename;
        string directoryName = fileDir;
        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);//生成解压目录

        string CurrentDirectory = directoryName;
        byte[] data = new byte[2048];
        int size = 2048;
        ZipEntry theEntry = null;

        using (ZipInputStream s = new ZipInputStream(zipData))
        {
            while ((theEntry = s.GetNextEntry()) != null)
            {
                if (theEntry.IsDirectory)
                {   // 该结点是目录
                    if (!Directory.Exists(CurrentDirectory + theEntry.Name)) Directory.CreateDirectory(CurrentDirectory + theEntry.Name);
                }
                else
                {
                    if (theEntry.Name != string.Empty)
                    {
                        //检查多级目录是否存在  
                        if (theEntry.Name.Contains("//"))
                        {
                            string parentDirPath = theEntry.Name.Remove(theEntry.Name.LastIndexOf("//") + 1);
                            if (!Directory.Exists(parentDirPath))
                            {
                                Directory.CreateDirectory(CurrentDirectory + parentDirPath);
                            }
                        }

                        //解压文件到指定的目录
                        using (FileStream streamWriter = File.Create(CurrentDirectory + "/" + theEntry.Name))
                        {
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size <= 0) break;

                                streamWriter.Write(data, 0, size);
                            }
                            streamWriter.Close();
                        }
                    }
                }
            }
            s.Close();
        }
    }

    /// <summary>
    /// 电脑绝对路径， 转Assets下的路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string AbsolutePathToAssetPath(string path)
    {
        if (path.IndexOf("/Assets/") >= 0)
            return path.Substring(path.IndexOf("/Assets/")).Replace("/Assets/", "Assets/");
        return "";
    }

    public static string GetDirectoryPath(string detailPath)
    {
        return detailPath.Remove(detailPath.LastIndexOf('/'));
    }


    /// <summary>
    /// 保存文件bytes
    /// </summary>
    public static void SaveBytesString(string path, string fileName, string str)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
        //byte[] bytes = Convert.FromBase64String(str);
        SaveBytes(path + "/" + fileName, bytes);
    }

    public static void SaveBytes(string path, string fileName, byte[] bytes)
    {
        SaveBytes(path + "/" + fileName, bytes);
    }


    public static void SaveBytes(string filePath, byte[] bytes)
    {
        string fileDir = Path.GetDirectoryName(filePath);
        //TDebug.Log("SaveBytes：" + filePath + "   " + fileDir);
        if (!Directory.Exists(fileDir))//判断保存路径是否存在
        {
            Directory.CreateDirectory(fileDir);
        }
        try
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
            }
            var sw = file.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
        }
        catch (Exception e)
        {
            TDebug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 读取文件bytes
    /// </summary>
    public static byte[] ReadBytes(string filePath)
    {
        if (!File.Exists(filePath))
            return null;
        return File.ReadAllBytes(filePath);
    }
    public static byte[] ReadBytes(string path, string fileName)
    {
        return ReadBytes(path + "/" + fileName);
    }
    public static string ReadBytesString(string path , string fileName)
    {
        byte[] data = ReadBytes(path + "/" + fileName);
        if (data == null)
            return "";
        //return Convert.ToBase64String(data);
        return System.Text.Encoding.UTF8.GetString(data);
    }




    //压缩文件
    public static byte[] GzipDirectoryFile(byte[] originData, string fileName)
    {
        MemoryStream ms = null;
        try
        {
            ms = new MemoryStream();
            ZipOutputStream zipOut = new ZipOutputStream(ms);
            zipOut.SetLevel(9);//压缩文件
            ZipEntry entry = new ZipEntry(fileName);
            entry.DateTime = DateTime.Now;
            entry.Size = originData.Length;

            zipOut.PutNextEntry(entry);
            zipOut.Write(originData, 0, originData.Length);
            zipOut.Finish();
            zipOut.Close();
            return ms.ToArray();

        }
        catch (System.Exception e)
        {
            return null;
        }
    }






    public static byte[] SyncReadStreamFile(string path)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        if (path.Contains("!/assets/"))
        {
            return JavaHelper.Instance.GetStreamAssets(path);
        }
        else
        {
             FileInfo file = new FileInfo(path);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                Debug.LogError("文件不存在" + path);
                return null;
            }
        }

#elif !UNITY_WEBPLAYER

        FileInfo file = new FileInfo(path);
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        return null;
#endif
    }
}

