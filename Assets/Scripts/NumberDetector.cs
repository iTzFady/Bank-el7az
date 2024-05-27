using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDetector : MonoBehaviour
{
    RollingDice rollingDice;
    private void Awake()
    {
        rollingDice = FindObjectOfType<RollingDice>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (rollingDice != null) {
            if (rollingDice.GetComponent<Rigidbody>().velocity == Vector3.zero) { 
                rollingDice.num = int.Parse(other.name);
                Debug.Log(rollingDice.num);
            }
        }
    }
}
