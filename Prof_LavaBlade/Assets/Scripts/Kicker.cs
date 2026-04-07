using UnityEngine;

public class Kicker : MonoBehaviour
{
    public Vector3 kickVev;
    Rigidbody myBod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBod = GetComponent<Rigidbody>();
        myBod.linearVelocity = kickVev;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
