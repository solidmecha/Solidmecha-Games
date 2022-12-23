using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

    public static GameControl singleton;
    public GameObject[] WeaponPrototypes;
    public System.Random RNG;
    public Text NoticeText;
    public Text SystemText;
    public enum DamageType {hp, energy, heat};
    public enum WeaponType { Gat, Nova, Beam, OrbitalCannon, Missle, Burst};
    public enum ItemType { PowerCore, Chasis, Thruster, Hydraulics, Shields, Weapon}; 
    public GameObject DmgNum;
    public bool isPlayingLevel;
    public int CurrentLvl;
    public Transform startCanvasParent;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
    }
    // Use this for initialization
    void Start () {
        GameObject go=ItemControl.singleton.DisplayItem(startCanvasParent.GetChild(0).position, ItemControl.singleton.GenerateItem(ItemType.PowerCore, CurrentLvl));
        Destroy(go.transform.GetChild(10).gameObject);
        go= ItemControl.singleton.DisplayItem(startCanvasParent.GetChild(1).position, ItemControl.singleton.GenerateItem(ItemType.Chasis, CurrentLvl));
        Destroy(go.transform.GetChild(10).gameObject);
        go = ItemControl.singleton.DisplayItem(startCanvasParent.GetChild(2).position, ItemControl.singleton.GenerateItem(ItemType.Thruster, CurrentLvl));
        Destroy(go.transform.GetChild(10).gameObject);
        go = ItemControl.singleton.DisplayItem(startCanvasParent.GetChild(3).position, ItemControl.singleton.GenerateItem(ItemType.Hydraulics, CurrentLvl));
        Destroy(go.transform.GetChild(10).gameObject);
        go = ItemControl.singleton.DisplayItem(startCanvasParent.GetChild(4).position, ItemControl.singleton.GenerateItem(ItemType.Shields, CurrentLvl));
        Destroy(go.transform.GetChild(10).gameObject);
        go = ItemControl.singleton.DisplayWeapon(startCanvasParent.GetChild(5).position, ItemControl.singleton.GenerateWeapon(CurrentLvl));
        Destroy(go.transform.GetChild(12).gameObject);
        Destroy(go.transform.GetChild(11).gameObject);
        Destroy(go.transform.GetChild(10).gameObject);
        go = ItemControl.singleton.DisplayWeapon(startCanvasParent.GetChild(6).position, ItemControl.singleton.GenerateWeapon(CurrentLvl));
        Destroy(go.transform.GetChild(12).gameObject);
        Destroy(go.transform.GetChild(11).gameObject);
        Destroy(go.transform.GetChild(9).gameObject);
        ItemControl.singleton.ItemCount = 7;
    }

    public void reset()
    {
        Application.LoadLevel(0);
    }

    public Vector3 RandPoint()
    {
        return new Vector3(RNG.Next(-100,101)/100f, 0, RNG.Next(-100, 101) / 100f).normalized;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
