using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public byte type;
	public bool walkable;
	public bool opaque;
	public bool filled;
	
	//basic floor: walkable true, opaque false, filled false
	//chasm: walkable false, opaque false, filled false
	//wall: walkable false, opaque true, filled true
	//mist: walkable true, opaque true, filled false
	
	// Use this for initialization
	void Start () {
		//tiles start walkable, non-opaque, and non-filled
		walkable = true;
		opaque = false;
		filled = false;
	}
	
	public void Draw() {
		foreach (Transform child in transform) {
			child.renderer.enabled = true;
		}
	}
	
	public void Clear() {
		foreach (Transform child in transform) {
			child.renderer.enabled = false;
		}
	}
	
}
