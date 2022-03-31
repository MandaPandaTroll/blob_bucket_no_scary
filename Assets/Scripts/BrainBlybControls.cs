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

    nutGrid m_nutgrid;

    BlybGenome genome;


void Awake(){
    
}
    // Start is called before the first frame update
    void Start()
    {   
        
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


    float NH4_Timer;
    // Update is called once per frame
    void LateUpdate()
    {   
        Vector2 brownian = new Vector2 (Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f));
        rb.AddForce(brownian);
        if(Time.time < 0.1f && initDiversity != 0.0f){InitDiversifier(); }
        
        pEnergy = energy;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        if(alive == false)
        {  this.gameObject.tag = "Carcass";
            this.gameObject.layer = deadLayer;
            


            Dead();
        }
    if(alive == true)
    {   
        NH4_Timer += Time.deltaTime;
        if (NH4_Timer >= 0.5f && protein > 4)
        {
            NH4 +=4;
            protein -= 4;
            NH4_Timer = 0f;

        }
        //Ammonia secretion
        if(NH4 >= 32){
            int posval = m_nutgrid.GetValue(transform.position);
                m_nutgrid.SetValue(transform.position, posval + NH4);
                NH4 = 0;
        }
        eCost = rb.mass/eCostCo;
        
        int dC = (int) ( (lifeLength*Mathf.Pow((3f*lifeLength/(age+1)),2f)) - (9f*lifeLength) );
        deathDice = Random.Range(1,dC);
                // rCo = 10 + (L/a)^2
       int rCo = 10 + (int)Mathf.Pow((lifeLength/(age+1)),2f); 
        
        rDice = Random.Range(1, rCo);
        
        age += Time.deltaTime;
        statAge = age;
         
        


            if(energy >= energyToReproduce && rDice == 1 && protein >= proteinToReproduce){
                
                Reproduce();

            }



        
            if  ( energy <= 100f || age > lifeLength || deathDice == 1)
            {
                
                alive = false;               
                   
            }
            



            if(nom == true){
                
                nom = false; 
               
                Resizer();
                
                
            }

        
        }

       

            
    }

            void Dead()
        {   this.gameObject.GetComponent<BrainBlyb>().enabled = false;
            energy -= 10f*Time.deltaTime;
            int posval = m_nutgrid.GetValue(transform.position);
                m_nutgrid.SetValue(transform.position, posval + 1);
                protein -= 1;
            if(energy <= 0f && protein <= 0)
            { 
                Destroy(this.gameObject,0.2f);
            }

        }

            void OnCollisionEnter2D(Collision2D col)
            {   
                GameObject booper = col.gameObject;
             if(alive == false)
                {   
                    

                    energy = 0 ;
                    protein = 0;
                }

            if( alive == true){
                if(booper.tag == ("Carcass"))
                {               
                    if(booper.layer == 6)
                    {
                        energy += (booper.GetComponent<BrainBlobControls>().pEnergy);
                        protein += (booper.GetComponent<BrainBlobControls>().protein);
                    }
                    if(booper.layer == 7)
                    {
                        energy += (booper.GetComponent<BrainBlubControls>().pEnergy);
                        protein += (booper.GetComponent<BrainBlubControls>().protein);
                    }
                    if(booper.layer == 8)
                    {
                        energy += (booper.GetComponent<BrainBlybControls>().pEnergy);
                        protein += (booper.GetComponent<BrainBlybControls>().protein);           
                    }
                    
                    nom = true; 
                }
                if (booper.tag == ("Prey"))
                {
                   BlibControls blib = booper.GetComponent<BlibControls>();
                   energy += blib.energy;
                   protein += blib.nutLevel;

                    nom = true;
                }



                if(booper.tag == "Predator2")
                {
                    
                    
                    colorR = redGene;
                    colorG = greenGene;
                    colorB = blueGene;

                    geneticColor = new Color(colorR, colorG, colorB, colorA);
                    m_SpriteRenderer.color = geneticColor;
                    

                    }
                
                
                    

                if(booper.tag == ("ApexPred"))
                {   eaten = true;
                    
                    Destroy(gameObject, 0.2f);
                    
        
                }
                
            }
                
                
                
            }









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
                    protein = (protein/2)-1;
                    
                
                float x = energy/10000f;
                float k = 0.7f;
                sigmoid = sizeGene/ (1f+ Mathf.Exp(-k*(x-1.5f)));
                newSize = new Vector3(sigmoid,sigmoid,sigmoid);
                transform.localScale = newSize;
                    maxEnergy = sigmoid*25000f;
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
                    
                    
                    
                    
                    
                    
                     daughter.GetComponent<BlybGenome>().mutate = true;


                    rCount += 1;
                        
                        
                        
                        Resizer();

                }    
                    }





            void Resizer()
            {
                float x = energy/10000;
                float k = 0.7f;
                sigmoid = sizeGene/ (1f+ Mathf.Exp(-k*(x-1.5f)));
                newSize = new Vector3(sigmoid,sigmoid,sigmoid);
                transform.localScale = newSize;
                maxEnergy = sigmoid*25000f;
                energyToReproduce = maxEnergy /2.0f;


                     




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
