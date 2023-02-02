
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


/* This script defines how nutrients are distributed across the map.
There is some inexplicable loss of nutrients, which means that nutrients must be added for the total amount to stay roughly the same within the system*/

public class Testing : MonoBehaviour
{

     System.Random rndA = new System.Random();
    public bool spawnAllInCenter;
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

    public float blibLocked, blobLocked, blybLocked, blubLocked;

    public int spawnRangeDivider;

    public int numRandomMiddle;
    public int spawndivider;
    public bool spawnInCorners;
    void OnGUI()
    {

        autoReplenish = GUI.Toggle(new Rect(10, 700, 200, 30), autoReplenish, "autoReplenish_nutes");
        autoRemove = GUI.Toggle(new Rect(10, 750, 200, 30), autoRemove, "autoRemove_nutes");
    }
    private void Awake()
    {
        
        kernel = new int[3, 3]{
                {0,0,0},
                {0,0,0},
                {0,0,0}
                        };

        totNutes = 0;
        gridX = (int)(box.localScale.x / cellScale);
        gridY = (int)(box.localScale.y / cellScale);
        perlDimX = box.localScale.x / cellScale;
        perlDimY = box.localScale.y / cellScale;
        CustomMethods.boxdims.x = box.localScale.x/2.0f;
        CustomMethods.boxdims.y = box.localScale.y/2.0f;
        CustomMethods.gridDims[0] = gridX;
        CustomMethods.gridDims[1] = gridY;
        perlMap = new float[gridX, gridY];

        int numCells = gridX*gridY;
        nutgrid = new nutGrid((int)gridX, (int)gridY, cellScale, new Vector3(-box.localScale.x / 2.0f, -box.localScale.y / 2.0f));

        if(spawnAllInCenter == true){
            float spawnRange_x = ((box.localScale.x/2f)-(float)cellScale)/spawnRangeDivider;
            float spawnRange_y = ((box.localScale.y/2f)-(float)cellScale)/spawnRangeDivider;
            for(int i = 0; i < numRandomMiddle; i++){
            nutgrid.SetValue(new Vector3(UnityEngine.Random.Range(-spawnRange_x,spawnRange_x),UnityEngine.Random.Range(-spawnRange_y,spawnRange_y),0f),(numCells*initConc/(numRandomMiddle))/spawndivider);
            }
            
            
            
        }else if(spawnInCorners == true){
            nutgrid.SetValue(1,1,((numCells*initConc)/4)/spawndivider);
            nutgrid.SetValue(gridX-1,1,((numCells*initConc)/4)/spawndivider);
            nutgrid.SetValue(1,gridY-1,((numCells*initConc)/4)/spawndivider);
            nutgrid.SetValue(gridX-1,gridY-1,((numCells*initConc)/4)/spawndivider);
        }
        else{
            for (int x = 0; x < nutgrid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < nutgrid.gridArray.GetLength(1); y++)
            {
                int floorCeil = Random.Range(1, 3);
                perlMap[x, y] = Mathf.PerlinNoise((float)x, (float)y) * initConc;
                if (floorCeil == 1)
                {
                    perlMap[x, y] = Mathf.FloorToInt(perlMap[x, y]);
                }
                else if (floorCeil == 2)
                {
                    perlMap[x, y] = Mathf.CeilToInt(perlMap[x, y]);
                }

                nutgrid.SetValue(x, y, (int)perlMap[x, y]);
                totNutes += (int)perlMap[x, y];

            }

            }
        }
        


        totNutes = nutgrid.gridArray.Cast<int>().Sum();

        freeNutes = totNutes;
        GameObject spawner = GameObject.Find("Spawner");
        totNutes += (spawner.GetComponent<BlibSpawner>().initProtein) * (spawner.GetComponent<BlibSpawner>().initBlib);
        totNutes += (spawner.GetComponent<BlobSpawner>().initProtein) * (spawner.GetComponent<BlobSpawner>().initBlob);
        totNutes += (spawner.GetComponent<BlybSpawner>().initProtein) * (spawner.GetComponent<BlybSpawner>().initBlyb);

