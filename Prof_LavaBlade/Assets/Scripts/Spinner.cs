using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 spinVec;

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles += spinVec * Time.deltaTime;   
    }

    // Update is called once per physics frame
    void FixedUpdate()
    {
        transform.eulerAngles += spinVec * Time.fixedDeltaTime;   
    }

}

