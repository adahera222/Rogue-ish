using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public Actor actor;
	
	private int maxHP;
	private int curHP;
	public bool canHeal;
	
	void Start() {
		actor = GetComponent<Actor>();
		actor.health = this;
	}
	
	public int Current {
		get {return curHP;}
	}
	
	public int Max {
		get {return maxHP;}
		set {maxHP = value;
			if (curHP > maxHP) curHP = maxHP;
		}
	}
	
	public float Percent {
		get { return (float)curHP/(float)maxHP;}
	}
	
	public int Heal(int amount) {
		int remainder = 0;
		
		if (canHeal) {
			curHP += amount;
			
			if (curHP > maxHP) {
				remainder = curHP-maxHP;
				curHP = maxHP;
			}
		} else {
			remainder = amount;
		}
		
		return remainder;
	}
}

