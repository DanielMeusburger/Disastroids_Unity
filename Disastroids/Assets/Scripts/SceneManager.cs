using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public Transform playerPrefab;

    private bool shouldInstantiate = false;
    private string nextControllerIP;

	// Use this for initialization
	void Start () {

        NetworkInputManager.setNewControllerDelegate(addNewPlayer);
	
	}
	
	// Update is called once per frame
	void Update () {
        if (shouldInstantiate)
        {
            Transform player = Instantiate(playerPrefab) as Transform;

            PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
            playerScript.controllerIP = nextControllerIP;

            player.position = new Vector2(0, 0);
            shouldInstantiate = false;
        }
        
    }

    public void addNewPlayer(string controllerIP)
    {
        nextControllerIP = controllerIP;
        shouldInstantiate = true;
    }
}
