using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class Testing : MonoBehaviour
{
    public nutGrid nutgrid;
    public Transform box;
    float diffusionTimer;
    public float diffRate;
    int grandNutes;
    GameObject[] blobs;
    GameObject[] blybs;
    GameObject[] blubs;
    GameObject[] blibs;
    int grandarr;

    List<GameObject> population = new List<GameObject>();
    int gridX;
    int gridY;
    public float cellScale;

    public int initConc;
    int totNutes;
   private void Awake() 
    {

        
        
        gridX = (int)(box.localScale.x/cellScale);
        gridY = (int)(box.localScale.x/cellScale);
        totNutes = gridX*gridY *initConc;
        nutgrid = new nutGrid((int)gridX,(int)gridY, cellScale, new Vector3(-box.localScale.x/2.0f, -box.localScale.y/2.0f));

                for (int x = 0; x< nutgrid.gridArray.GetLength(0);x++){
            for(int y = 0; y < nutgrid.gridArray.GetLength(1); y++){
                nutgrid.SetValue( x, y, initConc);
                
            }
            
        }
        
        
    }
int countNutes;
    private void FixedUpdate()
    {
        diffusionTimer += Time.deltaTime;
        if (diffusionTimer >= diffRate)
        { grandNutes = 0;
        blibs = GameObject.FindGameObjectsWithTag("Prey");
        blobs = GameObject.FindGameObjectsWithTag("Predator");
        blybs = GameObject.FindGameObjectsWithTag("Predator2");
        blubs = GameObject.FindGameObjectsWithTag("ApexPred");
            
                if(blibs.Length > 0)
                {    for (int i = 0; i < blibs.Length; i++)
                    {
                        grandNutes += blibs[i].GetComponent<BlibControls>().nutLevel;
                    }
                }
                if(blobs.Length > 0)
                {   for (int i = 0; i < blobs.Length; i++)              
                    {
                        grandNutes += blobs[i].GetComponent<BrainBlobControls>().protein;
                }   
                    }
                if(blybs.Length > 0)
                {
                    for (int i = 0; i < blybs.Length; i++)               
                    {
                        grandNutes += blybs[i].GetComponent<BrainBlybControls>().protein;
                    }    
                }
                if(blubs.Length > 0)
                {
                    for (int i = 0; i < blubs.Length; i++)                
                    {
                        grandNutes += blubs[i].GetComponent<BrainBlubControls>().protein;
                    }   
                }

            for (int x = 0; x< nutgrid.gridArray.GetLength(0);x++){
            for(int y = 0; y < nutgrid.gridArray.GetLength(1); y++){
                grandNutes += nutgrid.GetValue(x, y);
                        int[,] kernVals = new int[3,3] {

                        {nutgrid.GetValue(x-1, y-1), nutgrid.GetValue(x, y-1), nutgrid.GetValue(x+1, y-1) },
                        {nutgrid.GetValue(x-1, y),nutgrid.GetValue(x, y),nutgrid.GetValue(x+1, y) },
                        {nutgrid.GetValue(x-1, y+1),nutgrid.GetValue(x, y+1),nutgrid.GetValue(x+1, y+1) }};

                        for(int i = 0; i < 2; i++)
                        {for (int j = 0; j < 2; j++)
                            {
                                
                                if( kernVals[1,1] > 1 && kernVals[1,1] < kernVals[i,j])
                                {   

                                    kernVals[i,j] -= 1;
                                    kernVals[1,1] +=1;
                                    
                                }
                                if( kernVals[1,1] > 1 && kernVals[1,1] > kernVals[i,j])
                                {   
                                    
                                    kernVals[i,j] += 1;
                                    kernVals[1,1] -=1;
                                    
                                }



                            }
                            
                        }

                                                
                        nutgrid.SetValue(x-1, y-1, kernVals[0,0]);
                        nutgrid.SetValue(x, y-1, kernVals[0,1]);
                        nutgrid.SetValue(x+1, y-1, kernVals[0,2]);
                        nutgrid.SetValue(x-1, y, kernVals[1,0]);
                        nutgrid.SetValue(x, y, kernVals[1,1]);
                        nutgrid.SetValue(x+1, y, kernVals[1,2]);
                        nutgrid.SetValue(x-1, y+1, kernVals[2,0]);
                        nutgrid.SetValue(x, y+1, kernVals[2,1]);
                        nutgrid.SetValue(x+1, y+1, kernVals[2,2]);
                    }
                    
            }

                
            
          // Debug.Log("totNutes = " + totNutes + " , countNutes = " + countNutes + " , grandNutes = " + grandNutes);
            
           if (grandNutes < totNutes/2)
           {
                    int x = Random.Range(0,nutgrid.gridArray.GetLength(0));
                    int y = Random.Range(0,nutgrid.gridArray.GetLength(1));

                    int thisVal = nutgrid.GetValue(x,y);
                        
                        nutgrid.SetValue(x, y,thisVal + 500);
                        if(grandNutes > totNutes){FixedUpdate();}
                    
                
            }

                    
                
           

            if (grandNutes > totNutes+(totNutes/2))
           {
                     int x = Random.Range(0,nutgrid.gridArray.GetLength(0));
                    int y = Random.Range(0,nutgrid.gridArray.GetLength(1));

                    int thisVal = nutgrid.GetValue(x,y);
                        
                        nutgrid.SetValue(x, y,thisVal - 1);
                        if(grandNutes < totNutes){FixedUpdate();}

                    
                
           }

            
            diffusionTimer = 0f;
           
        } 


    }





}


