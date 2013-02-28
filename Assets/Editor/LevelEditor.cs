using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {
	bool enableEditing = true;
	string filename;
	byte selectType = 255;
	
	Plane plane = new Plane(Vector3.up, Vector3.zero);
	Ray ray;
	float outdist;
	//Vector3 selectPoint, offset = new Vector3(1, 0, 1);
	int selectX, selectY, new_selectX, new_selectY;
	Coordinates selectCoords = new Coordinates(), new_selectCoords = new Coordinates();
	bool newTile; //are we over a new tile since last frame? Prevents rapid modifying when dragging
	
	void OnEnable() {
		//Level t = (Level)target;
		
		//offset = new Vector3(Level.TileSize/2f, 0, Level.TileSize/2f);
	}
	
	void OnSceneGUI() {
		Level t = (Level)target;
		Event cur = Event.current;
		
		if (Camera.current != null) {
			//ray = Camera.current.ScreenPointToRay(cur.mousePosition);
			ray = HandleUtility.GUIPointToWorldRay(cur.mousePosition);
			
			if (plane.Raycast(ray, out outdist)) {
				//selectPoint = ray.GetPoint(outdist) + offset;
				
				//new_selectX = Mathf.FloorToInt(selectPoint.x / t.TileSize);
				//new_selectY = Mathf.FloorToInt(selectPoint.z / t.TileSize);
				new_selectCoords = Level.PositionToCoords(ray.GetPoint(outdist));
				
				if (new_selectCoords != selectCoords) {
					selectCoords = new_selectCoords;
					newTile = true;
				}
				
				//Debug.Log("Cursor Grid Position: "+selectX.ToString()+"-"+selectY.ToString());
			}
		}
		
		if (enableEditing) {
			if (cur.type == EventType.MouseDown || cur.type == EventType.MouseDrag) {
				if (cur.button == 1) {
					if (newTile) {
						t.ModTile(selectCoords.x, selectCoords.y, selectType);
						cur.Use();
						newTile = false;
					}
				}
			}
		}
		
		//cur.Use();
	}
	
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		selectType = (byte)(TileTypes)EditorGUILayout.EnumPopup("Tile:", (TileTypes)selectType);
		
		if (GUILayout.Button("Create Grid")) {
			((Level)target).CreateGrid();
		}
		if (GUILayout.Button("Destroy Grid")) {
			((Level)target).DestroyGrid();
		}
		
		filename = EditorGUILayout.TextField("Filename", filename);
		
		if (GUILayout.Button("Save Level")) {
			((Level)target).SaveLevelFile(filename);
		}
		if (GUILayout.Button("Load Level")) {
			((Level)target).LoadLevelFile(filename);
		}
	}
}
