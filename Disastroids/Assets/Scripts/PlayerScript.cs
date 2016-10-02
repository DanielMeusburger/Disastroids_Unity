using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    
    //The speed of the ship
    public Vector2 speed = new Vector2(50, 50);

    //Store the movement, the rotation and the component
    private Vector2 movement;
    private float rotation;
    private Rigidbody2D rigidbodyComponent;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float inputX = NetworkInputManager.Movement.X;
        float inputY = NetworkInputManager.Movement.Y;

        rotation = NetworkInputManager.Rotation.Z;

        //Movement per direction
        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);

        if(NetworkInputManager.FireCommandRegistered)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if(weapon != null)
            {
                weapon.Attack(false);
            }
            NetworkInputManager.FireCommandRegistered = false;
        }
    }

    void FixedUpdate()
    {
        //Get the component and store the reference
        if (rigidbodyComponent == null)
        {
            rigidbodyComponent = GetComponent<Rigidbody2D>();
        }

        //Move the game object
        rigidbodyComponent.velocity = movement;
        rigidbodyComponent.rotation = rotation;
    }
}
