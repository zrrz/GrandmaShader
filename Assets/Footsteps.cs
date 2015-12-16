using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

	Terrain terrain;

	public Transform leftFoot, rightFoot;

	[Range(0.0f, 0.3f)]public float footStepDepth = 0.01f;

	void Start () {
		terrain = Terrain.activeTerrain;
	}

	void ApplyLeftFootStep() {
		ApplyFootStep(leftFoot.position);
	}

	void ApplyRightFootStep() {
		ApplyFootStep(rightFoot.position);
	}

	void ApplyFootStep(Vector3 point) {
		Debug.Log("R Footstep Applied");
		Vector3 coords = ConvertWorldCor2TerrainCoordinates(point);
		float height = terrain.SampleHeight(point);
		float[,] sample = new float[1,1];
//		print (height);
		sample[0,0] = Mathf.InverseLerp(0.0f, terrain.terrainData.size.y, (height - footStepDepth));
//		print (sample[0,0] + " " + terrain.terrainData.size.y);
		terrain.terrainData.SetHeightsDelayLOD((int)coords.x, (int)coords.z, sample);
	}

	private Vector3 ConvertWorldCor2TerrainCoordinates(Vector3 worldCor) {
		Vector3 vecRet = new Vector3();
		Vector3 terPosition = terrain.transform.position;
		vecRet.x = ((worldCor.x - terPosition.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth;
		vecRet.z = ((worldCor.z - terPosition.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight; 
		return vecRet; 
	}
}
