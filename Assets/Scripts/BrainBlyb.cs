using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;
// Reward signals and behaviour.

public class BrainBlyb : Agent
{
const int DONOT = 0, EXCRETE = 1, ATTACK = 2, PHAGOCYTISE = 3, CONJUGATE = 4, REPRODUCE = 5;
public bool academySpeedModifier_enabled;
CurriculumHandler curriculumHandler;
float speedModifier;

public float happiness = 0;
List<float> resourceBuffer = new List<float>();
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
//Happiness weights
public float w_energy, w_protein, w_health;


void Awake(){
genome = this.gameObject.GetComponent<BlybGenome>();
bctrl = gameObject.GetComponent<BrainBlybControls>();
smeller = gameObject.GetComponent<Smeller_Blyb>();
}
void Start() 
{
        happiness = 0;
    curriculumHandler = FindObjectOfType<CurriculumHandler>();

                if(academySpeedModifier_enabled == true){
                                speedModifier = curriculumHandler.GetBloybSpeedModifier();
                    }else{speedModifier = 1.0f;}
    box = GameObject.Find("box");
    rb = GetComponent<Rigidbody2D>();
    
    energy = bctrl.energy;
    protein = bctrl.protein;
    resourceBuffer.Add((energy/bctrl.maxEnergy)+((float)protein/(float)bctrl.proteinToReproduce));
    thisRay = GetComponent<RayPerceptionSensorComponent2D>();
    thisRay.RayLength = ((genome.lookDistAllele1+genome.lookDistAllele2)/2f);
    nomCount = 0;
    
    
    angV = rb.angularVelocity/1000.0f;

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
float  angV = 0;
Vector2 v = new Vector2(0f,0f);
int step;
float maxEnergy;
float ObsAge;

public override void OnEpisodeBegin(){
step = 0;
nomCount = 0;
booperSize = 0;
if(academySpeedModifier_enabled == true){
    curriculumHandler = FindObjectOfType<CurriculumHandler>();
                                speedModifier = curriculumHandler.GetBloybSpeedModifier();
                    }else{speedModifier = 1.0f;}
}
float scaledSmellDistance = 0;




    Vector2[] scaledPreyDistance; 
    Vector2[] scaledMateDistance; 
    Vector2[] scaledApexPredDistance; 
    Vector2[] scaledCompetitorDistance;


const int NUM_BUMP_TYPES = (int)BumperType.LastBumper;
BumperType m_currentBumper;
  float deltaResource; 
  float booperSize = 0, deltaBooperSize;  
  bool booper_isLarger = false;
public override void CollectObservations(VectorSensor sensor)
{

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




  
protein = bctrl.protein;
 maxEnergy = bctrl.maxEnergy;
 ObsAge = bctrl.age;
if(ObsAge <0f )
{ObsAge = 0f;}


scaledPreyDistance = smeller.scaledPreyDistance;
scaledMateDistance = smeller.scaledMateDistance;
scaledApexPredDistance = smeller.scaledApexPredDistance;
scaledCompetitorDistance = smeller.scaledCompetitorDistance;

 v = transform.InverseTransformDirection(rb.velocity/64.0f);
 angV = rb.angularVelocity/1000.0f;
sensor.AddObservation(protein/bctrl.proteinToReproduce);
sensor.AddObservation(v);
sensor.AddObservation(angV);


sensor.AddObservation(bctrl.energy/maxEnergy);
sensor.AddObservation(bctrl.age/bctrl.lifeLength);
sensor.AddObservation(bctrl.currentHealth/bctrl.maxHealth);
sensor.AddObservation(((float)bctrl.NH4)/128f);

for (int i = 0; i < 8; i++){
sensor.AddObservation(scaledPreyDistance[i]);
sensor.AddObservation(scaledMateDistance[i]);
sensor.AddObservation(scaledApexPredDistance[i]);
sensor.AddObservation(scaledCompetitorDistance[i]);

}


sensor.AddOneHotObservation((int)m_currentBumper, NUM_BUMP_TYPES);
sensor.AddObservation(booper_isLarger);

}
float moveForce, turnTorque;
float forwardSignal, rotSignal;
int miscActions;
float energy;
float E0, E1, ETimer;
float meanResource0, meanResource1;
public float newHappiness = 0;
//float test_reward = 0;
public override void OnActionReceived(ActionBuffers actionBuffers)
{  

        if( alive == false)
    {
        return;
    }
    
    bctrl.excreting = false;
    bctrl.tryAttack = false;
    bctrl.tryPhagocytise = false;
    bctrl.tryConjugate = false;
    bctrl.tryReproduce = false;

    energy = bctrl.energy;
    alive = bctrl.alive;
    eaten = bctrl.eaten;
    hasReproduced = bctrl.hasReproduced;
    rCount = bctrl.rCount;


    moveForce = genome.moveForce;
    turnTorque = genome.turnTorque;
    forwardSignal = actionBuffers.DiscreteActions[0];
    rotSignal = actionBuffers.DiscreteActions[1];
    miscActions = actionBuffers.DiscreteActions[2];

    float fwdMag = 0;
    float rotMag = 0;

    if(miscActions == DONOT){           
        bctrl.excreting = false;
        bctrl.tryAttack = false;
        bctrl.tryPhagocytise = false;
        bctrl.tryConjugate = false;
        bctrl.tryReproduce = false;

        }else if (miscActions != DONOT){

            if(miscActions == EXCRETE){
                bctrl.excreting = true;
            }else 
            if (miscActions == ATTACK){
                bctrl.tryAttack = true;
            }else 
            if (miscActions == PHAGOCYTISE){
                bctrl.tryPhagocytise = true;
            }else 
            if (miscActions == CONJUGATE){
                bctrl.tryConjugate = true;
            }else 
            if (miscActions == REPRODUCE){
                bctrl.tryReproduce = true;
            }
    }
if(forwardSignal == 0){
        fwdMag = 0;
    }else
     if(forwardSignal == 1){
        fwdMag = -1f;
     }
     else
     if(forwardSignal == 2){
        fwdMag = 1f;
     }
    /*
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
    */


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
        


 rb.AddForce(fwd*speedModifier);
 rb.AddTorque(rotMag*turnTorque*rb.inertia);
 bctrl.energy -=  bctrl.eCost*Mathf.Abs(fwd.magnitude)*Mathf.Pow(rb.mass,0.66f);
 bctrl.energy -= bctrl.basalMet;
    float normEnergy = bctrl.energy/bctrl.maxEnergy;
    float normProtein = (float)bctrl.protein / (float)bctrl.maxProtein;
    float normHealth = bctrl.currentHealth/bctrl.maxHealth;

    float homeo = (w_energy*normEnergy+w_protein*normProtein+w_health*normHealth)/3.0f;
    //newHappiness = 0.5f + 0.5f*(float)System.Math.Tanh((double)((4*homeo)  -2.00f));
    newHappiness = 1.0f - (1.0f/((float)System.Math.Cosh((double)(4.0f*homeo))));
    float deltaHappiness = newHappiness - happiness;

        AddReward(deltaHappiness);
        
        happiness = Mathf.Clamp(newHappiness, -1f, 1f);

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
        
        if(bctrl.energy<= 105f)
        {
        
            SetReward(0.0f);
            EndEpisode();
            this.enabled = false;
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
    bump = true;
    GameObject booper = col.gameObject;
    
    if(alive == true )
    {
        extBooper = booper;
     if (booper.tag == "ApexPred")
        {
            m_currentBumper = BumperType.ApexPred;           
            AddReward(-1.0f);
            EndEpisode();
            this.enabled = false;
        }

             if (booper.tag == "Predator" ||  booper.tag == "Predator2")
            {   
                if (booper.tag == "Predator"){
                    m_currentBumper = BumperType.Predator;
                    }
                else if (booper.tag == "Predator2"){
                    m_currentBumper = BumperType.Predator2;
                    }

                booperSize = booper.transform.localScale.x;
                if(booperSize > transform.localScale.x){
                    booper_isLarger = true;
                }else{booper_isLarger = false;}
                
            }else{booperSize = 0; booper_isLarger = false;}



         if (booper.tag == "Prey" || booper.tag == "Carcass" )
         {
             if(booper.tag == "Prey"){
                 m_currentBumper = BumperType.Prey;
             }

             if(booper.tag == "Carcass"){
                 m_currentBumper = BumperType.Carcass;
             }
             if(energy <= bctrl.maxEnergy || bctrl.protein <= bctrl.proteinToReproduce){
            //AddReward((1.0f / (float)nomLim));
             nomCount +=1;
             }
            if (nomCount >= nomLim){
                nomCount = 0;
             
            }
            
                
         }
        
        if(booper.tag == "Wall"){
            
            m_currentBumper = BumperType.Wall;
             
        }



    }

        
}

    void OnCollisionStay2D(Collision2D col){
        GameObject booper = col.gameObject;
            if( energy >= 5f &&
                booper.tag == this.gameObject.tag &&
                bctrl.tryConjugate == true && 
                booper.GetComponent<BrainBlybControls>().tryConjugate == true){

                AddReward(genome.pythagDist);
                //Debug.Log("blobPythag = " + genome.pythagDist);
                bctrl.energy -= 5f;
                }
    }


    void OnCollisionExit2D(Collision2D col)
    {
        bump = false;
        m_currentBumper = BumperType.None;
    }






}