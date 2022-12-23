using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public Text text;
	public Action<int> skillMethod;
	public int id;
    public ItemManager IM;

	public void setUpButton()
	{
		GetComponent<Button>().onClick.AddListener(delegate {skillMethod(id);});
        GetComponent<Button>().onClick.AddListener(delegate { IM.timePasses(); });
        }

	public void removeListeners()
	{
		GetComponent<Button>().onClick.RemoveAllListeners();
	}

}
