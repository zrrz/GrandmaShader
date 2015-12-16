using UnityEngine;
using System.Collections;

public class snow : MonoBehaviour {
    public float snowlevel = 0;
    public float coverage;

    Material mat;
    Renderer rend;
    // Use this for initialization
    void Start()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;
        rend = gameObject.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rend.bounds.max.x < snowlevel)
            coverage = 1;

        else if (rend.bounds.max.x < snowlevel && rend.bounds.min.x < snowlevel)
        {
            float newconverage = (snowlevel + rend.bounds.min.x) / (rend.bounds.max.x + rend.bounds.min.x);
            coverage = coverage > newconverage ? coverage : newconverage;
        }

        coverage -= Time.deltaTime * 0.1f;
        coverage = coverage < 0 ? 0 : coverage;

        mat.SetFloat("_Thresh", coverage);
	}
}
