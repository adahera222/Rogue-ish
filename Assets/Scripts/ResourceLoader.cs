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
	
	//flavor, special types
	Chasm,
	Swamp,
	Mushroom_Forest,
	
	Default = 255,
}

enum ResourceTypes {
	Floor,
	Wall,
}

// The resource loader is responsible for loading and setting up all the initial pieces of the level.
//	When loading a tile, it takes the type of the tile and the walls. With this, it retrieves the proper meshes,
//		attaches them to a gameobject with a tile component, then returns it for use in the level.
//	When loading an object, it takes the type of object and the tile gameobject where it is located. It will then
//		return a newly created worldobject that is located on that tile.
public class ResourceLoader : MonoBehaviour {
	
	public Tile LoadTile(byte type, Vector3 pos) {
		GameObject _base = (GameObject)Instantiate(Resources.Load("Tiles/TileBase"), pos, Quaternion.identity);
		Tile _tile = _base.GetComponent<Tile>();
		GameObject _temp;
		Object[] _options;
		
		//type = 255; //this is an override for testing, which only loads the default tileset.
		_tile.type = type;
		
		switch ((TileTypes)type) {
		case TileTypes.Wall:
			//TODO: Create a "shadow box" that is applied to the wall tiles.
			
			break;
		case TileTypes.Dirt:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		case TileTypes.Gravel:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		case TileTypes.Stone:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		case TileTypes.Water_Shallow:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		case TileTypes.Water_Deep:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		case TileTypes.Chasm:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		case TileTypes.Swamp:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		case TileTypes.Mushroom_Forest:
			Debug.LogWarning("Tile Type " + type.ToString() + " has not been implemented yet");
			break;
		default:
			_options = Resources.LoadAll("Tiles/Default", typeof(GameObject)); //find all default tiles
			_temp = (GameObject)Instantiate(_options[Random.Range(0, _options.GetLength(0))]); //choose one at random and load it
			ParentModel(_base.transform, _temp.transform);
			_temp.transform.localRotation = RandomRot(); //apply a random rotation for extra variety
			break;
		}
		
		return _tile;
	}
	
	private void ParentModel(Transform p, Transform c) {
		c.parent = p; //parent the model
		c.localPosition = Vector3.zero; //move it to the same position as the parent
	}
	
	private Quaternion RandomRot() {
		return Quaternion.Euler(0f, Random.Range(0, 4)*90f, 0f); //integer random range is exclusive of the max, so put a 4 there
	}
}