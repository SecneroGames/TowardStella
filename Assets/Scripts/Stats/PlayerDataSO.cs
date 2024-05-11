using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
[CreateAssetMenu]
public class PlayerDataSO:ScriptableObject
{
    public string PlayerName;
    public int ChapterCompleted = 1;
    public float Credits = 999f;


    [Header("Current Ship Stat")]
    public float maxSpeed=60;
    public float acceleration=20;
    public float steering=10; //maneuverability
    public float boostDuration = 1; // how long do speed boost last
    public float collisionRecovery = 1.5f; // how long before ship can recover from hitting an obstacle

    //for blaster
    public bool hasBlaster = true;
    public int blasterLevel;

    [Header("Maximum ship upgrade")]
    public float maxShipSpeed = 150;
    public float maxShipAcceleration = 80;
    public float maxShipsteering = 25; 
    public float maxShipboostDuration = 2; 
    public float minShipcollisionRecovery = .25f;
    public float maxShipcollisionRecovery = 3;


    public void ResetToDefaultValues()
    {
        PlayerName = "";
        ChapterCompleted = 1;
        Credits = 999f;

        maxSpeed = 60;
        acceleration = 20;
        steering = 10;
        boostDuration = 1;
        collisionRecovery = 1.5f;

       hasBlaster = true;
       blasterLevel = 0;

        maxShipSpeed = 150;
        maxShipAcceleration = 80;
        maxShipsteering = 25;
        maxShipboostDuration = 2;
        minShipcollisionRecovery = .25f;
    }

    public void CopyFromSaveFile(SaveFile saveFile)
    {
        PlayerName = saveFile.PlayerName;
        ChapterCompleted = saveFile.ChapterCompleted;
        Credits = saveFile.Credits;

        maxSpeed = saveFile.maxSpeed;
        acceleration = saveFile.acceleration;
        steering = saveFile.steering;
        boostDuration = saveFile.boostDuration;
        collisionRecovery = saveFile.collisionRecovery;

        hasBlaster = saveFile.hasBlaster;
        blasterLevel = saveFile.blasterLevel;

        maxShipSpeed = saveFile.maxShipSpeed;
        maxShipAcceleration = saveFile.maxShipAcceleration;
        maxShipsteering = saveFile.maxShipsteering;
        maxShipboostDuration = saveFile.maxShipboostDuration;
        minShipcollisionRecovery = saveFile.minShipcollisionRecovery;
    }



    public bool IsMax(StatName statToCHeck)
    {
        switch (statToCHeck)
        {
            case StatName.MAX_SPEED:
                return maxSpeed >= maxShipSpeed;
             
            case StatName.ACCELERATION:
                return acceleration >= maxShipAcceleration;         
            case StatName.STEERING:
                return steering >= maxShipsteering;
            case StatName.BOOST_DURATION:
                return boostDuration >= maxShipboostDuration;              
            case StatName.COLLISSION_RECOVERY:
               return collisionRecovery <= minShipcollisionRecovery;
            //case StatName.BLASTER_LV:
            //    return blasterLevel >= 3;               
            //case StatName.HAS_BLASTER:
            //    return hasBlaster;

        }

        return false;
    }
    public void UpdateStat(StatName statToUpgrade, float value)
    {
        switch (statToUpgrade)
        {
            case StatName.MAX_SPEED:
                maxSpeed = value;
                maxSpeed = Mathf.Clamp(maxSpeed, 0, maxShipSpeed);
                break;
            case StatName.ACCELERATION:
                acceleration += value;
                acceleration = Mathf.Clamp(acceleration, 0, maxShipAcceleration);
                break;
            case StatName.STEERING:
                steering = value;
                steering = Mathf.Clamp(steering, 0, maxShipsteering);
                break;
            case StatName.BOOST_DURATION:
                boostDuration = value;
                boostDuration = Mathf.Clamp(boostDuration, 0, maxShipboostDuration);
                break;
            case StatName.COLLISSION_RECOVERY:
                collisionRecovery = value;
                collisionRecovery = Mathf.Clamp(collisionRecovery, minShipcollisionRecovery, maxShipcollisionRecovery);
                break;

            //case StatName.BLASTER_LV:
            //    blasterLevel++;
            //    blasterLevel = Mathf.Clamp(blasterLevel, 0, 5);
            //    break;
            //case StatName.HAS_BLASTER:
            //    if (value > 0)
            //    {
            //        hasBlaster = true;
            //    }
            //    break;

        }
    }

}