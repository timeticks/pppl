using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileBaseUtils
{
    public const string GameResPath = "/Game/Res";
    public static readonly string StreamingAssetsPath = string.Format("{0}/{1}", Application.streamingAssetsPath, PlatformUtils.PlatformTy);
    public static string AppSettingVersion;
    private static string mPersistentDataPath;


    static FileBaseUtils()
    {
        mPersistentDataPath = string.Format("{0}/{1}", Application.persistentDataPath, PlatformUtils.PlatformTy);
    }


    /// <summary>
    /// 电脑绝对路径， 转Assets下的路径
    /// </summary>
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



    public static string StreamingAssetsPathReadPath(string url, string filename)
    {
        return Path.Combine(StreamingAssetsPath + url, filename);
    }

    public static string PersistentDataReadPath(string url, string filename)
    {
        if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
            return Path.Combine(string.Format("{0}/{1}/{2}", mPersistentDataPath, AppSettingVersion, url), filename);
        }
        else if (PlatformUtils.EnviormentTy == EnviormentType.iOS)
        {
            return Path.Combine(string.Format("{0}/{1}/{2}", mPersistentDataPath, AppSettingVersion, url), filename);
        }
        else
        {
            return Path.Combine(string.Format("{0}/{1}{2}", mPersistentDataPath, AppSettingVersion, url), filename);
        }
    }

    public static void SaveBytes(string filePath, byte[] bytes)
    {
        string fileDir = Path.GetDirectoryName(filePath);
        TDebug.LogFormat("SaveBytes：{0}   {1}", filePath, fileDir);
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

    public static byte[] SyncReadStreamFile(string path)
    {
        if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
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
        }
        else
        {
            FileInfo file = new FileInfo(path);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }
    }
}
