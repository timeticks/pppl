using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class RPGRandomUtility 
{
	/// <summary>
    /// 根据概率返回真或假
    /// </summary>
    /// <param name="概率"></param>
    /// <returns></returns>
    public static bool GetTrueByProb(double probNum)
    {
        if (probNum >= 1) return true;
        if (probNum <= 0)
            return false;
        if (probNum > 0.1f)
        {
            if (probNum >= Random.value)
                return true;
            return false;
        }
        int num = (int)(1f / probNum); 
        int rangeNum = Random.Range(0, num + 1);
        int mediumNum = num/2;      //获得中间数
        if (rangeNum == mediumNum)//当等于中间数时，返回真
        {
            return true;
        }
        return false;
    }


	//根据一个形如 { 0f, 0.1f, 0.1f }的数组，根据概率返回index
	public static int GetIndexByPct(float[] pct) //float最多保留6位小数,若还想使之概率变小,把其他概率翻倍如0.8->8,即可
    {
  		if (pct.Length == 0) { TDebug.LogError("数组长度为0"); return 0; }
        float percentAmount = 0;//概率之和，避免其大于1
        float minPercent = 1;//最小概率
        int count = pct.Length;
        for (int i = 0; i < count;i++ )
        {
            if (minPercent > pct[i])
            {
                minPercent = (float)pct[i];
            }
            percentAmount += (float)pct[i];
        }
        if (percentAmount == 0)
        {
            string pctString = "数组为:";
            for (int i = 0; i < count; i++)
            {
                pctString = string.Format("{0}{1} ", pctString,pct[i]);
            }
            TDebug.LogErrorFormat("错误,总概率为0,{0}", pctString); return 0;
        }
        float percentRatio = 1.0f / percentAmount; //概率总和归一后，每个概率需要乘的倍数

        if (minPercent == 0) minPercent = 0.0000001f;
        if (minPercent > 0.1f) minPercent /= 10f;
        int rangeMax = (int)(1.0f / (0.1f * percentRatio * minPercent));//随机的范围

        int randomNum = Random.Range(0, rangeMax);
        float randomNumFloat = (float)randomNum / (float)rangeMax;

        float addPercent = 0;
        for (int i = 0; i < count; i++)
        {
            addPercent += (float)pct[i] * percentRatio;
            if (addPercent >= randomNumFloat)
            {
                return i;
            }
        }

        string pctString2 = "数组为:";
        for (int i = 0; i < count; i++)
        { pctString2 += pct[i] + " "; }
        TDebug.LogErrorFormat("错误{0}" , pctString2);
        return 0;
    }
	//根据一个形如 { 0f, 0.1f, 0.1f }的数组，在排除withoutList中的index之后，根据概率返回index
	public static int GetIndexByPct(float[] pct , List<int> withoutList)
    {
        int length = pct.Length;
        float[] pctCopy = new float[length]; 
        for (int i = 0; i < length; i++)
        {
            pctCopy[i] = pct[i];
        }

        int count = withoutList.Count;
        for (int i = 0; i < count; i++)
        {
            if (pctCopy.Length - 1 < withoutList[i]) continue;
            pctCopy[withoutList[i]] = 0f;
        }
        return GetIndexByPct(pctCopy);
    }



    /// <summary>
    /// 根据对象中的id和概率信息，返回随机出来的id
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static int GetRandomId(List<ObjectRandomInfo> list)
    {
        if (list == null || list.Count == 0) 
        { TDebug.LogError("列表为空"); return 0; }
        float percentAmount = 0;//概率之和，避免其大于1
        float minPercent=1;//最小概率
        int count = list.Count;
        foreach (ObjectRandomInfo f in list)
        {
            if (minPercent > f.getPercent)
            {
                minPercent = (float)f.getPercent;
            }
            percentAmount += (float)f.getPercent;
        }
        if (percentAmount == 0)
        {
            System.Text.StringBuilder pctString = new System.Text.StringBuilder("列表为:");
            for (int i = 0; i < count; i++)
            {
                pctString.Append(string.Format("({0},{1})", list[i].getPercent, list[i].objectId));
            }
            TDebug.LogErrorFormat("错误,总概率为0,{0}" , pctString); return list[0].objectId; 
        }
        float percentRatio = 1.0f / percentAmount; //概率总和归一后，每个概率需要乘的倍数

        if (minPercent == 0) minPercent = 0.000001f;
        if (minPercent > 0.1f) minPercent /= 10f;
        int rangeMax = (int)(1.0f / (0.1f*percentRatio * minPercent));//随机的范围

        int randomNum = Random.Range(0, rangeMax);
        float randomNumFloat = (float)randomNum / (float)rangeMax;

        float addPercent = 0;
        foreach (ObjectRandomInfo f in list)
        {
            
            addPercent += (float)f.getPercent * percentRatio;
            if (addPercent >= randomNumFloat)
            {
                return f.objectId;
            }
        }

        System.Text.StringBuilder pctString2 = new System.Text.StringBuilder("列表为:");
        for (int i = 0; i < count; i++)
        {
            pctString2.Append(string.Format("({0},{1})", list[i].getPercent, list[i].objectId));
        }
        TDebug.LogErrorFormat("错误,总概率为0,{0}" , pctString2); return list[0].objectId; 
    }



	/// <summary>
    /// 根据中心和间隔,返回一个网格型的位置坐标列表
    /// 当isSquare=true为正方形网格时,columnMax最大列数不生效
    /// isY=false ,则除x外第二个轴是z
    /// </summary>
    /// <param name="center"></param>
    /// <param name="offset"></param>
    /// <param name="amount"></param>
    /// <param name="columnMax"></param>
    /// <param name="isSquare"></param>
    /// <returns></returns>
    public static List<Vector3> GetPosList(Vector3 center , Vector2 offset , int amount , int columnMax  ,bool isSquare , bool isY)
    {
        List<Vector3> posList = new List<Vector3>();
        if (isSquare)
        {
            columnMax = Mathf.CeilToInt(Mathf.Sqrt((float)amount));
        }
        for (int i = 0; i < amount; i++)
        {
            posList.Add(GetCurGridPos(i, amount, columnMax, center, offset, isY));
        }
        return posList;
    }
    static Vector3 GetCurGridPos(int index, int amount, int columnMax, Vector3 center, Vector2 offset , bool isY)//得到当前格子的位置。当数量不同，格子的摆放位置不一样
    {
        if (amount == 1)
        {
            return new Vector3(center.x, center.y, center.z);
        }
        else if (amount <= columnMax)//当只有一排时
        {
            return new Vector3(GetPosXByIndex(index, amount, columnMax, center, offset), center.y, center.z);
        }
        else
        {
            if (isY)
            {
                float curPosY = center.y - offset.y * (index / columnMax);
                return new Vector3(GetPosXByIndex(index, amount, columnMax, center, offset), curPosY, center.z);
            }
            else
            {
                float curPosZ = center.z - offset.y * (index / columnMax);
                return new Vector3(GetPosXByIndex(index, amount, columnMax, center, offset), center.y, curPosZ);
            }
        }
    }
    static float GetPosXByIndex(int index, int amount, int columnMax, Vector3 center, Vector2 offset)
    {
        float posX = 0;
        if (index >= columnMax) index = index % (columnMax);
        amount = Mathf.Clamp(amount, 0, columnMax);
        float leftPosX = center.x - offset.x * ((float)(amount - 1) *0.5f);
        posX = leftPosX + index * offset.x;
        return posX;
    }


	/// <summary>
    /// 将总数根据每个元素代表的值，得出每个元素所需要的数量
    /// 如amount=1000   indexValue={10,110}
    /// 此时要凑够1000,需要最少元素为9个110，1个10。那么就返回{1,9}
    /// 需要大的数在后面
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="indexValue"></param>
    /// <returns></returns>
    public static int[] GetIndexNumByValue(int amount, int[] indexValue)//将总数根据每个元素代表的值，得出元素数量最少下，每个元素所需要的数量
    {
        
        int[] indexNum = new int[indexValue.Length];//长度与indexValue相同,全为零
        for (int i = indexValue.Length-1; i >= 0; i--)
        {
            if (indexValue[i] <= 0) continue;
            int getNum = amount / indexValue[i];
            if (getNum >= 1)
            {
                amount -= getNum * indexValue[i];
                indexNum[i] = getNum;
            }
        }
        return indexNum;
    }



    /// <summary>
    /// 使indexList中的值，进行交换性随机。即若交换前list[0]=0，交换后list[0]=4，那么之前list[x]=4的，现在list[x]=0
    /// canSwitchToSelf结对随机时，是否可以随机到自己
    /// </summary>
    /// <param name="indexList"></param>
    /// <param name="canSwitchToSelf"></param>
    /// <returns></returns>
    public static List<int> GetRandomSwitchList(List<int> indexList, bool canSwitchToSelf)
    {
        List<int> randomSwitchList = new List<int>();

        List<float> nextIndexPct = new List<float>();
        for (int i = 0; i < indexList.Count; i++)
        {
            randomSwitchList.Add(indexList[i]);
            nextIndexPct.Add(0.1f);
        }

        int pctCount = nextIndexPct.Count;
        for (int i = 0; i < indexList.Count; i++)//遍历，得到结对列表
        {
            if (nextIndexPct[i].Equals(0))
                continue;
            if (!canSwitchToSelf)
                nextIndexPct[i] = 0;
            bool isAllZero = true;//检查是否全为0
            for (int j = 0; j < pctCount; j++)
            {
                if (nextIndexPct[j] != 0)
                {
                    isAllZero = false;
                    break;
                }
            }
            if (isAllZero)
                break;

            int getIndex = GetIndexByPct(nextIndexPct.ToArray());
            int temp = randomSwitchList[i];
            randomSwitchList[i] = randomSwitchList[getIndex];
            randomSwitchList[getIndex] = temp;
            nextIndexPct[getIndex] = 0;
        }

        return randomSwitchList;
    }


    /// <summary>
    /// 得到cur数值，在min-max中的进度
    /// </summary>
    public static float GetRangePct(float min , float max,float cur)
    {
        if (max.Equals(min)) return 1f;
        return (cur - min) / (max - min);
    }



}

[System.Serializable]
public class ObjectRandomInfo  //物品随机出id的信息，即得到这个id的概率大小
{
    public int objectId;
    public double getPercent;
	public ObjectRandomInfo() { }
    public ObjectRandomInfo(int id , float pct)
    { 
        objectId = id;
        getPercent = pct;
    }
}
