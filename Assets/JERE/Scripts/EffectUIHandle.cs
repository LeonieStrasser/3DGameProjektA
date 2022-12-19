using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectUIHandle : MonoBehaviour
{
    private bool shuffleSprite; //If the handle should shuffle between sprite images
    public float timeBtwShuffle; //How long each item shuffles
    public Sprite[] AllEffectGraphics; //to cycle through when an item is being selected
    public Sprite EmptyEffect; //the graphic for no item

    public Image Img;

    //public KartItem Kart;

    private void Start()
    {
        shuffleSprite = true;
    }

    void Update()
    {
        if(shuffleSprite)
        {
            Invoke("Shuffle", timeBtwShuffle);
            shuffleSprite = false;
        }

        //Img.sprite = Kart.ItmUse.Viusal;
    }

    void Shuffle()
    {
        Img.sprite = AllEffectGraphics[Random.Range(0, AllEffectGraphics.Length)];
        shuffleSprite = true;
    }
}
