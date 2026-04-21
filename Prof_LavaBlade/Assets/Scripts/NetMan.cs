using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetMan : MonoBehaviourPunCallbacks
{
    Spinner lavaBlade;
    public GameObject playerPrefab;
    public GameObject ballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lavaBlade = GameObject.Find("LavaBlade").GetComponent<Spinner>();
    }

    //Runs on each client when they join the game.
    public override void OnJoinedRoom()
    {
        // Syncrhonizing Lava Blades with Server
        float netTime = (float) PhotonNetwork.Time;
        lavaBlade.transform.eulerAngles = lavaBlade.spinVec * netTime;

        //create my player on every laptop in the room.
        Vector3 pos = 
            new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);        
        PhotonNetwork.Instantiate(playerPrefab.name, pos, 
            Quaternion.identity); 
    }

    //RUNS ON SERVER WHEN PLAYER ENTERS GAME
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient) //RUN ON SERVER ONLY
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 4)
            {
                PhotonNetwork.Instantiate(ballPrefab.name, Vector3.zero, 
                    Quaternion.identity); 
            }
        }
    }
}
