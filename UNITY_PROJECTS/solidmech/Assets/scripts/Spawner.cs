using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Spawns;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 0f, 15f);
    }

    void Spawn()
    {
        Instantiate(Spawns[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
