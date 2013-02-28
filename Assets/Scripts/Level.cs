using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class Level : MonoBehaviour {
	//public Texture2D levelTex;
	public string levelFile;
	
	public string levelName;
	public int width, height;
	
	static public float TileSize = 2.0f;
	static public Coordinates PositionToCoords(Vector3 pos) {
		Vector3 adjPos = pos/TileSize + new Vector3(TileSize/2f, 0, TileSize/2f);
		
		return new Coordinates(Mathf.FloorToInt(adjPos.x), Mathf.FloorToInt(adjPos.z));
	}
	static public Vector3 CoordsToPosition(Coordinates coords) {
		return new Vector3(coords.x*TileSize, 0, coords.y*TileSize);
	}
	
	[UnityEngine.SerializeField]
	public Tile[] _tiles;
	
	public Tile GetTile(int x, int y) {
		int index = TileIndex(x,y);
		if (index >= 0) {
			return _tiles[index];
		}
		return null;
	}
	public Tile GetTile(Coordinates co) {
		return GetTile (co.x, co.y);
	}
	private int TileIndex(int x, int y) {
		//get the linear index for the 2d tile. Return -1 if position is not valid
		int index = y*width+x;
		
		if (_tiles == null) return -1;
		
		if (index < 0 || index >= _tiles.GetLength(0)) {
			return -1;
		} else {
			return index;
		}
	}
	
	private ResourceLoader _loader;
	public ResourceLoader Loader {
		get {
			if (_loader == null) {
				_loader = GetComponent<ResourceLoader>();
			}
			return _loader;
		}
	}
	
	// Use this for initialization
	void Start () {
		_loader = GetComponent<ResourceLoader>();
		
		if (!string.IsNullOrEmpty(levelFile)) {
			LoadLevelFile(levelFile);
		} else {
			Debug.Log("No level to load");
		}
		
		if (_tiles == null) {
			Debug.Log("No Level is loaded");
		}
	}
	
	void OnDrawGizmosSelected() {
		if (width > 0 && height > 0) {
			Gizmos.color = Color.grey;
			float halfwidth = TileSize/2.0f, halfheight = TileSize/2.0f;
			
			for (int i = 0; i <= width; i++) {
				Gizmos.DrawLine(new Vector3(i*TileSize - halfwidth, 0, -halfheight), new Vector3(i*TileSize - halfwidth, 0, height*TileSize - halfheight));
			}
			for (int j = 0; j <= height; j++) {
				Gizmos.DrawLine(new Vector3(-halfwidth, 0, j*TileSize - halfheight), new Vector3(width*TileSize - halfwidth, 0, j*TileSize - halfheight));
			}
			
			//Gizmos.color = Color.red;
			//Gizmos.DrawWireCube(new Vector3(selectX, 0, selectY) - offset, new Vector3(TileSize, 2, TileSize));
		}
	}
	
	public void DestroyGrid() {
		if (_tiles != null) {
			foreach (Tile t in _tiles) {
				if (t != null) {
					DestroyImmediate(t.gameObject);
				}
			}
		}
		
		_tiles = null;
	}
	
	public void CreateGrid() {
		DestroyGrid();
		
		_tiles = new Tile[width*height];
		Tile T;
		
		for (int j = 0; j < height; j++) {
			for (int i = 0; i < width; i++) {
				T = Loader.LoadTile(0, new Vector3(i*TileSize, 0, j*TileSize));
				_tiles[TileIndex(i,j)] = T;
			}
		}
	}
	
	public bool ModTile(int x, int y, byte newtype) {
		int index = TileIndex(x,y);
		
		if (index < 0) {
			Debug.Log("Invalid Index");
			return false;
		}
		
		if (_tiles[index] != null) {
			DestroyImmediate(_tiles[index].gameObject);
			_tiles[index] = null;
		}
		
		_tiles[index] = Loader.LoadTile(newtype, new Vector3(x*TileSize, 0, y*TileSize));
		
		return true;
	}
	
	public void LoadLevelFile(string filename) {
		//TODO: Implement proper file loading procedure
		
		//get the level file
		WWW file = new WWW("file://"+filename);
		
		//wait for it to finish loading
		while (!file.isDone) {
			Debug.Log("Waiting to load file...");
		}
		
		//extract all necessary information, create some containers
		Texture2D tileTex = file.texture;
		width = tileTex.width;
		height = tileTex.height;
		Color32[] col = tileTex.GetPixels32();
		Color32 tileData;
		
		//we can now create a new grid
		CreateGrid();
		
		//move through each pixel and create the proper tiles
		//TODO Convert this to use tile index rather than x,y coords
//		for (int i = 0; i < width; i++) {
//			for (int j = 0; j < height; j++) {
//				tileData = col[i+j*width];
//				
//				_tiles[i,j] = Loader.LoadTile(tileData.r, new Vector3(i*TileSize, 0, j*TileSize));
//			}
//		}
	}
	
	public void SaveLevelFile(string filename) {
		//TODO: Implement proper file saving procedure
		
		//Create a texture with all the relevant tile data
		Texture2D tileTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		tileTex.filterMode = FilterMode.Point;
		tileTex.wrapMode = TextureWrapMode.Clamp;
		
		Color32[] col = new Color32[width*height]; //the color which holds all data
		
		int iter = 0;
		foreach (Tile t in _tiles) {
			col[iter] = new Color32(t.type, t.wall, t.data, 0);
			
			iter++;
		}
		
		tileTex.SetPixels32(col);
		tileTex.Apply();
		
		File.WriteAllBytes(filename, tileTex.EncodeToPNG());
	}
}
