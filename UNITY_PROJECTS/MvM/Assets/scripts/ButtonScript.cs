using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public Text text;
	public Action skillMethod;

	public void setUpButton()
	{
		GetComponent<Button>().onClick.AddListener(delegate{skillMethod();});
	}

	public void removeListeners()
	{
		GetComponent<Button>().onClick.RemoveAllListeners();
	}

}
