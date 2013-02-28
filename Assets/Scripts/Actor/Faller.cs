using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Faller : MonoBehaviour {
	public Actor actor;
	
	public bool canFall;
	
	// Use this for initialization
	void Start () {
		actor = GetComponent<Actor>();
		actor.faller = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

