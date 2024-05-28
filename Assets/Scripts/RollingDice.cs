using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RollingDice : MonoBehaviour
{
    Rigidbody diceBody;
    [SerializeField] private float maxRandomForceValue, rollingForce;
    private float forceX, forceY , forceZ;
    public int num;

    private void Awake()
    {
        Initialize();
    }
    // Update is called once per frame
    void Update()
    {
        if (diceBody != null)
        {
            if (Input.GetMouseButtonDown(0)) {
                RollDice();
            }
        }
    }

    public void RollDice()
    {
        diceBody.isKinematic = false;
        forceX = Random.Range(0 , maxRandomForceValue);
        forceY = Random.Range(0, maxRandomForceValue);
        forceZ = Random.Range(0 ,maxRandomForceValue);
        diceBody.AddForce(Vector3.up * rollingForce);
        diceBody.AddTorque(forceX, forceY, forceZ);
    }
    private void Initialize() { 
        diceBody = GetComponent<Rigidbody>();
        diceBody.isKinematic = true;
        transform.rotation = new Quaternion(Random.Range(0 , 360) , Random.Range(0, 360) , Random.Range(0 , 360), 0);
    
    }

}
