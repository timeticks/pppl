using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUtils  {

    public static PlatformType PlatformTy { get; private set; }
    public static EnviormentType EnviormentTy { get; private set; }
    static PlatformUtils()
    {

#if UNITY_ANDROID
        PlatformTy = PlatformType.Android;
        EnviormentTy = EnviormentType.Android;
#elif UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
        PlatformTy = PlatformType.iOS;
        EnviormentTy = EnviormentType.iOS;
#else 
        PlatformTy = PlatformType.Standalone;
        EnviormentTy = EnviormentType.Standalone;
#endif

#if UNITY_EDITOR
        EnviormentTy = EnviormentType.Editor;
#endif
    }
}
public enum PlatformType 
{
    Standalone,
    Android,
    iOS,
}

public enum EnviormentType
{
    Editor=-1,
    Standalone,
    Android,
    iOS,
}