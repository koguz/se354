using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		// Destroy(gameObject);
		if(other.gameObject.layer == 10) {
			AITankScript tank = other.GetComponent<AITankScript>();
			tank.hitObstacle();
			//if(enemy.takeAHit(damage)) parent.increasePoints();
		}
	}
}
