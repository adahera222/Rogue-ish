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
		
	private Tile[,] _tiles;
	//private byte _tiletype, _tilewalls;
	
	private ResourceLoader loader;
	
	// Use this for initialization
	void Start () {
		loader = GetComponent<ResourceLoader>();
		
		if (!string.IsNullOrEmpty(levelFile)) {
			LoadLevelFile(levelFile);
		} else {
			Debug.Log("No level to load");
		}
	}
	
	void OnDrawGizmosSelected() {
		if (width > 0 && height > 0) {
			Gizmos.color = Color.blue;
			float halfwidth = tileWidth/2.0f, halfheight = tileHeight/2.0f;
			
			for (int i = 0; i <= width; i++) {
				Gizmos.DrawLine(new Vector3(i*tileWidth - halfwidth, 0, -halfheight), new Vector3(i*tileWidth - halfwidth, 0, height*tileHeight - halfheight));
			}
			for (int j = 0; j <= height; j++) {
				Gizmos.DrawLine(new Vector3(-halfwidth, 0, j*tileHeight - halfheight), new Vector3(width*tileWidth - halfwidth, 0, j*tileHeight - halfheight));
			}
		}
	}
	
	void LoadLevelFile(string filename) {
		/* JSON Documents are currently as follows:
		 * 	{
		 * 		"Name":"LevelName",
		 * 		"Width":10,
		 * 		"Height":10,
		 * 		"Tiles": [0,0,0,0,
		 * 				  0,0,0,0,
		 * 				  0,0,0,0,
		 * 				  0,0,0,0]
		 * }
		 */
		
		StreamReader reader = new StreamReader(filename);
		string JSONstring = reader.ReadToEnd();
		JSON file = JSON.fromString(JSONstring);
		
		levelName = file._get("Name").string_value;
		width = (int)file._get("Width").number_value;
		height = (int)file._get("Height").number_value;
		
		_tiles = new Tile[width, height];
		int i = 0, j = 0;
		
		foreach (JSON tile in file._get("Tiles").values) {
			_tiles[i,j] = loader.LoadTile((byte)tile.number_value, new Vector3(i*tileWidth, 0, j*tileHeight)).GetComponent<Tile>();
			
			if (i >= width) {
				if (j >= height) {
					break; //we have reached the end, go ahead and break early
				} else {
					j++; i = 0;
				}
			} else {
				i++;
			}
		}
	}
	
	void SaveLevelFile(string filename) {
		//This is most likely the wrong way to do it, but for now it compiles.
		//TODO Implement proper file writing procedure
		JSON json = new JSON();
		JSON tilejson = JSON._array();
		
		json._set("Name", levelName)._set("Width", (double)width)._set("Height", (double)height);
		foreach (Tile T in _tiles) {
			tilejson._push((double)(T.type));
		}
		json._set("Tiles", tilejson);
		
		string jsonstring = json.stringify();
		
		File.WriteAllText(filename, jsonstring);
	}
}
