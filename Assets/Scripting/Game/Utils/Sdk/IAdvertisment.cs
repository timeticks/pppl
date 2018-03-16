using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 广告接口
/// </summary>
public interface IAdvertisment
{
    /// <summary>
    /// 用AppId、AppKey进行初始化
    /// </summary>
    void Init();

    /// <summary>
    /// 播放广告
    /// </summary>
    void Play(System.Action<AdsStatus> callback);

    /// <summary>
    /// 设置广告及其位置
    /// </summary>
    void Setting(bool isFullScreen, Rect rect);
}

public enum AdsStatus
{
    Success,  //请求成功
    NoneAds,  //没有广告可展示
    Fail,     //失败
}

public enum AdsType
{
    Video,   //视频
    Banner,  //横幅条
    Page,    //插页式
}


public enum AdsPlarform
{
    YouMi_IOS,  //有米
    Max,
}