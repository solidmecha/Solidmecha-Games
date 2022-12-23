using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class curseScript : MonoBehaviour {

    public ItemScript.Affix curse;
    public GameObject curseBox;
    public ItemManager IM;
    public int Index;
    public Vector2 boxV=new Vector2(.88f,.78f);

    void OnMouseDown()
    {
       
        if (IM.DisplayBox != null)
        { Destroy(IM.DisplayBox); }
        GameObject go = (GameObject)Instantiate(curseBox, boxV, Quaternion.identity) as GameObject;
        IM.DisplayBox = go;
        if (IM.healing)
        {
            //move heal button and set it up
            go.transform.GetChild(0).GetChild(3).position = go.transform.GetChild(0).GetChild(4).position;
            go.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate { IM.removeCurse(Index); });
            go.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate { IM.SM.setCurseRemoved(); });
            go.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate { Destroy(go); });
        }
        go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = curse.Description;
        go.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate { Destroy(go); });

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
