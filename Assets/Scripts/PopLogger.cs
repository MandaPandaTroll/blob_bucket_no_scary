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

GameObject[] blobs;
GameObject[] blybs;
GameObject[] blibs;
GameObject[] blubs;
GameObject Alpha;
Detector Detector;
private List<string[]> rowData = new List<string[]>();


    
    float time;
    float totalTime;
    public float sampleRate;


    





    // Start is called before the first frame update
    void Start()
    {
        Alpha = GameObject.Find("Alpha");
        Detector = Alpha.GetComponent<Detector>();
        clones_blib = -1;
        
    }

    // Update is called once per frame
    void Update()
    {

        
        
        time += Time.deltaTime;
        
        
            if (time >= sampleRate){

                blibs  = GameObject.FindGameObjectsWithTag("Prey");
                blobs = GameObject.FindGameObjectsWithTag("Predator");
                blybs = GameObject.FindGameObjectsWithTag("Predator2");
                blubs  = GameObject.FindGameObjectsWithTag("ApexPred");



                blibN = blibs.Length;
                blobN = blobs.Length;
                blybN = blybs.Length;
                blubN = blubs.Length;

                


           
            
            
            totalTime = Mathf.Round(Time.time);

            
            
            
            Save();
            


            }
        


    }
        void Save(){
            itCount += 1;
            string[] rowDataTemp;
        if (itCount == 1){
            rowDataTemp = new string[5];
            rowDataTemp[0] = "t";
            rowDataTemp[1] = "blibN";
            rowDataTemp[2] = "blobN";
            rowDataTemp[3] = "blybN";
            rowDataTemp[4] = "blubN";

            rowData.Add(rowDataTemp);
        }
        // Creating First row of titles manually..
            rowDataTemp = new string[5];
            rowDataTemp[0] = totalTime.ToString();
            //Blibsamples
            rowDataTemp[1] = blibN.ToString();


            //Blobsamples
            rowDataTemp[2] = blobN.ToString();
            //Blybsamples
            rowDataTemp[3] = blybN.ToString();

            //Blubsamples
            rowDataTemp[4] = blubN.ToString();



            
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
        outStream.Close();
        

        Array.Clear(blobs,0,blobs.Length);
        Array.Clear(blybs,0,blybs.Length);
        Array.Clear(blibs,0,blibs.Length);
        Array.Clear(blubs,0,blubs.Length);
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



