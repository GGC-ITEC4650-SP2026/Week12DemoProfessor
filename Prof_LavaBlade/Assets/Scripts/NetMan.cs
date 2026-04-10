using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetMan : MonoBehaviourPunCallbacks
{
    Spinner lavaBlade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lavaBlade = GameObject.Find("LavaBlade").GetComponent<Spinner>();
    }

    public override void OnJoinedRoom()
    {
        float netTime = (float) PhotonNetwork.Time;
        lavaBlade.transform.eulerAngles = lavaBlade.spinVec * netTime;
    }
}
