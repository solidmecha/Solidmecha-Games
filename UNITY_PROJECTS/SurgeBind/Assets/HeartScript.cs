using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartScript : MonoBehaviour {

public Texture2D fullHeart;
public Texture2D emptyHeart;
public RawImage heart;

	// Use this for initialization
	void Start () {
	
	}

	public void takeDamage()
	{
		heart.texture= emptyHeart;
	}

	public void heal()
	{
		heart.texture= fullHeart;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
