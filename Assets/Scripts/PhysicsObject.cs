using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PhsyicsObject 
 * This component tracks position and movement information for a GameObject
 * It knows how to ApplyForce (but some other script will call that)
 * It runs its own BounceCheck method (to keep the object in-bounds)
 * Movement happens in LateUpdate, so that some other script can call
 *   ApplyForce in its Update, and we can be sure all Forces have been
 *   applied before moving the PhysicsObject.
 */
public class PhysicsObject : MonoBehaviour {

	// These guys should be familiar by now!
	Vector3 acceleration;
	Vector3 position;
	Vector3 velocity;

	// We'll use mu for our friction multiplier.
	float mu = 0.01f;

	// You can tweak these in the inspector.
	public float mass = 1f; // higher mass - more force needed to get moving
	public float elasticity = 0.9f; // percentage of energy conserved during a wall bounce

	void Start () {
		// get our position from the game object
		position = new Vector3 (transform.position.x, transform.position.y, 0f);

		// no forces to start off
		velocity = Vector3.zero;
		acceleration = Vector3.zero;
	}

	/*
	 * ApplyForce
	 * Takes in a force and adjusts acceleration according to mass.
	 */
	public void ApplyForce(Vector3 force) {
		// from Newton's Second Law
		acceleration += force / mass;
	}

	/*
	 * BounceCheck
	 * Inverts a component of velocity when a wall is passed,
	 *   which "bounces" the PhysicsObject.
	 * 
	 * (-velocity * elasticity) lets us lose some energy to the bounce.
	 */
	void BounceCheck() {
		// calculate this once, instead of every if statement
		Vector3 screenPos = Camera.main.WorldToScreenPoint (position);

		// if we're off screen in some direction
		if (0 > screenPos.y) {
			// bottom collision
			position = Camera.main.ScreenToWorldPoint (new Vector3 (screenPos.x, 0, 0));
			velocity.y = -velocity.y * elasticity;
		} else if (Screen.height < screenPos.y) {
			// top collision
			position = Camera.main.ScreenToWorldPoint (new Vector3 (screenPos.x, Screen.height, 0));
			velocity.y = -velocity.y * elasticity;
		}

		// except, if I don't recalculate this between y and x checks,
		// it has a tendency to sink in the corners
		screenPos = Camera.main.WorldToScreenPoint (position);

		if (0 > screenPos.x) {
			// left collision
			position = Camera.main.ScreenToWorldPoint (new Vector3 (0, screenPos.y, 0));
			velocity.x = -velocity.x * elasticity; 	
		} else if (Screen.width < screenPos.x) {
			// top collision
			position = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, screenPos.y, 0));
			velocity.x = -velocity.x * elasticity;
		}

		// zero out the z coordinate once for all checks
		position.z = 0;
	}

	/*
	 * LateUpdate
	 * Runs after every other scripts' regular Update()s
	 * In this case, it performs our friction, movement, and bounce checks.
	 */
	void LateUpdate () {
		// apply friction as a force opposite the velocity
		Vector3 friction = velocity.normalized * (-1f * mu);
		ApplyForce (friction);

		// update velocity, position
		velocity += acceleration;

		// up until this point, we're thinking of our units as something-per-second
		// by multiplying by the number of seconds-per-frame...
		// ... we're left with units as something-per-frame!
		position += velocity * Time.deltaTime;

		// check for bounces against the wall
		BounceCheck ();

		// update the transform so we actually move
		transform.position = position;

		// zero out the acceleration
		// so we only apply the forces we intend to
		acceleration = Vector3.zero;
	}
}
