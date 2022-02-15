//Behaviour and genetics
//tabacwoman november 2021


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrainingPreyBlobControls : MonoBehaviour
{



private Color geneticColor;
public float densityCo;
    Rigidbody2D rb;
    GameObject box;
    private  bool bump;

    private  int layer_mask; 

    public  int rDice;
    public  float age;
    public float energy = 1000;
    public float statAge;
    System.Random rndA = new System.Random();
    
  
      

    // Selection parameters
   public float moveAllele1;
   public float moveAllele2;

   float moveForce;
   public float turnTorque;
   public float turnTorqueAllele1;
   public float turnTorqueAllele2;
   public float intron1;
   public float intron2;
   public float intron3;
   public float intron4;

   public float redGene, greenGene, blueGene;
   public float redAllele1, redAllele2,greenAllele1,greenAllele2,blueAllele1,blueAllele2;
    public float initDiversity;
  public float lifeLength;
 int deathDice;
  public float turnDice;

  public float lookDistance;


  Transform boxTran;
  float boxArea;
  public int generation;

    SpriteRenderer m_SpriteRenderer;
  
    


    // Start is called before the first frame update
    void Start()
    {

       



        
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        geneticColor = m_SpriteRenderer.color;
        moveForce = (moveAllele1 + moveAllele2)/2.0f;
        rb = GetComponent<Rigidbody2D>();
       
        layer_mask = LayerMask.GetMask("Predator2");
        age = 0f + Random.Range(0f, lifeLength/2.0f);
        box = GameObject.Find("box");
        boxTran = box.GetComponent<Transform>();
        boxArea = boxTran.localScale.x*boxTran.localScale.y;

        
        
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



        
        
    }


    // Update is called once per frame
    void LateUpdate()
    {   

        



       

        energy = 150f*age;
        int dC = (int) ( (lifeLength*Mathf.Pow((3f*lifeLength/age),2f)) - (9f*lifeLength) );
        deathDice = Random.Range(1,dC);
        
        age += Time.deltaTime;
                   


                    
        statAge = age;
       



        if (bump == false )
        {
        GoForward();
        }


            if(deathDice == 1 && age > (lifeLength/2) || age >= lifeLength)
            {   
            
                Destroy(gameObject, 0.1f);
            
            }



            

    }



            void OnCollisionEnter2D(Collision2D col)

            {       
                bump = true;
                GameObject booper = col.gameObject;

                if(booper.tag == ("ApexPred"))
                {   
                    Destroy(gameObject, 0.1f);
                    
        
             
                }
                

                ContactPoint2D contact = col.GetContact(0);
                Vector2 norm = contact.normal.normalized;
                
                
                Quaternion turney = new Quaternion();
                turney.Set(0.0f, 0.0f, norm.x, norm.y);
                transform.rotation = turney;
                
                
                
            }

            void OnCollisionStay2D(Collision2D col)
            {


                
                ContactPoint2D contact = col.GetContact(0);
                float thisDir = rb.rotation*Mathf.Deg2Rad;
                Vector2 norm = contact.normal;
                
               
                rb.AddForce(contact.normal * moveForce*5f);
                
                rb.AddTorque((norm.y + norm.x )*Mathf.Rad2Deg);
                

                GoForward();
                



            }

            void OnCollisionExit2D(Collision2D col)
            {
                
                bump = false;
                GoForward();
            }




            void GoForward()

            {     
                    rb.AddForce(transform.up * moveForce*rb.mass);
                    energy -= moveForce/100f;
                    int randTurner = Random.Range(0,(int)turnDice);

                    if (randTurner == 0)
                    {
                        rb.AddTorque(turnTorque * Random.Range(-1f,1f));
                        
                    }
                    
                    Look(); 
            }


                
            



      void Look()
        {
                            //flee
            RaycastHit2D hitF = Physics2D.Raycast(transform.position,transform.up,lookDistance, layer_mask);
            RaycastHit2D hitL = Physics2D.Raycast(transform.position,-transform.right,lookDistance, layer_mask);
            RaycastHit2D hitR = Physics2D.Raycast(transform.position,transform.right,lookDistance, layer_mask);
            if (hitF.collider != null)
                 { Vector2 fleeDir = new Vector2((hitF.point.x - transform.position.x),(hitF.point.y - transform.position.y) );
                                          
                                           fleeDir.Normalize();
                    float rot_z = Mathf.Atan2(fleeDir.y, fleeDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    
                 
                 }

                if (hitL.collider != null)
                {Vector2 fleeDir = new Vector2((hitL.point.x - transform.position.x),(hitL.point.y - transform.position.y) ); 
                    
                                       fleeDir.Normalize();
                    float rot_z = Mathf.Atan2(fleeDir.y, fleeDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    
                }

                if (hitR.collider != null)
                {Vector2 fleeDir = new Vector2((hitR.point.x - transform.position.x),(hitR.point.y - transform.position.y) );
                
                                   fleeDir.Normalize();
                    float rot_z = Mathf.Atan2(fleeDir.y, fleeDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                    rb.AddForce(transform.up*moveForce*rb.mass*2f);
                    
                }
        }      
}
