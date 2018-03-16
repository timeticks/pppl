using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum BgName
{
    None=-1,
    AudioBg_Battle,
    AudioBg_Main,
    AudioBg_Map,
    AudioBg_Sect,
}
public enum AudioName
{
    Audio_Click,
    Audio_LevelUp,
    Audio_BattleWin,
    Audio_BattleLose,
}

public class AudioMgr : MonoBehaviour
{
    public class ViewObj
    {
        public AudioSource AudioBg0;
        public AudioSource AudioBg1;
        public AudioSource Audio;
        public AudioClip AudioBg_Battle;
        public AudioClip AudioBg_Main;
        public AudioClip AudioBg_Map;
        public AudioClip Audio_LevelUp;
        public AudioClip Audio_BattleWin;
        public AudioClip Audio_BattleLose;
        public AudioClip Audio_Click;
        public AudioClip AudioBg_Sect;

        public List<AudioSource> m_BgSource;

        public Dictionary<BgName, AudioClip> m_BgClips;
        public Dictionary<AudioName, AudioClip> m_AudioClips;
        public ViewObj(UIViewBase view)
        {
            AudioBg0 = view.GetCommon<AudioSource>("AudioBg0");
            AudioBg1 = view.GetCommon<AudioSource>("AudioBg1");
            Audio = view.GetCommon<AudioSource>("Audio");
            AudioBg_Battle = view.GetCommon<AudioClip>("AudioBg_Battle");
            AudioBg_Main = view.GetCommon<AudioClip>("AudioBg_Main");
            AudioBg_Map = view.GetCommon<AudioClip>("AudioBg_Map");
            Audio_LevelUp = view.GetCommon<AudioClip>("Audio_LevelUp");
            Audio_BattleWin = view.GetCommon<AudioClip>("Audio_BattleWin");
            Audio_BattleLose = view.GetCommon<AudioClip>("Audio_BattleLose");
            Audio_Click = view.GetCommon<AudioClip>("Audio_Click");
            AudioBg_Sect = view.GetCommon<AudioClip>("AudioBg_Sect");

            m_BgSource = new List<AudioSource>();
            m_BgSource.Add(AudioBg0);
            m_BgSource.Add(AudioBg1);

            m_BgClips = new Dictionary<BgName, AudioClip>();
            m_BgClips.Add(BgName.AudioBg_Battle, AudioBg_Battle);
            m_BgClips.Add(BgName.AudioBg_Main, AudioBg_Main);
            m_BgClips.Add(BgName.AudioBg_Map, AudioBg_Map);
            m_BgClips.Add(BgName.AudioBg_Sect,AudioBg_Sect);

            m_AudioClips = new Dictionary<AudioName, AudioClip>();
            m_AudioClips.Add(AudioName.Audio_LevelUp, Audio_LevelUp);
            m_AudioClips.Add(AudioName.Audio_BattleWin, Audio_BattleWin);
            m_AudioClips.Add(AudioName.Audio_BattleLose, Audio_BattleLose);
            m_AudioClips.Add(AudioName.Audio_Click, Audio_Click);

        }

    }


    public static AudioMgr Instance { private set; get; }

    private bool mIsMusic;
    private bool mIsAudio;

    private BgName mCurAudioName;
    private BgName mLastAudioName;
    private AudioSource mLastBgSoure;
    private AudioSource mCurBgSoure;
    private int bgSourceIndex = 0;
    private float mFadeInVolume = 0f;
    private float mFadeOutVolume = 0f;
    private float FadeTime = 1f;

    public bool IsMusic
    {
        get
        {
            return mIsMusic;
        }
        set
        {
            mIsMusic = value;
            PlayerPrefs.SetInt("IsMusic", value ? 1 : 0);
            if (value)
            {
                PlayeBg(BgName.AudioBg_Main);
            }
            else if (mCurBgSoure != null)
                mCurBgSoure.Stop();
        }
    }
    public bool IsAudio
    {
        get
        {
            return mIsAudio;
        }
        set
        {
            mIsAudio = value;
            PlayerPrefs.SetInt("IsAudio", value ? 1 : 0);
        }
    }
    private enum AuidoState
    {
        Normal,
        FadeOut,
        FadeIn,
    }


    private ViewObj mViewObj;

