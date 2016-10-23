﻿using UnityEngine;
using System.Collections;

// on click the model gets a rigid body, ideally falling apart
public class BreakableBarricades : MonoBehaviour {
	private GameObject thisObject;
	public int hitpoints;
    public float radius = 15.0F;
    public float power = 110.0F;
	private Rigidbody hasRigidBody;
	private Vector3 childscale;
	// Use this for initialization


		public Rigidbody rb;
        void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
        }
		void Start() {
            rb = this.GetComponent<Rigidbody>();
            if (rb != null) { EnableRagdoll(); }
			
		}

		void EnableRagdoll() {
			rb.isKinematic = false;
			rb.detectCollisions = true;
		}
		void DisableRagdoll() {
			rb.isKinematic = true;
			rb.detectCollisions = false;
		}

	void OnCollisionEnter(Collision collision) {
        GameObject colliderRootObject = ProjectileBehaviour.getChildRootObject(collision.collider.gameObject);
       // Debug.Log(colliderRootObject.name);
        string colliderTag = colliderRootObject.tag;
        if (colliderTag == "Zombie"){ explodeInstantly();}

		/*if (colliderTag == "Human" || colliderTag == "Zombie" || colliderTag == "Player" || colliderTag == "Projectile") {
			//explodeInstantly();
		}*/
	}
		

	/*void OnTriggerEnter(Collider other) {
        GameObject colliderRootObject = ProjectileBehaviour.getChildRootObject(other.gameObject);
        string colliderTag = colliderRootObject.tag;
        Debug.Log("OnTriggerEnter = " + colliderRootObject.name);
        if (colliderTag == "Human" || colliderTag == "Zombie" || colliderTag == "Player" || colliderTag == "Projectile")
        {
          //  Debug.Log(other.tag);
			hitpoints+= -2; // remove hit points
			if (hitpoints <= 0){
			hasRigidBody = this.GetComponent<Rigidbody>();
			//turn off the rigid body of the main object
			if (hasRigidBody != null) {
			DisableRagdoll();
			}		
			}

	}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Zombie")
		{
            hitpoints += -2; // remove hit points
            if (hitpoints <= 0)
            {
                hasRigidBody = this.GetComponent<Rigidbody>();
                //turn off the rigid body of the main object
                if (hasRigidBody != null)
                {
                    DisableRagdoll();
                  
                    explodeInstantly();
                }
            }
            
		}		
	}
*/
public	void explodeInstantly(){
		hitpoints += -100; // remove hit points
        this.tag = "Untagged";
		if (hitpoints <= 0) {
			hasRigidBody = this.GetComponent<Rigidbody> ();
			if (hasRigidBody != null) {
				DisableRagdoll ();
			}
			// iterate over child objects and give them rigidbody
			foreach (Transform child in transform) {
                child.gameObject.tag = "Wood";
				hasRigidBody = child.gameObject.GetComponent<Rigidbody> ();
				// break the object apart
				if (hasRigidBody == null) {
					Vector3 childscale = child.gameObject.transform.localScale;
					
					childscale.x = childscale.x * 0.45F;
					childscale.y = childscale.y * 0.45F;
					childscale.z = childscale.z * 0.75F;
					child.gameObject.transform.localScale = new Vector3 (childscale.x, childscale.y, childscale.z);
					Rigidbody gameObjectsRigidBody = child.gameObject.AddComponent<Rigidbody> (); // Add the rigidbody.
					gameObjectsRigidBody.mass = 1; // Set the GO's mass to 5 via the Rigidbody.
					// Rigidbody rb = hit.GetComponent<Rigidbody>();
					
					if (gameObjectsRigidBody != null) {
						gameObjectsRigidBody.AddExplosionForce (power, child.gameObject.transform.position, radius, 3.0F);

						
					}
				}
			}
		}
	}

	/*void OnMouseDown() {
		for (int i = 1; i <= 5; i++) {
			hasRigidBody = theseObjects [i].GetComponent<Rigidbody>();
			if(hasRigidBody == null)
			{
				Rigidbody gameObjectsRigidBody = theseObjects[i].AddComponent<Rigidbody> (); // Add the rigidbody.
			gameObjectsRigidBody.mass = 1; // Set the GO's mass to 5 via the Rigidbody.
			}
		}*/
		


	
		

	// end ClickedRigid.cs
}