        newArray = nutgrid.gridArray;
        int gridWidth = newArray.GetLength(0);
        int gridHeight = newArray.GetLength(1);

    }
    int newNuteTarget;
    void Start()
    {

    }

    int blibNutes, blobNutes, blybNutes, blubNutes, carcassNutes;
    public int extBlibNutes, extBlobNutes, extBlybNutes, extBlubNutes, extCarcassNutes;

    int[,] kernel;

    int[,] newArray;
    int gridWidth;
    int gridHeight;
    GameObject[] blibs;
    GameObject[] blobs;
    GameObject[] blybs;
    GameObject[] blubs;
    GameObject[] carcasses;
    public bool diffusionEnabled;
    public int diffusionLimit;
    int dirCounter = 0;
    private void Update()
    {
        





        diffusionTimer += Time.deltaTime;
        if (diffusionTimer >= diffRate)
        {
            
            System.Array.Clear(kernel,0,kernel.Length);
            newArray = nutgrid.gridArray;
            blibNutes = 0; blobNutes = 0; blybNutes = 0; blubNutes = 0; carcassNutes = 0;

            grandNutes = 0;

            lockedNutes = 0;



            blibs = GameObject.FindGameObjectsWithTag("Prey");
            blobs = GameObject.FindGameObjectsWithTag("Predator");
            blybs = GameObject.FindGameObjectsWithTag("Predator2");
            blubs = GameObject.FindGameObjectsWithTag("ApexPred");
            carcasses = GameObject.FindGameObjectsWithTag("Carcass");
            for (int i = 0; i < blibs.Length; i++)
            {
                blibNutes += blibs[i].GetComponent<BlibControls>().nutLevel;
            }
            extBlibNutes =  blibNutes;
            System.Array.Clear(blibs, 0, blibs.Length);

            for (int i = 0; i < blobs.Length; i++)
            {
                blobNutes += blobs[i].GetComponent<BrainBlobControls>().protein;
                blobNutes += blobs[i].GetComponent<BrainBlobControls>().NH4;
            }
            extBlobNutes =  blobNutes;
            System.Array.Clear(blobs, 0, blobs.Length);

            for (int i = 0; i < blybs.Length; i++)
            {
                blybNutes += blybs[i].GetComponent<BrainBlybControls>().protein;
                blybNutes += blybs[i].GetComponent<BrainBlybControls>().NH4;
            }
            extBlybNutes =  blybNutes;
            System.Array.Clear(blybs, 0, blybs.Length);
            for (int i = 0; i < blubs.Length; i++)
            {   
                blubNutes += blubs[i].GetComponent<BrainBlubControls>().protein;
                blubNutes += blubs[i].GetComponent<BrainBlubControls>().NH4;
            }
            extBlubNutes =  blubNutes;
            System.Array.Clear(blubs, 0, blubs.Length);
            for (int i = 0; i < carcasses.Length; i++)
            {
                if (carcasses[i].GetComponent<BrainBlobControls>() != null)
                {
                    carcassNutes += carcasses[i].GetComponent<BrainBlobControls>().protein;
                    carcassNutes += carcasses[i].GetComponent<BrainBlobControls>().NH4;
                }
                if (carcasses[i].GetComponent<BrainBlybControls>() != null)
                {
                    carcassNutes += carcasses[i].GetComponent<BrainBlybControls>().protein;
                    carcassNutes += carcasses[i].GetComponent<BrainBlybControls>().NH4;
                }
                if (carcasses[i].GetComponent<BrainBlubControls>() != null)
                {
                    carcassNutes += carcasses[i].GetComponent<BrainBlubControls>().protein;
                    carcassNutes += carcasses[i].GetComponent<BrainBlubControls>().NH4;
                }
            }
            extCarcassNutes =  carcassNutes;
            System.Array.Clear(carcasses, 0, carcasses.Length);

            
            lockedNutes = blibNutes + blobNutes + blybNutes + blubNutes + carcassNutes;
            
            
        
            blibNutes = 0; blobNutes = 0; blybNutes = 0; blubNutes = 0; carcassNutes = 0;








            //int sumPrevKernel;
            //float kernMean;
            int[] thereVals  = new int [8];
            int hereval;
           if(diffusionEnabled == true) {
            gridWidth = nutgrid.gridArray.GetLength(0);
            gridHeight = nutgrid.gridArray.GetLength(1);

            int dirs = UnityEngine.Random.Range(-2,3);
            if(dirCounter == 0){
                for(int x = 1; x < gridWidth-2; x++){
                    for(int y = 1; y < gridHeight-2; y++){

                    kernel = GetKernelFull(x,y);
                    
                    hereval = kernel[1,1];

                  
                    int maxValue = -1;
                    
                    int maxFirstIndex = -1;
                    int maxSecondIndex = -1;



                    
                    int value = -1;
                    dirs = rndA.Next(1,5);
                 switch(dirs){
                case 1:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                    for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                         value = kernel[i, j];

                        if (value > maxValue ) {
                            maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                case 2:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                     value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                 break;

                 case 3:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                        for (int j = kernel.GetLength(1)-1; j > 0; j--) {
                     value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;

                case 4:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = kernel.GetLength(1)-1; j > 0 ; j--) {
                     value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                
            }
                 
                 
                    int deltaval = maxValue - hereval;
                    int moveAmount = 0;
                gridWidth = nutgrid.gridArray.GetLength(0);
                gridHeight = nutgrid.gridArray.GetLength(1);
                    if(hereval < maxValue && maxValue > diffusionLimit /*&& x < gridWidth && y < gridHeight && x > 0 && y > 0*/){
                        if(deltaval > 0 && deltaval <= 1){
                            moveAmount = 1;
                        }
                        else if(deltaval > 1 && deltaval <= 2){
                            moveAmount = 1;
                        }else if(deltaval > 2 && deltaval <= 4){
                            moveAmount = 2;
                        }else if(deltaval > 4 && deltaval <= 8){
                            moveAmount = 4;
                        }else if(deltaval > 8 && deltaval <= 16){
                            moveAmount = 8;
                        }else if(deltaval > 16 && deltaval <= 32){
                            moveAmount = 16;
                        }else if(deltaval > 32){
                            moveAmount = 32;
                        }
                        
                        if(moveAmount > 0){
                        moveAmount = rndA.Next(1,moveAmount);
                        nutgrid.SetValue(maxFirstIndex+x-1,maxSecondIndex+y-1,maxValue-moveAmount);
                            nutgrid.SetValue(x,y,hereval+moveAmount);
                        }
                        
                        
                            
                            System.Array.Clear(kernel,0,kernel.Length);
                            
                    }

                }
                }
                dirCounter = 1;
            }else if(dirCounter == 1){
                for(int x = gridWidth-2; x > 1; x--){
                for(int y = gridHeight-2; y > 1; y--){
                    kernel = GetKernelFull(x,y);
                    

                    hereval = kernel[1,1];

                  
                    int maxValue = -1;
                    
                    int maxFirstIndex = -1;
                    int maxSecondIndex = -1;



                    
 
                    dirs = rndA.Next(1,5);
                 switch(dirs){
                case 1:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                        for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                case 2:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                 break;

                 case 3:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                        for (int j = kernel.GetLength(1)-1; j > 0; j--) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;

                case 4:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = kernel.GetLength(1)-1; j > 0 ; j--) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                
            }
                 
                 
                    int deltaval = maxValue - hereval;
                    int moveAmount = 0;
                gridWidth = nutgrid.gridArray.GetLength(0);
                gridHeight = nutgrid.gridArray.GetLength(1);
                    if(hereval < maxValue && maxValue > diffusionLimit /*&& x < gridWidth && y < gridHeight && x > 0 && y > 0*/){
                        if(deltaval > 0 && deltaval <= 1){
                            moveAmount = 1;
                        }
                        else if(deltaval > 1 && deltaval <= 2){
                            moveAmount = 1;
                        }else if(deltaval > 2 && deltaval <= 4){
                            moveAmount = 2;
                        }else if(deltaval > 4 && deltaval <= 8){
                            moveAmount = 4;
                        }else if(deltaval > 8 && deltaval <= 16){
                            moveAmount = 8;
                        }else if(deltaval > 16 && deltaval <= 32){
                            moveAmount = 16;
                        }else if(deltaval > 32){
                            moveAmount = 32;
                        }
                        if(moveAmount > 0){
                        moveAmount = rndA.Next(1,moveAmount);
                        nutgrid.SetValue(maxFirstIndex+x-1,maxSecondIndex+y-1,maxValue-moveAmount);
                            nutgrid.SetValue(x,y,hereval+moveAmount);
                        }
                        
                        
                            
                            System.Array.Clear(kernel,0,kernel.Length);
                            
                    }
                    
                }
            }
                dirCounter = 2;
            }else if(dirCounter == 2){
                for(int x = gridWidth-2; x > 1; x--){
                for(int y = 1; y < gridHeight-2; y++){
                    kernel = GetKernelFull(x,y);
                    

                    hereval = kernel[1,1];

                  
                    int maxValue = -1;
                    
                    int maxFirstIndex = -1;
                    int maxSecondIndex = -1;



                    
 
                    dirs = rndA.Next(1,5);
                 switch(dirs){
                case 1:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                        for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                case 2:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                 break;

                 case 3:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                        for (int j = kernel.GetLength(1)-1; j > 0; j--) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;

                case 4:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = kernel.GetLength(1)-1; j > 0 ; j--) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                
            }
                 
                 
                    int deltaval = maxValue - hereval;
                    int moveAmount = 0;
                gridWidth = nutgrid.gridArray.GetLength(0);
                gridHeight = nutgrid.gridArray.GetLength(1);
                    if(hereval < maxValue && maxValue > diffusionLimit /*&& x < gridWidth && y < gridHeight && x > 0 && y > 0*/){
                        if(deltaval > 0 && deltaval <= 1){
                            moveAmount = 1;
                        }
                        else if(deltaval > 1 && deltaval <= 2){
                            moveAmount = 1;
                        }else if(deltaval > 2 && deltaval <= 4){
                            moveAmount = 2;
                        }else if(deltaval > 4 && deltaval <= 8){
                            moveAmount = 4;
                        }else if(deltaval > 8 && deltaval <= 16){
                            moveAmount = 8;
                        }else if(deltaval > 16 && deltaval <= 32){
                            moveAmount = 16;
                        }else if(deltaval > 32){
                            moveAmount = 32;
                        }
                        if(moveAmount > 0){
                        moveAmount = rndA.Next(1,moveAmount);
                        nutgrid.SetValue(maxFirstIndex+x-1,maxSecondIndex+y-1,maxValue-moveAmount);
                            nutgrid.SetValue(x,y,hereval+moveAmount);
                        }
                        
                        
                            
                            System.Array.Clear(kernel,0,kernel.Length);
                            
                    }

                }
            }
                dirCounter = 3;
            }else if(dirCounter == 3){
                for(int x = 1; x < gridWidth-2; x++){
                for(int y = gridHeight-2; y > 1; y--){
                    kernel = GetKernelFull(x,y);
                    

                    hereval = kernel[1,1];

                  
                    int maxValue = -1;
                    
                    int maxFirstIndex = -1;
                    int maxSecondIndex = -1;



                    
 
                    dirs = rndA.Next(1,5);
                 switch(dirs){
                case 1:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                        for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                case 2:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = 0; j < kernel.GetLength(1)-1; j++) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                 break;

                 case 3:
                for (int i = 0; i < kernel.GetLength(0)-1; i++){
                        for (int j = kernel.GetLength(1)-1; j > 0; j--) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;

                case 4:
                for (int i = kernel.GetLength(0)-1; i > 0; i--){
                        for (int j = kernel.GetLength(1)-1; j > 0 ; j--) {
                    int value = kernel[i, j];

                    if (value > maxValue) {
                        maxFirstIndex = i;
                        maxSecondIndex = j;

                        maxValue = value;
                            }
                    }
                 }
                break;
                
            }
                 
                 
                    int deltaval = maxValue - hereval;
                    int moveAmount = 0;
                gridWidth = nutgrid.gridArray.GetLength(0);
                gridHeight = nutgrid.gridArray.GetLength(1);
                    if(hereval < maxValue && maxValue > diffusionLimit /*&& x < gridWidth && y < gridHeight && x > 0 && y > 0*/){
                        if(deltaval > 0 && deltaval <= 1){
                            moveAmount = 1;
                        }
                        else if(deltaval > 1 && deltaval <= 4){
                            moveAmount = 1;
                        }else if(deltaval > 2 && deltaval <= 8){
                            moveAmount = 2;
                        }else if(deltaval > 4 && deltaval <= 16){
                            moveAmount = 4;
                        }else if(deltaval > 8 && deltaval <= 32){
                            moveAmount = 8;
                        }else if(deltaval > 16 && deltaval <= 64){
                            moveAmount = 16;
                        }else if(deltaval > 64){
                            moveAmount = 32;
                        }
                        if(moveAmount > 0){
                        moveAmount = rndA.Next(1,moveAmount);
                        nutgrid.SetValue(maxFirstIndex+x-1,maxSecondIndex+y-1,maxValue-moveAmount);
                            nutgrid.SetValue(x,y,hereval+moveAmount);
                        }
                        
                        
                            
                            System.Array.Clear(kernel,0,kernel.Length);
                            
                    }

                }
            }
                dirCounter = 0;
            }
            
            
            
            
            
         

        }
        
            // freeNutes = 0;
//nutgrid.gridArray = newArray;
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
            freeNutes = nutgrid.GetSum();
            //for (int x = 0; x< nutgrid.gridArray.GetLength(0);x++){
            //for(int y = 0; y < nutgrid.gridArray.GetLength(1); y++){
            //freeNutes += nutgrid.GetValue(x, y);
            //}
            //}

            grandNutes = freeNutes + lockedNutes;





            // Debug.Log("totNutes = " + totNutes + " , countNutes = " + countNutes + " , grandNutes = " + grandNutes);
            if (autoReplenish == true)
            {
                if (grandNutes < ((9*totNutes)/10))
                {
                    freeNutes = nutgrid.GetSum();
                    grandNutes = freeNutes + lockedNutes;
                    /*
                    int minX = nutgrid.gridArray.GetLength(0)/4;
                    int maxX = nutgrid.gridArray.GetLength(0) + (nutgrid.gridArray.GetLength(0)/4);
                    int minY = nutgrid.gridArray.GetLength(1)/4;
                    int maxY = nutgrid.gridArray.GetLength(1) + (nutgrid.gridArray.GetLength(1)/4);


                        int x = System.Random.Next(minX,maxX);
                        int y = System.Random.Next(minX,maxX);
                        int thisVal = nutgrid.GetValue(x,y);
                           nutgrid.SetValue(x, y,thisVal + (totNutes/64));
                           
                    */
                    int[] randPos = new int[2];
                    randPos[0] = Random.Range(0,nutgrid.gridArray.GetLength(0));
                    randPos[1] = Random.Range(0,nutgrid.gridArray.GetLength(1));
                     int thisVal = nutgrid.GetValue(randPos[0], randPos[1]);
                     nutgrid.SetValue(randPos[0], randPos[1], thisVal +(totNutes/10));
                    /*

                    for (int x = 0; x < nutgrid.gridArray.GetLength(0); x++)
                    {

                        
                        for (int y = 0; y < nutgrid.gridArray.GetLength(1); y++)
                        {

                            int thisVal = nutgrid.GetValue(x, y);
                            


                            if (nutgrid.GetSum()+lockedNutes < totNutes)
                            {
                                nutgrid.SetValue(x, y, thisVal +1);
                                


                            }

                        }

                    }
                    */

                }



            }
            if (autoRemove == true)
            {

                if (freeNutes > 0 && grandNutes > totNutes)
                {
                    freeNutes = nutgrid.GetSum();
                    grandNutes = freeNutes + lockedNutes;
                    int nutleft = grandNutes - totNutes;
                    for (int x = 1; x < nutgrid.gridArray.GetLength(0)-1; x++)
                    {
                        
                        for (int y = 1; y < nutgrid.gridArray.GetLength(1)-1; y++)
                        {


                            int thisVal = nutgrid.GetValue(x, y);
                            
                            if (thisVal > 0 && grandNutes > totNutes && nutleft > 0)
                            {
                                nutleft += - thisVal;
                                nutgrid.SetValue(x, y, 0);
                                


                            }


                        }
                    }
                    
                }
            }


            diffusionTimer = 0f;
            statNutes = grandNutes;
            statTot = totNutes;
        }


    }




