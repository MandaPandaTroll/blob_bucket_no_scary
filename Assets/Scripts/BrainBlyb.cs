using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// Reward signals and behaviour.

public class BrainBlyb : Agent
{
public int nomLim;
 int nomCount;
RayPerceptionSensorComponent2D thisRay;
Rigidbody2D rb;
BrainBlybControls bctrl;
bool alive = true;
bool eaten = false;
bool hasReproduced = false;
bool starvation;
public int rCount;
bool bump;
GameObject box;
public static float boxExp;
float boxLength;
BlibControls[] blibControls;

GameObject extBooper;
Detector detector;

int smellMask;
Vector2 closest;
Collider2D[] smellColliders;

BlybGenome genome;
Smeller_Blyb smeller;


void Awake(){
genome = this.gameObject.GetComponent<BlybGenome>();
bctrl = gameObject.GetComponent<BrainBlybControls>();
smeller = gameObject.GetComponent<Smeller_Blyb>();
}
void Start() 
{
    
    box = GameObject.Find("box");
    rb = GetComponent<Rigidbody2D>();
    
    energy = bctrl.energy;
    thisRay = GetComponent<RayPerceptionSensorComponent2D>();
    thisRay.RayLength = ((genome.lookDistAllele1+genome.lookDistAllele2)/2f);
    nomCount = 0;
    protein = bctrl.protein;
    
    angV = rb.angularVelocity/1000.0f;
    detector = GameObject.Find("Alpha").GetComponent<Detector>();
    boxLength = box.transform.lossyScale.x;
    smellMask = LayerMask.GetMask("Prey");
    latestLookDistance = ((genome.lookDistAllele1+genome.lookDistAllele2)/2f);
    MaxScaledSmellDistance = Mathf.Sqrt( Mathf.Pow((((genome.lookDistAllele1+genome.lookDistAllele2)/2f)/4.0f),2.0f) + Mathf.Pow((((genome.lookDistAllele1+genome.lookDistAllele2)/2f)/4.0f),2.0f) );
    maxEnergy = bctrl.maxEnergy;
    ObsAge = bctrl.age;
}

float MaxScaledSmellDistance = 1f;
public float latestLookDistance = 100f;

int protein;
float  angV;
Vector2 v;
int step;
float maxEnergy;
float ObsAge;

public override void OnEpisodeBegin(){
step = 0;
nomCount = 0;
}
float scaledSmellDistance = 0, smellReward = 0;
Vector2 scaledClosest = new Vector2 (0f,0f);



