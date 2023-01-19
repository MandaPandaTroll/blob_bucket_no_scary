//prints population sizes to .csv file
//original code by smkplus
//modified by tabacwoman november 2021

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class PopLogger : MonoBehaviour
{

public static int clones_blib;
public static int clones_blob;
public static int clones_blyb;
public static int clones_blub;
    int itCount;
    public int blibN = 0, blobN = 0, blubN = 0, blybN = 0;

/*
GameObject[] blobs;
GameObject[] blybs;
GameObject[] blibs;
GameObject[] blubs;
GameObject Alpha;
Detector Detector;
*/
private List<string[]> rowData = new List<string[]>();


    
    float time;
    float totalTime;
    public float sampleRate;

    sampleGeneticDiversity genSamp;
    bool isGenSamp;
    
    float meanHeterozygosity_blob;
    float meanHeterozygosity_blyb;
    float meanHeterozygosity_blub;
    int genSampColumns;



    // Start is called before the first frame update
    void Start()
    {
        //Alpha = GameObject.Find("Alpha");
        //Detector = Alpha.GetComponent<Detector>();
        /*
       isGenSamp = false;
       genSampColumns = 0;
        clones_blib = -1;
        if(gameObject.GetComponent<sampleGeneticDiversity>() != null){
            isGenSamp = true;
                genSamp = gameObject.GetComponent<sampleGeneticDiversity>();
                genSampColumns = 3;

        }
        */
        
    }
 public int genSum = 0;
 public float meanGeneration;
    // Update is called once per frame
    void LateUpdate()
    {

        
        
        time += Time.deltaTime;
        
        
            if (time >= sampleRate){
                genSum = 0;
                blibN  = FindObjectsOfType<BlibControls>().Length;
                blobN  = FindObjectsOfType<BrainBlobControls>().Length;
                blybN  = FindObjectsOfType<BrainBlybControls>().Length;
                blubN  = FindObjectsOfType<BrainBlubControls>().Length;
                var blibs = GameObject.FindGameObjectsWithTag("Prey");

                foreach(GameObject blib in blibs){
                    genSum += blib.GetComponent<BlibControls>().generation;
                }
                meanGeneration = (float)genSum / (float)blibN;
                /*
                if (isGenSamp == true){
                    meanHeterozygosity_blib = genSamp.meanHeterozygosity_blib;
                    meanHeterozygosity_blob = genSamp.meanHeterozygosity_blob;
                    meanHeterozygosity_blyb = genSamp.meanHeterozygosity_blyb;
                    //meanHeterozygosity_blub = genSamp.meanHeterozygosity_blub;
                }
                */

                /*
                blibN = blibs.Length;
                blobN = blobs.Length;
                blybN = blybs.Length;
                blubN = blubs.Length;
                */

                


           
            
            
            totalTime = Mathf.Round(Time.time);

            
            
            
            Save();
            


            }
        


    }
        void Save(){
            itCount += 1;
            string[] rowDataTemp;
        if (itCount == 1){
            rowDataTemp = new string[5/*+genSampColumns*/];
            rowDataTemp[0] = "t";
            rowDataTemp[1] = "blibN";
            rowDataTemp[2] = "blobN";
            rowDataTemp[3] = "blybN";
            rowDataTemp[4] = "blubN";
            /*
            if(isGenSamp == true){
                rowDataTemp[5] = "meanHeterozygosity_blib";
                rowDataTemp[6] = "meanHeterozygosity_blob";
                rowDataTemp[7] = "meanHeterozygosity_blyb";
                //rowDataTemp[8] = "meanHeterozygosity_blub";
            }
            
            */

            rowData.Add(rowDataTemp);
        }
        // Creating First row of titles manually..
            rowDataTemp = new string[5/*+genSampColumns*/];
            rowDataTemp[0] = totalTime.ToString();
            //Blibsamples
            rowDataTemp[1] = blibN.ToString();


            //Blobsamples
            rowDataTemp[2] = blobN.ToString();
            //Blybsamples
            rowDataTemp[3] = blybN.ToString();

            //Blubsamples
            rowDataTemp[4] = blubN.ToString();
            /*
             if(isGenSamp == true){
                rowDataTemp[5] = meanHeterozygosity_blib.ToString();
                rowDataTemp[6] = meanHeterozygosity_blob.ToString();
                rowDataTemp[7] = meanHeterozygosity_blyb.ToString();
               // rowDataTemp[8] = meanHeterozygosity_blub.ToString();
            }
            */

            
            rowData.Add(rowDataTemp);
    

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
        outStream.Flush();
        outStream.Close();
        outStream.Dispose();
        
        /*
        Array.Clear(blobs,0,blobs.Length);
        Array.Clear(blybs,0,blybs.Length);
        Array.Clear(blibs,0,blibs.Length);
        Array.Clear(blubs,0,blubs.Length);
        */
        time = 0f;





    }


    public string GetName(string species){
        if (species == "blib")
        {
            clones_blib +=1;
            return "blib"+clones_blib.ToString("");
        }else{return "error";}
    }
    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Pop_data.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Pop_data.csv";
        #elif UNITY_STANDALONE_OSX
        return Application.dataPath+"/"+"Pop_data.csv";
        #else
        return Application.dataPath +"/"+"Pop_data.csv";
        #endif
    }
}



