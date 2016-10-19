using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class PlayerScript : MonoBehaviour {
    
    //The speed of the ship
    public Vector2 speed = new Vector2(50, 50);

    public string controllerIP { get; set; }
    public bool IsPlayer2 = false;

    //Store the movement, the rotation and the component
    private Vector2 movement;
    private float rotation;
    private Rigidbody2D rigidbodyComponent;

    private DisastroidController controller;
    private ControllerMovement initialState;
    private bool initialStateSet = false;


    // Use this for initialization
    void Start () {
        controller = (DisastroidController)NetworkInputManager.ConnectedControllers[controllerIP];
    }
	
	// Update is called once per frame
	void Update () {

        //BOF
        if (!initialStateSet)
        {
            initialState = new ControllerMovement();
            initialState.X = 0;// controller.Rotation.X;
            initialState.Y = 0;//controller.Rotation.Y;
            initialState.Z = 0;//controller.Rotation.Z;
            initialStateSet = true;
        }
        
        
        float inputX = ((controller.Rotation.Y - initialState.Y)%90)/-1000;
        float inputY = ((controller.Rotation.X - initialState.X)%90)/-1000;
        

        rotation = controller.Rotation.Y;

        //Movement per direction
        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);

        //Manage Fire Command
        if(controller.FireCommandRegistered)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if(weapon != null)
            {
                weapon.Attack(false);
            }
        }

        //Manage Charge Command
        if (controller.ChargeCommandRegistered)
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
