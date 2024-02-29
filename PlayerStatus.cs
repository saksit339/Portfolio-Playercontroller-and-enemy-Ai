using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private float playerHP = 100;
    private float gunEnergy;
    private float gunEnergyUse = 10;
    private float gunEnergyRecoveryWihtMeleeAttack = 10;

    private float maxGunEnergy = 100;
    private float minGunEnergy = 0;
    private float gunEnergyRecovery = 3;

    private void Start()
    {
        gunEnergy = maxGunEnergy;
    }
    void Update()
    {
        gunEnergy += (gunEnergyRecovery * Time.deltaTime);
        gunEnergy = Mathf.Clamp(gunEnergy,minGunEnergy,maxGunEnergy);
    }

    public void PlayerTakeDamage(float damage)
    {
        playerHP -= damage;
    }

    public float PlayerCurrenHP()
    {
        return playerHP;
    }

    public float PlyerGunEnergy()
    {
        return gunEnergy;
    }

}
