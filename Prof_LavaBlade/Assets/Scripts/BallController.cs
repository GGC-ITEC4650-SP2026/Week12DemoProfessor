using Photon.Pun;
using UnityEngine;

public class BallController : MonoBehaviourPunCallbacks
{
    public Color[] colors; //(good, bad)

    Renderer myRend;

    void Awake()
    {
        myRend = GetComponent<Renderer>();
    }

    void Start()
    {
        myRend.material.color = colors[0];   
    }

    void OnCollisionEnter(Collision collision)
    {
        if(PhotonNetwork.IsMasterClient) {
            GameObject otherGo = collision.gameObject;
            if(otherGo.tag == "Player")
            {
                if(myRend.material.color == colors[0])
                {
                    //setColor(1);
                    photonView.RPC("setColor", RpcTarget.AllBuffered, 1);
                    otherGo.GetComponent<PhotonView>().RPC("increaseScore", RpcTarget.AllBuffered, 10);
                }
                else
                {
                    //setColor(0);
                    photonView.RPC("setColor", RpcTarget.AllBuffered, 0);
                    otherGo.GetComponent<PhotonView>().RPC("increaseScore", RpcTarget.AllBuffered, -10);
                }
            }
        }
    }

    [PunRPC] // method is callable over network
    public void setColor(int i)
    {
        myRend.material.color = colors[i];
    }
}
