// Metabolic processes, physical status, etc.
//tabacwoman march 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainBlobControls : MonoBehaviour
{



public string[,] superSatellite = new string[4,27] {
    //VAGGARN
    {"A","T","G",  "G","T","T",  "G","C","T", 
     "G","G","T",  "G","G","T",  "G","C","T",
     "C","G","T",  "A","A","T",  "T","G","A"},
    
    //RASKARE
    {"A","T","G",  "C","G","T",  "G","C","T", 
     "T","C","T",  "A","A","A",  "G","C","T",
     "C","G","T",  "G","A","A",  "T","G","A"},
    //KARLARS
    {"A","T","G",  "A","A","A",  "G","C","T", 
     "C","G","T",  "C","T","T",  "G","C","T",
     "C","G","T",  "T","C","T",  "T","G","A"},
    
    //SYSTERN
    {"A","T","G",  "T","C","T",  "T","A","T", 
     "T","C","T",  "A","C","T",  "G","A","A",
     "C","G","T",  "A","A","T",  "T","G","A"},
};


public float geneticDistance;
public int rCount;
public bool hasReproduced;
public bool alive;
public bool eaten;
public bool nom;
public float eCostCo;
// Colour data sent to the sprite renderer.
float colorR;
float colorG;
float colorB;
float colorA = 1f;
Color geneticColor;
public float maxHealth = 14f;
public float currentHealth = 14f;
float alloScaleFactor = 0.66f;
//Script instance genetically related mate involved in conjugation
BrainBlobControls mate;

public float speciationDistance;

    Rigidbody2D rb;
  
   Vector3 newSize;


    public float energy;
    public float pEnergy;
    public float maxEnergy;
    public float eCost;
    
    public float conjAge =0;

 
System.Random rndA = new System.Random();

    public float age = 0;
     public float statAge;
   // Selection parameters
     public float moveAllele1;
     public float moveAllele2;
     public float moveForce;
    public float turnTorque;
    public float turnTorqueAllele1;
    public float turnTorqueAllele2;
    public float sizeAllele1, sizeAllele2, sizeGene;

    public float lookDistAllele1 = 100f, lookDistAllele2 = 100f;
    public float lookDistance = 100f;

    public float e2repAllele1, e2repAllele2;
    public float  energyToReproduce;
    
    public float lifeLengthAllele1, lifeLengthAllele2;
    public float lifeLength;
    public float intron1;
    public float intron2;
    public float intron3;
    public float intron4;
    public float redGene, greenGene, blueGene;
    public float redAllele1, redAllele2,greenAllele1,greenAllele2,blueAllele1,blueAllele2;

    public float initDiversity;
    public int rDice;
    private int deathDice;
    // Size stuff
  
    private float sigmoid;
    public int generation;
    public int deadLayer = 9;

 SpriteRenderer m_SpriteRenderer;
    public float basalMet;
    public int protein;
    public int proteinToReproduce;
    public int maxProtein;
    public int NH4;
    public int NH4_tox_lvl;
    public bool excreting, tryAttack, tryPhagocytise, tryConjugate, tryReproduce;
    

    nutGrid m_nutgrid;

    BlobGenome genome;
    BrainBlob thisBrain;

void Awake(){
    
}
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth/2;
        age = 0;
        
        genome = this.gameObject.GetComponent<BlobGenome>();
        thisBrain = this.gameObject.GetComponent<BrainBlob>();
        rCount = 0;
        eaten = false;
        alive = true;
        hasReproduced = false;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        moveForce = (moveAllele1 + moveAllele2)/2.0f;
        
        rb = GetComponent<Rigidbody2D>();

        redAllele1 = genome.redAllele1;
        redAllele2 = genome.redAllele2;
        redGene = (redAllele1 + redAllele2)/2f;

        greenAllele1 = genome.greenAllele1;
        greenAllele2 = genome.greenAllele2;
        greenGene = (greenAllele1 + greenAllele2)/2f;

        blueAllele1 = genome.blueAllele1;
        blueAllele2 = genome.blueAllele2;
        blueGene = (blueAllele1 + blueAllele2)/2f;

        moveAllele1 = genome.moveAllele1;
        moveAllele2 = genome.moveAllele2;
        if(moveAllele1 != 0 && moveAllele2 != 0){
    moveForce = (moveAllele1 + moveAllele2)/2f;
    }
        

        turnTorqueAllele1 = genome.turnTorqueAllele1;
        turnTorqueAllele2 = genome.turnTorqueAllele2;
        turnTorque = (turnTorqueAllele1 + turnTorqueAllele2)/2f;
        sizeAllele1 = genome.sizeAllele1;
        sizeAllele2 = genome.sizeAllele2;
        sizeGene = (sizeAllele1+sizeAllele2)/2f;
     

        e2repAllele1 = genome.e2repAllele1;
        e2repAllele2 = genome.e2repAllele2;
        energyToReproduce = maxEnergy/2.0f;

        lookDistAllele1 = genome.lookDistAllele1;
        lookDistAllele2 = genome.lookDistAllele2;
        lookDistance = (lookDistAllele1 + lookDistAllele2)/2f;

        lifeLengthAllele1 = genome.lifeLengthAllele1;
        lifeLengthAllele2 = genome.lifeLengthAllele2;
        lifeLength = (lifeLengthAllele1 + lifeLengthAllele2)/2f;
    

                    colorR = redGene;
                    colorG = greenGene;
                    colorB = blueGene;

                    geneticColor = new Color(colorR, colorG, colorB, colorA);
                    m_SpriteRenderer.color = geneticColor;

        
        m_nutgrid =GameObject.Find("Testing").GetComponent<Testing>().nutgrid;

        
        Resizer();
    }
    int posval;
    void FixedUpdate(){
        posval = m_nutgrid.GetValue(transform.position);
        
    }

    float NH4_Timer;
    // Update is called once per frame
    void LateUpdate()
    {   
        if(currentHealth < 0){
            currentHealth = 0;
        }
        if(Time.time < 0.1f && initDiversity != 0.0f){InitDiversifier(); }
        if(energy < 0){energy = 0;}
        if (energy > maxEnergy){energy = maxEnergy;}

        if(protein > maxProtein){
            int excessProtein = protein - maxProtein;
            NH4 += excessProtein;
            protein -= excessProtein;
        }

        pEnergy = energy;

        if(alive == false)
        {  this.gameObject.tag = "Carcass";
            this.gameObject.layer = deadLayer;
            


            Dead();
        }
    if(alive == true)
    {   
        if(energy < maxEnergy/256f){
            currentHealth += -0.1f;
        }
        if  ( age > lifeLength || deathDice == 1 || currentHealth <= 0 )
            {
                
                alive = false;               
                   
            }
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        if(tryAttack == true ||  tryConjugate == true  || tryReproduce == true || tryPhagocytise == true || excreting == true){
            if(energy >= 100.0f){
                energy += -0.01f*(newSize.x);
            }else if(energy < 100.0f){
                tryAttack = false;  tryConjugate = false; tryReproduce = false; tryPhagocytise = false; excreting = false;
            }
        }

        //Ammonia excretion
            if(excreting == true && NH4 > 0 && energy > 5f){
                m_nutgrid.SetValue(transform.position, (int)(posval + NH4));
                NH4 = 0;
                energy += -1f;
            }
        NH4_Timer += Time.deltaTime;
        if (NH4_Timer >= 8.0f && protein >= 1)
        {
            NH4 +=1;
            protein += -1;
            NH4_Timer = 0f;
            
            float NH4_excess = (float)(NH4 - NH4_tox_lvl);
            //Ammonia toxicity
            if(NH4 >= NH4_tox_lvl){

                currentHealth += 0.01f*NH4_excess;
                
                }

            
            if(energy > maxEnergy/4.00f && protein > 0 && currentHealth < maxHealth){
                energy += -0.01f*sizeGene;
                currentHealth += 1f;
                protein += -1;
                NH4 += 1;
            }

        }
        
        eCost = rb.mass/eCostCo;
        
        int dC = (int) ( (lifeLength*Mathf.Pow((3f*lifeLength/(age+1)),2f)) - (9f*lifeLength) );
        deathDice = Random.Range(1,dC);
                // rCo = 10 + (L/a)^2
       int rCo = (int)( ( 1+ (lifeLength/age)) ); 
        
        rDice = Random.Range(1, rCo);
        
        age += Time.deltaTime;
        statAge = age;
         
        

            if(tryReproduce == true){
                if( energy >= energyToReproduce && protein >= proteinToReproduce){
                
                Reproduce();

                }else if(energy >= 5f){
                    energy += -0.01f;
                }
            }






            if(nom == true){
                
                nom = false; 
               
                Resizer();
                
                
            }

        
        }

       

            
    }
        int decayCount = 0;
            void Dead()
        {   decayCount += 1;
        if(thisBrain.enabled == true){
            thisBrain.enabled = false;
        }
            if(decayCount >= 7){
            energy -= 16f;
            
            if(NH4 >= 1){
            m_nutgrid.SetValue(transform.position, (int)(posval + 1));
            NH4 += -1;
            }
            if(protein >= 1){
                protein += -1;
                NH4 += 1;
            }
            decayCount = 0;
            }
            if(energy < 1f && NH4 < 1 && protein < 1)
            { 
                Destroy(this.gameObject,0.2f);
            }

        }
            
            void OnCollisionEnter2D(Collision2D col)
            {   
                
                GameObject booper = col.gameObject;
             if(alive == false )
                {
                    if(booper.tag ==  "Predator"){
                    BrainBlobControls scavenger = booper.GetComponent<BrainBlobControls>();
                        if(scavenger.tryPhagocytise == true){
                            scavenger.protein += protein;
                            protein = 0;
                            scavenger.NH4 += NH4;
                            NH4 = 0;
                            scavenger.energy += energy;
                            energy = 0f;
                        }
                    }

                    if(booper.tag ==  "Predator2"){
                    BrainBlybControls scavenger = booper.GetComponent<BrainBlybControls>();
                        if(scavenger.tryPhagocytise == true){
                            scavenger.protein += protein;
                            protein = 0;
                            scavenger.NH4 += NH4;
                            NH4 = 0;
                            scavenger.energy += energy;
                            energy = 0f;
                        }
                    }

                    if(booper.tag ==  "ApexPred"){
                    BrainBlubControls scavenger = booper.GetComponent<BrainBlubControls>();
                    scavenger.protein += protein;
                    protein = 0;
                    scavenger.NH4 += NH4;
                    NH4 = 0;
                    scavenger.energy += energy;
                    energy = 0f;
                    }
                }

            if( alive == true){
                
                if(tryPhagocytise == true){
                    if(booper.tag == ("Carcass"))
                    {
                        nom = true; 
                    }
                    if (booper.tag == ("Prey"))
                    {
                        nom = true;
                    }
                }


                if(booper.tag == "Predator")
                {
                    BrainBlobControls contactor = booper.GetComponent<BrainBlobControls>();
                    float deltaSize = this.gameObject.transform.localScale.x - booper.gameObject.transform.localScale.x;
                    if(contactor.tryAttack == true){
                        if(deltaSize < 0 ){
                            currentHealth = 0;
                            this.gameObject.GetComponent<BrainBlob>().AddReward(-1f);
                        }
                    }
                    if(tryAttack == true){
                        if(deltaSize > 0 ){
                            contactor.currentHealth = 0;
                        }
                    }

                }

                if(booper.tag == "Predator2")
                {
                    BrainBlybControls contactor = booper.GetComponent<BrainBlybControls>();
                    float deltaSize = this.gameObject.transform.localScale.x - booper.gameObject.transform.localScale.x;
                    if(contactor.tryAttack == true){
                        if(deltaSize < 0 ){
                            this.gameObject.GetComponent<BrainBlob>().AddReward(-1f);
                            currentHealth = 0;
                            thisBrain.SetReward(-1f);
                        }
                    }
                    if(tryAttack == true){
                        if(deltaSize > 0 ){
                            contactor.currentHealth = 0;
                            
                        }
                    }

                }

                    

                if(booper.tag == ("ApexPred"))
                {   
                    BrainBlubControls hunter = booper.GetComponent<BrainBlubControls>();
                    hunter.protein += protein;
                    protein = 0;
                    hunter.NH4 += NH4;
                    NH4 = 0;
                    hunter.energy += energy;
                    energy = 0;
                    eaten = true;
                    currentHealth = 0;
                    thisBrain.SetReward(-1f);
                    thisBrain.enabled = false;
                    Destroy(gameObject,0.2f);
                    
        
                }
                
            }
                
                
                
            }

    void OnCollisionExit2D(Collision2D col){
        
    }


    public int tempProtein;
    public int tempNH4;


            void Reproduce()
            {   
                if (alive == true)
                {
                    rCount += 1;
                    

                    int satMutationRoll = rndA.Next(0,2);
                        if(satMutationRoll == 1){ 
                        int SatChunk = rndA.Next(0,4);
                        int SatIndex = rndA.Next(0,27);
                        int pointmutation = rndA.Next(0,4);
                        string pointString;
                        if      (pointmutation == 0){pointString = "A";}
                        else if (pointmutation == 1){pointString = "T";}
                        else if (pointmutation == 2){pointString = "C";}
                        else                        {pointString = "G";}
                        
                        superSatellite[SatChunk,SatIndex] = pointString;
                        }

                    hasReproduced = true;
                        List <int> randNosA = new List<int>();
                         
                        
                        for(int i = 0; i < 17; i++){
                            randNosA.Add(rndA.Next(-1,2));
                        }

                    

                    //Mutation
                    


                    

                    redGene = (redAllele1+redAllele2)/2.0f;
                    greenGene = (greenAllele1+greenAllele2)/2.0f;
                    blueGene = (blueAllele1+blueAllele2)/2.0f;



                    
                    
                    colorR = redGene;
                    colorG = greenGene;
                    colorB = blueGene;

                    geneticColor = new Color(colorR, colorG, colorB, colorA);
                    m_SpriteRenderer.color = geneticColor;
                    


                    //Reproduction

                    energy = (energy/2.0f);

                    int quotient = 0; int remainder = 0; int daughterNutes = 0; int motherNutes = 0;
                    int quotient_NH4 = 0; int remainder_NH4 = 0; int daughterNutes_NH4 = 0; int motherNutes_NH4= 0;
    if (protein > 0) {
      if(protein == 1){
        motherNutes = 1; daughterNutes = 0;
      }

      if(protein > 1){
        quotient = protein/2;
        remainder = protein%2;
        motherNutes = quotient+remainder;
        daughterNutes = quotient;
      }
                    

    }

    if (NH4 > 0) {
      if(NH4 == 1){
        motherNutes_NH4 = 1; daughterNutes_NH4 = 0;
      }

      if(NH4 > 1){
        quotient_NH4 = NH4/2;
        remainder_NH4 = NH4%2;
        motherNutes_NH4 = quotient_NH4+remainder_NH4;
        daughterNutes_NH4 = quotient_NH4;
      }
                    
        protein = 0;
        NH4 = 0;
    }
                    
                
                /*
                    bool odd = false;
                    int remainder = 0;
                    tempProtein = protein;
                    energy = (energy/2.0f);
                    if(protein % 2 == 0){
                        odd = false;
                    protein = (protein/2);
                    }
                    else{
                        remainder = protein - (protein%2);
                        odd = true;
                        protein = (protein - remainder )/2;}

                */
                float x = energy/35000f;
                float k = 1f;
                sigmoid = sizeGene/ (1f+ Mathf.Exp(-k*(x)));
                newSize = new Vector3(sigmoid,sigmoid,sigmoid);
                transform.localScale = newSize;
                NH4_tox_lvl = 16+(int)Mathf.Pow(8f,newSize.x);
                    maxEnergy = sigmoid*35000f;
                energyToReproduce = maxEnergy /4.0f;
                maxHealth = Mathf.Round(Mathf.Pow(4, newSize.x+1f));
                    if (generation == 100|| generation == 200 || generation == 300 || generation == 400 || generation == 500 || generation == 600 || generation == 800 || generation == 1000)
                    {
                        /*Debug.Log( 
                            "Blobgen "        +
                            generation        + "," + 
                            moveAllele1       + "," +
                            moveAllele2       + "," +
                            turnTorque        + "," +
                            energyToReproduce 
                                    );*/
                    }
                    GameObject daughter = Instantiate(this.gameObject);
                    daughter.GetComponent<BlobGenome>().mother = genome;
                    BrainBlobControls daughter_controls = daughter.GetComponent<BrainBlobControls>();
                    daughter_controls.generation = generation + 1;
                    daughter_controls.age = 0f;
                    /*
                    if (odd == true){
                      daughter_controls.protein = (tempProtein + remainder)/2;
                    }
                    */
                    
                    
                    
                    
                     daughter.GetComponent<BlobGenome>().mutate = true;
                    
                    daughter_controls.protein = daughterNutes;
                    daughter_controls.NH4 = daughterNutes_NH4;
                    protein = motherNutes;
                    NH4 = motherNutes_NH4;


                    rCount += 1;
                        
                        
                        
                        Resizer();

                }    
                    }





            void Resizer()
            {
                float x = energy/35000f;
                float k = 1f;
                sigmoid = sizeGene/ (1f+ Mathf.Exp(-k*(x)));
                newSize = new Vector3(sigmoid,sigmoid,sigmoid);
                transform.localScale = newSize;
                NH4_tox_lvl = 16+(int)Mathf.Pow(8f,newSize.x);
                maxEnergy = sigmoid*35000f;
                energyToReproduce = maxEnergy /4.0f;
                maxHealth = Mathf.Round(Mathf.Pow(4, newSize.x+1f));
                //Allometric scaling
                basalMet = 0.05f*Mathf.Pow(rb.mass, 1f/3f);
                float protScale = 16f*Mathf.Pow((float)sizeGene,alloScaleFactor);
                proteinToReproduce = (int)Mathf.Round(protScale);
                maxProtein = proteinToReproduce*16;
                     




            }

        void InitDiversifier()
        {
                       
                redAllele1 = genome.redAllele1;
        redAllele2 = genome.redAllele2;
        redGene = (redAllele1 + redAllele2)/2f;

        greenAllele1 = genome.greenAllele1;
        greenAllele2 = genome.greenAllele2;
        greenGene = (greenAllele1 + greenAllele2)/2f;

        blueAllele1 = genome.blueAllele1;
        blueAllele2 = genome.blueAllele2;
        blueGene = (blueAllele1 + blueAllele2)/2f;

        moveAllele1 = genome.moveAllele1;
        moveAllele2 = genome.moveAllele2;
        if(moveAllele1 != 0 && moveAllele2 != 0){
    moveForce = (moveAllele1 + moveAllele2)/2f;
    }

        turnTorqueAllele1 = 1f + genome.turnTorqueAllele1;
        turnTorqueAllele2 = 1f + genome.turnTorqueAllele2;
        turnTorque = (turnTorqueAllele1 + turnTorqueAllele2)/2f;

        sizeAllele1 = genome.sizeAllele1;
        sizeAllele2 = genome.sizeAllele2;
        sizeGene = (sizeAllele1+sizeAllele2)/2f;

        e2repAllele1 = 1f + genome.e2repAllele1;
        e2repAllele2 = 1f + genome.e2repAllele2;
        energyToReproduce = maxEnergy/4.0f;

        lookDistAllele1 = 1f + genome.lookDistAllele1;
        lookDistAllele2 = 1f + genome.lookDistAllele2;
        lookDistance = (lookDistAllele1 + lookDistAllele2)/2f;

        lifeLengthAllele1 = 1f + genome.lifeLengthAllele1;
        lifeLengthAllele2 = 1f + genome.lifeLengthAllele2;
        lifeLength = (lifeLengthAllele1 + lifeLengthAllele2)/2f;
    

                    colorR = redGene;
                    colorG = greenGene;
                    colorB = blueGene;

                    geneticColor = new Color(colorR, colorG, colorB, colorA);
                    m_SpriteRenderer.color = geneticColor;
                    



        } 


}
