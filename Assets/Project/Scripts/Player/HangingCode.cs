using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingCode : MonoBehaviour
{
    public GameObject rootP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            other.GetComponentInParent<PlayerController>().Hanging(rootP.transform);
        }
    }
}
