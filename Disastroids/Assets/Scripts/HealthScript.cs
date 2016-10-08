using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    /// <summary>
  /// Total hitpoints
  /// </summary>
  public int hp = 1;
    

  /// <summary>
  /// Inflicts damage and check if the object should be destroyed
  /// </summary>
  /// <param name="damageCount"></param>
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
        Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
      }
    }
  }
}
