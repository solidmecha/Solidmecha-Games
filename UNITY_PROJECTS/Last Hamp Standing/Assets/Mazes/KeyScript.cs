using UnityEngine;
using UnityEngine.Networking;

public class KeyScript : NetworkBehaviour {
    [SyncVar(hook ="UpdateColor")]
    public int ID;
    public void UpdateColor(int change)
    {   
        GetComponent<SpriteRenderer>().color = MazeServerControl.singleton.Colors[change];
    }

    private void Start()
    {
        UpdateColor(ID);
    }

}
