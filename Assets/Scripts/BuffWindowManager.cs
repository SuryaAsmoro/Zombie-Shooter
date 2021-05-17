
using System.Collections.Generic;
using UnityEngine;

public class BuffWindowManager : MonoBehaviour
{
    public List<GameObject> buffList;
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public PlayerShooting playerShooting;


    private void Update()
    {
        if (playerMovement.IsInSpeedBuff)
            buffList[1].SetActive(true);

        if (playerShooting.IsInAttackBuff)
            buffList[2].SetActive(true);

        if (playerHealth.IsInHealBuff)
            buffList[0].SetActive(true);

        if (!playerMovement.IsInSpeedBuff)
            buffList[1].SetActive(false);

        if (!playerShooting.IsInAttackBuff)
            buffList[2].SetActive(false);

        if (!playerHealth.IsInHealBuff)
            buffList[0].SetActive(false);
    }
}
