using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectUIHandle : MonoBehaviour
{
    private bool shuffleActiv; //If the handle should shuffle between sprite images
    private bool delayActiv;
    public float timeBtwShuffle; //How long each item shuffles
    List<Sprite> AllEffectGraphics; //to cycle through when an item is being selected
    public Sprite EmptyEffect; //the graphic for no item

    public Image Img;

    EffectHandle myEffectHandle;

    private void Awake()
    {

        myEffectHandle = FindObjectOfType<EffectHandle>();
    }
    private void Start()
    {
        shuffleActiv = false;
        delayActiv = false;
        Img.sprite = EmptyEffect;


        myEffectHandle.OnBonusEffectActivated += StartShuffle;

        AllEffectGraphics = new List<Sprite>();
    }

    void Update()
    {
        // Shuffle Effekt
        if (shuffleActiv && !delayActiv)
        {
            Invoke("Shuffle", timeBtwShuffle);
            delayActiv = true;
        }

        //Img.sprite = EffectUse.effectSprite;
    }

    public void StartShuffle(Sprite useEffectSprite, List<Sprite> otherSprites, float shuffleTime)
    {
        shuffleActiv = true;

        AllEffectGraphics.Clear();
        foreach (var sprite in otherSprites)
        {
            AllEffectGraphics.Add(sprite);
        }
        StartCoroutine(TimerIrgendwie(useEffectSprite, shuffleTime));
    }

    void Shuffle()
    {
        if (shuffleActiv)
            Img.sprite = AllEffectGraphics[Random.Range(0, AllEffectGraphics.Count)];
        delayActiv = false;
    }

    IEnumerator TimerIrgendwie(Sprite useEffectSprite2, float shuffleTime)
    {
        yield return new WaitForSeconds(shuffleTime);
        shuffleActiv = false;
        Img.sprite = useEffectSprite2;

        yield return new WaitForSeconds(2f);
        Img.sprite = EmptyEffect;
    }
}
