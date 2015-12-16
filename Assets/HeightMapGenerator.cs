using UnityEngine;
using System.Collections;

public class HeightMapGenerator : MonoBehaviour {

    public Texture2D heightMap;
    public Texture2D decal;
    Texture2D actualHeightMap;

    int heightMapWidth = 2048;
    int heightMapHeight = 2048;

    // assumption: script is on same object as material, using "extrudemap" shader
    Material material;
    // assumption: collider is a mesh collider
    Collider collider;

	// Use this for initialization
	void Start () 
    {
        collider = gameObject.GetComponent<Collider>();
        material = gameObject.GetComponent<Renderer>().material;
//        if (material.shader.name != "Custom/extrudemap")
//        {
//            print("ERROR: Incorrect shader!!");
//            Destroy(this);
//        }

        if (heightMap != null)
        {
            heightMapWidth = heightMap.width;
            heightMapHeight = heightMap.height;
        }
        actualHeightMap = new Texture2D(heightMapWidth, heightMapHeight);
        if (heightMap != null)
        {
            actualHeightMap.SetPixels(heightMap.GetPixels());
        }
        else
        {
            for (int x = 0; x < heightMapWidth; ++x)
                for (int y = 0; y < heightMapHeight; ++y)
                    actualHeightMap.SetPixel(x, y, Color.white);
        }
        actualHeightMap.Apply();

        material.SetTexture("_ExtrudeMap", actualHeightMap);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();
            if (collider.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                int x = (int)(hitInfo.textureCoord.x * heightMapWidth);
                int y = (int)(hitInfo.textureCoord.y * heightMapHeight);

                ApplyDecal(x, y);

                actualHeightMap.Apply();
            }
        }
	}

    void ApplyDecal(int centerX, int centerY)
    {
        for (int x = 0; x < decal.width; ++x)
        {
            for (int y = 0; y < decal.height; ++y)
            {
                Color c = decal.GetPixel(x, y);
                int offsetX = centerX - decal.width / 2;
                int offsetY = centerY - decal.height / 2;

                if (offsetX + x < 0 || offsetX + x >= heightMapWidth ||
                    offsetY + y < 0 || offsetY + y >= heightMapHeight)
                    continue;
                if (c.a == 0)
                    continue;

                actualHeightMap.SetPixel(offsetX + x, offsetY + y, c);
            }
        }
    }
}