    Vector2[] scaledPreyDistance; 
     Vector2[] scaledMateDistance; 
     Vector2[] scaledApexPredDistance; 
     int dbugVelCount;
     float[] vels  = new float[256];

const int NUM_BUMP_TYPES = (int)BumperType.LastBumper;
BumperType m_currentBumper;
     
public override void CollectObservations(VectorSensor sensor)
{
    /*
    dbugVelCount +=1;
    vels[dbugVelCount] = rb.velocity.magnitude;

    if(dbugVelCount >= 255){
        float meanVel = 0;
        for(int i = 0; i < 255;i++){
            meanVel += vels[i];
        }
        meanVel = meanVel/256;
        Debug.Log(meanVel);
        dbugVelCount = 0;
    }
    
     */
    step +=1;


if (bump == false)
{
    extBooper = null;
}

if(latestLookDistance != ((genome.lookDistAllele1+genome.lookDistAllele2)/2f)){
    
MaxScaledSmellDistance = Mathf.Sqrt( Mathf.Pow((((genome.lookDistAllele1+genome.lookDistAllele2)/2f)/4.0f),2.0f) + Mathf.Pow((((genome.lookDistAllele1+genome.lookDistAllele2)/2f)/4.0f),2.0f) );
latestLookDistance = ((genome.lookDistAllele1+genome.lookDistAllele2)/2f);
}
 scaledSmellDistance = closest.magnitude /MaxScaledSmellDistance;

 scaledClosest =closest/MaxScaledSmellDistance;

 smellReward = (1.0f-scaledSmellDistance)/2048f;
  
protein = bctrl.protein;
 maxEnergy = bctrl.maxEnergy;
 ObsAge = bctrl.age;
if(ObsAge <0f )
{ObsAge = 0f;}


scaledPreyDistance = smeller.scaledPreyDistance;
scaledMateDistance = smeller.scaledMateDistance;
scaledApexPredDistance = smeller.scaledApexPredDistance;
 v = transform.InverseTransformDirection(rb.velocity/64.0f);
 angV = rb.angularVelocity/1000.0f;
sensor.AddObservation(protein);
sensor.AddObservation(v);
sensor.AddObservation(angV);


sensor.AddObservation(bctrl.energy/maxEnergy);
sensor.AddObservation(bctrl.age);

for (int i = 0; i < 8; i++){
sensor.AddObservation(scaledPreyDistance[i]);
sensor.AddObservation(scaledMateDistance[i]);
sensor.AddObservation(scaledApexPredDistance[i]);

}


sensor.AddOneHotObservation((int)m_currentBumper, NUM_BUMP_TYPES);


}
float moveForce, turnTorque;
float forwardSignal, rotSignal;
float energy;
float E0, E1, ETimer;

public override void OnActionReceived(ActionBuffers actionBuffers)
{  
        
        if( alive == false)
    {
        return;
    }
    


energy = bctrl.energy;
alive = bctrl.alive;
eaten = bctrl.eaten;
hasReproduced = bctrl.hasReproduced;
rCount = bctrl.rCount;


    moveForce = genome.moveForce;
    turnTorque = genome.turnTorque;
    forwardSignal = actionBuffers.DiscreteActions[0];
    rotSignal = actionBuffers.DiscreteActions[1];
    float fwdMag = 0;
    float rotMag = 0;

    if(forwardSignal == 0)
    {
        fwdMag = -1.0f;
    }

     if(forwardSignal == 1)
    {
        fwdMag = 0.0f;
    }

     if(forwardSignal == 2)
    {
        fwdMag = 1.0f;
    }

     if(forwardSignal == 3)
    {
        fwdMag = 2.0f;
    }

     if(forwardSignal == 4)
    {
        fwdMag = 4.0f;
    }



    if(rotSignal == 0)
    {
        rotMag = -2.0f;
    }
     if(rotSignal == 1)
    {
        rotMag = -0.25f;
    }
     if(rotSignal == 2)
    {
        rotMag = 0.0f;
    }
     if(rotSignal == 3)
    {
        rotMag = 0.25f;
    }
     if(rotSignal == 4)
    {
        rotMag = 2.0f;
    }



 Vector2 fwd = transform.up*(fwdMag)*moveForce*rb.mass;
 
    if(alive == true)
    {
 rb.AddForce(fwd);
 rb.AddTorque(rotMag*turnTorque*rb.inertia);
 bctrl.energy -=  bctrl.eCost*Mathf.Abs(fwd.magnitude);
 bctrl.energy -= bctrl.basalMet;
    
        

        if(bctrl.energy<= 105f)
        {
        
            SetReward(-1.0f);
            EndEpisode();
            this.enabled = false;
        }

    if(bctrl.hasReproduced == true)
    {
        AddReward(0.5f);
        
        bctrl.hasReproduced = false;
    }






    


        }
}
  void OnCollisionEnter2D(Collision2D col)
{
        if( alive == false)
    {
        return;
    }
    bump = true;
    GameObject booper = col.gameObject;
    
    if(alive == true )
    {
        extBooper = booper;
     if (booper.tag == "ApexPred")
        {
            m_currentBumper = BumperType.ApexPred;           
            SetReward(-1.0f);
            EndEpisode();
            this.enabled = false;
        }

             if (booper.tag == "Predator" )
            {
                m_currentBumper = BumperType.Predator;
             
                
            }


            if (booper.tag == "Predator2" )
            {
                m_currentBumper = BumperType.Predator2;
                AddReward(genome.pythagDist);
                Debug.Log("blybPythag = " + genome.pythagDist);
                bctrl.energy -= 5f;
            }
            

         if (booper.tag == "Prey" || booper.tag == "Carcass" )
         {
             if(booper.tag == "Prey"){
                 m_currentBumper = BumperType.Prey;
             }

             if(booper.tag == "Carcass"){
                 m_currentBumper = BumperType.Carcass;
             }
             
            AddReward((1.0f / (float)nomLim));
             nomCount +=1;
            if (nomCount >= nomLim){
                nomCount = 0;
             EndEpisode();
            }
            
                
         }
        
        if(booper.tag == "Wall"){
            
            m_currentBumper = BumperType.Wall;
             
        }



    }

        
}




    void OnCollisionExit2D(Collision2D col)
    {
        bump = false;
        m_currentBumper = BumperType.None;
    }






}