    public void InitAudioManager()
    {
        Instance = this;
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
            LoadClickSound();
        }
    }



    public void SetBgVolum(int value = 0)
    {
        mCurBgSoure.volume = value;
    }

    public void PlayeBg(BgName name)
    {
        if (!mIsMusic) return;
        if (mCurBgSoure == null) mCurBgSoure = mViewObj.m_BgSource[bgSourceIndex];
        mCurBgSoure.clip = mViewObj.m_BgClips[name];
        mCurBgSoure.loop = true;
        mCurBgSoure.Play();
        mCurAudioName = name;
    }
    private IEnumerator aduioFadeOut;
    private IEnumerator aduioFadeIn;
    public void SwitchBgAudio(BgName name)
    {
        if (!mIsMusic) return;
        if (mCurAudioName == name) return;
        if (mCurBgSoure != null)
        {
            mLastBgSoure = mCurBgSoure;
            mLastAudioName = mCurAudioName;
            if (aduioFadeOut != null)
            {
                StopCoroutine(aduioFadeOut);
                aduioFadeOut = null;
            }
            aduioFadeOut = AduioFadeOut();
            StartCoroutine(aduioFadeOut);
        }
        bgSourceIndex += 1;
        if (bgSourceIndex > 1) bgSourceIndex = 0;
        mCurBgSoure = mViewObj.m_BgSource[bgSourceIndex];
        mCurBgSoure.clip = mViewObj.m_BgClips[name];
        mCurAudioName = name;
        mCurBgSoure.loop = true;
        if (aduioFadeIn != null)
        {
            StopCoroutine(aduioFadeIn);
            aduioFadeIn = null;
        }
        aduioFadeIn = AduioFadeIn();
        StartCoroutine(aduioFadeIn);
    }
    public void RePlayLastBgAudio()
    {
        if (!mIsMusic) return;
        if (mLastAudioName != BgName.None)
            SwitchBgAudio(mLastAudioName);
    }
    IEnumerator AduioFadeOut()
    {
        mFadeOutVolume = 1f;
        for (int i = 0; i < 100; i++)
        {
            mFadeOutVolume -= 0.01f;
            mLastBgSoure.volume = mFadeOutVolume;
            yield return new WaitForSeconds(FadeTime / 100f);
        }
        mLastBgSoure.Stop();
        mLastBgSoure.clip = null;
    }
    IEnumerator AduioFadeIn()
    {
        yield return new WaitForSeconds(0.5f);
        mFadeInVolume = 0f;
        mCurBgSoure.volume = 0f;
        mCurBgSoure.Play();
        for (int i = 0; i < 100; i++)
        {
            mFadeInVolume += 0.01f;
            mCurBgSoure.volume = mFadeInVolume;
            yield return new WaitForSeconds(FadeTime / 100f);
        }
    }


    public void PlayeAudio(AudioName name)
    {
        if (!mIsAudio) return;
        mViewObj.Audio.clip = mViewObj.m_AudioClips[name];
        mViewObj.Audio.loop = false;
        mViewObj.Audio.Play();
    }

    private float longAduioLength = 0;
    private IEnumerator increaseBgAudio;
    //播放长音效
    public void PlayLongAudio(AudioName name)
    {
        if (!mIsAudio) return;
        if (mViewObj == null)
        {
            TDebug.LogError("mViewObj is null");
            return;
        }
        if (mViewObj.Audio == null)
        {
            TDebug.LogError("mViewObj.Audio is null");
            return;
        }
        if (mViewObj.m_AudioClips == null)
        {
            TDebug.LogError("m_AudioClips is null");
            return;
        }
        if (!mViewObj.m_AudioClips.ContainsKey(name))
        {
            TDebug.LogError("m_AudioClips is null"+ name);
            return;
        }
        if (mViewObj.m_AudioClips[name]==null)
        {
            TDebug.LogError(name + "AudioClips is null");
            return;
        }
        mViewObj.Audio.clip = mViewObj.m_AudioClips[name];
        mViewObj.Audio.loop = false;
        mViewObj.Audio.Play();
        if (mCurBgSoure == null || !mIsMusic)
            return;
        mCurBgSoure.volume = 0.4f;
        longAduioLength = mViewObj.Audio.clip.length;
        if (increaseBgAudio != null)
        {
            StopCoroutine(increaseBgAudio);
            increaseBgAudio = null;
        }
        increaseBgAudio = IncreaseBgAudio();
        if (increaseBgAudio == null)
        {
            TDebug.LogError("increaseBgAudio is null");
            return;
        }
        StartCoroutine(increaseBgAudio);
    }
    IEnumerator IncreaseBgAudio()
    {
        yield return new WaitForSeconds(longAduioLength);
        int count = (int)((1 - 0.4f) / 0.01f);
        for (int i = 0; i < count; i++)
        {
            mFadeInVolume += 0.01f;
            mCurBgSoure.volume = mFadeInVolume;
            yield return new WaitForSeconds(FadeTime / 100f);
        }
        mCurBgSoure.volume = 1;
    }


    public static void PlayClickAudio()
    {
        if (Instance == null || !Instance.mIsAudio) return;
        if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
            playSound(AudioClickId, 0.4f);
        }
        else if (PlatformUtils.EnviormentTy == EnviormentType.iOS)
        {
            Instance.PlayeAudio(AudioName.Audio_Click);
        }
    }


    /////////////////////////////////////////安卓播放////////////////////////////////////////////
    public static AndroidJavaClass unityActivityClass;
    public static AndroidJavaObject activityObj;
    private static AndroidJavaObject soundObj;
    private static int AudioClickId;

    public static void playSound(int soundId)
    {
        soundObj.Call("playSound", new object[] { soundId });
    }

    public static void playSound(int soundId, float volume)
    {
        soundObj.Call("playSound", new object[] { soundId, volume });
    }

    public static void playSound(int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate)
    {
        soundObj.Call("playSound", new object[] { soundId, leftVolume, rightVolume, priority, loop, rate });
    }

    public static int loadSound(string soundName)
    {
        return soundObj.Call<int>("loadSound", new object[] { "Android/Game/Res/sfx/" + soundName + ".wav" });
    }

    public static void unloadSound(int soundId)
    {
        soundObj.Call("unloadSound", new object[] { soundId });
    }
    public static void LoadClickSound()
    {
        AudioClickId = loadSound("Audio_Click");
    }
    private void Awake()
    {
        if (PlatformUtils.EnviormentTy != EnviormentType.Android)
            return;

        unityActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activityObj = unityActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
        soundObj = new AndroidJavaObject("com.placegame.nativeplugins.AudioCenter", 5, activityObj);

        AndroidJavaClass jc = new AndroidJavaClass("com.placegame.nativeplugins.Restart");
        jo = jc.CallStatic<AndroidJavaObject>("getInstance", gameObject.name);
    }

    private AndroidJavaObject jo;

}
