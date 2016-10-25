using UnityEngine;
using System.Collections;

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
        //We check if there are any controllers that just joined the game, and add player gameObjects accordingly to the scene
        foreach(string controllerIP in controllersToInstanciate)
        {
            if (NumberOfPlayers < MaxNumberPlayer)
            {
                Transform player = Instantiate(playerPrefab) as Transform;

                PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
                playerScript.controllerIP = controllerIP;

                //We make the 2 players appear on different place
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
