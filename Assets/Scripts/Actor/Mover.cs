using UnityEngine;
using System.Collections;

public enum MoveType {
	Normal,
	Dig,
	Teleport,
	Ghost,
}

public class Mover : MonoBehaviour {
	public Actor actor;
	
	public bool canMove = true;
	public float moveSpeed = 10f;
	
	private bool isMoving;
	private bool releasedOrigin; //determine whether the lock has been released on the origin
	private Tile origin;
	private Tile destination;
	
	private float PathLength {
		get {return (destination.transform.position-origin.transform.position).sqrMagnitude;}
	}
	private float Remaining {
		get {return (transform.position-destination.transform.position).sqrMagnitude;}
	}
	private float Percent { //this is a squared percentage, to save cpu cycles. Keep that in mind
		get {return (1-Remaining/PathLength);}
	}
	
	void Start () {
		actor = GetComponent<Actor>();
		actor.mover = this;
		
		origin = actor.level.GetTile(actor.coords);
	}
	
	private IEnumerator Move() {
		isMoving = true;
		
		destination.occupied = true;
		releasedOrigin = false;
		
		while (Remaining > .001f) {
			if (Percent > .25 && !releasedOrigin) {
				origin.occupied = false;
				releasedOrigin = true;
			}
			
			transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, moveSpeed*Time.deltaTime);
			
			yield return null;
		}
		transform.position = destination.transform.position;
		
		isMoving = false;
		origin = destination; //prepare for the next move
		//yield return null;
	}
	
	public void MoveTo(int x, int y) {
		if (canMove) {
			if (!isMoving) {
				Debug.Log("Moving To "+x.ToString()+" "+y.ToString());
				destination = actor.level.GetTile(x,y);
				
				if (destination == null) return; //there is no tile at position
				if (destination.occupied) return; //the tile is already occupied, can't move into it
				if (!destination.canWalk) return; //the tile is not walkable
				
				actor.coords = new Coordinates(x,y);
				StartCoroutine(Move());
			}
		}
	}
	public void MoveTo(Coordinates coords) {
		MoveTo(coords.x, coords.y);
	}
}

