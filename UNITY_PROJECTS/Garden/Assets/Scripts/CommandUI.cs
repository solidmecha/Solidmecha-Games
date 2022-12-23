using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommandUI : MonoBehaviour {

    public GameObject CommandSprite;
    public Button CloseButton;
    public NeighborHelper NH;
    public CellUI cellui;
    public Dropdown Drop;

	// Use this for initialization
	void Start () {
        CloseButton.onClick.AddListener(delegate { Destroy(gameObject); });
        CloseButton.onClick.AddListener(delegate { cellui.InvokeResetActive(); });
        CommandSprite.GetComponent<SpriteRenderer>().sprite=NH.sprite ;
        CommandSprite.transform.localPosition = NH.Loc*CommandSprite.transform.localScale.x;
        cellui.ActiveButton.GetComponent<Button>().interactable = false;
        cellui.ScanToggle.interactable = false;
        Drop.value = cellui.CS.WS.DropValue;
        Drop.onValueChanged.AddListener(delegate { setDropValue(); });
    }
    void setDropValue()
    {
        cellui.CS.WS.DropValue = Drop.value;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
