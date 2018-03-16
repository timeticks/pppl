using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface IGuideStepFetcher
{
    GuideStep GetGuideStepByCopy(int idx);
    GuideStep GetGuideStepNoCopy(int idx);
}
public class GuideStep : DescObject
{
    private static IGuideStepFetcher mFetcher;
    public static IGuideStepFetcher GuideStepFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public enum TextStatus
    {
        Default,
        Close,
        ForceHigh,  //强制在上方
        ForceLow,    //强制在下方
        ForceHighAndEdge,   //强制在上方，且位置在uiTrans的上边界
        ForceLowAndEdge     //强制在上方，且位置在uiTrans的下边界
    }

    public enum MaskStatus
    {
        Default,
        TransparentMask,    //有遮罩，但是透明
        ClickMaskCanFinish, //显示遮罩，且点击遮罩可完成引导
        NoMask,             //没有任何遮罩
        MaskButFinishByOut, //有遮罩，但是通过外部调用引导完成
        NoMaskFinishByOut,
    }

    private int mStep ;
    private string mDesc = "";
    private int[] mGuideParam = new int[0];
    private TextStatus mTextStatus; //点击遮罩是否完成
    private MaskStatus mMaskStatus;  //是否需要遮罩

    public GuideStep():base()
    {
        
    }
    public GuideStep(GuideStep origin): base(origin)
    {
        this.mStep = origin.mStep;
        this.mDesc = origin.mDesc;
        this.mMaskStatus = origin.mMaskStatus;
        this.mTextStatus = origin.mTextStatus;
        this.mGuideParam = origin.mGuideParam;

    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mTextStatus = (GuideStep.TextStatus)ios.ReadByte();
        this.mMaskStatus = (GuideStep.MaskStatus)ios.ReadByte();

        this.mStep = ios.ReadInt16();
        this.mDesc = NetUtils.ReadUTF(ios);

        this.mGuideParam = new int[ios.ReadByte()];
        for (int i = 0; i < this.mGuideParam.Length; i++)
        {
            this.mGuideParam[i] = ios.ReadInt32();
        }
    }


    public int Step
    {
        get { return mStep; }
    }

    public string Desc
    {
        get { return mDesc; }
    }

    public int[] GuideParam
    {
        get { return mGuideParam; }
    }
    public MaskStatus MyMaskStatus
    {
        get { return mMaskStatus; }
    }
    public TextStatus MyTextStatus
    {
        get { return mTextStatus; }
    }

    public int TryGetInt(int _index)
    {
        if (_index >= 0 && _index < GuideParam.Length)
        {
            return GuideParam[_index];
        }
        TDebug.LogError(string.Format("获取GuideStepParam失败:{0}|[{0}]|length:{1}",mStep, _index, GuideParam.Length));
        return 0;
    }

    //获取某个引导的参数
    public static int TryGetParam(int step , int _index=0)
    {
        GuideStep guideStep = GuideStep.GuideStepFetcher.GetGuideStepNoCopy(step);
        if (step != null)
            return guideStep.TryGetInt(_index);
        return 0;
    }

    //获取某引导的内容
    public static string GetDesc(int step)
    {
        GuideStep guideStep = GuideStep.GuideStepFetcher.GetGuideStepNoCopy(step);
        if (guideStep != null)
            return guideStep.Desc;
        return "";
    }


}


public enum GuideStepNum //步骤枚举，便于查找引用
{
    None,
    _1,
    _2,
    _3,
    _4,
    _5,
    _6,
    _7,
    _8,
    _9,
    _10,
    _11,
    _12,
    _13,
    _14,
    _15,
    _16,
    _17,
    _18,
    _19,
    _20,
    _21,
    _22,
    _23,
    _24,
    _25,
    _26,
    _27,
    _28,
    _29,
    _30,
    _31
}