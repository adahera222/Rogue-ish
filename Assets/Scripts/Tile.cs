using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public byte type; //the type of tile
	public byte wall; //the walls surrounding this tile ( N,E,S,W : 00-open, 01-wall, 02-door, 03-grate )
	public byte data; //data about this tile, including rotation, visibility, and others;
	
	public bool canWalk = true; //can walking actors move through this tile?
	public bool canFly = true; //can flying actors move through this tile?
	public bool canDig = true; //can digging actors move through this tile?
	public bool canSee = true; //can actors see through this tile?
	
	public bool occupied = false;
	
	//basic floor: walkable true, opaque false, filled false
	//chasm: walkable false, opaque false, filled false
	//wall: walkable false, opaque true, filled true
	//mist: walkable true, opaque true, filled false
	
	// Use this for initialization
	void Start () {
		//
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
