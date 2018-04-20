using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface IRecipeFetcher
{
    Recipe GetRecipeByCopy(int idx);
    List<Recipe> GetRecipeAll(AuxSkillLevel.SkillType type, int level, Sect.SectType sect);
}
public class Recipe : DescObject
{
    private static IRecipeFetcher mFetcher;
    public static IRecipeFetcher RecipeFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private string mIcon = "";
    private string mDesc = "";
    private AuxSkillLevel.SkillType mSkillType = AuxSkillLevel.SkillType.Forge; //技能需求
    private int mSkillLevel;
    private RecipeType mType = RecipeType.Equip;
    private int mMoney;
    private int[] mScience = new int[0];
    private int[] mNumber = new int[0];
    private int mProficiency; //熟练度
    private int[] mTranslate = new int[0];
    private int[] mWeight = new int[0];
    private int mMaxQuality;
    private int mQuality0;
    private int mQuality1;
    private int mQuality2;
    private int mQuality3;

    private Sect.SectType mSect;
    private string mOneStr;
    private string mTwoStr; // 低，中，高
    private string mThreeStr;

    private int mTime = 0;

    public bool isUnLock = false;

    public enum RecipeType
    {
    	Equip=0,//
    	Drug,//炼丹
        Mine, //挖矿
        GatherHerb,//采药
    	Max
    }
  
