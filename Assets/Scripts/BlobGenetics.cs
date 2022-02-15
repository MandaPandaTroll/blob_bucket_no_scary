//prints genetic data to .csv file
//original code by smkplus
//modified by tabacwoman november 2021


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class BlobGenetics : MonoBehaviour
{

GameObject[] blobs;

private List<string[]> rowData = new List<string[]>();
    int itCount;
    
    float time;
    float totalTime;
    public int maxSampleSize;
    int sampleSize;
    public float sampleRate;
    
   

    int sampler;


    public List <int> generation;
    public List <float> intron1;
    public List <float> intron2;
    public List <float> intron3;
    public List <float> intron4;

    public List <float> moveAllele1;
    public List <float> moveAllele2;
    public List <float> redAllele1;
    public List <float> redAllele2;
    public List <float> greenAllele1;
    public List <float> greenAllele2;
    public List <float> blueAllele1;
    public List <float> blueAllele2;
    public List <float> lifeLengthAllele1;
    public List <float> lifeLengthAllele2;
    public List <float> lookDistAllele1;
    public List <float> lookDistAllele2;
    public List <float> sizeAllele1;
    public List <float> sizeAllele2;

    public List <float> turnTorqueAllele1;
    public List <float> turnTorqueAllele2;
    public List <float> e2repAllele1;
    public List <float> e2repAllele2;
 


    bool refCheck = false;
    bool brainCheck = false;
    int sampleGroup;
    

    // Start is called before the first frame update
    void Start()
    {
        itCount = 0;
        sampleGroup = 0;
    }

    // Update is called once per frame
    void Update()
    {

        
        totalTime = Mathf.Round(Time.time);
        time += Time.deltaTime;
        
        if (time >= sampleRate){
               refCheck = false;
               brainCheck = false;
                blobs  = GameObject.FindGameObjectsWithTag("Predator");
             if(blobs.Length <= 0){return;}
                if(blobs.Length >= 1){
                    if(blobs.Length < maxSampleSize){sampleSize = blobs.Length;}
                    if(blobs.Length >= maxSampleSize){sampleSize = maxSampleSize;}
            
                refCheck = blobs[0].TryGetComponent(out BlobControls blobControls);
                brainCheck = blobs[0].TryGetComponent(out BrainBlobControls brainBlobControls);
                


                        /*
                        if (refCheck == true)
                        {
                            for (int i = 0; i < sampleSize; i++)
                            {   
                    
                    
                                sampler = UnityEngine.Random.Range(0,sampleSize);
                                BlobControls  sampledBlob = blobs[sampler].GetComponent<BlobControls>();

                                intron1.Add(sampledBlob.intron1);
                                intron2.Add(sampledBlob.intron2);
                                intron3.Add(sampledBlob.intron3);
                                intron4.Add(sampledBlob.intron4);

                                moveAllele1.Add(sampledBlob.moveAllele1);
                                moveAllele2.Add(sampledBlob.moveAllele2); 

                               redAllele1.Add(sampledBlob.redAllele1);
                               redAllele2.Add(sampledBlob.redAllele2);

                               greenAllele1.Add(sampledBlob.greenAllele1);
                               greenAllele2.Add(sampledBlob.greenAllele2);

                                blueAllele1.Add(sampledBlob.blueAllele1);
                                blueAllele2.Add(sampledBlob.blueAllele2);
                                lifeLengthAllele1.Add(sampledBlob.lifeLengthAllele1);
                                lifeLengthAllele2.Add(sampledBlob.lifeLengthAllele2);
                    
                                lookDistAllele1.Add(sampledBlob.lookDistAllele1);
                                lookDistAllele2.Add(sampledBlob.lookDistAllele2);

                                turnTorqueAllele1.Add(sampledBlob.turnTorqueAllele1);
                                turnTorqueAllele2.Add(sampledBlob.turnTorqueAllele2);

                                e2repAllele1.Add(sampledBlob.e2repAllele1);
                                e2repAllele2.Add(sampledBlob.e2repAllele2);

                                sizeAllele1.Add(sampledBlob.sizeAllele1);
                                sizeAllele2.Add(sampledBlob.sizeAllele2);


                                generation.Add(sampledBlob.generation);
                                
                            }
                        }
                        */

                        if (brainCheck == true)
                            for (int i = 0; i < sampleSize; i++)
                            {  
                            sampler = UnityEngine.Random.Range(0,sampleSize);
                            BrainBlobControls sampledBlob = blobs[sampler].GetComponent<BrainBlobControls>();


                                intron1.Add(sampledBlob.intron1);
                                intron2.Add(sampledBlob.intron2);
                                intron3.Add(sampledBlob.intron3);
                                intron4.Add(sampledBlob.intron4);

                                moveAllele1.Add(sampledBlob.moveAllele1);
                                moveAllele2.Add(sampledBlob.moveAllele2); 

                               redAllele1.Add(sampledBlob.redAllele1);
                               redAllele2.Add(sampledBlob.redAllele2);

                               greenAllele1.Add(sampledBlob.greenAllele1);
                               greenAllele2.Add(sampledBlob.greenAllele2);

                                blueAllele1.Add(sampledBlob.blueAllele1);
                                blueAllele2.Add(sampledBlob.blueAllele2);
                                lifeLengthAllele1.Add(sampledBlob.lifeLengthAllele1);
                                lifeLengthAllele2.Add(sampledBlob.lifeLengthAllele2);
                    
                                lookDistAllele1.Add(sampledBlob.lookDistAllele1);
                                lookDistAllele2.Add(sampledBlob.lookDistAllele2);

                                turnTorqueAllele1.Add(sampledBlob.turnTorqueAllele1);
                                turnTorqueAllele2.Add(sampledBlob.turnTorqueAllele2);

                                e2repAllele1.Add(sampledBlob.e2repAllele1);
                                e2repAllele2.Add(sampledBlob.e2repAllele2);

                                sizeAllele1.Add(sampledBlob.sizeAllele1);
                                sizeAllele2.Add(sampledBlob.sizeAllele2);


                                generation.Add(sampledBlob.generation);

                            }


            }           
                
            
             


            



        
            
                Save();
            
                }

            


            }
        


        void Save(){
            itCount += 1;
            string[] rowDataTemp;
        if (itCount == 1){
            if (refCheck == true)
            {
            rowDataTemp = new string[27];
            rowDataTemp[0] ="time" ;
            rowDataTemp[1] ="species" ;
            rowDataTemp[2] = "sampleGroup";
            rowDataTemp[3] = "sampleNumber";
            rowDataTemp[4] ="generation";
            rowDataTemp[5] = "intron1";
            rowDataTemp[6] = "intron2";
            rowDataTemp[7] = "intron3";
            rowDataTemp[8] = "intron4";
            rowDataTemp[9] = "moveAllele1";
            rowDataTemp[10] = "moveAllele2";
            rowDataTemp[11] = "redAllele1";
            rowDataTemp[12] = "redAllele2";
            rowDataTemp[13] = "greenAllele1";
            rowDataTemp[14] = "greenAllele2";
            rowDataTemp[15] = "blueAllele1";
            rowDataTemp[16] = "blueAllele2";
            rowDataTemp[17] = "lifeLengthAllele1";
            rowDataTemp[18] = "lifeLengthAllele2";
            rowDataTemp[19] = "lookDistAllele1";
            rowDataTemp[20] = "lookDistAllele2";
            rowDataTemp[21] = "turnTorqueAllele1";
            rowDataTemp[22] = "turnTorqueAllele2";
            rowDataTemp[23] = "e2repAllele1";
            rowDataTemp[24] = "e2repAllele2";
            rowDataTemp[25] = "sizeAllele1";
            rowDataTemp[26] = "sizeAllele2";
            

            rowData.Add(rowDataTemp);
            }

            if (brainCheck == true)

            {
            rowDataTemp = new string[27];
            rowDataTemp[0] ="time" ;
            rowDataTemp[1] ="species" ;
            rowDataTemp[2] = "sampleGroup";
            rowDataTemp[3] = "sampleNumber";
            rowDataTemp[4] ="generation";
            rowDataTemp[5] = "intron1";
            rowDataTemp[6] = "intron2";
            rowDataTemp[7] = "intron3";
            rowDataTemp[8] = "intron4";
            rowDataTemp[9] = "moveAllele1";
            rowDataTemp[10] = "moveAllele2";
            rowDataTemp[11] = "redAllele1";
            rowDataTemp[12] = "redAllele2";
            rowDataTemp[13] = "greenAllele1";
            rowDataTemp[14] = "greenAllele2";
            rowDataTemp[15] = "blueAllele1";
            rowDataTemp[16] = "blueAllele2";
            rowDataTemp[17] = "lifeLengthAllele1";
            rowDataTemp[18] = "lifeLengthAllele2";
            rowDataTemp[19] = "lookDistAllele1";
            rowDataTemp[20] = "lookDistAllele2";
            rowDataTemp[21] = "turnTorqueAllele1";
            rowDataTemp[22] = "turnTorqueAllele2";
            rowDataTemp[23] = "e2repAllele1";
            rowDataTemp[24] = "e2repAllele2";
            rowDataTemp[25] = "sizeAllele1";
            rowDataTemp[26] = "sizeAllele2";

            rowData.Add(rowDataTemp);
            }
        }



        // You can add up the values in as many cells as you want.
        for(int i = 0; i < sampleSize; i++)
        {   if (refCheck == true)

            {
            rowDataTemp = new string[27];
            rowDataTemp[0] =totalTime.ToString() ;
            rowDataTemp[1] ="blob" ;
            rowDataTemp[2] = sampleGroup.ToString();
            rowDataTemp[3] = i.ToString();
            rowDataTemp[4] = generation[i].ToString();
            rowDataTemp[5] = intron1[i].ToString();
            rowDataTemp[6] = intron2[i].ToString();
            rowDataTemp[7] = intron3[i].ToString();
            rowDataTemp[8] = intron4[i].ToString();
            rowDataTemp[9] = moveAllele1[i].ToString();
            rowDataTemp[10] = moveAllele2[i].ToString();
            rowDataTemp[11] = redAllele1[i].ToString();
            rowDataTemp[12] = redAllele2[i].ToString();
            rowDataTemp[13] = greenAllele1[i].ToString();
            rowDataTemp[14] = greenAllele2[i].ToString();
            rowDataTemp[15] = blueAllele1[i].ToString();
            rowDataTemp[16] = blueAllele2[i].ToString();
            rowDataTemp[17] = lifeLengthAllele1[i].ToString();
            rowDataTemp[18] = lifeLengthAllele2[i].ToString();
            rowDataTemp[19] = lookDistAllele1[i].ToString();
            rowDataTemp[20] = lookDistAllele2[i].ToString();
            rowDataTemp[21] = turnTorqueAllele1[i].ToString();
            rowDataTemp[22] = turnTorqueAllele2[i].ToString();
            rowDataTemp[23] = e2repAllele1[i].ToString();
            rowDataTemp[24] = e2repAllele2[i].ToString();
            rowDataTemp[25] = sizeAllele1[i].ToString();
            rowDataTemp[26] = sizeAllele2[i].ToString();
            
            rowData.Add(rowDataTemp);

            }
            if (brainCheck == true)

            {
            rowDataTemp = new string[27];
            rowDataTemp[0] =totalTime.ToString() ;
            rowDataTemp[1] ="blob" ;
            rowDataTemp[2] = sampleGroup.ToString();
            rowDataTemp[3] = i.ToString();
            rowDataTemp[4] = generation[i].ToString();
            rowDataTemp[5] = intron1[i].ToString();
            rowDataTemp[6] = intron2[i].ToString();
            rowDataTemp[7] = intron3[i].ToString();
            rowDataTemp[8] = intron4[i].ToString();
            rowDataTemp[9] = moveAllele1[i].ToString();
            rowDataTemp[10] = moveAllele2[i].ToString();
            rowDataTemp[11] = redAllele1[i].ToString();
            rowDataTemp[12] = redAllele2[i].ToString();
            rowDataTemp[13] = greenAllele1[i].ToString();
            rowDataTemp[14] = greenAllele2[i].ToString();
            rowDataTemp[15] = blueAllele1[i].ToString();
            rowDataTemp[16] = blueAllele2[i].ToString();
            rowDataTemp[17] = lifeLengthAllele1[i].ToString();
            rowDataTemp[18] = lifeLengthAllele2[i].ToString();
            rowDataTemp[19] = lookDistAllele1[i].ToString();
            rowDataTemp[20] = lookDistAllele2[i].ToString();
            rowDataTemp[21] = turnTorqueAllele1[i].ToString();
            rowDataTemp[22] = turnTorqueAllele2[i].ToString();
            rowDataTemp[23] = e2repAllele1[i].ToString();
            rowDataTemp[24] = e2repAllele2[i].ToString();
            rowDataTemp[25] = sizeAllele1[i].ToString();
            rowDataTemp[26] = sizeAllele2[i].ToString();
            
            rowData.Add(rowDataTemp);
            }


        }
    

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = rowData[i];
        }

        int     length         = output.GetLength(0);
        string     delimiter     = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        

        intron1.Clear();
        intron2.Clear();
        intron3.Clear();
        intron4.Clear();

        moveAllele1.Clear();
        moveAllele2.Clear();
        redAllele1.Clear();
        redAllele2.Clear();
        greenAllele1.Clear();
        greenAllele2.Clear();
        blueAllele1.Clear();
        blueAllele2.Clear();
        lifeLengthAllele1.Clear();
        lifeLengthAllele2.Clear();
        lookDistAllele1.Clear();
        lookDistAllele2.Clear();
        turnTorqueAllele1.Clear();
        turnTorqueAllele2.Clear();
        e2repAllele1.Clear();
        e2repAllele2.Clear();
        sizeAllele1.Clear();
        sizeAllele2.Clear();
        
        generation.Clear();
        

        Array.Clear(blobs,0,blobs.Length);
        time = 0f;
        sampleGroup += 1;


    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Blob_genetics.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Blob_genetics.csv";
        #elif UNITY_STANDALONE_OSX
        return Application.dataPath+"/"+"Blob_genetics.csv";
        #else
        return Application.dataPath +"/"+"Blob_genetics.csv";
        #endif
    }
}


