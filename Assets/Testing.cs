using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Linq;


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

    int grandarr;
   
   
    List<GameObject> population = new List<GameObject>();
    public int gridX;
    public int gridY;
    public float cellScale;

    public int initConc;
    int totNutes;
    public int freeNutes, lockedNutes;
    
    float perlSamp, perlDimX, perlDimY;

    float[,] perlMap;


     float vSliderValue = 0.0f;
     

    void OnGUI()
    {
        
      autoReplenish = GUI.Toggle( new Rect(10,700,200,30),autoReplenish,"autoReplenish_nutes" );
      autoRemove = GUI.Toggle( new Rect(10,750,200,30),autoRemove,"autoRemove_nutes" );
    }
   private void Awake() 
    {

        kernel = new int[3,3]{
                {0,0,0},
                {0,0,0},
                {0,0,0}
                        };
        
        totNutes = 0;
        gridX = (int)(box.localScale.x/cellScale);
        gridY = (int)(box.localScale.y/cellScale);
        perlDimX = box.localScale.x/cellScale;
        perlDimY = box.localScale.y/cellScale;

        perlMap = new float[gridX,gridY];
        
        
        nutgrid = new nutGrid((int)gridX,(int)gridY, cellScale, new Vector3(-box.localScale.x/2.0f, -box.localScale.y/2.0f));
        

                for (int x = 0; x< nutgrid.gridArray.GetLength(0);x++){
            for(int y = 0; y < nutgrid.gridArray.GetLength(1); y++){
                perlMap[x,y] = Mathf.PerlinNoise((float)x,(float)y)*initConc;
                perlMap[x,y] = Mathf.Round(perlMap[x,y]);
                nutgrid.SetValue( x, y, (int)perlMap[x,y]);
                totNutes += (int)perlMap[x,y];
                
            }
            
        }
        totNutes =nutgrid.gridArray.Cast<int>().Sum();
        
        freeNutes = totNutes;
        GameObject spawner = GameObject.Find("Spawner");
        totNutes += (spawner.GetComponent<BlibSpawner>().initProtein)*(spawner.GetComponent<BlibSpawner>().initBlib); 

            newArray = nutgrid.gridArray;
            int gridWidth = newArray.GetLength(0); 
            int gridHeight = newArray.GetLength(1); 
        
    }
        int newNuteTarget;
        void Start(){
            
        }

            int blibNutes, blobNutes, blybNutes, blubNutes;

            
            int[,] kernel;

            int[,] newArray;
            int gridWidth; 
            int gridHeight;
            GameObject[] blibs;
            GameObject[] blobs;
            GameObject[] blybs;
            GameObject[] blubs;

    private void FixedUpdate()
    {
        
        
        
        initConc = (int)Mathf.Round(vSliderValue);
        

        diffusionTimer += Time.fixedDeltaTime;
        if (diffusionTimer >= diffRate)
        { 
            
            blibNutes = 0; blobNutes = 0; blybNutes = 0; blubNutes = 0;
            
            grandNutes = 0;
            
            lockedNutes = 0;

            
            
             blibs = GameObject.FindGameObjectsWithTag("Prey");
             blobs = GameObject.FindGameObjectsWithTag("Predator");
             blybs = GameObject.FindGameObjectsWithTag("Predator2");
             blubs = GameObject.FindGameObjectsWithTag("ApexPred");
                for(int i = 0; i < blibs.Length; i++){
                blibNutes += blibs[i].GetComponent<BlibControls>().nutLevel;
                 }
                 System.Array.Clear(blibs, 0, blibs.Length);

                 for(int i = 0; i < blobs.Length; i++){
                blobNutes += blobs[i].GetComponent<BrainBlobControls>().protein;
                blobNutes += blobs[i].GetComponent<BrainBlobControls>().NH4;
                 }
                 System.Array.Clear(blobs, 0, blobs.Length);

                 for(int i = 0; i < blybs.Length; i++){
                blybNutes += blybs[i].GetComponent<BrainBlybControls>().protein;
                blybNutes += blybs[i].GetComponent<BrainBlybControls>().NH4;
                 }
                 System.Array.Clear(blybs, 0, blybs.Length);
                 for(int i = 0; i < blubs.Length; i++){
                blubNutes += blubs[i].GetComponent<BrainBlubControls>().protein;
                blubNutes += blubs[i].GetComponent<BrainBlubControls>().NH4;
                 }
                 System.Array.Clear(blubs, 0, blubs.Length);


                lockedNutes = blibNutes+blobNutes+blybNutes+blubNutes;
                blibNutes = 0; blobNutes = 0; blybNutes = 0; blubNutes = 0;
                
            
            
            
            
                
                
                /*Box blur kernel from wikipedia

                Box blur (image)
{
    set newImage to image;
    For x /row/, y/column/ on newImage do:
    {
        // Kernel would not fit!
        If x < 1 or y < 1 or x + 1 == width or y + 1 == height then:
            Continue;
        // Set P to the average of 9 pixels:
           X X X
           X P X
           X X X
        // Calculate average.
        Sum = image[x - 1, y + 1] + // Top left
              image[x + 0, y + 1] + // Top center
              image[x + 1, y + 1] + // Top right
              image[x - 1, y + 0] + // Mid left
              image[x + 0, y + 0] + // Current pixel
              image[x + 1, y + 0] + // Mid right
              image[x - 1, y - 1] + // Low left
              image[x + 0, y - 1] + // Low center
              image[x + 1, y - 1];  // Low right

        newImage[x, y] = Sum / 9;
    }
    Return newImage;
}
                */
            
            
            
            
            for (int x = 1; x< gridWidth-1;x++){
            for(int y = 1; y < gridHeight-1; y++){

                        kernel[0,2] = newArray[x - 1, y + 1];  // Top left
                        kernel[1,2] = newArray[x + 0, y + 1];  // Top center
                        kernel[2,2] = newArray[x + 1, y + 1];  // Top right
                        kernel[0,1] = newArray[x - 1, y + 0];  // Mid left
                        kernel[1,1] = newArray[x + 0, y + 0];  // Current pixel
                        kernel[2,1] = newArray[x + 1, y + 0];  // Mid right
                        kernel[0,0] = newArray[x - 1, y - 1];  // Low left
                        kernel[1,0] = newArray[x + 0, y - 1];  // Low center
                        kernel[2,0] = newArray[x + 1, y - 1];  // Low right
                        //sumPrevKernel = kernel.Cast<int>().Sum();

                        if(kernel[1,1] < kernel[0,2] && kernel[0,2] > 1){
                            kernel[1,1] += 1; kernel[0,2] -= 1;
                        }
                        if(kernel[1,1] < kernel[1,2] && kernel[1,2] > 1){
                            kernel[1,1] += 1; kernel[1,2] -= 1;
                        }
                        if(kernel[1,1] < kernel[2,2] && kernel[2,2] > 1){
                            kernel[1,1] += 1; kernel[2,2] -= 1;
                        }
                        if(kernel[1,1] < kernel[0,1] && kernel[0,1] > 1){
                            kernel[1,1] += 1; kernel[0,1] -= 1;
                        }
                        if(kernel[1,1] < kernel[2,1] && kernel[2,1] > 1){
                            kernel[1,1] += 1; kernel[2,1] -= 1;
                        }
                        if(kernel[1,1] < kernel[0,0] && kernel[0,0] > 1){
                            kernel[1,1] += 1; kernel[0,0] -= 1;
                        }
                        if(kernel[1,1] < kernel[1,0] && kernel[1,0] > 1){
                            kernel[1,1] += 1; kernel[0,1] -= 1;
                        }
                        if(kernel[1,1] < kernel[2,0] && kernel[2,0] > 1){
                            kernel[1,1] += 1; kernel[2,0] -= 1;
                        }

                        newArray[x - 1, y + 1] = kernel[0,2];  // Top left
                        newArray[x + 0, y + 1] = kernel[1,2];  // Top center
                        newArray[x + 1, y + 1] = kernel[2,2];  // Top right
                        newArray[x - 1, y + 0] = kernel[0,1];  // Mid left
                        newArray[x + 0, y + 0] = kernel[1,1];  // Current pixel
                        newArray[x + 1, y + 0] = kernel[2,1];  // Mid right
                        newArray[x - 1, y - 1] = kernel[0,0];  // Low left
                        newArray[x + 0, y - 1] = kernel[1,0];  // Low center
                        newArray[x + 1, y - 1] = kernel[2,0];  // Low right

                        if(x == 1 && 
                           y > 1  && 
                           y < gridHeight-1 ){
                               newArray[2,y] = newArray[x,y]; 
                               newArray[x,y] = 0;}

                        if(x == gridWidth-1 &&
                           y > 1              && 
                           y < gridHeight-1 ){
                            newArray[gridWidth-2,y] = newArray[x,y];
                            newArray[x,y] = 0;}

                        if(y == 1   && 
                            x > 1   && 
                            x < gridWidth-1 ){
                                newArray[x,2] = newArray[x,y]; 
                                newArray[x,y] = 0;}

                        if(y == gridHeight-1 &&
                           x > 1              && 
                           y < gridWidth-1 ){
                            newArray[x,gridHeight-2] = newArray[x,y];
                            newArray[x,y] = 0;}
                /*
                sum =   newArray[x - 1, y + 1] + // Top left
                        newArray[x + 0, y + 1] + // Top center
                        newArray[x + 1, y + 1] + // Top right
                        newArray[x - 1, y + 0] + // Mid left
                        newArray[x + 0, y + 0] + // Current pixel
                        newArray[x + 1, y + 0] + // Mid right
                        newArray[x - 1, y - 1] + // Low left
                        newArray[x + 0, y - 1] + // Low center
                        newArray[x + 1, y - 1];  // Low right
                        meanP = (float)sum /9.0f;
                        resultP = (int)Mathf.Round(meanP);
                        newArray[x,y] = resultP;
                        */

                }
            }
                    
                    nutgrid.gridArray = newArray;

            freeNutes = 0;

            /*
            for (int x = 1; x< nutgrid.gridArray.GetLength(0)-1;x++){
            for(int y = 1; y < nutgrid.gridArray.GetLength(1)-1; y++){
                
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
                */
                freeNutes =nutgrid.GetSum();
                //for (int x = 0; x< nutgrid.gridArray.GetLength(0);x++){
                //for(int y = 0; y < nutgrid.gridArray.GetLength(1); y++){
                //freeNutes += nutgrid.GetValue(x, y);
                    //}
                //}

                grandNutes = freeNutes + lockedNutes;



                
            
          // Debug.Log("totNutes = " + totNutes + " , countNutes = " + countNutes + " , grandNutes = " + grandNutes);
            if(autoReplenish == true){
                if (grandNutes < (totNutes - (totNutes/32)))
                {
                    /*
                    int minX = nutgrid.gridArray.GetLength(0)/4;
                    int maxX = nutgrid.gridArray.GetLength(0) + (nutgrid.gridArray.GetLength(0)/4);
                    int minY = nutgrid.gridArray.GetLength(1)/4;
                    int maxY = nutgrid.gridArray.GetLength(1) + (nutgrid.gridArray.GetLength(1)/4);


                        int x = UnityEngine.Random.Range(minX,maxX);
                        int y = UnityEngine.Random.Range(minX,maxX);
                        int thisVal = nutgrid.GetValue(x,y);
                           nutgrid.SetValue(x, y,thisVal + (totNutes/64));
                    */
                        for(int x = 0; x < nutgrid.gridArray.GetLength(0); x++){
                            for(int y = 0; y < nutgrid.gridArray.GetLength(1); y++){

                                int thisVal = nutgrid.GetValue(x,y);
                                    nutgrid.SetValue(x, y,thisVal + 1);
                                    
                            }
                       
                         }
                            
                
                }

                    
                
            }
        if(autoRemove == true){

            if(freeNutes > 0 && grandNutes > totNutes)
           {freeNutes =nutgrid.GetSum();
                        grandNutes = freeNutes + lockedNutes;
                for(int x = 0; x < nutgrid.gridArray.GetLength(0);x++){

                    

                    for(int y = 0; y < nutgrid.gridArray.GetLength(1);y++){
                        
                        
                        int thisVal = nutgrid.GetValue(x, y);
                        if (thisVal > 0){
                            nutgrid.SetValue(x, y , thisVal - 1);
                            
                            
                        }

                        
                    }
                }
                freeNutes =nutgrid.GetSum();
                grandNutes = freeNutes + lockedNutes;       
           }
        }

            
            diffusionTimer = 0f;
           statNutes = grandNutes;
           statTot = totNutes;
        } 


    }





}


