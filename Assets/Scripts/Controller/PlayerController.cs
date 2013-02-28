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
			if (Input.GetKey(KeyCode.W)) {
				player.mover.MoveTo(player.coords.x, player.coords.y+1);
			} else if (Input.GetKey(KeyCode.S)) {
				player.mover.MoveTo(player.coords.x, player.coords.y-1);
			} else if (Input.GetKey(KeyCode.A)) {
				player.mover.MoveTo(player.coords.x-1, player.coords.y);
			} else if (Input.GetKey(KeyCode.D)) {
				player.mover.MoveTo(player.coords.x+1, player.coords.y);
			}
		}
	}
}

