using UnityEngine;
using System.Collections;

public class ShotScript : MonoBehaviour {
    
    public int damage = 1;
    public string origin;

    void Start()
    {
        //Limited time to live to free memory
        Destroy(gameObject, 15);
    }
}
