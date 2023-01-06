using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    // SCRIPT DESCR.
    #region
    // -- COPY/PASTE --
    /*
    
    [1] private EventInstance "NAME";    <-- Anführungszeichen entfernen
    
    [2] In "Awake()" die neue EventInstance "NAME" mit FMOD-Path einfügen:

        "NAME = FMODUnity.RuntimeManager.CreateInstance("event:/..");    <-- FMOD-Path ändern
    
    [3] Im "//GENERAL" - TAB unter "AmbientStart()" und "AmbientStop()" die EventInstance jeweils starten/stoppen:
        
        [3.1.] "AmbientStart()":
        
        public void AmbientStart()
        {
            if (EventIsNotPlaying("NAME"))
            {
                "NAME".start();
            }
        }
        
        ----------------------------------------

        [3.2.] "AmbientStop()":
        
        public void AmbientStop()
        {
            "NAME".stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        ----------------------------------------

    [4] WICHTIG, wenn man mit FMOD Parametern arbeitet (z.B. Geschwindigkeit des Playercharacters):
        
        In ..

         public void "NAME der Funktion"(float "name")
         {
            "NAME".setParameterByName("FMOD Parameter NAME", "name");
         }

        .. die jeweiligen Namen ändern.


    [5] Audio Manager verknüpfen (z.B. mit dem "WhingMovement01" - Script):
        
        Bsp.:

        AudioManager.instance.AmbientSetSpeedIntensity(myRigidbody.velocity.magnitude);  <-- einfügen
        

        In der "Start()" Funktion den AudioManager ansprechen:

        AudioManager.instance.AmbientStart();

        
        In der jeweiligen Funktion den AudioManager stoppen:
        
        AudioManager.instance.AmbientStop();

    */
    // ---------------
    #endregion
    // ---------------------------------------------------------------------------------------------------------------------------------------

    public static AudioManager instance = null;
    private EventInstance WindPressureInstance; // Wind Pressure Sound (Adaptive)
    private EventInstance BoostSingleUse; // Boost Single Use (Oneshot)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        WindPressureInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Player_related/Wind_pressure/wind_pressure"); //     <-- PATH KORREKT
    }


    //  GENERAL

    public void SoundStart()
    {
        if (EventIsNotPlaying(WindPressureInstance))
        {
            WindPressureInstance.start();
        }

    }

    public void SoundStop()
    {
        WindPressureInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    public void SetSpeedIntensity(float speedIntensity)
    {
        WindPressureInstance.setParameterByName("Speed", speedIntensity);
    }

    bool EventIsNotPlaying(EventInstance instance)
    {
        PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.PLAYING;
    }


    // BOOST SOUNDS
    // SINGLE USE (ONESHOT)

    public void BoostOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player_related/Boost/Single_use/Boost_single_use");
    }




    // FOR LATER
    //Oneshots-----------------


    //public void SwitchAuto(Vector3 OneShotPosition)
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Spatialized/SwitchAuto", OneShotPosition);
    //}
    //public void SwitchPlayer(Vector3 OneShotPosition)
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Spatialized/SwitchPlayer", OneShotPosition);
    //}
    //public void WagonOverTrackpoint(VisitorCart cart)
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Spatialized/WagonOverTrackpoint", cart.Position);
    //}

    //public void MenuButtonAccept()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/MenuButtonAccept");        
    //}

    //public void MenuButtonBack()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/MenuButtonBack()");
    //}

    //public void PointsPositive()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/PointsPositive");
    //}
    //public void PointsNegative()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/PointsNegative");
    //}



    //Enviromental Emitters-----------------


    //    public void StopAllEnvEmitters()
    //{
    //    //Stops all instances of Enviromental Emitters
    //    EnvEmittersBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //    wagonToEventInstance.Clear();
    //}

}
