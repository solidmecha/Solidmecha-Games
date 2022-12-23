using UnityEngine;
using System.Collections;

public class TextControl : MonoBehaviour {

    public string[] Words;
    public UnityEngine.UI.Text Text;

	// Use this for initialization
	void Start () {
        Text = GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
