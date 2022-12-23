using UnityEngine;
using System.Collections.Generic;

public class WirePuzzle : MonoBehaviour {
    GameControl GC;

    public bool selected;
    public NodeScript SelectedNode;
    public GameObject SelectedWire;
    public int SelectedWireID;
    List<Color> Colors = new List<Color> { Color.black, Color.blue, Color.red, Color. yellow};
    List<KeyNode> PuzzleNodes;

    class KeyNode
    {
        public bool isPath;
        public NodeScript node;
        public int wireID;
        public List<NodeScript> nodesOnPath;
        public bool isSurrounded;
        public bool isClear;
        public bool isCount;
        public int conCount;

        public KeyNode(NodeScript n, bool p, int w, List<NodeScript> nop, bool s, bool c, bool count, int concount)
        {
            isPath = p;
            node = n;
            nodesOnPath = nop;
            isSurrounded = s;
            isClear = c;
            isCount = count;
            conCount = concount;
        }
    }

    public class Connection
    {
        public NodeScript NS;
        public int wireID; //yellow, green, red, blue

        public Connection(NodeScript ns, int wid)
        {
            NS = ns;
            wireID = wid;
        }
    }

    void GeneratePuzzle()
    {

    }
	// Use this for initialization
	void Start () {
        //selected = false;
        NodeScript[][] Neighborhood = new NodeScript[8][];
	    for(int i=0;i<8;i++)
        {
            Neighborhood[i] = new NodeScript[8];
            for (int j = 0; j < 8; j++)
            {
                transform.GetChild(i * 8 + j).localPosition = new Vector2(i, j)*.1f + new Vector2(-.35f, -.35f);
                NodeScript NS = (NodeScript)transform.GetChild(i * 8 + j).GetComponent(typeof(NodeScript));
                NS.WP = this;
                int I = i * 8 + j;
                NS.ID = I;
                Neighborhood[i][j] = NS;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (i > 0)
                {
                    Neighborhood[i][j].Neighbors.Add(Neighborhood[i - 1][j]);
                    if(j>0)
                        Neighborhood[i][j].Neighbors.Add(Neighborhood[i - 1][j - 1]);
                    if(j<7)
                        Neighborhood[i][j].Neighbors.Add(Neighborhood[i - 1][j + 1]);
                }
                if(i<7)
                {
                    Neighborhood[i][j].Neighbors.Add(Neighborhood[i + 1][j]);
                    if (j > 0)
                        Neighborhood[i][j].Neighbors.Add(Neighborhood[i + 1][j - 1]);
                    if (j < 7)
                        Neighborhood[i][j].Neighbors.Add(Neighborhood[i + 1][j + 1]);
                }
                if(j>0)
                    Neighborhood[i][j].Neighbors.Add(Neighborhood[i][j-1]);
                if(j<7)
                    Neighborhood[i][j].Neighbors.Add(Neighborhood[i][j + 1]);
            }
        }

            }
	
    public void placeWire(NodeScript node)
    {
        if(!checkConnection(SelectedNode, node))
        {
            GameObject wire=Instantiate(SelectedWire, SelectedNode.transform.position+(node.transform.position - SelectedNode.transform.position)*.5f, Quaternion.identity) as GameObject;
            wire.transform.eulerAngles = new Vector3(0,0,getAngle(SelectedNode.transform.position, node.transform.position));
            wire.GetComponent<SpriteRenderer>().color = Colors[SelectedWireID];
            Connection c1 = new Connection(SelectedNode, SelectedWireID);
            Connection c2 = new Connection(node, SelectedWireID);
            SelectedNode.connectedNodes.Add(c2);
            node.connectedNodes.Add(c1);
        }
    }

    public float getAngle(Vector2 v1, Vector2 v2)
    {
        if ((v1.x < v2.x && v1.y > v2.y) || (v1.x > v2.x && v1.y < v2.y))
        {
            return -45;
        }
        else if ((v1.x > v2.x && v1.y > v2.y) || (v1.x < v2.x && v1.y < v2.y))
        { return 45; }
        else if (v1.y < v2.y || v1.y > v2.y)
        { return 90; }
        else
        {
            return 0;
        }
    }

    bool checkConnection(NodeScript NS1, NodeScript NS2)
    {
            foreach (Connection c in NS1.connectedNodes)
            {
                if (c.NS.Equals(NS2))
                { return true; }
            }
        return false;
    }
    string GenerateManualOveride()
    {
        string s="";

        return s;
    }

    public bool checkPath(NodeScript NS1, NodeScript NS2, int WireID)
    {
        List<NodeScript> NodesOnPath = new List<NodeScript> { NS1};
        for(int i=0;i<NodesOnPath.Count;i++)
        {
            foreach(Connection n in NodesOnPath[i].connectedNodes)
            {
                if(!NodesOnPath.Contains(n.NS) && n.wireID==WireID)
                { NodesOnPath.Add(n.NS); }
            }
        }
        if (NodesOnPath.Contains(NS2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkClearSurround(NodeScript ns)
    {
        foreach(NodeScript n in ns.Neighbors)
        {
            if (n.connectedNodes.Count > 0)
                return false;
        }
        return true;
    }

    public bool checkFullSurround(NodeScript ns)
    {
        foreach (NodeScript n in ns.Neighbors)
        {
            if (n.connectedNodes.Count == 0)
                return false;
        }
        return true;
    }

    public bool checkWireCross(int w1, int w2)
    {
        return true;
    }

    bool checkConnectionCount(NodeScript ns, int i)
    {
        if (ns.connectedNodes.Count == i)
            return true;
        else
            return false;
    }

    bool checkSolution()
    {
        foreach (KeyNode k in PuzzleNodes)
        {
            if (k.isPath)
            {
                foreach (NodeScript n in k.nodesOnPath)
                {
                    if (!checkPath(k.node, n, k.wireID))
                    { return false; }
                }
            }

            if(k.isSurrounded && !checkFullSurround(k.node))
            {
                return false;
            }
            if(k.isClear && !checkClearSurround(k.node))
            {
                return false;
            }
            if(k.isCount && !checkConnectionCount(k.node, k.conCount))
            {
                return false;
            }
        }
        return true;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
