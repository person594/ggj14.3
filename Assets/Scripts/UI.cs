using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public int health;

	// Use this for initialization
	void Start () {
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		GUILayout.Label ("Health: "+health.ToString());
	}

	void damageHealth(int damage) {
		health -= damage;
	}
}
