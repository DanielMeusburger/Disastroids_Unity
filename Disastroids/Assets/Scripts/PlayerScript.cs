using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    
    //The speed of the ship
    public Vector2 speed = new Vector2(50, 50);

    public string controllerIP { get; set; }
    public bool IsPlayer2 = false;

    //Store the movement, the rotation and the component
    private Vector2 movement;
    private float rotation;
    private Rigidbody2D rigidbodyComponent;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        
        float inputX = (NetworkInputManager.ConnectedControllers[controllerIP].Rotation.Y%90)/-1000;
        float inputY = (NetworkInputManager.ConnectedControllers[controllerIP].Rotation.X%90)/-1000;

        rotation = NetworkInputManager.ConnectedControllers[controllerIP].Rotation.Y;

        //Movement per direction
        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);

        //Manage Fire Command
        if(NetworkInputManager.ConnectedControllers[controllerIP].FireCommandRegistered)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if(weapon != null)
            {
                weapon.Attack(false);
            }
            NetworkInputManager.ConnectedControllers[controllerIP].FireCommandRegistered = false;
        }

        //Manage Charge Command
        if (NetworkInputManager.ConnectedControllers[controllerIP].ChargeCommandRegistered)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if(weapon != null)
            {
                weapon.ChargeSuperShot();
            }
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
