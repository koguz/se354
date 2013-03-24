using UnityEngine;
using System.Collections;

public class SimpleRider : MonoBehaviour {
	int state;
	Vector3 target;
	// Use this for initialization
	void Start () {
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == 0) {
			float d = float.MaxValue;
			target = Vector3.zero;
			GameObject[] l = GameObject.FindGameObjectsWithTag("Spawner");
			for(int i=0;i<l.Length;i++) {
				float dd = (transform.position - l[i].transform.position).magnitude;
				if (dd < d) {
					d = dd;
					target = l[i].transform.position;
					target.y = 0;
				} 
				state = 1;
			}
		}
		else {
			Vector3 dir = target - transform.position;
			dir.Normalize();
			transform.position += dir * Time.deltaTime;
			transform.LookAt(target);
		}
		
		
	}
}
