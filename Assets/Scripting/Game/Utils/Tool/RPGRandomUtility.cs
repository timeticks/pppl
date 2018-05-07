using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class RPGRandomUtility 
{
	/// <summary>
    /// ���ݸ��ʷ�������
    /// </summary>
    /// <param name="����"></param>
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
        int mediumNum = num/2;      //����м���
        if (rangeNum == mediumNum)//�������м���ʱ��������
        {
            return true;
        }
        return false;
    }


	//����һ������ { 0f, 0.1f, 0.1f }�����飬���ݸ��ʷ���index
	public static int GetIndexByPct(float[] pct) //float��ౣ��6λС��,������ʹ֮���ʱ�С,���������ʷ�����0.8->8,����
    {
  		if (pct.Length == 0) { TDebug.LogError("���鳤��Ϊ0"); return 0; }
        float percentAmount = 0;//����֮�ͣ����������1
        float minPercent = 1;//��С����
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
            string pctString = "����Ϊ:";
            for (int i = 0; i < count;i++ )
            {   pctString+=pct[i]+" ";   }
            TDebug.LogError("����,�ܸ���Ϊ0," + pctString); return 0;
        }
        float percentRatio = 1.0f / percentAmount; //�����ܺ͹�һ��ÿ��������Ҫ�˵ı���

        if (minPercent == 0) minPercent = 0.0000001f;
        if (minPercent > 0.1f) minPercent /= 10f;
        int rangeMax = (int)(1.0f / (0.1f * percentRatio * minPercent));//����ķ�Χ

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

        string pctString2 = "����Ϊ:";
        for (int i = 0; i < count; i++)
        { pctString2 += pct[i] + " "; }
        TDebug.LogError("����" + pctString2);
        return 0;
    }
	//����һ������ { 0f, 0.1f, 0.1f }�����飬���ų�withoutList�е�index֮�󣬸��ݸ��ʷ���index
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

    private static System.Random rand = new System.Random();
    public static int GetIndexByPct(int[] probabillty)
    {
        int total = 0;

        for (int i = 0; i < probabillty.Length; i++)
        {
            total += probabillty[i];
        }
        int randomPoint = (int)(rand.NextDouble() * total);
        for (int i = 0; i < probabillty.Length; i++)
        {
            if (randomPoint < probabillty[i])
            {
                return i;
            }
            randomPoint -= probabillty[i];
        }
        return probabillty.Length - 1;
    }


    /// <summary>
    /// ���ݶ����е�id�͸�����Ϣ���������������id
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static int GetRandomId(List<ObjectRandomInfo> list)
    {
        if (list == null || list.Count == 0) 
        { TDebug.LogError("�б�Ϊ��"); return 0; }
        float percentAmount = 0;//����֮�ͣ����������1
        float minPercent=1;//��С����
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
            string pctString = "�б�Ϊ:";
            for (int i = 0; i < count; i++)
            { pctString += "(" + list[i].getPercent + "," + list[i].objectId + " "; }
            TDebug.LogError("����,�ܸ���Ϊ0," + pctString); return list[0].objectId; 
        }
        float percentRatio = 1.0f / percentAmount; //�����ܺ͹�һ��ÿ��������Ҫ�˵ı���

        if (minPercent == 0) minPercent = 0.000001f;
        if (minPercent > 0.1f) minPercent /= 10f;
        int rangeMax = (int)(1.0f / (0.1f*percentRatio * minPercent));//����ķ�Χ

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

        string pctString2 = "�б�Ϊ:";
        for (int i = 0; i < count; i++)
        { pctString2 += "(" + list[i].getPercent + "," + list[i].objectId + " "; }
        TDebug.LogError("����,�ܸ���Ϊ0," + pctString2); return list[0].objectId; 
    }



	/// <summary>
    /// �������ĺͼ��,����һ�������͵�λ�������б�
    /// ��isSquare=trueΪ����������ʱ,columnMax�����������Ч
    /// isY=false ,���x��ڶ�������z
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
    static Vector3 GetCurGridPos(int index, int amount, int columnMax, Vector3 center, Vector2 offset , bool isY)//�õ���ǰ���ӵ�λ�á���������ͬ�����ӵİڷ�λ�ò�һ��
    {
        if (amount == 1)
        {
            return new Vector3(center.x, center.y, center.z);
        }
        else if (amount <= columnMax)//��ֻ��һ��ʱ
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
    /// ʹindexList�е�ֵ�����н������������������ǰlist[0]=0��������list[0]=4����ô֮ǰlist[x]=4�ģ�����list[x]=0
    /// canSwitchToSelf������ʱ���Ƿ����������Լ�
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
        for (int i = 0; i < indexList.Count; i++)//�������õ�����б�
        {
            if (nextIndexPct[i].Equals(0))
                continue;
            if (!canSwitchToSelf)
                nextIndexPct[i] = 0;
            bool isAllZero = true;//����Ƿ�ȫΪ0
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
    /// �õ�cur��ֵ����min-max�еĽ���
    /// </summary>
    public static float GetRangePct(float min , float max,float cur)
    {
        if (max.Equals(min)) return 1f;
        return (cur - min) / (max - min);
    }



}

[System.Serializable]
public class ObjectRandomInfo  //��Ʒ�����id����Ϣ�����õ����id�ĸ��ʴ�С
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
