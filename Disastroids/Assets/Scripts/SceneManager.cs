using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class SceneManager : MonoBehaviour {

    public Transform playerPrefab;
    public int MaxNumberPlayer = 2;

    private int NumberOfPlayers ;
    private ArrayList controllersToInstanciate = new ArrayList();

    
	void Start () {
        NumberOfPlayers = 0;
        NetworkInputManager.setNewControllerDelegate(addNewPlayer);
	}
	
	void Update () {
        foreach(string controllerIP in controllersToInstanciate)
        {
            if (NumberOfPlayers < MaxNumberPlayer)
            {
                Transform player = Instantiate(playerPrefab) as Transform;

                PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
                playerScript.controllerIP = controllerIP;

                //If it is player 1, we make it appear on the bottom of the screen, facing upwards
                if(NumberOfPlayers==0)
                {
                    player.position = new Vector2(-9, -2);
                } else
                {
                    player.position = new Vector2(9, 2);
                    playerScript.IsPlayer2 = true;
                }
                

                NumberOfPlayers++;
            } else
            {
                Debug.Log("Maximum number of players reached.");
            }
        }

        controllersToInstanciate.Clear();
        
    }

    //Instanciation of new GameObjects must be done in main thread
    public void addNewPlayer(string controllerIP)
    {
        controllersToInstanciate.Add(controllerIP);
        NetworkInputManager.ConnectedControllers[controllerIP] = new DisastroidController();
    }
}
