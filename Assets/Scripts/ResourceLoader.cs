using UnityEngine;
using System.Collections;

public enum TileTypes {
	Wall = 0,
	
	//ground types
	Dirt,
	Gravel,
	Stone,
	
	//water types
	Water_Shallow,
	Water_Deep,
	
	//flavor
	Chasm,
	Swamp,
	Mushroom_Forest,
}

// The resource loader is responsible for loading and setting up all the initial pieces of the level.
//	When loading a tile, it takes the type of the tile and the walls. With this, it retrieves the proper meshes,
//		attaches them to a gameobject with a tile component, then returns it for use in the level.
//	When loading an object, it takes the type of object and the tile gameobject where it is located. It will then
//		return a newly created worldobject that is located on that tile.
public class ResourceLoader : MonoBehaviour {
	public GameObject GetTile(byte type, byte walls) {
		GameObject tileobj = (GameObject)Instantiate(Resources.Load("Tiles/Tile")); //the base gameobject which contains the tile component
		Tile tile = tileobj.GetComponent<Tile>(); //the tile component in the tile object
		GameObject mesh; //a holder for any created meshes;
		
		tile.type = type;
		
		//get the base mesh
		switch ((TileTypes)type) {
		case TileTypes.Wall:
			break;
		default:
			mesh = (GameObject)Instantiate(Resources.Load("Tiles/Default/Base"));
			mesh.transform.parent = tileobj.transform;
			mesh = null;
			break;
		}
		
		//get the wall meshes
		if ((walls & 1) > 0) {
			//north wall
		} else if ((walls & 2) > 0) {
			//north door
		} else if ((walls & 3) > 0) {
			//north grating
		}
		
		if ((walls & (1 << 2)) > 0) {
			//east wall
		}
		
		if ((walls & (1 << 4)) > 0) {
			//south wall
		}
		
		if ((walls & (1 << 6)) > 0) {
			//west wall
		}
		
		//return the completed object
		return tileobj;
	}
	
	public GameObject LoadTile(byte type, Vector3 pos) {
		GameObject _base = (GameObject)Instantiate(Resources.Load("Tiles/TileBase"), pos, Quaternion.identity);
		GameObject _temp;
		Object[] _options;
		
		type = 255; //this is an override for testing, which only loads the default tileset.
		
		switch ((TileTypes)type) {
		case TileTypes.Wall:
			return _base;
		case TileTypes.Dirt:
			break;
		case TileTypes.Gravel:
			break;
		case TileTypes.Stone:
			break;
		case TileTypes.Water_Shallow:
			break;
		case TileTypes.Water_Deep:
			break;
		case TileTypes.Chasm:
			break;
		case TileTypes.Swamp:
			break;
		case TileTypes.Mushroom_Forest:
			break;
		default:
			_options = Resources.LoadAll("Tiles/Default"); //find all default tiles
			_temp = (GameObject)Instantiate(_options[Random.Range(0, _options.GetLength(0))]); //choose one at random and load it
			_temp.transform.parent = _base.transform; //parent the new tile to the base
			_temp.transform.localPosition = Vector3.zero; //move the mesh to the position of the base
			_temp.transform.localRotation = RandomRot(); //apply a random rotation for extra variety	
			
			return _base;
		}
		
		return _base;
	}
	
	private Quaternion RandomRot() {
		return Quaternion.Euler(0f, Random.Range(0, 3)*90f, 0f);
	}
}