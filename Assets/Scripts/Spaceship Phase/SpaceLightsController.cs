using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLightsController : MonoBehaviour
{
    
    public ParticleSystem spacelightParticles;
     ParticleSystemRenderer particleRenderer;
    public float newSpeedScale = 1.0f;
    bool ftl = false;

    void Start()
    {        
        
       particleRenderer = spacelightParticles.GetComponent<ParticleSystemRenderer>();
           
    }

    public void ToggleFTL(bool val)
    {
        particleRenderer.lengthScale =(val)? 400: 0;
       
    }

    public void AdjustSpeed(float speed)
    {
        var main = spacelightParticles.main;
        main.simulationSpeed = Mathf.Clamp(speed, .2f, 5f);
    }

}
