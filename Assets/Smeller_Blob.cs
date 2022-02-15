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

    public Vector2[] scaledPreyDistance; 
    public Vector2[] scaledMateDistance; 
    public Vector2[] scaledApexPredDistance; 
    Collider2D[] smellCircleResults;
    int smellMask;
    
    float latestLookDistance;
    float smellDistance;
    BrainBlob blob;
   
    void Start()
    {

        blob = gameObject.GetComponent<BrainBlob>();
        smellMask = LayerMask.GetMask("Prey", "Predator", "ApexPred");
        
        latestLookDistance = blob.latestLookDistance;
        smellDistance= latestLookDistance/4f;


        preyPositions = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    matePositions = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    apexPredPositions = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 

    scaledPreyDistance = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledMateDistance = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
    scaledApexPredDistance = new Vector2[9]{n0,n0,n0,n0,n0,n0,n0,n0,n0}; 
        
    }

    // Update is called once per frame
    void Update()
    {
        latestLookDistance = blob.latestLookDistance;
        here = gameObject.transform.position;
        smellDistance= latestLookDistance/4f;
        Smell();

    }

    void Smell(){

            smellCircleResults = Physics2D.OverlapCircleAll(here, smellDistance,smellMask);
            if (smellCircleResults.Length > 0){
                int nPrey = 0, nMate = 0, nApex = 0;
                for(int i = 0; i < smellCircleResults.Length;i++)
                    {
                        if(smellCircleResults[i].gameObject.tag == "Prey" && nPrey < 8){

                            preyPositions[nPrey] = smellCircleResults[i].transform.position;
                            nPrey +=1;
                        }
                        if(smellCircleResults[i].gameObject.tag == "Predator" && nMate < 8){
                            matePositions[nMate] = smellCircleResults[i].transform.position;
                            nMate += 1;
                        }
                        if(smellCircleResults[i].gameObject.tag == "ApexPred" && nApex < 8){
                            apexPredPositions[nApex] = smellCircleResults[i].transform.position;
                            nApex += 1;
                        }
         
                    }
                    
                    if(nPrey > 0){
                    for(int i = 0; i < 8; i++){
                        if(preyPositions[i] != n0){
                            scaledPreyDistance[i] = (preyPositions[i] - here)/smellDistance;

                            }else{scaledPreyDistance[i] = n0;}
                        }
                    }


                    if(nMate > 0){
                    for(int i = 0; i < 8; i++){
                        if(matePositions[i] != n0){
                            scaledMateDistance[i] = (matePositions[i] - here)/smellDistance;

                            }else{scaledMateDistance[i] = n0;}
                        }
                    }
                    
                    if(nApex > 0){
                    for(int i = 0; i < 8; i++){
                        if(apexPredPositions[i] != n0){
                            scaledApexPredDistance[i] = (apexPredPositions[i] - here)/smellDistance;
                            
                            }else{scaledApexPredDistance[i] = n0;}
                        }
                    }
                    
                    

            }


    }

}
