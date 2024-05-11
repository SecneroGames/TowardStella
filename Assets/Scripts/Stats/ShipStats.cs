using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum StatName
{
    MAX_SPEED,
    ACCELERATION,
    STEERING,
    BOOST_DURATION,
    COLLISSION_RECOVERY,
    HAS_BLASTER,
    BLASTER_LV
}


[System.Serializable]
public class ShipStats
{  

    public float maxSpeed = 75;
    public float acceleration = 15;
    public float steering = 10; //maneuverability for side to side movement
    public float boostDuration = 1; // how long do speed boost last
    public float collisionRecovery = 1.5f; // how long before ship can recover from hitting an obstacle
    

    //for blaster
     public bool hasBlaster;
    //public int blasterLevel;
    public void CopyFromSO(PlayerDataSO template)
    {

        maxSpeed = template.maxSpeed;
        acceleration = template.acceleration;
        steering = template.steering;
        boostDuration = template.boostDuration;
        collisionRecovery = template.collisionRecovery;
        //hasBlaster = template.hasBlaster;
        //blasterLevel = template.blasterLevel;
    }

    public void Upgrade(StatName statToUpgrade, float value)
    {
        switch(statToUpgrade)
        {
            case StatName.MAX_SPEED:
                maxSpeed += value;
                break;
            case StatName.ACCELERATION:
                acceleration += value;
                break;
            case StatName.STEERING:
                steering += value;
                break;
            case StatName.BOOST_DURATION:
                steering = value;
                break;
            case StatName.COLLISSION_RECOVERY:
                collisionRecovery = value;
                break;

                //case StatName.BLASTER_LV:
                //    blasterLevel++; 
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
