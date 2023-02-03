using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScript : MonoBehaviour
{
    [Header("Slides")]
    public GameObject firstSlide;
    public GameObject secondSlide;
    public GameObject thirdSlide;
    public GameObject fourthSlide;

    [Header("Buttons")]
    public GameObject rbFirstSlide;
    public GameObject lbSecondSlide;
    public GameObject rbSecondSlide;
    public GameObject lbThirdSlide;
    public GameObject rbThirdSlide;
    public GameObject lbFourthSlide;

     [Header("Animator")]
     public Animator anim;
    private void Start() 
    {
        anim.SetBool("FirstSlide", true);
    }

    //METHODS
    // We start at Slide #1
    // Change from Slide #1 to #2
   public void ChangeToSecondSlide()
   {    

        anim.SetBool("FirstSlide", true);
        
        //SET ACTIVE
        //SLIDES
        secondSlide.SetActive(true);
        
        //BUTTONS
        lbSecondSlide.SetActive(true);
        rbSecondSlide.SetActive(true);


        //SET FALSE
        //SLIDES
        firstSlide.SetActive(false);
        thirdSlide.SetActive(false);
        fourthSlide.SetActive(false);

        //BUTTONS
        rbFirstSlide.SetActive(false);
        lbThirdSlide.SetActive(false);
        rbThirdSlide.SetActive(false);
        lbFourthSlide.SetActive(false);  
   }

   // Go back from Slide #2 to #1
    public void ChangeToFirstSlide()
   {
        //SET ACTIVE
        //SLIDES
        firstSlide.SetActive(true);

        //BUTTONS
        rbFirstSlide.SetActive(true);
        
        
        //SET FALSE
        //SLIDES
        secondSlide.SetActive(false);
        thirdSlide.SetActive(false);
        fourthSlide.SetActive(false);

        //BUTTONS
        lbSecondSlide.SetActive(false);
        rbSecondSlide.SetActive(false);
        lbThirdSlide.SetActive(false);
        rbThirdSlide.SetActive(false);
        lbFourthSlide.SetActive(false);
   }

    // Go from Slide #2 to #3
   public void ChangeToThirdSlide()
   {
        //SET ACTIVE
        //SLIDES
        thirdSlide.SetActive(true);

        //BUTTONS
        lbThirdSlide.SetActive(true);
        rbThirdSlide.SetActive(true);
        

        //SET FALSE
        //SLIDES
        firstSlide.SetActive(false);
        secondSlide.SetActive(false);
        fourthSlide.SetActive(false);

        //BUTTONS
        rbFirstSlide.SetActive(false);
        lbSecondSlide.SetActive(false);
        rbSecondSlide.SetActive(false);
        lbFourthSlide.SetActive(false);
   }

    // Go from Slide #3 to #4
   public void ChangeToFourthSlide()
   {
        //SET ACTIVE
        //SLIDES
        fourthSlide.SetActive(true);

        //BUTTONS
        lbFourthSlide.SetActive(true);


        //SET FALSE
        //SLIDES
        firstSlide.SetActive(false);
        secondSlide.SetActive(false);
        thirdSlide.SetActive(false);

        //BUTTONS
        rbFirstSlide.SetActive(false);
        lbSecondSlide.SetActive(false);
        rbSecondSlide.SetActive(false);
        lbThirdSlide.SetActive(false);
        rbThirdSlide.SetActive(false);
   }
}