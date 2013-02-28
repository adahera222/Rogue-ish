using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	public bool blocking;
	
	public Coordinates coords;
	
	static public Level _level;
	public Level level {
		get {return Actor._level;}
	}
	
	public GameObject model;
	public Health health;
	public Mover mover;
	public Faller faller;
	
	// Use this for initialization
	void Start () {
		if (level == null) {
			_level = (Level)FindObjectOfType(typeof(Level));
		}
		
		coords = Level.PositionToCoords(transform.position);
		transform.position = Level.CoordsToPosition(coords);
	}
}

