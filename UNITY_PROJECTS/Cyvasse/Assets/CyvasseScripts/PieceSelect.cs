using UnityEngine;
using System.Collections;

public class PieceSelect : MonoBehaviour {

GameObject rulesObj;

	public struct Piece
	{	public int diagonalMovement;
		public int orthogonalMovement;
		public int continuousMovement;
		public int rank;
		public int file;
		public int combat;
		public bool isCaptured;
	}

	public Piece thisPiece;

	void OnMouseDown()
	{
		GameManager.showMovement=false;
		GameManager.stopShowingMovement=true;
		GameManager.SelectedPiece = gameObject;
		GameManager.highlighted = false;
		if(!thisPiece.isCaptured)
		{GameManager.showMovement=true;}
		if (GameManager.redOutline) {
			Destroy(GameManager.redOutline);}
		}
	// Use this for initialization
	void Start () {

		rulesObj=GameObject.Find("Bear");
		GameRules Rules = (GameRules)rulesObj.GetComponent (typeof(GameRules));
		thisPiece = new Piece ();
		string s;
		string n = name;
		s=name.TrimEnd(new char[]{'2'});
		thisPiece.continuousMovement=0;

		
		switch (s) {
				case "King":
						GameRules.King king=new GameRules.King();
						thisPiece.diagonalMovement=king.diagonalMovement;
			thisPiece.orthogonalMovement=king.orthogonalMovement;
			thisPiece.combat=king.combat;
						break;
				case "Rabble":
				GameRules.Rabble rabble=new GameRules.Rabble();
					thisPiece.diagonalMovement=rabble.diagonalMovement;
			thisPiece.orthogonalMovement=rabble.orthogonalMovement;
			thisPiece.combat=rabble.combat;

						break;
							case "LightHorse":
						GameRules.LightHorse lightHorse=new GameRules.LightHorse();
						thisPiece.diagonalMovement=lightHorse.diagonalMovement;
			thisPiece.orthogonalMovement=lightHorse.orthogonalMovement;
			thisPiece.continuousMovement=lightHorse.continuousMovement;
			thisPiece.combat=lightHorse.combat;
						break;

						case "HeavyHorse":
						{
							GameRules.HeavyHorse heavyHorse=new GameRules.HeavyHorse();
								thisPiece.diagonalMovement=heavyHorse.diagonalMovement;
			thisPiece.orthogonalMovement=heavyHorse.orthogonalMovement;
			thisPiece.combat=heavyHorse.combat;
			break;

						}
							case "Elephant":
						{
							GameRules.Elephant elephant=new GameRules.Elephant();
								thisPiece.diagonalMovement=elephant.diagonalMovement;
			thisPiece.orthogonalMovement=elephant.orthogonalMovement;
			thisPiece.combat=elephant.combat;
			break;

						}

						case "Crossbow":
						GameRules.Crossbow crossbow=new GameRules.Crossbow();
								thisPiece.diagonalMovement=crossbow.diagonalMovement;
			thisPiece.orthogonalMovement=crossbow.orthogonalMovement;
			thisPiece.combat=crossbow.combat;
			break;
						
						case "Spear":
						GameRules.Spear spear=new GameRules.Spear();
								thisPiece.diagonalMovement=spear.diagonalMovement;
			thisPiece.orthogonalMovement=spear.orthogonalMovement;
			thisPiece.combat=spear.combat;
			break;

						case "Catapault":
						GameRules.Catapult catapult=new GameRules.Catapult();
								thisPiece.diagonalMovement=catapult.diagonalMovement;
			thisPiece.orthogonalMovement=catapult.orthogonalMovement;
			thisPiece.combat=catapult.combat;
						break;

						case "treb":
							GameRules.Trebuchet treb=new GameRules.Trebuchet();
								thisPiece.diagonalMovement=treb.diagonalMovement;
			thisPiece.orthogonalMovement=treb.orthogonalMovement;
			thisPiece.combat=treb.combat;
						break;

						case "Dragon":
							GameRules.Dragon dragon=new GameRules.Dragon();
								thisPiece.diagonalMovement=dragon.diagonalMovement;
			thisPiece.orthogonalMovement=dragon.orthogonalMovement;
			thisPiece.continuousMovement=dragon.continuousMovement;
			thisPiece.combat=dragon.combat;
						break;

						default:
						break;


				}

		thisPiece.isCaptured=true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
