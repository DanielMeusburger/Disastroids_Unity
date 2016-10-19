using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
    
    //Projectile prefab for shooting
    public Transform shotPrefab;
    
    //Cooldown in seconds between two shots
    public float shootingRate = 0.25f;
    private float shootCooldown;

    public int superShotNeededCharge;
    private int superShotChargeStatus = 0;

    void Start()
    {
        shootCooldown = 0f;
    }

    void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    //Create a new projectile if possible
    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            if (superShotChargeStatus < superShotNeededCharge)
            {
                // Make the weapon shot always towards it
                Vector2 shotDirection = this.transform.up; //Here we define the shoot direction à la space invaders
                singleShot(shotDirection);
                ChargeSuperShot();
            } else
            {
                //SUPERSHOOOOOOOT  \   |   /
                //
                //                   \ | / 
                int angle = 5;
                this.transform.Rotate(0, 0, -2 * angle);

                for(int i = 0; i < 5; i++)
                {
                    Vector2 shotDirection = this.transform.up;
                    singleShot(shotDirection);
                    this.transform.Rotate(0, 0, angle);
                }

                this.transform.Rotate(0, 0, -2 * angle);
                superShotChargeStatus = 0;

            }
            

        }
    }
    
    //Is the weapon ready to create a new projectile?
    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }

    public void ChargeSuperShot()
    {
        if(superShotChargeStatus < superShotNeededCharge)
        {
            superShotChargeStatus++;
        }
    }

    private void singleShot(Vector2 direction)
    {
        shootCooldown = shootingRate;

        // Create a new shot
        var shotTransform = Instantiate(shotPrefab) as Transform;

        // Assign position
        shotTransform.position = transform.position;
        shotTransform.rotation = transform.rotation;

        //Here we define who is the owner of the shot
        ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            shot.origin = gameObject.GetComponent<PlayerScript>().controllerIP;
        }

        MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
        if (move != null)
        {
            move.direction = direction;
        }

        
    }
}
