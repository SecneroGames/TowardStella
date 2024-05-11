using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ShipTracker : MonoBehaviour
{
    [System.Serializable]
    public enum ShipTag
    {
        PLAYER,
        ENEMY
    }

    public ShipTag shipTag = ShipTag.ENEMY;


}
