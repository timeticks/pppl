using UnityEngine;
using System.Collections;

public class piano : MonoBehaviour {
	public GameObject key;
	// Use this for initialization
	void Start () {
		Vector3 position;
		position.x = 0;
		position.y = 0;
		position.z = 0;
		for(int i = 1; i<=7; i++){
			position.x -= (float)0.8;
			GameObject cloneKey = (GameObject)Instantiate(key,position,gameObject.transform.rotation);
			cloneKey.name = "key" + i.ToString();
			cloneKey.GetComponent<key>().keyNum = i;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
