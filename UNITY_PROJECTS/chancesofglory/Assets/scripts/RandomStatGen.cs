using UnityEngine;
using System.Collections;

public class RandomStatGen : MonoBehaviour {

    public int[] minMaxHP;
    public int[] minMaxMP;
    public int[] minMaxArmor;

	// Use this for initialization
	void Start () {
       StatScript S= GetComponent<StatScript>();
        S.HP[1] = GameControl.singleton.RNG.Next(minMaxHP[0], minMaxHP[1]);
        S.MP[1] = GameControl.singleton.RNG.Next(minMaxMP[0], minMaxMP[1]);
        S.UpdateArmor(GameControl.singleton.RNG.Next(minMaxArmor[0], minMaxArmor[1]));
        S.UpdateHP(-1*GameControl.singleton.RNG.Next(S.HP[1]/2, S.HP[1]));
        S.UpdateMP(-1*GameControl.singleton.RNG.Next(S.MP[1]));
        S.Snapshot();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
