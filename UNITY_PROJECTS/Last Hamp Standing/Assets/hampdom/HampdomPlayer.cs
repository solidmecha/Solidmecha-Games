using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HampdomPlayer : NetworkBehaviour {

    public int[] Workers;//Peasants, Farmers, Miners, Scholars, scouts
    public int[] Buildings;
    public int[] Army; // Archers, Mages, Templar, Knights, Dragons
    public int[] Resources; //food, gold, mana
    public int[] Upgrades; //magic, melee, ranged, worker
    public int KingHP;
    public UnityEngine.UI.Text TextBox;
    public int MessageCount;
    public int MaxMessage;

    [ClientRpc]
    public void RpcResolveOffensive(int[] OppArmy)
    {
        if(isLocalPlayer)
        {
            int[] Losses=new int[5];
            for (int i = 0; i < 5; i++)
                OppArmy[i] -= Losses[i];
        }
    }

    [ClientRpc]
    public void RpcMessage(string Message)
    {
        if(isLocalPlayer)
        {
            if (MessageCount == MaxMessage)
                TextBox.text = TextBox.text.Substring(TextBox.text.IndexOf('\n') + 1);
            TextBox.text = Message + '\n';
        }
    }

    public bool CanPay(int[] cost)
    {
        bool b = true;
        for (int i = 0; i < 3; i++)
        {
            if (cost[i] > Resources[i])
            {
                b = false;
                break;
            }
        }
        return b;
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
