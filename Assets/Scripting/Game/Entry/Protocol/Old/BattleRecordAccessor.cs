using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRecordAccessor : DescObject
{
    public Dictionary<string, BattleRecordStr> BattleRecordPool = new Dictionary<string, BattleRecordStr>();
    public string NewestRecord="";  //最新的战报key



}
