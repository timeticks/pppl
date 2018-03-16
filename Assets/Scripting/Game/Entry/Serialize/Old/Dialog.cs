using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDialogFetcher
{
    Dialog GetDialogByCopy(int idx);
}

public class Dialog : DescObject
{

    private static IDialogFetcher mDialogInter;


    public static IDialogFetcher DialogFetcher
    {
        get { return mDialogInter; }
        set
        {
            if (mDialogInter == null)
                mDialogInter = value;
        }
    }

    private string mDesc;


    private string[] mButton = new string[0];

    public Dialog() : base()
    {
        
    }
    public Dialog(Dialog origin)
        : base(origin)
    {
        mDesc = origin.mDesc;
        mButton = origin.mButton;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        mDesc = NetUtils.ReadUTF(ios);

        int length = ios.ReadByte();
        mButton = new string[length];
        for (int i = 0; i < length; i++)
        {
            mButton[i] = NetUtils.ReadUTF(ios);
        }
    }



    public string Desc
    {
        get { return mDesc; }
    }

    public string[] Button
    {
        get { return mButton; }
    }

}
