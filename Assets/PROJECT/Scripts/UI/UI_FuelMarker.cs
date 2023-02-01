using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FuelMarker : UI_Marker
{
    [SerializeField] GameObject uiFuelObject;
   Animator uiFuelAnim;
    private WhingMovement01 myPlayer;


    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<WhingMovement01>();
        uiFuelAnim = GetComponent<Animator>();

        myPlayer.OnBoostStart += RessourceStart;
        myPlayer.OnBoostEnd += RessourtceEnd;
        myPlayer.OnSlowMoStart += RessourceStart;
        myPlayer.OnSlowMoEnd += RessourtceEnd;
        myPlayer.OnRessourceEmpty += RessourceEmpty;
        myPlayer.OnRessourceAdd += AddFuel;
    }

    void RessourceStart()
    {
        uiFuelAnim.SetBool("boost", true);
    }

    void RessourtceEnd()
    {
        uiFuelAnim.SetBool("boost", false);

    }

    void RessourceEmpty()
    {
        uiFuelAnim.SetTrigger("empty");
    }

    void AddFuel()
    {
        uiFuelAnim.SetTrigger("addFuel");

    }
}
