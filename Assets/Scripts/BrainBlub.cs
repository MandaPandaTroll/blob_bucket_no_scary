using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;


// Reward signals and behaviour.

public class BrainBlub : Agent
{


float happiness = 0;
    float e;
List<float> resourceBuffer = new List<float>();

public int nomLim;
int nomCount;
Collider2D[] smellColliders;

public RayPerceptionSensorComponent2D thisRay;
Rigidbody2D rb;
BrainBlubControls bctrl;
bool alive = true;
bool eaten = false;
bool hasReproduced = false;

GameObject box;

//Collision identification sensor

int smellMask;
Vector2 closest;
Smeller_Blub smeller;

List<float> lastClosest;
void Awake(){
smeller = gameObject.GetComponent<Smeller_Blub>();
bctrl = gameObject.GetComponent<BrainBlubControls>();
e = Mathf.Log(1);
float d = 0;
float distFunc = 1.0f - Mathf.Pow( ((Mathf.Pow(e,3f*d)-Mathf.Pow(e,-3f*d))/(Mathf.Pow(e,3f*d)-Mathf.Pow(e,-3f*d))),2f );

}
void Start() 
{
    
    box = GameObject.Find("box");
    rb = GetComponent<Rigidbody2D>();
    
    energy = bctrl.energy;
    protein = bctrl.protein;
    resourceBuffer.Add((energy/bctrl.maxEnergy)+((float)protein/(float)bctrl.proteinToReproduce));
    thisRay = GetComponent<RayPerceptionSensorComponent2D>();
    thisRay.RayLength = bctrl.lookDistance;
    
    v = rb.velocity.magnitude/1000.0f;
    angV = rb.angularVelocity/1000.0f;
    smellMask = LayerMask.GetMask("Predator", "Predator2");
   latestLookDistance = bctrl.lookDistance;
MaxScaledSmellDistance = Mathf.Sqrt( Mathf.Pow((bctrl.lookDistance/4.0f),2.0f) + Mathf.Pow((bctrl.lookDistance/4.0f),2.0f) );
    
}

float MaxScaledSmellDistance;
public float latestLookDistance;
public override void OnEpisodeBegin()
{

nomCount = 0;
}



int protein;
float v, angV;

List<float> closestX = new List<float>();
List<float> closestY = new List<float>();
float scaledSmellDistance, smellReward;
Vector2 scaledClosest;

