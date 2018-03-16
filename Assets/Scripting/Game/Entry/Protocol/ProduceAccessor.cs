using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceAccessor:IUpdate
{
    public int mLastRewardTime = 0;
    public Dictionary<int, int> ProduceDict = new Dictionary<int, int>(); //产品id、工人数


    public void UpdateOnSecond()
    {
        int offsetTime = AppTimer.CurTimeStampSecond - mLastRewardTime;

        int curRewardInterval = GameConstUtils.produce_origin_time; //收益间隔
        if (offsetTime > curRewardInterval)
        {
            if (offsetTime > GameConstUtils.produce_max_time) //最大离线收益时间
                offsetTime = GameConstUtils.produce_max_time;

            int rewardNum = offsetTime/curRewardInterval;
            mLastRewardTime = AppTimer.CurTimeStampSecond - offsetTime % curRewardInterval;


            //计算收益
            foreach (var temp in ProduceDict)
            {
                Produce produce = Produce.Fetcher.GetProduceCopy(temp.Key);
                if (produce == null) continue;


            }


        }


    }


    public void FreshProduce()
    {
        float timeDown = 12f;
        int lastTime = 0;
        int curTime = AppTimer.CurTimeStampSecond;
        if (curTime - lastTime >= timeDown)
        {
            lastTime = AppTimer.CurTimeStampSecond;
            

        }
    }

}

public interface IUpdate
{
    void UpdateOnSecond();
}