using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

    public string InputString;
    public RenderTexture tex;
    public Material mat;
    public Texture2D Initial_Texture;
    Texture2D buffer;
    float LastUpdateTime;
    public float Interval;
    public bool isUsingBuffer;

	// Use this for initialization
	void Start () {
        Graphics.Blit(Initial_Texture, tex);
        buffer = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
	}

    void UpdateScreen()
    {
        int c=InputString.GetHashCode();
        if (c < 0)
            c *= -1;
        int r=c % 1000;
        c -= r;
        int g= (c % 1000000)/1000;
        c -= g;
        int b= c/1000000;
        Color color = new Color((float)r / 1000f, (float)g / 1000f, (float)b / 1000f);
        print(color);
        Camera.main.backgroundColor = color;
        //Color[] ca = new Color[1] { color };
        //tex.SetPixels(ca);
        //tex.Apply();
    }

    void UpdateTexture()
    {
        RenderTexture.active = tex;
        buffer.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        buffer.Apply();
        Graphics.Blit(buffer, tex);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            InputString+=Input.inputString;
            UpdateScreen();
        }
        if(Time.time>LastUpdateTime+Interval)
        {
           // OnRenderImage(tex, tex);
            UpdateTexture();
            LastUpdateTime = Time.time;
        }
	}
}
