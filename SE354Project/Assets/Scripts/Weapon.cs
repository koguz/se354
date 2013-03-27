using UnityEngine;
using System.Collections;

public class Weapon { // : MonoBehaviour {
	public string name = "Machine Gun";
	public int ammoCount = int.MaxValue;
	public float ammoPerSec= 1;
	public int damPerAmmo= 90;
	public float lastFired = Time.time;
}






