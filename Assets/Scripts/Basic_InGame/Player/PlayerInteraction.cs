using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerHP playerHp;

    private void Start()
    {
        playerHp = GetComponent<PlayerHP>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zombie_Arm"))
        {
            if (other.GetComponentInParent<Zombie>().IsAttack)
            {
                Debug.Log("2");
                StartCoroutine(playerHp.MinousHP(5));
            }
        }
    }
}
