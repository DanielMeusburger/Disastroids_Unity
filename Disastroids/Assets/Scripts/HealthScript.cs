﻿using UnityEngine;

public class HealthScript : MonoBehaviour {
    
  //Healthpoints
  public int hp = 1;
    
    
  //Inflicts damage and check if the object should be destroyed
  public void Damage(int damageCount)
  {
    hp -= damageCount;

    if (hp <= 0)
    {
      // Dead!
      Destroy(gameObject);
    }
  }

  void OnTriggerEnter2D(Collider2D otherCollider)
  {
    // Is this a shot?
    ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
    PlayerScript myself = this.gameObject.GetComponent<PlayerScript>();

    if (shot != null)
    {
      // Avoid friendly fire
      if (shot.origin != myself.controllerIP)
      {
        Damage(shot.damage);

        // Destroy the shot
        Destroy(shot.gameObject);
      }
    }
  }
}
