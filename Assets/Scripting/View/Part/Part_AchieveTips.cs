using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Part_AchieveTips : MonoBehaviour {

    public Text TextName;
    public Text TextDesc;
    public Text TextPoint;

    public void Init(string name,string desc,int point)
    {
        TextName.text = name;
        TextDesc.text = desc;
        TextPoint.text = point.ToString();
        gameObject.SetActive(true);
    }
	
}
