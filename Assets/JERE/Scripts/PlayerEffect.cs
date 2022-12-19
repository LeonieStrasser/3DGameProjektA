using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    private EffectHandle Handle;

    public float DelayBeforeItemEffectup = 1; //Delay bis sich der Effect aktiviert

    public int HeldEffect; //0 equals null

    public bool CanEffectUp; //Ob ein Effect abgespielt werden darf.
    private bool UseEffect;

    public ScriptableObjectEffekte EffectUse; //what our current effect is
    
    void Start()
    {
        //Handle = GameObject.FindGameObjectsWithTag("GameController").GetComponent<EffectHandle>();
        Handle = GetComponent<EffectHandle>();

        ResetEffect();
    }

    public void StartEffectUp()
    {
        StartCoroutine(EffectUp());
    }
    
    public IEnumerator EffectUp()
    {
        if(HeldEffect == -1 && CanEffectUp)
        {
            CanEffectUp = false;
            //play animation
            yield return new WaitForSeconds(DelayBeforeItemEffectup);
            //choose a held effect
            int EffectRand = Random.Range(0, Handle.AllEffects.Length);

            EffectUse = Handle.AllEffects[EffectRand];
            HeldEffect = EffectRand;
        }
    }

    void Update()
    {
        if(HeldEffect != -1)
        {
            ActivateEffect();
        }

        //ResetEffect(); //Wenn der effekt done ist
    }

    public void ActivateEffect()
    {
        /*if(Effekt#1)
        {
            Mache Effekt #1
        }
        */
    }

    public void ResetEffect()
    {
        HeldEffect = -1;
        CanEffectUp = true;
    }
}
