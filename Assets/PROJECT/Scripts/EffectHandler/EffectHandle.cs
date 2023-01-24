using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectHandle : MonoBehaviour
{
    public List<ScriptableObjectEffekte> AllEffects;



    public float DelayBeforeItemEffectup = 1; //Delay bis sich der Effect aktiviert

    public ScriptableObjectEffekte EffectUse; //what our current effect is

    BonusManager myBoniManager;

    public event Action<Sprite, List<Sprite>, float> OnBonusEffectActivated;

    void Start()
    {
        myBoniManager = FindObjectOfType<BonusManager>();

    }

    public void StartBonusEffect()
    {
        StartCoroutine(EffectUp());


        List<Sprite> allCurrentSpritesInList = new List<Sprite>();
        foreach (var item in AllEffects)
        {
            allCurrentSpritesInList.Add(item.effectSprite);
        }

        // Invoke ein EVevt damit dat UI das mitbekommt
        OnBonusEffectActivated?.Invoke(EffectUse.effectSprite, allCurrentSpritesInList, DelayBeforeItemEffectup);
    }

    public IEnumerator EffectUp()
    {
        //choose a held effect
        int EffectRand = UnityEngine.Random.Range(0, AllEffects.Count);
        EffectUse = AllEffects[EffectRand];

        //play animation
        AudioManager.instance.GamblingInProgress(); // <-- Gambling Sound takes about 5 secs
        yield return new WaitForSeconds(DelayBeforeItemEffectup);



        ActivateEffect();

        // L�scht einmal wirkende Effekte aus der Liste
        if(EffectUse.singleUse)
        {
            AllEffects.Remove(EffectUse);
        }

    }

    
    public void ActivateEffect()
    {
        myBoniManager.ActivateBonusByKey(EffectUse.bonusType, EffectUse.fuelVolume, EffectUse.scoreEffectTime, EffectUse.scoreMultiplyer, EffectUse.typeToActivate);
    }


}
