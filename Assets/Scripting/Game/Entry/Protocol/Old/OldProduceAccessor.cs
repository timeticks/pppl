using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OldProduceAccessor : DescObject
{

    public List<ProduceItem> ProduceList = new List<ProduceItem>();

    public OldProduceAccessor() { }

    public OldProduceAccessor(OldProduceAccessor origin)
    {
        ProduceList = origin.ProduceList;
    }

    public OldProduceAccessor(NetPacket.S2C_SnapShotProduceBotting msg)
    {
        ProduceList = msg.ProduceItemList;
    }

    public List<int> GetFinishRecipeList()//得到已经完成的配方列表
    {
        List<int> finishList = new List<int>();
        for (int i = 0; i < ProduceList.Count; i++)
        {
            if (ProduceList[i].FinishTime <= AppTimer.CurTimeStampMsSecond)
                finishList.Add(ProduceList[i].RecipeID);
        }
        return finishList;
    }

}

public class ProduceItem
{
    public int RecipeID;
    public int Num;
    public long FinishTime;
    public Recipe.RecipeType MyRecipeType;
    public ProduceItem() {  }

    public void ReadFrom(BinaryReader ios)
    {
        RecipeID = ios.ReadInt32();
        Num = ios.ReadByte();
        FinishTime = ios.ReadInt64();
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(RecipeID);
        MyRecipeType = recipe.Type;
    }
}
