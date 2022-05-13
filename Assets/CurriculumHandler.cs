using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class CurriculumHandler : MonoBehaviour
{
    EnvironmentParameters m_ResetParams;
   public float bloybSpeedModifier = 1.0f;
   int initNutes, newNuteTarget;
   float nuteModifier;
    //float eCostModifier;
    //Vector2 boxSize;
    // Start is called before the first frame update
    void Start()
    {   //boxSize = new Vector2(0,0);
        bloybSpeedModifier = Academy.Instance.EnvironmentParameters.GetWithDefault("bloybSpeedModifier", 0.0f);
        //boxSize.x = Academy.Instance.EnvironmentParameters.GetWithDefault("boxSize_x", 0.0f);
        //boxSize.y = Academy.Instance.EnvironmentParameters.GetWithDefault("boxSize_y", 0.0f);
        //eCostModifier = Academy.Instance.EnvironmentParameters.GetWithDefault("eCostModifier", 0.0f);
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public float GetBloybSpeedModifier(){
        float bloybSpeedModifier = Academy.Instance.EnvironmentParameters.GetWithDefault("bloybSpeedModifier", 0.0f);
        
        return bloybSpeedModifier;
    }

    
}
