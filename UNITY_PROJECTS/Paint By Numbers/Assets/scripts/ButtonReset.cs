using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonReset : MonoBehaviour {

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(delegate{Application.LoadLevel(0);});
	}
}