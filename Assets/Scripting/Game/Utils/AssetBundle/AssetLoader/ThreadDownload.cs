using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ThreadDownload :MonoBehaviour
{
    //下载进度
    public float Progress { get; private set; }
    public long TotelLength { get; private set; }
    public int LastSpeed { get; private set; }  //上一帧的速度

    //是否下载完成
    public bool IsDone { get; private set; }

    //涉及子线程要注意,Unity关闭的时候子线程不会关闭，所以要有一个标识
    private bool ClientExit;

    //子线程负责下载，否则会阻塞主线程，Unity界面会卡主
    private Thread mThread;
    private Action mCallBack;
    private string ErrorString="";

    void Update()
    {
        if (mCallBack != null && IsDone)
        {
            Action call = mCallBack;
            mCallBack = null;
            call();
            IsDone = false;
        }
        lock (this)
        {
            LastSpeed = 0;
        }
        if (ErrorString.Length > 0)
        {
            lock (ErrorString)
            {
                TDebug.LogError(ErrorString);
                ErrorString = "";
            }
        }
    }

   /// <summary>
    /// 开启子线程下载(支持断点续传)
   /// </summary>
   /// <param name="urlHead">地址</param>
   /// <param name="savePath">存放路径</param>
   /// <param name="fileName">文件名</param>
    /// <param name="needResume">是否允许断点续传，不允许则先删除</param>
   /// <param name="callBack"></param>
    public void StartDownLoad(string urlHead, string savePath , string fileName , bool needResume, Action callBack)
    {
        string url = urlHead + "/" + fileName.Replace("\\","/");
        string filePath = savePath + "/" + fileName.Replace("\\", "/");
        AsyncOperation asyncData = new AsyncOperation(); 
        ClientExit = false;
        mCallBack = callBack;
        string fileDir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(fileDir))//判断保存路径是否存在
        {
            Directory.CreateDirectory(fileDir);
        }
        TDebug.LogFormat("开始下载：{0}  {1}   {2}" , url , filePath , fileDir);
        mThread = new Thread(delegate()//开启子线程下载
        {
            //TDebug.Log(string.Format("子线程准备下载:{0}", url));
            if (!needResume && File.Exists(filePath))  //如果不需要断点续传，先删除
            {
                File.Delete(filePath);
            }
            FileStream fs =null;
            try
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            }
            catch(System.Exception e)
            {
                ErrorString += "创建错误" + e.Message + "  路径:" + filePath;
                //TDebug.LogError("创建错误" + e.Message + "  路径:" + filePath);
            }
            
            long fileLength = fs.Length;//获取文件现在的长度
            TotelLength = GetLength(url);//获取下载文件的总长度
            //TDebug.Log(string.Format("子线程开始下载{0}, 总长度:{1}byte | 当前长度:{2}byte", filePath, TotelLength, fileLength));
            //如果没下载完
            if (fileLength < TotelLength)
            {
                //断点续传核心，设置本地文件流的起始位置
                fs.Seek(fileLength, SeekOrigin.Begin);
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                
                //断点续传核心，设置远程访问文件流的起始位置
                request.AddRange((int)fileLength);
                try
                {
                    Stream stream = request.GetResponse().GetResponseStream();

                    byte[] buffer = new byte[2048]; //每次最大4k
                    int length = stream.Read(buffer, 0, buffer.Length); //使用流读取内容到buffer中
                    while (length > 0)
                    {
                        if (ClientExit) break; //如果Unity客户端关闭，停止下载
                        lock (this)
                        {
                            LastSpeed += length;
                        }
                        fs.Write(buffer, 0, length); //将内容写入本地文件中
                        fileLength += length;
                        Progress = (float) fileLength/(float) TotelLength; //进度
                        length = stream.Read(buffer, 0, buffer.Length);
                    }
                    stream.Close();
                    stream.Dispose();
                    //TDebug.Log(filePath+"文件是否存在：" + File.Exists(filePath));
                }
                catch(Exception e)
                {
                    ErrorString += "创建错误" + e.Message + "  " + url;
                    //TDebug.LogError("创建错误" + e.Message + "  " + url);
                }
            }
            else
            {
                Progress = 1;
            }
            fs.Close();
            fs.Dispose();
            //如果下载完毕，在Update中执行回调（必须在主线程中执行回调）
            if (Progress == 1)
            {
                IsDone = true;
            }
        });
        //开启子线程
        mThread.IsBackground = true;
        mThread.Start();
    }


    /// <summary>
    /// 获取下载文件的大小
    /// </summary>
    /// <returns>The length.</returns>
    /// <param name="url">URL.</param>
    long GetLength(string url)
    {
        HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
        requet.Method = "HEAD";
        HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
        return response.ContentLength;
    }




    public void Decompress(int aaa)
    {
        string md5 = AssetUpdate.GetStreamingMd5();
        TDebug.Log(md5);
        Dictionary<string,string> dict = MD5Compare.ParseVersionFile(md5);
        List<string> bbb = new List<string>(dict.Keys);
        if (aaa == 0)
        {
            StartCoroutine(StartDecompress(bbb, null));
        }
        else
        {
            StartCoroutine(StartDecompress2(bbb, null));
        }
    }

    public IEnumerator StartDecompress(List<string> bundleList, Action callBack)
    {
        AsyncOperation asyncData = new AsyncOperation();
        ClientExit = false;

        foreach (var temp in bundleList)
        {
            string url = FileUtils.StreamingAssetsPath + "/" + temp.Replace("\\", "/");
            string filePath = FileUtils.PersistentDataPath + "/" + temp.Replace("\\", "/");
            UnityWebRequest web = new UnityWebRequest(url);
            yield return web.Send();
            if (web.isError)
            {
                Debug.LogError(web.error);
            }
            else
            {
                if (web.responseCode == 200)
                {
                    FileUtils.SaveBytes(filePath, web.downloadHandler.data);
                }
            }  
            
        }
    }

    public IEnumerator StartDecompress2(List<string> bundleList, Action callBack)
    {
        AsyncOperation asyncData = new AsyncOperation();
        ClientExit = false;
        TDebug.LogFormat("开始解压：{0}" , bundleList.Count);
        for (int i = 0; i < bundleList.Count; i++)
        {
            string url = "file://" + FileUtils.StreamingAssetsPath + "/Res/" + bundleList[i].Replace("\\", "/");
            string filePath = FileUtils.PersistentDataPath + "/" + bundleList[i].Replace("\\", "/");
            WWW www = new WWW(url);
            TDebug.LogFormat("{0}  {1}", i , url);
            yield return www;
            if (www.error != null) TDebug.LogError(www.error);
            FileUtils.SaveBytes(filePath, www.bytes);
        }
    }











    public void OnDestroy()
    {
        ClientExit = true;
    }

}