public int[,] GetKernelFull(int x, int y){
    int[,] internalKernel = new int[3,3];

    internalKernel[0, 2] = nutgrid.GetValue(x - 1, y + 1);  // Top left
    internalKernel[1, 2] = nutgrid.GetValue(x + 0, y + 1);  // Top center
    internalKernel[2, 2] = nutgrid.GetValue(x + 1, y + 1);  // Top right
    internalKernel[0, 1] = nutgrid.GetValue(x - 1, y + 0);  // Mid left
    internalKernel[1, 1] = nutgrid.GetValue(x + 0, y + 0);  // Current pixel
    internalKernel[2, 1] = nutgrid.GetValue(x + 1, y + 0);  // Mid right
    internalKernel[0, 0] = nutgrid.GetValue(x - 1, y - 1);  // Low left
    internalKernel[1, 0] = nutgrid.GetValue(x + 0, y - 1);  // Low center
    internalKernel[2, 0] = nutgrid.GetValue(x + 1, y - 1);  // Low right
    return internalKernel;
}

public int[,] GetKernelSmall(int x, int y){
    int[,] internalKernel = new int[3,3];

    internalKernel[0, 2] = 0;                               // Top left
    internalKernel[1, 2] = nutgrid.GetValue(x + 0, y + 1);  // Top center
    internalKernel[2, 2] = 0;                               // Top right
    internalKernel[0, 1] = nutgrid.GetValue(x - 1, y + 0);  // Mid left
    internalKernel[1, 1] = nutgrid.GetValue(x + 0, y + 0);  // Current pixel
    internalKernel[2, 1] = nutgrid.GetValue(x + 1, y + 0);  // Mid right
    internalKernel[0, 0] =0;                                // Low left
    internalKernel[1, 0] = nutgrid.GetValue(x + 0, y - 1);  // Low center
    internalKernel[2, 0] = 0;                               // Low right
    return internalKernel;
}

//public void writeNutesFile( int[,] ingrid){
//new StreamWriter

//}

}


