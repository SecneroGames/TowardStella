using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradePanel : MonoBehaviour
{
    [System.Serializable]
    public class StatSet
    {
        public StatName statType;
        public ButtonHoldBehaviour UpgradeButton;
        public TextMeshProUGUI UpgradeText;
        public Image fillPercent;
        [Header("Upgrade multipler")]
        public float currentValue;
        public float baseCost = 2;
        public float costMultiplier = 1.1f;
        public float increment = .2f;
    }

    public StatSet MaxSpeed;
    public StatSet Acceleration;
    public StatSet Steering;
    public StatSet Booster;
    public StatSet Recovery;

    public PlayerDataSO PlayerShipSO;

    [Header("UI")]
    public TextMeshProUGUI Currency;
    
    private void Start()
    {
        Input.multiTouchEnabled = false;
      
        UpdateStatSet();


    }
    private void OnEnable()
    {
        MaxSpeed.UpgradeButton.OnButtonHold.AddListener(UpgradeMaxSpeed);
        Acceleration.UpgradeButton.OnButtonHold.AddListener(UpgradeAcceleration);
        Steering.UpgradeButton.OnButtonHold.AddListener(UpgradeSteering);
        Booster.UpgradeButton.OnButtonHold.AddListener(UpgradeBooster);
        Recovery.UpgradeButton.OnButtonHold.AddListener(UpgradeRecovery);
    }

    private void OnDisable()
    {
        MaxSpeed.UpgradeButton.OnButtonHold.RemoveAllListeners();
        Acceleration.UpgradeButton.OnButtonHold.RemoveAllListeners();
        Steering.UpgradeButton.OnButtonHold.RemoveAllListeners();
        Booster.UpgradeButton.OnButtonHold.RemoveAllListeners();
        Recovery.UpgradeButton.OnButtonHold.RemoveAllListeners();
    }
    //update stats to match scriptable object
    public void UpdateStatSet()
    {
        MaxSpeed.currentValue = PlayerShipSO.maxSpeed;
        Acceleration.currentValue = PlayerShipSO.acceleration;
        Steering.currentValue = PlayerShipSO.steering;
        Booster.currentValue = PlayerShipSO.boostDuration;
        Recovery.currentValue = PlayerShipSO.collisionRecovery;
        MaxSpeed.UpgradeText.text = $"{MaxSpeed.currentValue.ToString("N2")} Cost {CalculateCost(MaxSpeed).ToString("N2")} credits";
        Acceleration.UpgradeText.text = $"{Acceleration.currentValue.ToString("N2")}  Cost {CalculateCost(Acceleration).ToString("N2")} credits";
        Steering.UpgradeText.text = $"{Steering.currentValue.ToString("N2")} Cost {CalculateCost(Steering).ToString("N2")} credits";
        Booster.UpgradeText.text = $"{Booster.currentValue.ToString("N2")}s Cost {CalculateCost(Booster).ToString("N2")} credits";
        Recovery.UpgradeText.text = $"{Recovery.currentValue.ToString("N2")}s Cost {CalculateCost(Recovery).ToString("N2")} credits";

        MaxSpeed.fillPercent.transform.localScale = new Vector3((MaxSpeed.currentValue / PlayerShipSO.maxShipSpeed), 1, 1);
        Acceleration.fillPercent.transform.localScale = new Vector3((Acceleration.currentValue / PlayerShipSO.maxShipAcceleration), 1, 1);
        Steering.fillPercent.transform.localScale = new Vector3((Steering.currentValue / PlayerShipSO.maxShipsteering), 1, 1);
        Booster.fillPercent.transform.localScale = new Vector3((Booster.currentValue / PlayerShipSO.maxShipboostDuration), 1, 1);
        Recovery.fillPercent.transform.localScale = new Vector3(1 - (Recovery.currentValue/ PlayerShipSO.maxShipcollisionRecovery), 1, 1);
        //Debug.LogError(Recovery.currentValue + " " + PlayerShipSO.maxShipcollisionRecovery);

        Currency.text = $"{PlayerShipSO.Credits} credits";
    }

    public void UpgradeMaxSpeed()
    {
        // dont upgrade if the stat is already maxed out
        if (PlayerShipSO.IsMax(MaxSpeed.statType))
            return;

        var cost = CalculateCost(MaxSpeed);
        //only upgrade if player can afford it
        if (cost <= PlayerShipSO.Credits)
        {
            PlayerShipSO.Credits -= cost;
            MaxSpeed.currentValue += MaxSpeed.increment;
            PlayerShipSO.UpdateStat(MaxSpeed.statType, MaxSpeed.currentValue);
            AudioManager.instance.PlaySFX("Blip");
            UpdateStatSet();
            if (PlayerShipSO.IsMax(MaxSpeed.statType))
                AudioManager.instance.PlaySFX("Ting");
        }
    }


    public void UpgradeAcceleration()
    {
        // dont upgrade if the stat is already maxed out
        if (PlayerShipSO.IsMax(Acceleration.statType))
            return;

        var cost = CalculateCost(Acceleration);
        //only upgrade if player can afford it
        if (cost <= PlayerShipSO.Credits)
        {
            PlayerShipSO.Credits -= cost;
            Acceleration.currentValue += Acceleration.increment;
            PlayerShipSO.UpdateStat(Acceleration.statType, Acceleration.currentValue);
            UpdateStatSet();
            AudioManager.instance.PlaySFX("Blip");

            if (PlayerShipSO.IsMax(Acceleration.statType))
                AudioManager.instance.PlaySFX("Ting");
        }
    }

    public void UpgradeSteering()
    {
        // dont upgrade if the stat is already maxed out
        if (PlayerShipSO.IsMax(Steering.statType))
            return;

        var cost = CalculateCost(Steering);
        //only upgrade if player can afford it
        if (cost <= PlayerShipSO.Credits)
        {
            PlayerShipSO.Credits -= cost;
            Steering.currentValue += Steering.increment;
            PlayerShipSO.UpdateStat(Steering.statType, Steering.currentValue);
            UpdateStatSet();
            AudioManager.instance.PlaySFX("Blip");

            if (PlayerShipSO.IsMax(Steering.statType))
                AudioManager.instance.PlaySFX("Ting");
        }
    }

    public void UpgradeBooster()
    {
        // dont upgrade if the stat is already maxed out
        if (PlayerShipSO.IsMax(Booster.statType))
            return;

        var cost = CalculateCost(Booster);
        //only upgrade if player can afford it
        if (cost <= PlayerShipSO.Credits)
        {
            PlayerShipSO.Credits -= cost;
            Booster.currentValue += Booster.increment;
            PlayerShipSO.UpdateStat(Booster.statType, Booster.currentValue);
            UpdateStatSet();
            AudioManager.instance.PlaySFX("Blip");

            if (PlayerShipSO.IsMax(Booster.statType))
                AudioManager.instance.PlaySFX("Ting");
        }
    }

    public void UpgradeRecovery()
    {
        // dont upgrade if the stat is already maxed out
        if (PlayerShipSO.IsMax(Recovery.statType))
            return;

        var cost = CalculateCost(Recovery);
        //only upgrade if player can afford it
        if (cost <= PlayerShipSO.Credits)
        {
            PlayerShipSO.Credits -= cost;
            Recovery.currentValue -= Recovery.increment;
            PlayerShipSO.UpdateStat(Recovery.statType, Recovery.currentValue);
            UpdateStatSet();
            AudioManager.instance.PlaySFX("Blip");

            if (PlayerShipSO.IsMax(Recovery.statType))
                AudioManager.instance.PlaySFX("Ting");
        }
    }

    private float CalculateCost(StatSet stat)
    {
        // Calculate the cost using a formula that scales with the current stat
        float cost = stat.baseCost * Mathf.Pow(stat.costMultiplier, stat.currentValue);
        if(stat.statType == StatName.COLLISSION_RECOVERY)
        {
            cost = stat.baseCost * Mathf.Pow(stat.costMultiplier, PlayerShipSO.maxShipcollisionRecovery - stat.currentValue);
        }
        cost = Mathf.Ceil(cost * 100.0f) / 100.0f;
    
        return cost;
    }

}
