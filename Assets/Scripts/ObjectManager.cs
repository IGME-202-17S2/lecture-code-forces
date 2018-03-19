using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ObjectManager 
 * This component tracks collections of PhysicsObjects
 * It also orchestrates the forces and applies them.
 */
public class ObjectManager : MonoBehaviour {

	// wind and gravity are constant throughout the scene
	// so we'll track them here (instead of on every object)
	Vector3 wind;
	Vector3 gravity;

	// for PhysicsObjects that already exist in the scene
	// (populated by dragging them into the inspector)
	public List<PhysicsObject> scenePO;

	// for PhysicsObjects generated at runtime
	// (populated with the GeneratePOs() call
	List<PhysicsObject> generatedPO = new List<PhysicsObject>();

	// a reference to the prefab used for PhysicsObjects
	// (also populated in the inspector)
	public GameObject poPrefab;


	void Start () {
		// initialize forces
		wind = new Vector3 (0.01f, 0, 0);
		gravity = new Vector3 (0, -0.3f, 0);

		// generate additional PhysicsObjects
		GeneratePOs ();
	}

	void GeneratePOs() {
		// this is a very basic script to generate new instances of the PhysicsSphere
		for (int i = 0; i < 3; i++) {
			GameObject go = Instantiate (poPrefab, new Vector3 (i * 1.8f - 1.8f, -1.8f, 0), Quaternion.identity);
			// notice that, instead of storing the GameObject, I'm storing the PhysicsObject component
			// that way, I don't have to call obj.GetComponent<PhysicsObject> (); in every Update()
			PhysicsObject po = go.GetComponent<PhysicsObject> ();
			// give the new objects a random mass
			po.mass = Random.Range (1f, 20f);
			// track them in the list!
			generatedPO.Add (po);
		}
	}

	void SimulateObject (PhysicsObject po) {
		// apply wind and gravity to the given po
		po.ApplyForce (wind);
		po.ApplyForce (gravity * po.mass);
		// notice I multiplied by mass here - because gravity doesn't care about mass!
	}
		
	void Update () {
		// foreach certainly works...
		// but I'll bet you ran into some difficulty
		// as soon as you tried to delete/remove things
		foreach (PhysicsObject po in scenePO) {
			SimulateObject (po);
		}

		// instead...

		// a for loop that counts down works better
		// especially if you find yourself needing to remove things
		// from the collection while the loop is still running
		for (int i = generatedPO.Count - 1; i >= 0; i--) {
			SimulateObject (generatedPO [i]);
		}
	}
}
