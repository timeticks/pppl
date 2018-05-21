using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISelectDialogFetcher
{
    SelectDialog GetSelectDialogByCopy(string key);
}

public class SelectDialog : DescObject
{

    private static ISelectDialogFetcher mDialogInter;


    public static ISelectDialogFetcher DialogFetcher
    {
        get { return mDialogInter; }
        set
        {
            if (mDialogInter == null)
                mDialogInter = value;
        }
    }

    public string desc;
    public string[] button;

    public SelectDialog() : base()
    {
        
    }

    public SelectDialog Clone()
    {
        return this.MemberwiseClone() as SelectDialog;
    }


    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
    }

    public static string[] GetButtons(string dialogName)
    {
        SelectDialog selectDialog = SelectDialog.DialogFetcher.GetSelectDialogByCopy(dialogName);
        if (selectDialog == null)
            return new string[0];
        return selectDialog.button;
    }
    public static string GetDesc(string dialogName)
    {
        SelectDialog selectDialog = SelectDialog.DialogFetcher.GetSelectDialogByCopy(dialogName);
        if (selectDialog == null)
            return "nul";
        return selectDialog.desc.RemoveN();
    }
}
