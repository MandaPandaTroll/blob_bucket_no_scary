using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


/* This script defines how nutrients are distributed across the map.
There is some inexplicable loss of nutrients, which means that nutrients must be added for the total amount to stay roughly the same within the system*/

public class Testing : MonoBehaviour
{
    public bool autoReplenish;
    public bool autoRemove;
    public nutGrid nutgrid;
    public Transform box;
    float diffusionTimer;
    public float diffRate;
    int grandNutes;
    public int statNutes;
    public int statTot;
    GameObject[] blobs;
    GameObject[] blybs;
    GameObject[] blubs;
    GameObject[] blibs;
    int grandarr;
   
   
    List<GameObject> population = new List<GameObject>();
    public int gridX;
    public int gridY;
    public float cellScale;

    public int initConc;
    int totNutes;

     float vSliderValue = 0.0f;
     

    void OnGUI()
    {
        vSliderValue = GUI.VerticalSlider(new Rect(25, 750, 0, 250), vSliderValue, 32.0f, 0.0f);
    }
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
        
        vSliderValue = (float)initConc;
        
    }
int countNutes;
    private void FixedUpdate()
    {
        initConc = (int)Mathf.Round(vSliderValue);
        totNutes = gridX*gridY *initConc;

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
            if(autoReplenish == true){
                if (grandNutes < (totNutes - (totNutes/32)))
                {
                    int minX = nutgrid.gridArray.GetLength(0)/4;
                    int maxX = nutgrid.gridArray.GetLength(0) + (nutgrid.gridArray.GetLength(0)/4);
                    int minY = nutgrid.gridArray.GetLength(1)/4;
                    int maxY = nutgrid.gridArray.GetLength(1) + (nutgrid.gridArray.GetLength(1)/4);


                        int x = UnityEngine.Random.Range(minX,maxX);
                        int y = UnityEngine.Random.Range(minX,maxX);
                        int thisVal = nutgrid.GetValue(x,y);
                           nutgrid.SetValue(x, y,thisVal + (totNutes/64));
                    /*
                        for(int x = 0; x < nutgrid.gridArray.GetLength(0); x++){
                            for(int y = 0; y < nutgrid.gridArray.GetLength(0); y++){
                           

                                    if(grandNutes >= totNutes+(totNutes/20)){break;}
                            }
                       
                         }
                            */
                
                }

                    
                
            }
        if(autoRemove == true){

            if (grandNutes > totNutes+(totNutes/32))
           {
                for(int x = 0; x < nutgrid.gridArray.GetLength(0);x++){
                    for(int y = 0; y < nutgrid.gridArray.GetLength(0);y++){

                        int thisVal = nutgrid.GetValue(x,y);
                        if (thisVal > 0){
                            nutgrid.SetValue(x, y,thisVal - 1);
                        }

                        if(grandNutes <= totNutes){
                            FixedUpdate();}
                    }
                }

                    
                
           }
        }

            
            diffusionTimer = 0f;
           statNutes = grandNutes;
           statTot = totNutes;
        } 


    }





}


