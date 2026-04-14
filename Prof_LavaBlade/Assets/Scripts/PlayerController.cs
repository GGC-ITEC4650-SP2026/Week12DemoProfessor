using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movePower;
    Rigidbody myBod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBod = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        //THIS NEEDS TO ONLY CONTROL THE PLAYER I OWN!
        float h = Input.GetAxis("Horizontal");   
        float v = Input.GetAxis("Vertical");
        Vector3 f = new Vector3(h, v, 0);
        myBod.AddForce(f * movePower * Time.deltaTime);
    }
}
