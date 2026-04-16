using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{    
    public float movePower;
    public int health;

    Rigidbody myBod;
    Transform healthBar;
    Text namePlate;

    // Is called as soon as gameobject is instantiated before anything else.
    void Awake()
    {
        myBod = GetComponent<Rigidbody>();  
        healthBar = transform.Find("Canvas/GreenHealth");
        namePlate = transform.Find("Canvas/NamePlate").GetComponent<Text>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        namePlate.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(photonView.IsMine) { //My Laptop's Player (Client Authority)
            float h = Input.GetAxis("Horizontal");   
            float v = Input.GetAxis("Vertical");
            Vector3 f = new Vector3(h, v, 0);
            myBod.AddForce(f * movePower * Time.deltaTime);
            //send the pos and vel to all my clones.
            //done by photon rigidbody view component
        }
        else // other players' clones
        {
            //recieve pos and vel for other players' clones   
            //done by photon rigidbody view component
        }

        // all players irrelvant of ownership
        healthBar.localScale = new Vector3(health/100f, 1, 1);
        if(health <= 0)
        {
            //die
            myBod.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Collider>().enabled = false;
            namePlate.color = Color.gray;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if(photonView.IsMine) {
            health--;
            if(health < 0)
            {
                health = 0;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(photonView.IsMine)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int) stream.ReceiveNext();
        }
    }
}
