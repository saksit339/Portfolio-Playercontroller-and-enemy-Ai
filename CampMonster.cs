using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampMonster : MonoBehaviour
{
    public List<GameObject> monster;
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().SetCampMonster(this);
        }
    }
}
