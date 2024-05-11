using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile 
{
    public string PlayerName = "Stella";
    public int ChapterCompleted = 0;
    public float Credits = 500 ;

    //shipStats
    public float maxSpeed = 75;
    public float acceleration = 50;
    public float steering = 15; 
    public float boostDuration = 1; 
    public float collisionRecovery = 1.5f; 

    //for blaster
    public bool hasBlaster;
    public int blasterLevel;

    public float maxShipSpeed = 150;
    public float maxShipAcceleration = 80;
    public float maxShipsteering = 25;
    public float maxShipboostDuration = 2;
    public float minShipcollisionRecovery = .25f;

    public SaveFile(PlayerDataSO playerData)
    {      
        PlayerName = playerData.PlayerName;
        ChapterCompleted = playerData.ChapterCompleted;
        Credits = playerData.Credits;

        maxSpeed = playerData.maxSpeed;
        acceleration = playerData.acceleration;
        steering = playerData.steering;
        boostDuration = playerData.boostDuration;
        collisionRecovery = playerData.collisionRecovery;

        hasBlaster = playerData.hasBlaster;
        blasterLevel = playerData.blasterLevel;

        maxShipSpeed = playerData.maxShipSpeed;
        maxShipAcceleration = playerData.maxShipAcceleration;
        maxShipsteering = playerData.maxShipsteering;
        maxShipboostDuration = playerData.maxShipboostDuration;
        minShipcollisionRecovery = playerData.minShipcollisionRecovery;
    }
}
