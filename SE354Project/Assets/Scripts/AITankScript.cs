using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class NotAStar {
	public Vector3 start;
	public Vector3 end;
	public bool running;
	public bool complete;
	public int[,] map;
	public ArrayList targets;
	
	public NotAStar() {
		running = false;
		complete = false;
		targets = new ArrayList();
	}
	
	public void calculatePath() {
		Debug.Log("starting thread");
		running = true;
		complete = false;
		for(int i=0;i<int.MaxValue;i++) {
			int k=i-1;
		}
		complete = true;
		Debug.Log("finished thread");
	}
}

public class AITankScript : MonoBehaviour {
	private int health = 100;
	private int armour = 50;
	private int puan = 0;
	public float disabledTime;
	private float invulTime;
	private bool invulnerable;
	private float hexTime;
	private int damageMult;
	private NotAStar astar;
	Thread myThread;
	
	public string playername;
	
	private List<Weapon> weapons;
	private int currentWeapon;
	
	// Use this for initialization
	void Start () {
		weapons = new List<Weapon>();
		weapons.Add(new Weapon()); // default is machine gun
		currentWeapon = 0;
		invulnerable = false;
		damageMult = 1;
		astar = new NotAStar();
		
	}
	
	public int getHealth() { return health; }
	public int getArmour() { return armour; }
	public int getPuan()   { return puan;   }
	
	public void ClearValues() {
		weapons.Clear();
		weapons.Add(new Weapon());
		currentWeapon = 0;
		invulnerable = false;
		damageMult = 1;
		health = 100;
		armour = 50;
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetKeyDown(KeyCode.Space)) Fire();
		//Fire ();
		if(invulnerable && (Time.time - invulTime > 10)) {
			invulnerable = false;
		}
		if(damageMult > 1 && (Time.time - hexTime > 10)) {
			damageMult = 1; 
		}
		
		if(Input.GetKeyDown(KeyCode.P)) {
			if (astar.running) {
				// check if it is complete
				if(astar.complete) {
					myThread.Join();
					astar.running = false;
				}
			} else {
				astar.start = new Vector3(0, 0, 0);
				astar.end   = new Vector3(10, 0, 10);
				astar.map   = GameObject.Find("Level").GetComponent<Level>().getMap();
				myThread = new Thread(new ThreadStart(astar.calculatePath));
				myThread.Start();
			}
		}
	}
	
	public void pickupItem(Item item) {
		health += item.health;
		armour += item.armour;
		if (health > 100) health = 100;
		if (armour > 100) armour = 100;
		if(item.invulnerability) {
			invulnerable = true;
			invulTime = Time.time;
		}
		if(item.damage > 1) {
			damageMult = item.damage;
			hexTime = Time.time;
		}
	}
	
	public void pickupItem(Weapon weapon) {
		bool iDontHaveThisWeapon = true;
		foreach (Weapon w in weapons) {
			if (w.name.Equals(weapon.name)) {
				iDontHaveThisWeapon = false;
				w.ammoCount = weapon.ammoCount;
			}
		}
		if(iDontHaveThisWeapon) {
			weapons.Add(weapon);
		}
	}
	
	public List<Weapon> getWeapons() { 
		return weapons; 
	}
	
	public void setCurrentWeapon(int index) {
		if(index >= weapons.Count) {
			Debug.LogError("Weapon can not be set - index out of range");
			return; 
		}
		currentWeapon = index;
	}
	
	public bool takeAHit(int damage) {
		if(invulnerable) return false;
		// full armour saves all hit damage, but gets the damage itself...
		health -= damage - (damage * armour/100); 
		armour -= damage;
		if(armour < 0) armour = 0;
		if(health <= 0) {
			kill ();
			return true;
		} else return false;
	}
	
	private void kill() {
		disabledTime = Time.time;
		gameObject.SetActive(false);
	}
	
	public void hitObstacle() {
		kill ();
		puan--;
	}
	
	public void increasePoints() {
		puan++;
	}
	
	public void Fire() {
		if 
			( weapons[currentWeapon].ammoCount == 0 ||
			  (Time.time - weapons[currentWeapon].lastFired) < weapons[currentWeapon].ammoPerSec
			) {
			Debug.LogWarning("Cannot fire... yet");
			return;
		}
		Vector3 direction = gameObject.transform.forward;
		GameObject mermi = (GameObject) GameObject.Instantiate(Resources.Load ("Bullet"));
		mermi.transform.position = gameObject.transform.position + (gameObject.transform.forward*0.7f);
		mermi.GetComponent<Bullet>().damage = weapons[currentWeapon].damPerAmmo * damageMult;
		mermi.GetComponent<Bullet>().parent = this;
		mermi.GetComponent<Bullet>().direction = direction;
		weapons[currentWeapon].lastFired = Time.time;
		if(!weapons[currentWeapon].name.Equals("Machine Gun")) {
			weapons[currentWeapon].ammoCount--;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.layer == 10) {
			kill ();
			puan--;
		}
	}
	
}
