using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Spawn {
	public Spawn(Vector3 p) {
		position = p;
		lastSpawn = -1;
		spawnInterval = 0;
		/*item = GameObject.CreatePrimitive(PrimitiveType.Cube);
		item.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		item.transform.position = position + (new Vector3(0, 0.3f, 0));
		item.AddComponent(typeof(Sinus));
		item.AddComponent(typeof(Spawner));
		item.tag = "Spawner";*/
	}
	public virtual void itemPicked() { 
		Debug.LogError("You MUST implement the itemPicked()"); 
	}
	public virtual void spawnItem() {
		Debug.LogError("You MUST implement the itemPicked()"); 
	}
	public Vector3 position;
	public GameObject item;
	public float lastSpawn;
	public float spawnInterval;
}

public class SMachineGun:Spawn {
	public SMachineGun(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 3;
	}
	
	public override void itemPicked() {
		GameObject.Destroy(item);
		item = null;
		lastSpawn = Time.fixedTime;
	}
	public override void spawnItem() {
		item = (GameObject) GameObject.Instantiate(Resources.Load ("MachineGun"));
		item.transform.position = position;
		item.GetComponent<Spawner>().setParent(this);
		lastSpawn = Time.fixedTime;
	}
}


public class Level : MonoBehaviour {
	public string LevelFileName = "Level00.txt";
	public List<GameObject> players;
	int[,] map;
	List<Vector3> playerSpawns;
	List<Spawn> itemSpawns;
	List<Spawn> weaponSpawns;
	int size = 0;
	/* Level File
	 * 0 -> empty
	 * 1 -> wall
	 * 2 -> machine gun spawn point
	 * 3 -> armor spawn point
	 * 4 -> player spawn point
	 * 5 -> water
	 */
	// Use this for initialization
	void Start () {
		LoadMap();
		LoadPlayers();
	}
	
	// Update is called once per frame
	void Update () {
		// loop through and spawn if necessary
		
		for(int i=0;i<weaponSpawns.Count;i++) {
			if (weaponSpawns[i].item == null && (Time.fixedTime - weaponSpawns[i].lastSpawn > weaponSpawns[i].spawnInterval)) {
				weaponSpawns[i].spawnItem();
			}
		}
		
	}
	
	void LoadPlayers() {
		if(players.Count < playerSpawns.Count) {
			Debug.LogError("More players than spawn points :(");
			return;
		}
		for(int i=0;i<players.Count;i++) {
			GameObject player = (GameObject) Instantiate(players[i], playerSpawns[i], Quaternion.identity);
			player.tag = "Player";
			player.AddComponent(typeof(SimpleRider));
		}
	}
	
	void LoadMap() {
		StreamReader dosya = File.OpenText("Assets/" + LevelFileName);
		string icerik = dosya.ReadToEnd();
		dosya.Close();
		string[] satirlar = icerik.Split("\r\n"[0]);
		size = satirlar.Length;
		map = new int[size, size];
		Material zemin = (Material) Resources.Load ("Floor", typeof(Material));
		Material duvar = (Material) Resources.Load ("Duvar", typeof(Material));
		Material su    = (Material) Resources.Load ("Su", typeof(Material));
		playerSpawns = new List<Vector3>();
		itemSpawns   = new List<Spawn>();
		weaponSpawns = new List<Spawn>();
		
		for(int i=0;i<size;i++) {
			string[] hucreler = satirlar[i].Split(" "[0]);
			for(int j=0;j<size;j++) {
				int.TryParse(hucreler[j], out map[i, j]); 
				GameObject kare = GameObject.CreatePrimitive(PrimitiveType.Cube);
				kare.transform.position = new Vector3(i, -0.04f, j);
				kare.transform.localScale = new Vector3(1.0f, 0.01f, 1.0f);
				kare.renderer.material = zemin;
				switch(map[i, j]) {
				case 0:
					break;
				case 1:
					kare.transform.localScale = new Vector3(1.0f, 1.4f, 1.0f);
					kare.renderer.material = duvar;
					break;
				case 2:
					weaponSpawns.Add(new SMachineGun(new Vector3(i, 0, j)));
					break;
				case 3:
					itemSpawns.Add(new Spawn(new Vector3(i, 0, j)));
					break;
				case 4:
					playerSpawns.Add(new Vector3(i, 0, j));
					break;
				case 5:
					kare.renderer.material = su;
					kare.AddComponent(typeof(WaterSimple));
					break;
				}
			}
		}
	}
}
