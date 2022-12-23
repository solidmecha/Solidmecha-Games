using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodScript : MonoBehaviour {
    public int Value;
    public int Nom;
    public bool inShop;
    public GameObject shopBox;
    public Vector2 V = new Vector2(0.93f, 0.27f);
    public ItemManager IM;
    int invLoc;

    void OnMouseDown()
    {
        if(!inShop)
        omNomNom();
        else
        {
            if(IM.DisplayBox!=null)
            { Destroy(IM.DisplayBox); }
           GameObject go= (GameObject )Instantiate(shopBox, V, Quaternion.identity) as GameObject;
            IM.DisplayBox = go;
            go.transform.GetChild(0).GetChild(6).gameObject.GetComponent<Text>().text = Value.ToString();
            GameObject go0 = go.transform.GetChild(0).GetChild(8).gameObject;
            //make exit button work
            go0.GetComponent<Button>().onClick.AddListener(delegate { Destroy(go0.transform.parent.parent.gameObject); });

            if (IM.CS.Gold >= Value)
            {
                go.transform.GetChild(0).GetChild(9).GetComponent<Button>().onClick.AddListener(delegate { IM.SM.shopInventory.Remove(gameObject); });
                go.transform.GetChild(0).GetChild(9).GetComponent<Button>().onClick.AddListener(delegate { buyFood(); });
                go.transform.GetChild(0).GetChild(9).GetComponent<Button>().onClick.AddListener(delegate { IM.SM.cleanUp(); });

            }
        }

    }

    void buyFood()
    {
        inShop = false;
        IM.CS.Gold -= Value;
        if (IM.invLocPoint < 9)
        {
            transform.position = IM.InventoryLocs[IM.invLocPoint];
            invLoc = IM.invLocPoint;
            IM.inventoryArrayBool[invLoc] = true;

            for (int i = 0; i < 9; i++)
            {
                if (!IM.inventoryArrayBool[i])
                {
                    IM.invLocPoint = i;
                    break;
                }
                if (i == 8 && IM.inventoryArrayBool[8])
                {
                    IM.invLocPoint = 10;
                }
            }
        }
    }

    void omNomNom()
    {
        IM.invLocPoint = invLoc;
        IM.inventoryArrayBool[invLoc] = false;
        if (IM.CS.Problems[2] + Nom <= 100)
        {
            IM.CS.Problems[2] += Nom;
        }
        else
        {
            IM.CS.Problems[2] = 100;
        }
        IM.updateUI();
        Destroy(gameObject);

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
