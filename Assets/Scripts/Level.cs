using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class Level : MonoBehaviour {
	//public Texture2D levelTex;
	public string levelFile;
	
	public string levelName;
	public int width, height;
	public float tileWidth = 2.0f, tileHeight = 2.0f;
	
	//[UnityEngine.SerializeField]
	public Tile[,] _tiles;
	//private byte _tiletype, _tilewalls;
	
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
	}
	
	void OnDrawGizmosSelected() {
		if (width > 0 && height > 0) {
			Gizmos.color = Color.grey;
			float halfwidth = tileWidth/2.0f, halfheight = tileHeight/2.0f;
			
			for (int i = 0; i <= width; i++) {
				Gizmos.DrawLine(new Vector3(i*tileWidth - halfwidth, 0, -halfheight), new Vector3(i*tileWidth - halfwidth, 0, height*tileHeight - halfheight));
			}
			for (int j = 0; j <= height; j++) {
				Gizmos.DrawLine(new Vector3(-halfwidth, 0, j*tileHeight - halfheight), new Vector3(width*tileWidth - halfwidth, 0, j*tileHeight - halfheight));
			}
			
			//Gizmos.color = Color.red;
			//Gizmos.DrawWireCube(new Vector3(selectX, 0, selectY) - offset, new Vector3(tileWidth, 2, tileHeight));
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
	}
	
	public void CreateGrid() {
		DestroyGrid();
		
		_tiles = new Tile[width, height];
		Tile T;
		
		for (int j = 0; j < height; j++) {
			for (int i = 0; i < width; i++) {
				T = Loader.LoadTile(0, new Vector3(i*tileWidth, 0, j*tileHeight));
				_tiles[i, j] = T;
			}
		}
	}
	
	public bool ModTile(int x, int y, byte newtype) {
		if (_tiles == null) {
			Debug.LogWarning("Tile grid does not exist");
			return false;
		}
		
		if (x < 0 || y < 0) {
			//Debug.LogWarning("Tried to modify negative position");
			return false;
		} else if (x >= width || y >= height) {
			//Debug.LogWarning("Tried to modify higher position than current size");
			return false;
		}
		
		if (_tiles[x, y] != null) {
			DestroyImmediate(_tiles[x, y].gameObject);
			_tiles[x, y] = null;
		}
		
		_tiles[x, y] = Loader.LoadTile(newtype, new Vector3(x*tileWidth, 0, y*tileHeight));
		
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
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tileData = col[i+j*width];
				
				_tiles[i,j] = Loader.LoadTile(tileData.r, new Vector3(i*tileWidth, 0, j*tileWidth));
			}
		}
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
