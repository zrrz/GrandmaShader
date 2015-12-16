using UnityEngine;
using System.Collections;

public class ReflectionMapip : MonoBehaviour 
{
    public Texture2D startTexture = null; // reflective tex

    Texture2D progressTexture; //copy of start tex
    Material material;

	void Start () 
    {
        material = GetComponent<Renderer>().material;
        if (startTexture != null)
        {
            progressTexture = startTexture;
        }
        else // creates 2d texture(black)
        {
            progressTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            progressTexture.name = "Ermagurd";
            for (int x = 0; x < 256; ++x)
                for (int y = 0; y < 256; ++y)
                    progressTexture.SetPixel(x, y, Color.black);

            progressTexture.Apply();
        }
        material.SetTexture("_ReflectionMap", progressTexture);
	}
	
	void Update () 
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();
           // Collider col = GetComponent<Collider>();

            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                //convert from uv_coord to pixel_coord
                Vector2 uv = hitInfo.textureCoord;

                for( int x_offset = -25; x_offset <= 25; ++x_offset )
                {
                    for(int y_offset = -25; y_offset <= 25; ++y_offset)
                    {
                        if ((x_offset*x_offset) + (y_offset*y_offset) > (25*25)) // gives us circle?
                            continue;

                         int x = (int)(uv.x * progressTexture.width) + x_offset;
                         int y = (int)(uv.y * progressTexture.height) + y_offset;

                         if (x < 0 || x >= progressTexture.width)
                             continue;
                         if (y < 0 || y >= progressTexture.height)
                             continue;

                        //get color, add to it, & put it back
                        Color pixelColor = progressTexture.GetPixelBilinear(x, y);
                        pixelColor += new Color(1f,1f,1f,1f);
                        progressTexture.SetPixel(x,y,pixelColor);
                    }
                }
                progressTexture.Apply();
                material.SetTexture("_ReflectionMap", progressTexture);
            }
        }
	}

}
