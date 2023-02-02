using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCollectible : ScoreTrigger
{
    [SerializeField] GameObject fuelVisual; // NUR Visuelle Komponente
    [SerializeField] GameObject popVFX;
    [SerializeField] Collider colliCollider;



    public override void FuelTrigger(Collider other)
    {
        base.FuelTrigger(other);
        DisableFuel();
        AudioManager.instance.FuelPickUp();
    }

    public override void OnCooldownEnd()
    {
        base.OnCooldownEnd();

        EnableFuel();
    }

    private void DisableFuel()
    {
        Instantiate(popVFX, transform.position, Quaternion.identity);
        colliCollider.enabled = false;
        fuelVisual.SetActive(false);
    }

    private void EnableFuel()
    {
        colliCollider.enabled = true;
        fuelVisual.SetActive(true);
    }
}
