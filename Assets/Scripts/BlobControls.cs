//Behaviour and genetics
//tabacwoman november 2021


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobControls : MonoBehaviour
{

private bool alive;
private bool nom;
public float eCostCo;
// Colour data sent to the sprite renderer.
float colorR;
float colorG;
float colorB;
float colorA = 1f;
Color geneticColor;

//Script instance genetically related mate involved in conjugation
BlobControls mate;

public float speciationDistance;

    Rigidbody2D rb;
  
   Vector3 newSize;

    
    private float energy = 4500f;
    public float pEnergy;
    private float eCost;
    private int layer_mask; 
    private int flee_mask;
    private int mate_mask;
    public float conjAge;

 
System.Random rndA = new System.Random();

    public float age;
     public float statAge;
   // Selection parameters
     public float moveAllele1;
     public float moveAllele2;
     private float moveForce;
    public float turnTorque;
    public float turnTorqueAllele1;
    public float turnTorqueAllele2;
    public float turnDice;
    public float lookDistance;
    public float  energyToReproduce;
    

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


    // Start is called before the first frame update
    void Start()
    {   
        alive = true;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        moveForce = (moveAllele1 + moveAllele2)/2.0f;
        geneticColor = m_SpriteRenderer.color;
        rb = GetComponent<Rigidbody2D>();

        
        layer_mask = LayerMask.GetMask("Prey","Carcass");
        flee_mask = LayerMask.GetMask("ApexPred");
        mate_mask = LayerMask.GetMask("Predator");
      

        redGene = geneticColor.r;
        greenGene = geneticColor.g;
        blueGene = geneticColor.b;

        redAllele1 = redGene;
        redAllele2 = redGene;
        greenAllele1 = greenGene;
        greenAllele2 = greenGene;
        blueAllele1 = blueGene;
        blueAllele2 = blueGene;

        redGene = (redAllele1+redAllele2)/2.0f;
        greenGene = (greenAllele1+greenAllele2)/2.0f;
        blueGene = (blueAllele1+blueAllele2)/2.0f;


        
        Resizer();
    }



    // Update is called once per frame
    void LateUpdate()
    {   
        if(Time.time < 0.1f && initDiversity != 0.0f){InitDiversifier(); }
        
        pEnergy = energy;
        if(alive == false)
        {  this.gameObject.tag = "Carcass";
            this.gameObject.layer = deadLayer;
            Dead();
        }
    if(alive == true)
    {
        eCost = rb.mass/eCostCo;
        int dC = (int) ( (lifeLength*Mathf.Pow((3f*lifeLength/age),2f)) - (9f*lifeLength) );
        deathDice = Random.Range(1,dC);
                // rCo = 10 + (L/a)^2
       int rCo = 10 + (int)Mathf.Pow((lifeLength/age),2f); 
        
        rDice = Random.Range(1, rCo);
        
        age += Time.deltaTime;
        statAge = age;
         Look();
        


            if(energy >= energyToReproduce && rDice == 1){
                
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

        




        if (energy > 100f && alive == true)
        {
            
                               
                GoForward();
                           
        }
        }

       

            
    }

            void Dead()
        {   
            energy -= 10f*Time.deltaTime;
            if(energy <= 0f)
            {
                GameObject.Destroy(gameObject);
            }

        }

            void OnCollisionEnter2D(Collision2D col)
            {   
                GameObject booper = col.gameObject;
             if(alive == false)
                {   
                    if(booper.tag == ("ApexPred"))
                    {
                        energy -= energy;
                    }

                    energy -= 100f + energy/2f;
                }

            if( alive == true){
                if(booper.tag == ("Carcass"))
                {               
                    if(booper.layer == 6)
                    {
                        energy += 100f + (booper.GetComponent<BlobControls>().pEnergy)/2f;
                    }
                    if(booper.layer == 7)
                    {
                        energy += 100f + (booper.GetComponent<BlubControls>().pEnergy)/10f;
                    }
                    if(booper.layer == 8)
                    {
                        energy += 100f + (booper.GetComponent<BlybControls>().pEnergy)/2f;
                    }
                    
                    nom = true; 
                }
                if (booper.tag == ("Prey"))
                {
                   BlibControls blib = booper.GetComponent<BlibControls>();
                   energy += blib.energy;
                    nom = true;
                }



                if(booper.tag == "Predator")
                {
                    mate = booper.GetComponent<BlobControls>();
                    float distIntron1 = Mathf.Abs(((float)intron1 - (float)mate.intron1));
                    float distIntron2 = Mathf.Abs(((float)intron2 - (float)mate.intron2));
                    float distIntron3 = Mathf.Abs(((float)intron3 - (float)mate.intron3));
                    float distIntron4 = Mathf.Abs(((float)intron4 - (float)mate.intron4));
                    float geneticDistance = (distIntron1 + distIntron2 + distIntron3 + distIntron4)/4f;
                    if(geneticDistance < speciationDistance)
                    {
                    moveAllele1 = (moveAllele1 + mate.moveAllele1)/2.0f;
                    moveAllele2 = (moveAllele2 + mate.moveAllele2)/2.0f;

                    turnTorqueAllele1 = (turnTorqueAllele1 + mate.turnTorqueAllele1)/2;
                    turnTorqueAllele2 = (turnTorqueAllele2 + mate.turnTorqueAllele2)/2;
                    
                    turnDice = (turnDice + mate.turnDice)/2.0f;
                    lookDistance = (lookDistance + mate.lookDistance)/2.0f;
                    energyToReproduce = (energyToReproduce + mate.energyToReproduce)/2.0f;
                    lifeLength = (lifeLength + mate.lifeLength)/2.0f;

                    intron1 = (intron1 + mate.intron1)/2;
                    intron2 = (intron2 + mate.intron2)/2;
                    intron3 = (intron3 + mate.intron3)/2;
                    intron4 = (intron4 + mate.intron4)/2;

                    redAllele1   = (redAllele1 + mate.redAllele1)/2.0f;
                    redAllele2   = (redAllele1 + mate.redAllele1)/2.0f;
                    greenAllele1 = (greenAllele1 + mate.greenAllele1)/2.0f;
                    greenAllele2 = (greenAllele1 + mate.greenAllele1)/2.0f;
                    blueAllele1  = (blueAllele1 + mate.blueAllele1)/2.0f;
                    blueAllele2  = (blueAllele1 + mate.blueAllele1)/2.0f;

                    redGene = (redAllele1+redAllele2)/2.0f;
                    greenGene = (greenAllele1+greenAllele2)/2.0f;
                    blueGene = (blueAllele1+blueAllele2)/2.0f;

                    conjAge = (conjAge + mate.conjAge)/2f;

        if(redAllele1 < 0.0f){redAllele1 = 0.0f;}
        if(redAllele2 < 0.0f){redAllele2 = 0.0f;}
        if(greenAllele1 < 0.0f){greenAllele1 = 0.0f;}
        if(greenAllele2 < 0.0f){greenAllele2 = 0.0f;}
        if(blueAllele1 < 0.0f){blueAllele1 = 0.0f;}
        if(blueAllele2 < 0.0f){blueAllele2 = 0.0f;}

                    if(redAllele1 > 1.0f){redAllele1 = 1.0f;}
                    if(redAllele2 > 1.0f){redAllele2 = 1.0f;}
                    if(greenAllele1 > 1.0f){greenAllele1 = 1.0f;}
                    if(greenAllele2 > 1.0f){greenAllele2 = 1.0f;}
                    if(blueAllele1 > 1.0f){blueAllele1 = 1.0f;}
                    if(blueAllele2 > 1.0f){blueAllele2 = 1.0f;}

                    if (redGene < 0.0f){
                        redGene = 0.0f;
                    }
                    if (redGene > 1.0f){
                        redGene = 1.0f;
                    }
                    if (greenGene < 0.0f){
                        greenGene = 0.0f;
                    }
                    if (greenGene > 1.0f){
                        greenGene = 1.0f;
                    }
                    if (blueGene < 0.0f){
                        blueGene = 0.0f;
                    }
                    if (blueGene > 1.0f){
                        blueGene = 1.0f;
                    }
                    
                    colorR = redGene;
                    colorG = greenGene;
                    colorB = blueGene;

                    geneticColor = new Color(colorR, colorG, colorB, colorA);
                    m_SpriteRenderer.color = geneticColor;
                    }

                    }
                
                
                    

                if(booper.tag == ("ApexPred"))
                {   
                    Destroy(gameObject, 0.2f);
                    
        
                }
                
            }
                
                
                
            }

            void OnCollisionStay2D(Collision2D col)
            { 
            GameObject booper = col.gameObject;

if(alive == true)
{

                ContactPoint2D contact = col.GetContact(0);

                if (booper.tag != ("Prey") || booper.tag != ("Predator") || booper.tag != ("Carcass"))
                {

                
                Vector2 turney = new Vector2((contact.point.x - transform.position.x),(contact.point.y - transform.position.y));
                
                    float rot_z = Mathf.Atan2(turney.y, turney.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                    rb.AddForce(transform.up*moveForce*rb.mass);
                    energy = energy - eCost * moveForce;

                    }  
                
            }
}

            void OnCollisionExit2D(Collision2D col)
            {
                          if(alive == true)
            {
                
             
                GoForward();
            }
            }




            void GoForward()

            {     
                     
                    rb.AddForce(transform.up * moveForce*rb.mass);
                    energy = energy - eCost*moveForce;
                    int randTurner = Random.Range(0,(int)turnDice);
                    if (randTurner == 0)
                    {
                        rb.AddTorque(turnTorque* rb.inertia * Random.Range(-1,2),ForceMode2D.Impulse);
                        rb.AddForce(transform.up * moveForce*rb.mass);
                    }
                rb.AddTorque(Random.Range(-1,2)*rb.inertia*turnTorque/100f);      
            }

            void Reproduce()
            {   
                if (alive == true)
                {
                    
                        List <int> randNosA = new List<int>();
                         
                        
                        for(int i = 0; i < 18; i++){
                            randNosA.Add(rndA.Next(-1,2));
                        }

                    GameObject clone;

                    //Mutation
                    moveAllele1 += (float)randNosA[0]*rndA.Next(2);
                    moveAllele2 += (float)randNosA[1]*rndA.Next(2);
                    lifeLength += (float)randNosA[2]*rndA.Next(2);
                    moveForce = (moveAllele1 + moveAllele2)/2.0f;

                    turnTorqueAllele1 += (float)randNosA[2]*rndA.Next(2);
                    turnTorqueAllele2 += (float)randNosA[3]*rndA.Next(2);
                    turnTorque = (turnTorqueAllele1 + turnTorqueAllele2)/2.0f;

                    lookDistance += (float)randNosA[4]*rndA.Next(2);
                    energyToReproduce += (float)randNosA[5]*rndA.Next(2);
                    turnDice += (float)randNosA[6]*rndA.Next(2);

                    intron1 += randNosA[7]*rndA.Next(2);
                    intron2 += randNosA[8]*rndA.Next(2);
                    intron3 += randNosA[9]*rndA.Next(2);
                    intron4 += randNosA[10]*rndA.Next(2);

                    redAllele1   += (float)randNosA[11]*rndA.Next(2)*0.01f;
                    redAllele2   += (float)randNosA[12]*rndA.Next(2)*0.01f;
                    greenAllele1 += (float)randNosA[13]*rndA.Next(2)*0.01f;
                    greenAllele2 += (float)randNosA[14]*rndA.Next(2)*0.01f;
                    blueAllele1  += (float)randNosA[15]*rndA.Next(2)*0.01f;
                    blueAllele2  += (float)randNosA[16]*rndA.Next(2)*0.01f;

                    conjAge += (float)randNosA[17]*rndA.Next(2);


                    if(redAllele1 < 0.0f){redAllele1 = 0.0f;}
                    if(redAllele2 < 0.0f){redAllele2 = 0.0f;}
                    if(greenAllele1 < 0.0f){greenAllele1 = 0.0f;}
                    if(greenAllele2 < 0.0f){greenAllele2 = 0.0f;}
                    if(blueAllele1 < 0.0f){blueAllele1 = 0.0f;} 
                    if(blueAllele2 < 0.0f){blueAllele2 = 0.0f;}

                    if(redAllele1 > 1.0f){redAllele1 = 1.0f;}
                    if(redAllele2 > 1.0f){redAllele2 = 1.0f;}
                    if(greenAllele1 > 1.0f){greenAllele1 = 1.0f;}
                    if(greenAllele2 > 1.0f){greenAllele2 = 1.0f;}
                    if(blueAllele1 > 1.0f){blueAllele1 = 1.0f;}
                    if(blueAllele2 > 1.0f){blueAllele2 = 1.0f;} 

                    redGene = (redAllele1+redAllele2)/2.0f;
                    greenGene = (greenAllele1+greenAllele2)/2.0f;
                    blueGene = (blueAllele1+blueAllele2)/2.0f;



                    if (redGene < 0.0f){
                        redGene = 0.0f;
                    }
                    if (redGene > 1.0f){
                        redGene = 1.0f;
                    }
                    if (greenGene < 0.0f){
                        greenGene = 0.0f;
                    }
                    if (greenGene > 1.0f){
                        greenGene = 1.0f;
                    }
                    if (blueGene < 0.0f){
                        blueGene = 0.0f;
                    }
                    if (blueGene > 1.0f){
                        blueGene = 1.0f;
                    }
                    
                    colorR = redGene;
                    colorG = greenGene;
                    colorB = blueGene;

                    geneticColor = new Color(colorR, colorG, colorB, colorA);
                    m_SpriteRenderer.color = geneticColor;
                    

                    //Reproduction
                    energy = (energy/2.0f);
                    
                
                float x = energy/10000f;
                float k = 0.7f;
                sigmoid = 4f/ (1f+ Mathf.Exp(-k*(x-1.5f)));
                newSize = new Vector3(sigmoid,sigmoid,sigmoid);
                transform.localScale = newSize;
                    
                    if (generation == 100|| generation == 200 || generation == 300 || generation == 400 || generation == 500 || generation == 600 || generation == 800 || generation == 1000)
                    {
                        Debug.Log( 
                            "Blobgen "        +
                            generation        + "," + 
                            moveAllele1       + "," +
                            moveAllele2       + "," +
                            turnTorque        + "," +
                            turnDice          + "," +
                            lookDistance      + "," +
                            energyToReproduce 
                                    );
                    }
                    clone = Instantiate(this.gameObject);
                    clone.GetComponent<BlobControls>().generation +=1;
                    clone.GetComponent<BlobControls>().age = 0f;
                    
                        
                        rb.AddTorque(turnTorque * rb.inertia * Random.Range(-1,2));
                        
                        Resizer();

                }    
                    }




            void Look()
            {
                //Hunt
            RaycastHit2D hitF = Physics2D.Raycast(transform.position,transform.up,lookDistance, layer_mask);
            RaycastHit2D hitL = Physics2D.Raycast(transform.position,-transform.right,lookDistance, layer_mask);
            RaycastHit2D hitR = Physics2D.Raycast(transform.position,transform.right,lookDistance, layer_mask);
            if (hitF.collider != null)
                 { Vector2 huntDir = new Vector2((hitF.point.x - transform.position.x),(hitF.point.y - transform.position.y) );
                                          
                                           huntDir.Normalize();
                    float rot_z = Mathf.Atan2(huntDir.y, huntDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    energy = energy - eCost * moveForce*2f;
                 
                 }

                if (hitL.collider != null)
                {Vector2 huntDir = new Vector2((hitL.point.x - transform.position.x),(hitL.point.y - transform.position.y) ); 
                    
                                       huntDir.Normalize();
                    float rot_z = Mathf.Atan2(huntDir.y, huntDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    energy = energy - eCost * moveForce*2f;
                }

                if (hitR.collider != null)
                {Vector2 huntDir = new Vector2((hitR.point.x - transform.position.x),(hitR.point.y - transform.position.y) );
                
                                   huntDir.Normalize();
                    float rot_z = Mathf.Atan2(huntDir.y, huntDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    energy = energy - eCost * moveForce*2f;
                }



                //Flee

            RaycastHit2D hitF2 = Physics2D.Raycast(transform.position,transform.up,lookDistance, flee_mask);
            RaycastHit2D hitL2 = Physics2D.Raycast(transform.position,-transform.right,lookDistance, flee_mask);
            RaycastHit2D hitR2 = Physics2D.Raycast(transform.position,transform.right,lookDistance, flee_mask);
            if (hitF2.collider != null)
                 { Vector2 fleeDir = new Vector2((hitF2.point.x - transform.position.x),(hitF2.point.y - transform.position.y) );
                                          
                                           fleeDir.Normalize();
                    float rot_z = Mathf.Atan2(fleeDir.y, fleeDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2);
                    energy = energy - eCost * moveForce*2f;
                 
                 }

                if (hitL2.collider != null)
                {Vector2 fleeDir = new Vector2((hitL2.point.x - transform.position.x),(hitL2.point.y - transform.position.y) ); 
                    
                                       fleeDir.Normalize();
                    float rot_z = Mathf.Atan2(fleeDir.y, fleeDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2);
                    energy = energy - eCost * moveForce*2f;
                }

                if (hitR2.collider != null)
                {Vector2 fleeDir = new Vector2((hitR2.point.x - transform.position.x),(hitR2.point.y - transform.position.y) );
                
                                   fleeDir.Normalize();
                    float rot_z = Mathf.Atan2(fleeDir.y, fleeDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2);
                    energy = energy - eCost * moveForce*2f;
                }




                //Conjugate

            if(age > (lifeLength-conjAge))
            {

            RaycastHit2D hitF3 = Physics2D.Raycast(transform.position,transform.up,lookDistance, mate_mask);
            RaycastHit2D hitL3 = Physics2D.Raycast(transform.position,-transform.right,lookDistance, mate_mask);
            RaycastHit2D hitR3 = Physics2D.Raycast(transform.position,transform.right,lookDistance, mate_mask);
            if (hitF3.collider != null)
                 { Vector2 mateDir = new Vector2((hitF3.point.x - transform.position.x),(hitF3.point.y - transform.position.y) );
                                          
                                           mateDir.Normalize();
                    float rot_z = Mathf.Atan2(mateDir.y, mateDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    energy = energy - eCost * moveForce*2f;
                 
                 }

                if (hitL3.collider != null)
                {Vector2 mateDir = new Vector2((hitL3.point.x - transform.position.x),(hitL3.point.y - transform.position.y) ); 
                    
                                       mateDir.Normalize();
                    float rot_z = Mathf.Atan2(mateDir.y, mateDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    energy = energy - eCost * moveForce*2f;
                }

                if (hitR3.collider != null)
                {Vector2 mateDir = new Vector2((hitR3.point.x - transform.position.x),(hitR3.point.y - transform.position.y) );
                
                                   mateDir.Normalize();
                    float rot_z = Mathf.Atan2(mateDir.y, mateDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    energy = energy - eCost * moveForce*2f;
                }
            }



                    GoForward();

 
            }

            void Resizer()
            {
                float x = energy/10000;
                float k = 0.7f;
                sigmoid = 4f/ (1f+ Mathf.Exp(-k*(x-1.5f)));
                newSize = new Vector3(sigmoid,sigmoid,sigmoid);
                transform.localScale = newSize;
                GoForward();
            }

        void InitDiversifier()
        {
                       

                    //Mutation
                    moveAllele1 += (float)rndA.Next(-1,2)*initDiversity;
                    moveAllele2 += (float)rndA.Next(-1,2)*initDiversity;
                    lifeLength += (float)rndA.Next(-1,2)*initDiversity;
                    moveForce = (moveAllele1 + moveAllele2)/2.0f;

                    turnTorqueAllele1 += (float)(rndA.Next(-1,2)*initDiversity);
                    turnTorqueAllele2 += (float)(rndA.Next(-1,2)*initDiversity);
                    turnTorque = (turnTorqueAllele1 + turnTorqueAllele2)/2.0f;

                    lookDistance += (float)rndA.Next(-1,2)*initDiversity;
                    energyToReproduce += (float)rndA.Next(-1,2)*initDiversity;
                    turnDice += (float)rndA.Next(-1,2)*initDiversity;

                    intron1 += (float)(rndA.Next(-1,2)*initDiversity);
                    intron2 += (float)(rndA.Next(-1,2)*initDiversity);
                    intron3 += (float)(rndA.Next(-1,2)*initDiversity);
                    intron4 += (float)(rndA.Next(-1,2)*initDiversity);

                    redAllele1   += (float)(rndA.Next(-1,2)*0.01f*initDiversity);
                    redAllele2   += (float)(rndA.Next(-1,2)*0.01f*initDiversity);
                    greenAllele1 += (float)(rndA.Next(-1,2)*0.01f*initDiversity);
                    greenAllele2 += (float)(rndA.Next(-1,2)*0.01f*initDiversity);
                    blueAllele1  += (float)(rndA.Next(-1,2)*0.01f*initDiversity);
                    blueAllele2  += (float)(rndA.Next(-1,2)*0.01f*initDiversity);

                    if(redAllele1 < 0.0f){redAllele1 = 0.0f;}
                    if(redAllele2 < 0.0f){redAllele2 = 0.0f;}
                    if(greenAllele1 < 0.0f){greenAllele1 = 0.0f;}
                    if(greenAllele2 < 0.0f){greenAllele2 = 0.0f;}
                    if(blueAllele1 < 0.0f){blueAllele1 = 0.0f;}
                    if(blueAllele2 < 0.0f){blueAllele2 = 0.0f;}

                    if(redAllele1 > 1.0f){redAllele1 = 1.0f;}
                    if(redAllele2 > 1.0f){redAllele2 = 1.0f;}
                    if(greenAllele1 > 1.0f){greenAllele1 = 1.0f;}
                    if(greenAllele2 > 1.0f){greenAllele2 = 1.0f;}
                    if(blueAllele1 > 1.0f){blueAllele1 = 1.0f;}
                    if(blueAllele2 > 1.0f){blueAllele2 = 1.0f;}
                    
                    redGene = (redAllele1+redAllele2)/2.0f;
                    greenGene = (greenAllele1+greenAllele2)/2.0f;
                    blueGene = (blueAllele1+blueAllele2)/2.0f;



        } 


}