    public Recipe():base()
    {
    	
    }
    public Recipe(Recipe origin): base(origin)
    {   
        this.mIcon         = origin.mIcon;
        this.mDesc          = origin.mDesc;
        this.mSkillType   =  origin.mSkillType;
        this.mSkillLevel  =  origin.mSkillLevel;
        this.mType = origin.mType;
        this.mMoney  = origin.mMoney;
        this.mScience = origin.mScience;
        this.mNumber = origin.mNumber;
        this.mProficiency = origin.mProficiency;
        this.mTranslate = origin.mTranslate;
        this.mTime = origin.mTime;
        this.mQuality0 = origin.mQuality0;
        this.mQuality1 = origin.mQuality1;
        this.mQuality2 = origin.mQuality2;
        this.mQuality3 = origin.mQuality3;
        this.mMaxQuality = origin.mMaxQuality;
        this.mTwoStr = origin.mTwoStr;
        this.mOneStr = origin.mOneStr;
        this.mThreeStr = origin.mThreeStr;
        this.mSect = origin.mSect;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mSkillType = (AuxSkillLevel.SkillType)ios.ReadByte();
        this.mType = (RecipeType)ios.ReadByte();
        this.mMaxQuality = ios.ReadByte();
        this.mSect = (Sect.SectType)ios.ReadByte();

        this.mSkillLevel = ios.ReadInt16();

        this.mMoney = ios.ReadInt32();
        this.mProficiency = ios.ReadInt32();
        this.mTime = ios.ReadInt32();
        mQuality0 = ios.ReadInt32();
        mQuality1 = ios.ReadInt32();
        mQuality2 = ios.ReadInt32();
        mQuality3 = ios.ReadInt32();

        this.mIcon = NetUtils.ReadUTF(ios);
        this.mDesc = NetUtils.ReadUTF(ios);
        this.mOneStr = NetUtils.ReadUTF(ios);
        this.mTwoStr = NetUtils.ReadUTF(ios);
        this.mThreeStr = NetUtils.ReadUTF(ios);

        int length = ios.ReadByte();
        this.mScience = new int[length];
        for (byte i = 0; i < length; i++)
        {
            mScience[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        this.mNumber = new int[length];
        for (byte i = 0; i < length; i++)
        {
            mNumber[i] = ios.ReadInt16();
        }       
    }

    public BaseObject GetTranslateObject()
    {
    //    if (mQuality0 == 0)
    //    {
    //        TDebug.LogError(string.Format("配方错误{0}", idx));
    //    }
    //    int idx;
    //    if (mMaxQuality == 0) idx = mQuality0;
    //    else if (mMaxQuality == 1) idx = mQuality1;
    //    else if (mMaxQuality == 2) idx = mQuality2;
    //    else idx = mQuality3;
    //    switch (mType)
    //    {
    //        case RecipeType.Equip:
    //            Dictionary<int, LootType> loot = Loot.GetLootsId(idx);
    //            foreach (var item in loot)
    //            {
    //                return OldEquip.EquipFetcher.GetEquipByCopy(item.Key);
    //            }             
    //            TDebug.LogError(string.Format("配方错误{0}", idx));
    //            return null;
    //        case RecipeType.Drug:
    //            return Item.ItemFetcher.GetItemByCopy(idx);
    //    }
        return null;
    }
    public static GoodsToDrop[] GetGoodsDropList(List<int> translateID ,int recipeID)
    {
        List<GoodsToDrop> dropList = new List<GoodsToDrop>();
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(recipeID);
        if (recipe.Type == RecipeType.Drug)
        {        
            for (int i = 0; i < translateID.Count; i++)
            {
                bool isExist = false;
                for (int j = 0; j < dropList.Count; j++)
                {
                    if (translateID[i] == dropList[j].GoodsIdx)
                    {
                        dropList[j].Amount++;
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    GoodsToDrop drop = new GoodsToDrop();
                    drop.GoodsIdx = translateID[i];
                    drop.Amount = 1;
                    drop.MyType = LootType.Item;
                    dropList.Add(drop);
                }
            }
        }
        else 
        {
            for (int i = 0; i < translateID.Count; i++)
            {
                GoodsToDrop drop = new GoodsToDrop();
                drop.Amount = 1;
                drop.MyType = LootType.Equip;
                drop.GoodsIdx = translateID[i];
                dropList.Add(drop);
            }
        } 
        return dropList.ToArray();
    }
    /// <summary>
    /// 某生活技能等级下的技能排序，将已拥有的配方置顶
    /// </summary>
    /// <param name="type"></param>
    /// <param name="level"></param>
    /// <param name="ownRecipes"></param>
    /// <returns></returns>
    public static List<Recipe> GetLevelRecipes(AuxSkillLevel.SkillType type ,int level,Sect.SectType sect)
    {
        List<Recipe> tempList = new List<Recipe>();
        List<int> ownRecipes = GetOwnLevelRecipe(type,level);
        List<Recipe> cachedRecipe = Recipe.RecipeFetcher.GetRecipeAll(type, level, sect);
        if (cachedRecipe == null) return null; 

        for (int i = 0; i < cachedRecipe.Count; i++)
        {
            if (ownRecipes != null && ownRecipes.Contains(cachedRecipe[i].idx))
            {
                cachedRecipe[i].isUnLock = true;
                tempList.Insert(0, cachedRecipe[i]);
            }
            else
                tempList.Add(cachedRecipe[i]);               
        }
        return tempList.Count == 0? null:tempList;
    }


    public static List<int> GetOwnLevelRecipe(AuxSkillLevel.SkillType type, int level)
    {
        return null;
    }

    public static List<AuxSkillLevel.SkillType> GetSkillListByRecipe(List<int> recipeList)
    {
        List<AuxSkillLevel.SkillType> skillList = new List<AuxSkillLevel.SkillType>();
        for (int i = 0; i < recipeList.Count; i++)
        {
            Recipe recipe = Recipe.mFetcher.GetRecipeByCopy(recipeList[i]);
            if (recipe == null) continue;
            if (!skillList.Exists(x => { return x == recipe.MySkillType; }))
            {
                skillList.Add(recipe.MySkillType);
            }
        }
        return skillList;
    }


    public string Icon
    {
        get { return mIcon; }
    }
    public string Desc
    {
        get { return mDesc; }
    }
    public  AuxSkillLevel.SkillType MySkillType
    {
        get { return mSkillType; }
    }
    public int SkillLevel
    {
        get { return mSkillLevel; }
    }
    public RecipeType Type
    {
        get { return mType; }
    }
    public int Money
    {
        get { return mMoney; }
    }
    public int[] Science
    {
        get { return mScience; }
    }
    public int[] Number
    {
        get { return mNumber; }
    }
    public int[] Translate
    {
        get { return mTranslate; }
    }
    public int[] Weight
    {
        get { return mWeight; }
    }
    public int Time
    {
        get { return mTime; }
    }
    public int Quality0
    {
        get { return mQuality0; }
    }
    public int Quality1
    {
        get { return mQuality1; }
    }
    public int Quality2
    {
        get { return mQuality2; }
    }
    public int Quality3
    {
        get { return mQuality3; }
    }
    public int MaxQuality
    {
        get { return mMaxQuality; }
    }
    public int Proficiency
    {
        get { return mProficiency; }
    }
    public string OneStr
    {
        get { return mOneStr; }
    }
    public string TwoStr
    {
        get { return mTwoStr; }
    }
    public string ThreeStr
    {
        get { return mThreeStr; }
    }
    public Sect.SectType Sect
    {
        get { return mSect; }
    }
}
