using UnityEngine;
using System.Collections;

public class WeatherFollow : MonoBehaviour {

	public Transform target;

	float height;
	void Start () {
		height = Mathf.Abs (target.position.y - transform.position.y);
	}
	
	void Update () {
		Vector3 pos = target.position;
		pos.y += height;
		transform.position = pos;
	}
}
