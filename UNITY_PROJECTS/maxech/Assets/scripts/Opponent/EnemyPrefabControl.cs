using UnityEngine;
using System.Collections;

public class EnemyPrefabControl : MonoBehaviour {

    public static EnemyPrefabControl singleton;

    public GameObject EvilOrb;

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
