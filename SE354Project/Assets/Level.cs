using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Spawn {
	public Spawn(Vector3 p) {
		position = p;
		lastSpawn = -1;
		spawnInterval = 0;
	}
	public virtual void itemPicked(AITankScript tank) { 
		GameObject.Destroy(itemMesh);
		itemMesh = null;
		lastSpawn = Time.time;
		
	}
	public virtual void spawnItem() {
		itemMesh.transform.position = position;
		itemMesh.GetComponent<Spawner>().setParent(this);
		lastSpawn = Time.time;
	}
	public Vector3 position;
	public GameObject itemMesh;
	public float lastSpawn;
	public float spawnInterval;
	public Weapon weapon = null;
	public Item item = null;
}

public class HeavyMachineGun:Spawn {
	public HeavyMachineGun(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 10;
		weapon = new Weapon();
		weapon.name = "Heavy Machine Gun";
		weapon.ammoCount = 30;
		weapon.ammoPerSec = 0.3f;
		weapon.damPerAmmo = 2;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (weapon != null) tank.pickupItem(weapon);
	}
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("MachineGun"));
		base.spawnItem();
	}
}

public class Grenade:Spawn {
	public Grenade(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 10;
		weapon = new Weapon();
		weapon.name = "Grenade";
		weapon.ammoCount = 3;
		weapon.ammoPerSec = 2;
		weapon.damPerAmmo = 15;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (weapon != null) tank.pickupItem(weapon);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Grenade"));
		base.spawnItem();
	}
}

public class Missile:Spawn {
	public Missile(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 15;
		weapon = new Weapon();
		weapon.name = "Missile";
		weapon.ammoCount = 3;
		weapon.ammoPerSec = 3;
		weapon.damPerAmmo = 30;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (weapon != null) tank.pickupItem(weapon);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Missile"));
		base.spawnItem();
	}
}

public class Cannon:Spawn {
	public Cannon(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 30;
		weapon = new Weapon();
		weapon.name = "Cannon";
		weapon.ammoCount = 2;
		weapon.ammoPerSec = 5;
		weapon.damPerAmmo = 40;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (weapon != null) tank.pickupItem(weapon);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Cannon"));
		base.spawnItem();
	}
}

public class DeathBringer:Spawn {
	public DeathBringer(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 120;
		weapon = new Weapon();
		weapon.name = "DeathBringer";
		weapon.ammoCount = 1;
		weapon.ammoPerSec = 5;
		weapon.damPerAmmo = 80;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (weapon != null) tank.pickupItem(weapon);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("DeathBringer"));
		base.spawnItem();
	}
}

public class RepairKit:Spawn {
	public RepairKit(Vector3 p):base(p) {
		spawnItem(); spawnInterval = 30; // ?
		item = new Item();
		item.itemName = "Repair Kit";
		item.health = 10;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (item!=null) tank.pickupItem(item);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Wrench"));
		base.spawnItem();
	}
}

public class Armour:Spawn {
	public Armour(Vector3 p):base(p) {
		spawnItem(); spawnInterval = 30; // ?
		item = new Item();
		item.itemName = "Armour";
		item.armour = 10;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (item!=null) tank.pickupItem(item);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Armour"));
		base.spawnItem();
	}
}

public class HexDamage:Spawn {
	public HexDamage(Vector3 p):base(p) {
		spawnItem(); spawnInterval = 180; // ?
		item = new Item();
		item.itemName = "Hex Damage";
		item.damage = 6;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (item!=null) tank.pickupItem(item);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("HexDamage"));
		base.spawnItem();
	}
}

public class Invulnerability:Spawn {
	public Invulnerability(Vector3 p):base(p) {
		spawnItem(); spawnInterval = 120; // ?
		item = new Item();
		item.itemName = "Invulnerability";
		item.invulnerability = true;
	}
	
	public override void itemPicked(AITankScript tank) {
		base.itemPicked(tank);
		if (item!=null) tank.pickupItem(item);
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Invulnerability"));
		base.spawnItem();
	}
}

public class Level : MonoBehaviour {
	public string LevelFileName = "Level00.txt";
	public List<GameObject> players;
	GameObject[] ps;
	private int sure;
	int[,] map;
	List<Vector3> playerSpawns;
	List<Spawn> itemSpawns;
	List<Spawn> weaponSpawns;
	int size = 0;
	/* Level File
	 * 0 -> empty
	 * 1 -> wall
	 * 2 -> machine gun spawn point
	 * 3 -> repair kit spawn point
	 * 4 -> player spawn point
	 * 5 -> water
	 * 6 -> armour spawn point
	 * 7 -> bfg spawn point
	 * 8 -> cannon spawn point
	 * 9 -> grenade spawn point
	 * 10 -> hex damage spawn point
	 * 11 -> invulnerability spawn point
	 * 12 -> missile spawn point
	 */
	// Use this for initialization
	void Start () {
		LoadMap();
		LoadPlayers();
		ps = GameObject.FindGameObjectsWithTag("Player");
		sure = 310;
	}
	
