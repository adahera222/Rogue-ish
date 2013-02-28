using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Actor player;
	
	public Vector3 inputDir;
	public Coordinates inputCoords;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player.mover != null) {
			inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //input direction is now rounded off to 0 or 1
			inputCoords = Level.PositionToCoords(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
			
			if (player.mover.canMove && inputDir.sqrMagnitude > .1f) {
				Debug.Log("Moving Player");
				player.mover.MoveTo(player.coords+inputCoords);
			}
		}
	}
}

