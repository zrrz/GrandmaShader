using UnityEngine;
using System.Collections;

public class SkyboxCycle : MonoBehaviour {

	float blend = 0f;
	[Range(0f, 0.1f)]public float speed = 0.01f;
	public Material mat;

	bool day = false;

	public Light sun;

	Color dayColor, nightColor;

	public Material snowHouseMat;
	float snowLevelDay = 0.1f, snowLevelNight = 0.4f;
	float snowLevel = 0f;

	void Start () {
		nightColor = sun.color;
		dayColor = Color.white;
	}
	
	void Update () {
		mat.SetFloat("_Blend", blend);
		snowHouseMat.SetFloat("_Snow", snowLevel);
		snowLevel = Mathf.Lerp(snowLevelNight, snowLevelDay, blend);
		sun.color = Color.Lerp(nightColor, dayColor, blend);
		if(day) {
			blend += speed * Time.deltaTime;
			if(blend >= 1f)
				day = false;
		} else {
//			snowLevel = Mathf.Lerp(snowLevelNight, snowLevelDay, blend);
//			sun.color = Color.Lerp(dayColor, nightColor, blend);
			blend -= speed * Time.deltaTime;
			if(blend <= 0f)
				day = true;
		}
	}
}
