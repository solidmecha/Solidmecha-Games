using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { Application.LoadLevel(0); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
