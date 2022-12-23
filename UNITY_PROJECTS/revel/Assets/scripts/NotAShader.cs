using UnityEngine;
using System.Collections;

public class NotAShader : MonoBehaviour {

    public Texture2D Field;
    public Texture2D Fluid_Texture;
    Texture2D Buffer;
    float LastUpdateTime;
    public float Interval;

	// Use this for initialization
	void Start () {
        Buffer = new Texture2D(Fluid_Texture.width, Fluid_Texture.width, Fluid_Texture.format, false, true);
        Buffer.filterMode = FilterMode.Trilinear;
        Buffer.wrapMode = TextureWrapMode.Clamp;
        Buffer.SetPixels(Fluid_Texture.GetPixels());
        GetComponent<Renderer>().material.SetTexture("_MainTex", Buffer);
        UpdateTexture();
	}

    void UpdateTexture()
    {
        Vector2 NewCoordinate=Vector2.zero;

        for (int i = Field.width; i >=0; i--)
        {
            for (int j = Field.height; j >=0; j--)
            {
                Color c=Buffer.GetPixel(i, j);
                if(c.r>.5f)
                {
                    Color F = Field.GetPixel(i, j);
                    if (F.r > .5f)
                    {
                        if (F.r > .75f)
                            NewCoordinate += Vector2.up;
                        else
                            NewCoordinate += Vector2.down;
                    }
                    if (F.g > .5f)
                    {
                        if (F.g > .75f)
                            NewCoordinate += Vector2.right;
                        else
                            NewCoordinate += Vector2.left;
                    }
                    if (NewCoordinate.x != 0 || NewCoordinate.y != 0)
                    {
                        print("set");
                        Buffer.SetPixel(i+(int)NewCoordinate.x,j+(int)NewCoordinate.y, c);
                        Buffer.SetPixel(i, j, Color.black);
                    }
                }
            }
        }
        Buffer.Apply();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > LastUpdateTime + Interval)
        {
            // OnRenderImage(tex, tex);
            UpdateTexture();
            LastUpdateTime = Time.time;
        }
    }
}
