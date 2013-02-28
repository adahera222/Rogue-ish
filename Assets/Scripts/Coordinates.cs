using UnityEngine;
using System.Collections;

public class Coordinates {
	public int x,y;
	
	public Coordinates() {
		x = 0; y = 0;
	}
	public Coordinates(int x, int y) {
		this.x = x; this.y = y;
	}
	
	static public Coordinates operator+ (Coordinates lhs, Coordinates rhs) {
		return new Coordinates(lhs.x+rhs.x, lhs.y+rhs.y);
	}
	static public Coordinates operator- (Coordinates lhs, Coordinates rhs) {
		return new Coordinates(lhs.x-rhs.x, lhs.y-rhs.y);
	}
	static public bool operator== (Coordinates lhs, Coordinates rhs) {
		if (lhs.x == rhs.x && lhs.y == rhs.y) return true;
		return false;
	}
	static public bool operator!= (Coordinates lhs, Coordinates rhs) {
		return !(lhs == rhs);
	}
}

