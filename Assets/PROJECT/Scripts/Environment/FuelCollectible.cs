using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCollectible : ScoreTrigger
{
    [SerializeField] GameObject fuelVisual; // NUR Visuelle Komponente

    public override void FuelTrigger(Collider other)
    {
        base.FuelTrigger(other);

        DisableFuel();
    }

    public override void OnCooldownEnd()
    {
        base.OnCooldownEnd();

        EnableFuel();
    }

    private void DisableFuel()
    {
        fuelVisual.SetActive(false);
    }

    private void EnableFuel()
    {
        fuelVisual.SetActive(true);
    }
}
