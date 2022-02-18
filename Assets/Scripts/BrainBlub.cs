using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class BrainBlub : Agent
{

Collider2D[] smellColliders;

public RayPerceptionSensorComponent2D thisRay;
Rigidbody2D rb;
BrainBlubControls bctrl;
bool alive = true;
bool eaten = false;
bool hasReproduced = false;

bool bump;
GameObject box;

//Collision identification sensor
GameObject extBooper;
int smellMask;
Vector2 closest;
Smeller_Blub smeller;

void Awake(){
smeller = gameObject.GetComponent<Smeller_Blub>();
bctrl = gameObject.GetComponent<BrainBlubControls>();
}
void Start() 
{
    box = GameObject.Find("box");
    rb = GetComponent<Rigidbody2D>();
    
    energy = bctrl.energy;
    thisRay = GetComponent<RayPerceptionSensorComponent2D>();
    thisRay.RayLength = bctrl.lookDistance;
    protein = bctrl.protein;
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


}



int protein;
float v, angV;

List<float> closestX = new List<float>();
List<float> closestY = new List<float>();
float scaledSmellDistance, smellReward;
Vector2 scaledClosest;

    Vector2[] scaledPreyDistance; 
     Vector2[] scaledMateDistance; 
     

// int obsCount;
// float cumSmellReward;
public override void CollectObservations(VectorSensor sensor)
{
//obsCount +=1;

protein = bctrl.protein;
int minindex = -1;
float minDistance = Mathf.Infinity;
if (bump == false)
{
    extBooper = null;
}
Vector2 smellA = new Vector2(transform.position.x -bctrl.lookDistance/4.0f, transform.position.y -bctrl.lookDistance/4.0f);
Vector2 smellB = new Vector2(transform.position.x +bctrl.lookDistance/4.0f, transform.position.y +bctrl.lookDistance/4.0f);
smellColliders = Physics2D.OverlapAreaAll(smellA,smellB,smellMask);
for(int i = 0; i < smellColliders.Length;i++){
float preyDist = (smellColliders[i].transform.position - transform.position).sqrMagnitude;
if(preyDist < minDistance)
minDistance = preyDist;
minindex = i;
}
if (smellColliders.Length <1){closest = Vector2.zero;}
else{closest = (smellColliders[minindex].transform.position - transform.position);}

if(latestLookDistance != bctrl.lookDistance){
    
MaxScaledSmellDistance = Mathf.Sqrt( Mathf.Pow((bctrl.lookDistance/4.0f),2.0f) + Mathf.Pow((bctrl.lookDistance/4.0f),2.0f) );
latestLookDistance = bctrl.lookDistance;
}
 scaledSmellDistance = closest.magnitude / MaxScaledSmellDistance;

Vector2 scaledClosest =closest/MaxScaledSmellDistance;

 smellReward = (1.0f-scaledSmellDistance)/2048f;



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
 v = rb.velocity.magnitude/1000.0f;
 angV = rb.angularVelocity/1000.0f;
sensor.AddObservation(scaledClosest);
sensor.AddObservation(protein);
sensor.AddObservation(v);
sensor.AddObservation(angV); 


sensor.AddObservation(bctrl.energy/bctrl.energyToReproduce);
sensor.AddObservation(bctrl.age);
for (int i = 0; i < 8; i++){
sensor.AddObservation(scaledPreyDistance[i]);
sensor.AddObservation(scaledMateDistance[i]);

}




   
}
float moveForce, turnTorque;
float forwardSignal, rotSignal;
float energy;


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
        AddReward(smellReward);
        //cumSmellReward += smellReward;
        if(bctrl.energy<= 101f)
        {
        
            SetReward(-1.0f);
            EndEpisode();
            
        }
 


    if(bctrl.hasReproduced == true)
    {
        AddReward(1.0f);
        EndEpisode();
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

            //conjugation reward or penalty depending on genetic distance
             if (booper.tag == "ApexPred" && energy >= bctrl.energyToReproduce*0.75f)
            {
             AddReward(bctrl.geneticDistance*2f);
                
            }

            

            //Feeding reward
         if (booper.tag == "Predator" || booper.tag == "Predator2" || booper.tag == "Carcass" )
         {
            AddReward(1.0f);
            EndEpisode();           
                
         }




    }

        
}




    void OnCollisionExit2D(Collision2D col)
    {
        bump = false;
        
    }








}