	// Update is called once per frame
	void Update () {
		if (sure - Time.time < 0) {
			Time.timeScale = 0;
		}
		
		// loop through and spawn if necessary
		for(int i=0;i<weaponSpawns.Count;i++) {
			if (weaponSpawns[i].itemMesh == null && (Time.time - weaponSpawns[i].lastSpawn > weaponSpawns[i].spawnInterval)) {
				weaponSpawns[i].spawnItem();
			}
		}
		
		for(int i=0;i<itemSpawns.Count;i++) {
			if (itemSpawns[i].itemMesh == null && (Time.time - itemSpawns[i].lastSpawn > itemSpawns[i].spawnInterval)) {
				itemSpawns[i].spawnItem();
			}
		}
		
		for(int i=0;i<ps.Length;i++) {
			if(!ps[i].activeSelf && 
				(Time.time - ps[i].GetComponent<AITankScript>().disabledTime) > 5) {
				ps[i].transform.position = playerSpawns[Random.Range(0, playerSpawns.Count-1)];
				ps[i].GetComponent<AITankScript>().ClearValues();
				ps[i].SetActive(true);
			} 
		}
	}
	
	void OnGUI() {
		string display = "";
		for(int i=0;i<ps.Length;i++) {
			AITankScript temp = ps[i].GetComponent<AITankScript>();
			display += temp.playername + " (" + temp.getHealth() + ", " + temp.getArmour() + "): " + temp.getPuan() + "\n";
		}
		display += "Time: " + (sure-Time.time) + "\n";
		GUI.Label(new Rect(5, 5, 200, 400), display);
	}
	
	public int[,] getMap() { return map; }
	
	void LoadPlayers() {
		if(players.Count < playerSpawns.Count) {
			Debug.LogError("More players than spawn points :(");
			return;
		}
		for(int i=0;i<players.Count;i++) {
			GameObject player = (GameObject) Instantiate(players[i], playerSpawns[i], Quaternion.identity);
			player.tag = "Player";
			//player.GetComponent<AITankScript>().playername = "Player" + i;
		}
	}
	
	void LoadMap() {
		StreamReader dosya = File.OpenText("Assets/" + LevelFileName);
		string icerik = dosya.ReadToEnd();
		dosya.Close();
		string[] satirlar = icerik.Split("\n"[0]);
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
					GameObject t1 = new GameObject("walltrigger");
					t1.transform.position = new Vector3(i, -0.04f, j);
					t1.transform.localScale = new Vector3(0.25f, 1, 0.25f);
					t1.AddComponent(typeof(BoxCollider));
					t1.collider.isTrigger = true;
					t1.layer = 11;
					t1.AddComponent(typeof(Obstacle));
					break;
				case 2:
					weaponSpawns.Add(new HeavyMachineGun(new Vector3(i, 0, j)));
					break;
				case 3:
					itemSpawns.Add (new RepairKit(new Vector3(i, 0, j)));
					break;
				case 4:
					playerSpawns.Add(new Vector3(i, 0, j));
					break;
				case 5:
					kare.renderer.material = su;
					kare.AddComponent(typeof(WaterSimple));
					
					BoxCollider t12 = (BoxCollider)kare.collider;
					t12.isTrigger = true;
					t12.size = new Vector3(1, 80, 1);
					t12.center = new Vector3(0, 30, 0);
					
					kare.layer = 4; // water does not collide with bullets...
					
					GameObject t2 = new GameObject("watertrigger");
					t2.transform.position = new Vector3(i, -0.04f, j);
					t2.transform.localScale = new Vector3(0.25f, 1, 0.25f);
					t2.AddComponent(typeof(BoxCollider));
					t2.collider.isTrigger = true;
					t2.layer = 11;
					t2.AddComponent(typeof(Obstacle));
					break;
				case 6:
					itemSpawns.Add (new Armour(new Vector3(i, 0, j)));
					break;
				case 7:
					weaponSpawns.Add (new DeathBringer(new Vector3(i, 0, j)));
					break;
				case 8:
					weaponSpawns.Add (new Cannon(new Vector3(i, 0, j)));
					break;
				case 9:
					weaponSpawns.Add (new Grenade(new Vector3(i, 0, j)));
					break;
				case 10:
					itemSpawns.Add (new HexDamage(new Vector3(i, 0, j)));
					break;
				case 11:
					itemSpawns.Add (new Invulnerability(new Vector3(i, 0, j)));
					break;
				case 12:
					weaponSpawns.Add (new Missile(new Vector3(i, 0, j)));
					break;
				}
			}
		}
	}
}
