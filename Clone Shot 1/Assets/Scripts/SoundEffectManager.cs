using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffects
{
    Assist,
    Ahhh,
    BlueLead,
    Defense,
    Denied,
    Dominating,
    Excellent,
    Fight,
    FirstBlood,
    FiveMinutes,
    Godlike,
    Headshot,
    HolyCrap,
    Humiliation,
    Impressive,
    KillingSpree,
    MonsterKill,
    MultiKill,
    MyLeg,
    One,
    OneMinute,
    Owww,
    Perfect,
    PlasmaShot,
    Rampage,
    RedLead,
    RoundOver,
    SuddenDeath,
    TeamsAreTied,
    Three,
    Two,
    UltraKill,
    Unstoppable,
    WelcomeToCloneShot,
    YouHaveLostTheLead,
    YouHaveTakenTheLead
}

public static class SoundEffectManager
{
    public static AudioClip GetClip(SoundEffects SoundEffect)
    {
        AudioClip newClip = new AudioClip();
        try
        {
            switch (SoundEffect)
            {
                case SoundEffects.Ahhh:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Ahhh");
                    break;
                case SoundEffects.Assist:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Assist");
                    break;
                case SoundEffects.BlueLead:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Blue Lead");
                    break;
                case SoundEffects.Defense:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Defense");
                    break;
                case SoundEffects.Denied:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Denied");
                    break;
                case SoundEffects.Dominating:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Dominating");
                    break;
                case SoundEffects.Excellent:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Excellent");
                    break;
                case SoundEffects.Fight:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Fight");
                    break;
                case SoundEffects.FirstBlood:
                    newClip = Resources.Load<AudioClip>("Sound Effects/First Blood");
                    break;
                case SoundEffects.FiveMinutes:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Five minute warning");
                    break;
                case SoundEffects.Godlike:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Godlike");
                    break;
                case SoundEffects.Headshot:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Headshot");
                    break;
                case SoundEffects.HolyCrap:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Holy Crap");
                    break;
                case SoundEffects.Humiliation:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Humiliation");
                    break;
                case SoundEffects.Impressive:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Impressive");
                    break;
                case SoundEffects.KillingSpree:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Killing Spree");
                    break;
                case SoundEffects.MonsterKill:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Monster Kill");
                    break;
                case SoundEffects.MultiKill:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Multikill");
                    break;
                case SoundEffects.MyLeg:
                    newClip = Resources.Load<AudioClip>("Sound Effects/My leg");
                    break;
                case SoundEffects.One:
                    newClip = Resources.Load<AudioClip>("Sound Effects/One");
                    break;
                case SoundEffects.Owww:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Owww");
                    break;
                case SoundEffects.OneMinute:
                    newClip = Resources.Load<AudioClip>("Sound Effects/One minute warning");
                    break;
                case SoundEffects.PlasmaShot:
                    newClip = Resources.Load<AudioClip>("Sound Effects/dspistol");
                    break;
                case SoundEffects.Perfect:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Perfect");
                    break;
                case SoundEffects.Rampage:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Rampage");
                    break;
                case SoundEffects.RedLead:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Red lead");
                    break;
                case SoundEffects.RoundOver:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Round Over");
                    break;
                case SoundEffects.SuddenDeath:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Sudden Death");
                    break;
                case SoundEffects.TeamsAreTied:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Teams are tied");
                    break;
                case SoundEffects.Three:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Three");
                    break;
                case SoundEffects.Two:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Two");
                    break;
                case SoundEffects.UltraKill:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Ultrakill");
                    break;
                case SoundEffects.Unstoppable:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Unstoppable");
                    break;
                case SoundEffects.WelcomeToCloneShot:
                    newClip = Resources.Load<AudioClip>("Sound Effects/Welcome to clone shot");
                    break;
                case SoundEffects.YouHaveLostTheLead:
                    newClip = Resources.Load<AudioClip>("Sound Effects/You have lost the lead");
                    break;
                case SoundEffects.YouHaveTakenTheLead:
                    newClip = Resources.Load<AudioClip>("Sound Effects/You have taken the lead");
                    break;
                
            }
        }
        catch(Exception e)
        {
            Debug.Log("An error occured when loading a sound effect! \nError Message: " + e.Message);
        }

        return newClip;
           
    }

}
