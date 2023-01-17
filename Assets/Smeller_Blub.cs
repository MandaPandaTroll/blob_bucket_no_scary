// Returns matrices with positions, which are used as sensor inputs.

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Smeller_Blub : MonoBehaviour
{
    
    
    Vector2 here;
    Vector2 n0 = Vector2.zero;
   public Vector2[] preyPositions; 
    Vector2[] matePositions;
    public Vector2[] blibPositions; 

    
   

    

    public Vector2[] scaledPreyDistance; 
    public Vector2[] scaledMateDistance; 
    public Vector2[] scaledBlibDistance; 

    Collider2D[] smellCircleResults;
    int smellMask;
    
    float latestLookDistance;
   public float smellDistance;
    BrainBlub Blub;
    public Vector2[] testAngles = new Vector2[16];
    public float[] angles = new float[16];
   void Awake(){
       latestLookDistance = 1f;
       smellDistance = 1f;
       preyPositions     = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
        matePositions     = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
        blibPositions     = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
     
    

        scaledPreyDistance = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
        scaledMateDistance = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0};
        scaledBlibDistance = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0};  
        smellCircleResults = new Collider2D[32];
   }
    void Start()
    {

        Blub = gameObject.GetComponent<BrainBlub>();
        smellMask = LayerMask.GetMask("Prey", "Predator", "Predator2", "ApexPred");
        
        latestLookDistance = Blub.latestLookDistance;
        smellDistance= latestLookDistance;

        
        
    
    
        
    }
   public Vector2 forward;
    // Update is called once per frame
    void FixedUpdate()
    {
        latestLookDistance = Blub.latestLookDistance;
        
        smellDistance= latestLookDistance;
        forward = new Vector2(transform.up.x,transform.up.y);
        Smell();

    }
    float[] tempAngles = new float[16];
    public Vector2[] debugPositions = new Vector2[16];
    void Smell(){
        here = gameObject.transform.position;
        preyPositions = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
        matePositions     = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
        blibPositions     = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0};
    

    scaledPreyDistance     = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledMateDistance     = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0};
    scaledBlibDistance     = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0};
     

            smellCircleResults = Physics2D.OverlapCircleAll(here, smellDistance,smellMask);
            if (smellCircleResults.Length > 0){
                int nPrey = 0, nMate = 0, nBlib = 0;
                
                for(int i = 0; i < smellCircleResults.Length;i++)
                    {
                        

                        if(smellCircleResults[i].gameObject.tag == "Carcass" && nPrey < 15){
                            preyPositions[nPrey] = smellCircleResults[i].transform.position;
                            nPrey += 1;
                        }

                        if(smellCircleResults[i].gameObject.tag == "Predator" && nPrey < 15){
                            preyPositions[nPrey] = smellCircleResults[i].transform.position;
                            nPrey += 1;
                        }

                        if(smellCircleResults[i].gameObject.tag == "Predator2" && nPrey < 15){
                            preyPositions[nPrey] = smellCircleResults[i].transform.position;
                            nPrey += 1;
                        }

                        if(smellCircleResults[i].gameObject.tag == "ApexPred" && nMate < 15){
                            matePositions[nMate] = smellCircleResults[i].transform.position;
                            nMate += 1;
                        }
                        if(smellCircleResults[i].gameObject.tag == "Prey" && nBlib < 15){
                            blibPositions[nBlib] = smellCircleResults[i].transform.position;
                            nBlib += 1;
                        }
                        
                        
                        
         
                    }
                    
                    if(nPrey > 0){
                        //Vector2[] newPos = new Vector2[16];
                        Vector2[] tempDist = new Vector2[16];
                        Vector2[] relativepos = new Vector2[16];
                        float[] relativeAngle = new float[16];
                        //float dotProd;
                        for(int i = 0; i < 15; i++){
                            if(preyPositions[i] != n0){
                                relativepos[i] =  ( preyPositions[i] - (Vector2)gameObject.transform.position);
                                //dotProd = relativepos[i].normalized.x*gameObject.transform.up.x + relativepos[i].normalized.y*gameObject.transform.up.y;
                                //float multiMag = relativepos[i].normalized.magnitude*gameObject.transform.up.magnitude;
                             relativeAngle[i] = ( Vector2.SignedAngle(transform.up,relativepos[i]))*Mathf.Deg2Rad;//Mathf.Acos((dotProd/multiMag)*Mathf.Deg2Rad);
                             
                           
                            tempDist[i] = relativepos[i]/smellDistance;//rotate(relativepos[i],relativeAngle[i])/smellDistance;
                            }else{tempDist[i] = preyPositions[i];}
                            
                           
                            
                            
                            
                            //angles[i] = Mathf.Atan2(relativepos.y, relativepos.x);
                            
                            //tempAngles[i] = transform.rotation.z+Mathf.Atan2(relativepos.y, relativepos.x) * Mathf.Rad2Deg;
                            //newPos[i].x = relativepos.x*Mathf.Cos(tempAngles[i]) + -relativepos.y*Mathf.Sin(tempAngles[i]);
                            //newPos[i].y = relativepos.x*Mathf.Sin(tempAngles[i]) + relativepos.y*Mathf.Cos(tempAngles[i]);
                            //tempDist[i] =  preyPositions[i]/smellDistance;
                            
                            debugPositions[i] = tempDist[i];
                            
                        }
                            
                          //tempDist = tempDist.OrderBy((d) => d.magnitude).ToArray();
                          //tempDist = tempDist.Where(e => e.sqrMagnitude != 0).ToArray();
                          //preyPositions = tempDist;
                        


                    for(int i = 0; i < 8; i++){
                            
                                    
                               
                        if(tempDist[i] != n0){
                            scaledPreyDistance[i] = (tempDist[i]);
                            
                            }else{scaledPreyDistance[i] = n0;}
                    
                            
                            
                            
                            
                        }
                        
                    }


                    if(nMate > 0){
                        
                        Vector2[] tempDist = new Vector2[16];
                        for(int i = 0; i < 15; i++){
                            tempDist[i] =  ( (matePositions[i] - here)/smellDistance);
                            
                        }
                        
                          tempDist = tempDist.OrderBy((d) => d.magnitude).ToArray();
                          tempDist = tempDist.Where(e => e.sqrMagnitude != 0).ToArray();
                          matePositions = tempDist;

                       

                    for(int i = 0; i < 8; i++){
                        if(matePositions[i] != n0){
                            scaledMateDistance[i] = (matePositions[i]);
                            
                            }else{scaledMateDistance[i] = n0;}
                        
                        }
                        
                    }
                    
                    if(nBlib > 0){
                        
                        Vector2[] tempDist = new Vector2[16];
                        for(int i = 0; i < 15; i++){
                            tempDist[i] =  ( (blibPositions[i] - here)/smellDistance);
                            
                        }
                        
                          tempDist = tempDist.OrderBy((d) => d.magnitude).ToArray();
                          tempDist = tempDist.Where(e => e.sqrMagnitude != 0).ToArray();
                          blibPositions = tempDist;



                    for(int i = 0; i < 8; i++){
                        
                            if(blibPositions[i] != n0){
                            scaledBlibDistance[i] = (blibPositions[i]);
                            
                            }else{scaledBlibDistance[i] = n0;}
                                
                            
                            
                        }
                        
                    }

                    
                    
                    

            }


    }
    public static Vector2 rotate(Vector2 v, float delta) { //By Boz0r, unity forums
    return new Vector2(
        v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
        v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
    );
}

}
