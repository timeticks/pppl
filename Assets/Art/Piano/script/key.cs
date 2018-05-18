using UnityEngine;
using System.Collections;

public class key : MonoBehaviour {

	public int keyNum;
	public AudioClip clip1;
	public AudioClip clip2;
	public AudioClip clip3;
	public AudioClip clip4;
	public AudioClip clip5;
	public AudioClip clip6;
	public AudioClip clip7;

	private KeyCode code;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(keyNum){
			case 1:
				code = KeyCode.Alpha1;
				gameObject.GetComponent<AudioSource>().clip = clip1;
			break;

			case 2:
				code = KeyCode.Alpha2;
				gameObject.GetComponent<AudioSource>().clip = clip2;
			break;

			case 3:
				code = KeyCode.Alpha3;
				gameObject.GetComponent<AudioSource>().clip = clip3;
			break;

			case 4:
				code = KeyCode.Alpha4;
				gameObject.GetComponent<AudioSource>().clip = clip4;
			break;

			case 5:
				code = KeyCode.Alpha5;
				gameObject.GetComponent<AudioSource>().clip = clip5;
			break;

			case 6:
				code = KeyCode.Alpha6;
				gameObject.GetComponent<AudioSource>().clip = clip6;
			break;

		    case 7:
				code = KeyCode.Alpha7;
				gameObject.GetComponent<AudioSource>().clip = clip7;
			break;
		};



		if(Input.GetKeyDown(code)){
			gameObject.transform.Rotate(3,0,0,Space.Self);
			gameObject.GetComponent<AudioSource>().Play();
			GameObject.Find("/" + gameObject.name + "/key").GetComponent<Renderer>().material.color = Color.red;
		}
		if(Input.GetKeyUp(code)){
			gameObject.transform.Rotate(-3,0,0,Space.Self);
			GameObject.Find("/" + gameObject.name + "/key").GetComponent<Renderer>().material.color = Color.white;
		}
	}
}

