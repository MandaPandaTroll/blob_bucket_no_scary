//prints genetic data to .csv file
//original code by smkplus
//modified by tabacwoman january 2022


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class BlubSatLogger : MonoBehaviour
{

GameObject[] blubs;

    private List<string[]> rowData = new List<string[]>();
    private List<string> sat1 = new List<string>();
    private List<string> sat2 = new List<string>();
    private List<string> sat3 = new List<string>();
    private List<string> sat4 = new List<string>();
    private int itCount, sampler, sampleGroup, sampleSize;  
    float time, totalTime;
    public int maxSampleSize;
    public float sampleRate;

   

    

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
        
        if (time >= sampleRate)
        {
             blubs  = GameObject.FindGameObjectsWithTag("ApexPred");
             if(blubs.Length <= 0){return;}

            if(blubs.Length >= 1)
            {
                if (blubs.Length < maxSampleSize){
                    sampleSize = blubs.Length;
                    }else{
                        sampleSize = maxSampleSize;
                        }
                        
                for (int i = 0; i < sampleSize; i++)
                {                                          
                    sampler = UnityEngine.Random.Range(0,sampleSize);
                    BrainBlubControls  sampledBlub = blubs[sampler].GetComponent<BrainBlubControls>();
                    string[] nucleotides = new string[27];
                    for(int a = 0; a < 27; a++)
                    {
                        nucleotides[a] = sampledBlub.superSatellite[0,a];
                    }
                    string sampleSat1 = String.Join("", nucleotides);
                    sat1.Add(sampleSat1);

                    for(int b = 0; b < 27; b++)
                    {
                        nucleotides[b] = sampledBlub.superSatellite[1,b];
                    }
                    string sampleSat2 = String.Join("", nucleotides);
                    sat2.Add(sampleSat2);

                    for(int c = 0; c < 27; c++)
                    {
                        nucleotides[c] = sampledBlub.superSatellite[2,c];
                    }
                    string sampleSat3 = String.Join("", nucleotides);
                    sat3.Add(sampleSat3);

                    for(int d = 0; d < 27; d++)
                    {
                        nucleotides[d] = sampledBlub.superSatellite[3,d];
                    }
                    string sampleSat4 = String.Join("", nucleotides);
                    sat4.Add(sampleSat4);

                }
            }   Save();
        }        
    }

            


            
        


        void Save(){
            itCount += 1;
            string[] rowDataTemp;
        if (itCount == 1){

            rowDataTemp = new string[8];
            rowDataTemp[0] ="time" ;
            rowDataTemp[1] ="species" ;
            rowDataTemp[2] = "sampleGroup";
            rowDataTemp[3] = "sample_number";
            rowDataTemp[4] = "sat1";
            rowDataTemp[5] = "sat2";
            rowDataTemp[6] = "sat3";
            rowDataTemp[7] = "sat4";
            

            rowData.Add(rowDataTemp);
            }


        for(int i = 0; i < sampleSize; i++)
        {   

            rowDataTemp = new string[8];
            rowDataTemp[0] = totalTime.ToString();
            rowDataTemp[1] = "blub";
            rowDataTemp[2] = sampleGroup.ToString();
            rowDataTemp[3] = i.ToString();
            rowDataTemp[4] = sat1[i].ToString();
            rowDataTemp[5] = sat2[i].ToString();
            rowDataTemp[6] = sat3[i].ToString();
            rowDataTemp[7] = sat4[i].ToString();
            
            
            rowData.Add(rowDataTemp);

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
        

        sat1.Clear();sat2.Clear();sat3.Clear();sat4.Clear();

        Array.Clear(blubs,0,blubs.Length);
        time = 0f;
        sampleGroup += 1;


    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"blub_sats.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"blub_sats.csv";
        #elif UNITY_STANDALONE_OSX
        return Application.dataPath+"/"+"blub_sats.csv";
        #else
        return Application.dataPath +"/"+"blub_sats.csv";
        #endif
    }
}


