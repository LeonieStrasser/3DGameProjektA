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
    private EventInstance BoostHold; // Boost Hold (Adaptive)
    private EventInstance RaceInProgress; // Race in Progress (Adaptive)
    private EventInstance Pulse3D; // Pulse Sphere (3D Sound)
    private EventInstance EdgeSpark; // Edge Spark (Adaptive)
    private EventInstance TwirlInProgresss; // Race in Progress (Adaptive)

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

        WindPressureInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Player_related/Wind_pressure/wind_pressure");
        BoostHold = FMODUnity.RuntimeManager.CreateInstance("event:/Player_related/Boost/Hold/Boost_hold");
        RaceInProgress = FMODUnity.RuntimeManager.CreateInstance("event:/Races/Race_Music/Race_Music");
        TwirlInProgresss = FMODUnity.RuntimeManager.CreateInstance("event:/Player_related/Twirl/Oneshot/twirl_oneshot"); // DAS HIER IST DER TWIRL VERDAMMTE AXT
        EdgeSpark = FMODUnity.RuntimeManager.CreateInstance("event:/Player_related/Edge_Sparks/Edge_Sparks");
    }

    // EDGE SPARK START/STOP

    public void EdgeSparkStart()
    {
        if (EventIsNotPlaying(EdgeSpark))
        {
            EdgeSpark.start();
        }
    }

    public void EdgeSparkStop()
    {
        EdgeSpark.setParameterByName("closeToEdge", 0f);
        EdgeSpark.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    // TWIRL
    public void TwirlStart()
    {
        TwirlInProgresss.start();
    }

    public void TwirlStop()
    {
        TwirlInProgresss.stop(STOP_MODE.ALLOWFADEOUT);
    }


    //  RACE IN PROGRESS START/STOP

    public void RaceInProgressStart()
    {
        if (EventIsNotPlaying(RaceInProgress))
        {
            RaceInProgress.start();
        }
    }

    public void RaceInProgressStop()
    {
        RaceInProgress.setParameterByName("inRace", 0f);
        RaceInProgress.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    //  PULSE3D START/STOP
    public void Pulse3DStart()
    {
        
            Pulse3D.start();
        
    }

    public void Pulse3DStop()
    {
        Pulse3D.setParameterByName("inRace", 0f);
        Pulse3D.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void Pulse3DPosition(Vector3 position)
    {
        Pulse3D.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
    }



    //  WIND PRESSURE START/STOP

    public void WindSoundStart()
    {
        if (EventIsNotPlaying(WindPressureInstance))
        {
            WindPressureInstance.start();
        }
    }


    public void WindSoundStop()
    {
        WindPressureInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    //-----------------------

    // BOOST HOLD START/STOP

    public void BoostHoldSoundStart()
    {
        if (EventIsNotPlaying(BoostHold))
        {
            BoostHold.start();
        }
    }

    public void BoostHoldSoundStop()
    {
        BoostHold.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // BOOST HOLD PARAMETER
    public void BoostHolding(float boosting)
    {
        BoostHold.setParameterByName("isBoosting", boosting);
    }

    // WIND PARAMETER

    public void SetSpeedIntensity(float speedIntensity)
    {
        WindPressureInstance.setParameterByName("Speed", speedIntensity);
    }

    // RACE MUSIC PARAMETER
    public void SetRaceInProgress()
    {
        RaceInProgress.setParameterByName("inRace", 1f);
    }

    // EDGE SPARK PARAMETERS
    // WEITE DISTANZ
    public void SetEdgeSparkLongDistance()
    {
        EdgeSpark.setParameterByName("closeToEdge", 0.6f);
    }

    // MEDIUM DISTANZ
    public void SetEdgeSparkMediumDistance()
    {
        EdgeSpark.setParameterByName("closeToEdge", 0.8f);
    }

    // CLOSEST DISTANZ
    public void SetEdgeSparkCloseDistance()
    {
        EdgeSpark.setParameterByName("closeToEdge", 1f);
    }

    // EDGE SPARK PAUSE PARAMETER
    public void PauseEdgeSpark()
    {
        EdgeSpark.setParameterByName("closeToEdge", 0f);
    }

    // RACE MUSIC PAUSE PARAMETER
    public void PauseRaceInProgress()
    {
        RaceInProgress.setParameterByName("inRace", 0f);
    }

    // PULSE3D PARAMETER
    public void SetPulse3D()
    {
        Pulse3D.setParameterByName("inRace", 1f);
    }

    // RACE MUSIC PAUSE PARAMETER
    public void PausePulse3D()
    {
        Pulse3D.setParameterByName("inRace", 0f);
    }



    // --------------------------

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

    // BOOST RESSOURCE EMPTY
    public void BoostEmpty()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player_related/Boost/Empty/Boost_Empty");
    }




    // INTRO
    // SOUND (ONESHOT)

    public void IntroSoundOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Intro/Sound/Intro_sound");
    }

    // RACE
    // NEW RACE SPAWN (ONESHOT)
    public void NewRaceSpawn()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Races/Race_Spawn/Race_Spawn");
    }

    // RACE START (ONESHOT)
    public void StartRace()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Races/Race_Start/Race_Start");
    }

    // RACE TIME UP (ONESHOT)
    public void RaceTimeUp()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Races/Race_Time_Up/Race_Time_Up");
    }

    // RACE FINISHED (ONESHOT)
    public void RaceFinished()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Races/Race_Finished/Race_Finished");
    }

    // GAMBLING
    // IN PROGRESS (ONESHOT)
    public void GamblingInProgress()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Races/Gambling/In_Progress/Gambling_in_Progress");
    }

    // PLAYERCRASH (ONESHOT)
    public void PlayerCrash()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player_related/Crash/Crash");
    }

    // EXTERNAL BOOST (ONESHOT)
    public void ExternalBoost()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Environment/External_Boost/External_Boost");
    }

    // TELEPORT (ONESHOT)
    public void Teleport()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player_related/Teleport/Teleport");
    }





    //---------------------------------------- W I P S --------------------------------------------
    // PASSING THROUGH RING (ONESHOT)
    public void PassingRing()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Environment/Passing_Ring/Passing_Ring");
    }









































    //public void BoostHoldOneShot()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Player_related/Boost/Hold/Boost_hold");
    //}


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
