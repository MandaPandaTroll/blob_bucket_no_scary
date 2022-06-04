// Metabolic processes, physical status, etc.
//tabacwoman march 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainBlybControls : MonoBehaviour
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
public float maxHealth = 256f;
public float currentHealth = 256f;
bool bump;
//Script instance genetically related mate involved in conjugation
BrainBlybControls mate;

public float speciationDistance;

    Rigidbody2D rb;
  
   Vector3 newSize;


    public float energy = 4500f;
    public float pEnergy;
    public float maxEnergy;
    public float eCost;

    public float conjAge =0;

 
System.Random rndA = new System.Random();

    public float age;
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
    public int NH4;
    public bool excreting, tryAttack, tryPhagocytise, tryConjugate, tryReproduce;
    

    nutGrid m_nutgrid;

    BlybGenome genome;


void Awake(){
    
}
    // Start is called before the first frame update
    void Start()
    {
        age = 0;
        
        genome = this.gameObject.GetComponent<BlybGenome>();
        
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
        
        if(Time.time < 0.1f && initDiversity != 0.0f){InitDiversifier(); }
        if(energy < 0){energy = 0;}
        if (energy > maxEnergy){energy = maxEnergy;}
        if(energy < maxEnergy/64){
            currentHealth += -0.1f;
        }

        pEnergy = energy;

        if(alive == false)
        {  this.gameObject.tag = "Carcass";
            this.gameObject.layer = deadLayer;
            


            Dead();
        }
    if(alive == true)
    {   
        if  ( age > lifeLength || deathDice == 1 || currentHealth <= 0 )
            {
                
                alive = false;               
                   
            }
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        if(tryAttack == true ||  tryConjugate == true || tryPhagocytise == true){
            if(bump == false && energy >= 5f){
                energy += -0.5f;
            }
        }

        //Ammonia excretion
            if(excreting == true && NH4 > 0 && energy > 5f){
                m_nutgrid.SetValue(transform.position, posval + NH4);
                NH4 = 0;
                energy += -5f;
            }
        NH4_Timer += Time.deltaTime;
        if (NH4_Timer >= 3.0f && protein >= 1)
        {
            NH4 +=1;
            protein += -1;
            NH4_Timer = 0f;
            
            //Ammonia toxicity
            if(NH4 >= (int)Mathf.Pow(2f,sizeGene)){
                currentHealth += -1f;
                
                }

            
            if(energy > maxEnergy/2.00f && protein > 0 && currentHealth < maxHealth){
                energy += -sizeGene;
                currentHealth += 1f;
                protein += -1;
                NH4 += 1;
            }

        }
        
        eCost = rb.mass/eCostCo;
        
        int dC = (int) ( (lifeLength*Mathf.Pow((3f*lifeLength/(age+1)),2f)) - (9f*lifeLength) );
        deathDice = Random.Range(1,dC);
                // rCo = 10 + (L/a)^2
       int rCo = (int)( lifeLength*( (lifeLength/age) -1) ); 
        
        rDice = Random.Range(1, rCo);
        
        age += Time.deltaTime;
        statAge = age;
         
        

            if(tryReproduce == true){
                if( energy >= energyToReproduce && protein >= proteinToReproduce && rDice == 1){
                
                Reproduce();

                }else if(energy >= 5f){
                    energy += -0.5f;
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
            if(this.gameObject.GetComponent<BrainBlyb>().enabled == true){
                this.gameObject.GetComponent<BrainBlyb>().enabled = false;
        }
            if(decayCount >= 7){
            energy -= 16f;
            
            if(NH4 >= 1){
            m_nutgrid.SetValue(transform.position, posval + 1);
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
                bump = true;
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
                            currentHealth += deltaSize*deltaSize;
                        }
                    }
                    if(tryAttack == true){
                        if(deltaSize > 0 ){
                            contactor.currentHealth += deltaSize*deltaSize*(-1f);
                        }
                    }

                }

                if(booper.tag == "Predator2")
                {
                    BrainBlybControls contactor = booper.GetComponent<BrainBlybControls>();
                    float deltaSize = this.gameObject.transform.localScale.x - booper.gameObject.transform.localScale.x;
                    if(contactor.tryAttack == true){
                        if(deltaSize < 0 ){
                            currentHealth += deltaSize*deltaSize;
                        }
                    }
                    if(tryAttack == true){
                        if(deltaSize > 0 ){
                            contactor.currentHealth += deltaSize*deltaSize*(-1f);
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
                    gameObject.GetComponent<BrainBlyb>().enabled = false;
                    Destroy(gameObject,0.2f);
                    
        
                }
                
            }
                
                
                
            }

    void OnCollisionExit2D(Collision2D col){
        bump = false;
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
                    bool odd_protein = false;
                    bool odd_NH4 = false;
                    
                if(protein > 0){



                    if(protein == 1){
                        odd_protein = true;
                        tempProtein = 0;
                    }else if (protein > 1){

                        if(protein % 2 == 0){
                        odd_protein = false;
                        tempProtein = (protein/2);
                        }
                        else{
                        odd_protein = true;
                        tempProtein = (protein -1 )/2;}
                        
                    }
                    protein = 0;
                }

                if(NH4 > 0){



                    if(NH4 == 1){
                        odd_NH4 = true;
                        tempNH4 = 0;
                    }else if (protein > 1){

                        if(protein % 2 == 0){
                        odd_NH4 = false;
                        tempNH4 = (NH4/2);
                        }
                        else{
                        odd_NH4 = true;
                        tempNH4 = (NH4 -1 )/2;}
                        
                    }
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
                    maxEnergy = sigmoid*35000f;
                energyToReproduce = maxEnergy /2.0f;
                maxHealth = (int)Mathf.Round(Mathf.Pow(4, newSize.x+1f));
                    if (generation == 100|| generation == 200 || generation == 300 || generation == 400 || generation == 500 || generation == 600 || generation == 800 || generation == 1000)
                    {
                        Debug.Log( 
                            "Blybgen "        +
                            generation        + "," + 
                            moveAllele1       + "," +
                            moveAllele2       + "," +
                            turnTorque        + "," +
                            energyToReproduce 
                                    );
                    }
                    GameObject daughter = Instantiate(this.gameObject);
                    daughter.GetComponent<BlybGenome>().mother = genome;
                    BrainBlybControls daughter_controls = daughter.GetComponent<BrainBlybControls>();
                    daughter_controls.generation = generation + 1;
                    daughter_controls.age = 0f;
                    /*
                    if (odd == true){
                      daughter_controls.protein = (tempProtein + remainder)/2;
                    }
                    */
                    
                    
                    
                    
                     daughter.GetComponent<BlybGenome>().mutate = true;
                    if(odd_protein == true){
                        daughter_controls.protein = tempProtein +1;
                    }else{daughter_controls.protein = tempProtein;}
                    
                    protein = tempProtein;

                    if(odd_NH4 == true){
                        daughter_controls.NH4 = tempNH4 +1;
                    }else{daughter_controls.NH4 = tempNH4;}
                    
                    NH4 = tempNH4;


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
                maxEnergy = sigmoid*35000f;
                energyToReproduce = maxEnergy /2.0f;
                maxHealth = (int)Mathf.Round(Mathf.Pow(4, newSize.x+1f));

                     




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
        energyToReproduce = maxEnergy/2.0f;

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
