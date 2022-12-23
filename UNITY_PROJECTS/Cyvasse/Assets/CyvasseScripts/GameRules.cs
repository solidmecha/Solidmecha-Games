using UnityEngine;
using System.Collections;

public class GameRules : MonoBehaviour {

	public class King{
		public int diagonalMovement=0;
		public int orthogonalMovement=1;
		public int combat=1;
	}
	public class Rabble{
		public int diagonalMovement=0;
		public int orthogonalMovement=1;
		public int combat=1;}
	public class LightHorse{
		public int diagonalMovement=0;
		public int orthogonalMovement=0;
		public int continuousMovement=2;
		public int combat=2;
	}
	public class HeavyHorse{
		public int diagonalMovement=0;
		public int orthogonalMovement=4;
		public int combat=3;
	}
	public class Spear{
		public int diagonalMovement=1;
		public int orthogonalMovement=1;
		public int combat=2;
	}
	public class Crossbow{
		public int diagonalMovement=0;
		public int orthogonalMovement=2;
		public int combat=2;
	}
	public class Catapult{
		public int diagonalMovement=0;
		public int orthogonalMovement=1;
		public int combat=3;
	}
	public class Trebuchet{
		public int diagonalMovement=0;
		public int orthogonalMovement=2;
		public int combat=4;
	}
	public class Elephant{
		public int diagonalMovement=2;
		public int orthogonalMovement=1;
		public int combat=3;
	}
	public class Dragon{
		public int diagonalMovement=0;
		public int orthogonalMovement=0;
		public int continuousMovement=3;
		public int combat=5;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
