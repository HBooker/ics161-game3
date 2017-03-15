using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBoard : MonoBehaviour {
	public GameObject tile_prefab;
	public GameObject spawn_tile_prefab1;
	public GameObject spawn_tile_prefab2;
	public int width;
	public int length;
	public static bool build_enable;
	int x_scale = 10;
	int z_scale = 10;
	public static int num_boards;
	public static GameObject[] all_boards;

	// Use this for initialization
	void Start () {
		width = 8; //default width of board;
		length = 12; //default length of board;
		build_enable = false; //start as false until told to build map again
		num_boards = width * length;
		all_boards = new GameObject[num_boards];
	}
	
	// Update is called once per frame
	void Update () {
		if (build_enable) {
			Debug.Log ("starting build");
			int i = 0;
			build_enable = false;
			bool need_spawntiles = true;
			Debug.Log (width.ToString()+ ", " + length.ToString());
			for (int x = 0; x < width/2; x++) {
				for (int z = 0; z < length / 2; z++) {
					if (need_spawntiles && x == width / 2 - 2 && z == length / 2 - 2) {
						all_boards[i++] = Instantiate (spawn_tile_prefab1, new Vector3 ((5 + (x_scale * x)), 0, (5 + (z_scale * z))), Quaternion.identity);
						all_boards[i++] = Instantiate (spawn_tile_prefab1, new Vector3 (-1 * (5 + (x_scale * x)), 0, (5 + (z_scale * z))), Quaternion.identity);
						all_boards[i++] = Instantiate (spawn_tile_prefab2, new Vector3 ((5 + (x_scale * x)), 0, -1 * (5 + (z_scale * z))), Quaternion.identity);
						all_boards[i++] = Instantiate (spawn_tile_prefab2, new Vector3 (-1 * (5 + (x_scale * x)), 0, -1 * (5 + (z_scale * z))), Quaternion.identity);
						need_spawntiles = false;
					} else {
						all_boards[i++] = Instantiate (tile_prefab, new Vector3 ((5 + (x_scale * x)), 0, (5 + (z_scale * z))), Quaternion.identity);
						all_boards[i++] = Instantiate (tile_prefab, new Vector3 (-1 * (5 + (x_scale * x)), 0, (5 + (z_scale * z))), Quaternion.identity);
						all_boards[i++] = Instantiate (tile_prefab, new Vector3 ((5 + (x_scale * x)), 0, -1 * (5 + (z_scale * z))), Quaternion.identity);
						all_boards[i++] = Instantiate (tile_prefab, new Vector3 (-1 * (5 + (x_scale * x)), 0, -1 * (5 + (z_scale * z))), Quaternion.identity);
					}
				}
			}
			Debug.Log ("finished build");
		}


	}
	public void enable_build() {
		build_enable = true;
	}
}
