// Returns matrices with positions, which are used as sensor inputs.

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smeller_Blob : MonoBehaviour
{
    
    Vector2 here;
    Vector2 n0 = Vector2.zero;
    Vector2[] preyPositions; 
    Vector2[] matePositions; 
    Vector2[] apexPredPositions;
    Vector2[] competitorPositions;
   

    

    public Vector2[] scaledPreyDistance; 
    public Vector2[] scaledMateDistance; 
    public Vector2[] scaledApexPredDistance; 
    public Vector2[] scaledCompetitorDistance; 

    Collider2D[] smellCircleResults;
    int smellMask;
    
    float latestLookDistance;
    float smellDistance;
    BrainBlob blob;
   
    void Start()
    {

        blob = gameObject.GetComponent<BrainBlob>();
        smellMask = LayerMask.GetMask("Prey", "Predator", "Predator2", "ApexPred");
        
        latestLookDistance = blob.latestLookDistance;
        smellDistance= latestLookDistance/4f;


    preyPositions       = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    matePositions       = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    apexPredPositions   = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    competitorPositions = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    

    scaledPreyDistance          = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledMateDistance          = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledApexPredDistance      = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledCompetitorDistance    = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0};  
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        latestLookDistance = blob.latestLookDistance;
        here = gameObject.transform.position;
        smellDistance= latestLookDistance;
        Smell();

    }

    void Smell(){
    preyPositions       = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    matePositions       = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    apexPredPositions   = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    competitorPositions = new Vector2[16]{n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    

    scaledPreyDistance          = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledMateDistance          = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledApexPredDistance      = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledCompetitorDistance    = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 

            smellCircleResults = Physics2D.OverlapCircleAll(here, smellDistance,smellMask);
            if (smellCircleResults.Length > 0){
                int nPrey = 0, nMate = 0, nApex = 0, nCompetitor = 0;
                
                for(int i = 0; i < smellCircleResults.Length;i++)
                    {
                        if(smellCircleResults[i].gameObject.tag == "Prey" && nPrey < 15){

                            preyPositions[nPrey] = smellCircleResults[i].transform.position;
                            nPrey +=1;
                        }

                        if(smellCircleResults[i].gameObject.tag == "Carcass" && nPrey < 15){
                            preyPositions[nPrey] = smellCircleResults[i].transform.position;
                            nPrey += 1;
                        }

                        if(smellCircleResults[i].gameObject.tag == "Predator" && nMate < 15){
                            matePositions[nMate] = smellCircleResults[i].transform.position;
                            nMate += 1;
                        }
                        if(smellCircleResults[i].gameObject.tag == "ApexPred" && nApex < 15){
                            apexPredPositions[nApex] = smellCircleResults[i].transform.position;
                            nApex += 1;
                        }
                        if(smellCircleResults[i].gameObject.tag == "Predator2" && nCompetitor < 15){
                            competitorPositions[nCompetitor] = smellCircleResults[i].transform.position;
                            nCompetitor += 1;
                        }
                        
         
                    }
                    
                    if(nPrey > 0){
                        
                        Vector2[] tempDist = new Vector2[16];
                        for(int i = 0; i < 15; i++){
                            tempDist[i] =  ( (preyPositions[i] - here)/smellDistance);
                            
                        }
                        
                          tempDist = tempDist.OrderBy((d) => d.magnitude).ToArray();
                          tempDist = tempDist.Where(e => e.sqrMagnitude != 0).ToArray();
                          preyPositions = tempDist;



                    for(int i = 0; i < 8; i++){
                        
                            if(preyPositions[i] != n0){
                            scaledPreyDistance[i] = (preyPositions[i]);
                            
                            }else{scaledPreyDistance[i] = n0;}
                                
                            
                            
                        }
                        /*
                        Debug.Log("blob " + scaledPreyDistance[0]+","+
                        scaledPreyDistance[1]+","+
                        scaledPreyDistance[2]+","+
                        scaledPreyDistance[3]+","+
                        scaledPreyDistance[4]+","+
                        scaledPreyDistance[5]+","+
                        scaledPreyDistance[6]+","+
                        scaledPreyDistance[7]+","+
                        scaledPreyDistance[8]);
                        */
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
                    
                    if(nApex > 0){
                        
                        Vector2[] tempDist = new Vector2[16];
                        for(int i = 0; i < 15; i++){
                            tempDist[i] =  ( (apexPredPositions[i] - here)/smellDistance);
                            
                        }
                        
                          tempDist = tempDist.OrderBy((d) => d.magnitude).ToArray();
                          tempDist = tempDist.Where(e => e.sqrMagnitude != 0).ToArray();
                          apexPredPositions = tempDist;

                       

                    for(int i = 0; i < 8; i++){
                        if(apexPredPositions[i] != n0){
                            scaledApexPredDistance[i] = (apexPredPositions[i]);
                            
                            }else{scaledApexPredDistance[i] = n0;}
                        
                        }
                        
                    }
                    if(nCompetitor > 0){
                        
                        Vector2[] tempDist = new Vector2[16];
                        for(int i = 0; i < 15; i++){
                            tempDist[i] =  ( (competitorPositions[i] - here)/smellDistance);
                            
                        }
                        
                          tempDist = tempDist.OrderBy((d) => d.magnitude).ToArray();
                          tempDist = tempDist.Where(e => e.sqrMagnitude != 0).ToArray();
                          competitorPositions = tempDist;

                       

                    for(int i = 0; i < 8; i++){
                        if(competitorPositions[i] != n0){
                            scaledCompetitorDistance[i] = (competitorPositions[i]);
                            
                            }else{scaledApexPredDistance[i] = n0;}
                        
                        }
                        
                    }

                    
                    
                    

            }


    }

}
