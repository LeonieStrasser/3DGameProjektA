using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScript : MonoBehaviour
{
     [Header("Animator")]
     public Animator anim;

     public void FirstSlideAnim()
     {
        anim.SetBool("FirstSlide", true);
     }

     public void SecondSlideAnim()
     {    
        anim.SetBool("SecondSlide", true);
        anim.SetBool("FirstSlide", false);
        anim.SetBool("ThirdSlide", false);
     }

     public void ThirdSlideAnim()
     {
        anim.SetBool("ThirdSlide", true);
        anim.SetBool("SecondSlide", false);
     }

   public void FourthSlideAnim()
   {
        anim.SetBool("ThirdSlide", false);
   }
}