    Vector2[] scaledPreyDistance; 
     Vector2[] scaledMateDistance; 
     Vector2[] scaledBlibDistance; 
     const int NUM_BUMP_TYPES = (int)BumperType.LastBumper;
BumperType m_currentBumper;

// int obsCount;
// float cumSmellReward;


public override void CollectObservations(VectorSensor sensor)
{
//obsCount +=1;

protein = bctrl.protein;



/*
int minindex = -1;
float minDistance = Mathf.Infinity;
Vector2 smellA = new Vector2(transform.position.x -bctrl.lookDistance/4.0f, transform.position.y -bctrl.lookDistance/4.0f);
Vector2 smellB = new Vector2(transform.position.x +bctrl.lookDistance/4.0f, transform.position.y +bctrl.lookDistance/4.0f);
smellColliders = Physics2D.OverlapAreaAll(smellA,smellB,smellMask);
for(int i = 0; i < smellColliders.Length;i++){
float preyDist = (smellColliders[i].transform.position - transform.position).sqrMagnitude;
if(preyDist < minDistance)
minDistance = preyDist;
minindex = i;
}
//if (smellColliders.Length <1){closest = Vector2.zero;}
//else{closest = (smellColliders[minindex].transform.position - transform.position);}
*/
if(latestLookDistance != bctrl.lookDistance){
    
//MaxScaledSmellDistance = Mathf.Sqrt( Mathf.Pow((bctrl.lookDistance/4.0f),2.0f) + Mathf.Pow((bctrl.lookDistance/4.0f),2.0f) );
latestLookDistance = bctrl.lookDistance;
}
 //scaledSmellDistance = closest.magnitude / MaxScaledSmellDistance;

//Vector2 scaledClosest =closest/MaxScaledSmellDistance;

// smellReward = (1.0f-scaledClosest.magnitude)/1024;



/*

if(obsCount > 512){
    Debug.Log(cumSmellReward);
    cumSmellReward = 0;
    obsCount = 0;
}

if(closest.magnitude > 10f){
    Debug.Log("scaledClosest = " + scaledClosest + " " + "scaledSmellDistance = " + scaledSmellDistance);
}
if(closest.magnitude > 10f){

closestX.Add(closest.x);
closestY.Add(closest.y);

}

if(closestX.Count >= 1000 || closestY.Count >= 1000){
    float xMax = closestX.Max();
    float yMax = closestY.Max();
    
    Debug.Log(xMax +","+ yMax);
    closestX.Clear();
    closestY.Clear();
    
}
*/
scaledPreyDistance = smeller.scaledPreyDistance;
scaledMateDistance = smeller.scaledMateDistance;
scaledBlibDistance = smeller.scaledBlibDistance;

 v = rb.velocity.magnitude/1000.0f;
 angV = rb.angularVelocity/1000.0f;
//sensor.AddObservation(scaledClosest);
sensor.AddObservation(protein/bctrl.proteinToReproduce);
sensor.AddObservation(v);
sensor.AddObservation(angV); 


sensor.AddObservation(bctrl.energy/bctrl.energyToReproduce);
sensor.AddObservation(bctrl.age/bctrl.lifeLength);


for (int i = 0; i < 8; i++){
sensor.AddObservation(scaledPreyDistance[i]);
sensor.AddObservation(scaledMateDistance[i]);
sensor.AddObservation(scaledBlibDistance[i]);

    }

    sensor.AddOneHotObservation((int)m_currentBumper, NUM_BUMP_TYPES);

  
}
float moveForce, turnTorque;
float forwardSignal, rotSignal;
float energy;
float lastSqrClosest = 0;
float closeTimer = 0;
float distFunc;
float meanResource0, meanResource1;
public override void OnActionReceived(ActionBuffers actionBuffers)
{  

    


    if( alive == false)
    {
        return;
    }

    
    
    if(closeTimer <= Time.fixedDeltaTime && scaledPreyDistance[0].sqrMagnitude > 0){
        lastSqrClosest = scaledPreyDistance[0].sqrMagnitude;
        distFunc = 0;
    }
    closeTimer += Time.fixedDeltaTime;

    if(closeTimer >= 0.2f && scaledPreyDistance[0].sqrMagnitude > 0){
        float d = scaledPreyDistance[0].sqrMagnitude;
        if( d < lastSqrClosest){
            distFunc = 0;
             distFunc = 1.0f - Mathf.Pow( ((Mathf.Pow(e,3f*d)-Mathf.Pow(e,-3f*d))/(Mathf.Pow(e,3f*d)-Mathf.Pow(e,-3f*d))),2f );
            if (distFunc != distFunc) {distFunc = 0;}//else{AddReward(0);} //or distFunc
            
        }
        closeTimer = 0;
        
    }

    
    
    
energy = bctrl.energy;
alive = bctrl.alive;
eaten = bctrl.eaten;
hasReproduced = bctrl.hasReproduced;



    moveForce = bctrl.moveForce;
    turnTorque = bctrl.turnTorque;

    //Control signals from decisions
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
    float normEnergy = bctrl.energy/bctrl.maxEnergy;
    float normProtein = (float)bctrl.protein / (float)bctrl.proteinToReproduce;

    float test_enerProt = (normEnergy+normProtein)/2.0f;
    float newHappiness = 0.50f + 0.50f*(float)System.Math.Tanh((double)((4*test_enerProt)  -2.00f));
    float deltaHappiness = newHappiness - happiness;

        AddReward(deltaHappiness);
        
        happiness = newHappiness;

        /*
        resourceBuffer.Add((energy/bctrl.maxEnergy)+((float)protein/(float)bctrl.proteinToReproduce));

        if(resourceBuffer.Count >= 9){
            meanResource0 = (resourceBuffer[0]+resourceBuffer[1]+resourceBuffer[2]+
                           resourceBuffer[3]+resourceBuffer[4]+resourceBuffer[5]+
                           resourceBuffer[6]+resourceBuffer[7])
                           
                           / 8.0f;
            meanResource1 = (+resourceBuffer[8]);

             deltaResource = meanResource1 - meanResource0;
            //if(deltaResource >0 ){AddReward(1f);}
            //AddReward(deltaEnergy/bctrl.maxEnergy);
            resourceBuffer.Clear();
        }
        */



    if(bctrl.hasReproduced == true)
    {
        AddReward(1.0f);
        
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
    
    GameObject booper = col.gameObject;
    
    if(alive == true )
    {
        
            if(booper.tag == "Wall"){
            
            m_currentBumper = BumperType.Wall;
             
        }
            //conjugation reward 
             if (booper.tag == "ApexPred" && energy >= bctrl.energyToReproduce*0.75f)
            {
                m_currentBumper = BumperType.ApexPred;   
             AddReward(bctrl.geneticDistance*2f);
                
            }

            if(booper.tag == "Prey"){m_currentBumper = BumperType.Prey;}

            //Feeding reward
         if (booper.tag == "Predator" || booper.tag == "Predator2" || booper.tag == "Carcass" )
         {  if(booper.tag == "Predator"){m_currentBumper = BumperType.Predator;}
            if(booper.tag == "Predator2"){m_currentBumper = BumperType.Predator2;}
            if(booper.tag == "Carcass"){m_currentBumper = BumperType.Carcass;}
             if(m_currentBumper == BumperType.Predator || m_currentBumper == BumperType.Predator2 || m_currentBumper == BumperType.Carcass ){
                 //AddReward(1.0f);
             }
            if(energy <= bctrl.maxEnergy || bctrl.protein <= bctrl.proteinToReproduce){
            //AddReward((1.0f / (float)nomLim));
             nomCount +=1;
            }
                   if (nomCount >= nomLim){
                nomCount = 0;
             //EndEpisode();
            }
                
         }




    }

        
}




    void OnCollisionExit2D(Collision2D col)
    {
        
        m_currentBumper = BumperType.None;
    }






}