using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IErrorStatusFetcher
{
    ErrorStatus GetErrorStatusByCopy(int idx);
}
public class ErrorStatus :DescObject
{
    private static IErrorStatusFetcher mFetcher;

    public static IErrorStatusFetcher ErrorStatusFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public ErrorStatus() : base() { }

    public ErrorStatus(ErrorStatus origin)
        : base(origin)
    {
    }


    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
    }


}
