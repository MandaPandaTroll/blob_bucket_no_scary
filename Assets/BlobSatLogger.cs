//prints genetic data to .csv file
//original code by smkplus
//modified by tabacwoman january 2022


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class BlobSatLogger : MonoBehaviour
{

GameObject[] blobs;

    private List<string[]> rowData = new List<string[]>();
    private List<string> unitName = new List<string>();
    private List<string> sat1 = new List<string>();
    private List<string> sat2 = new List<string>();
    private List<string> sat3 = new List<string>();
    private List<string> sat4 = new List<string>();
    private List<string> A0 = new List<string>();
    private List<string> A1 = new List<string>();
    private List<string> A2 = new List<string>();
    private List<string> A3 = new List<string>();
    private List<string> A4 = new List<string>();
    private List<string> A5 = new List<string>();
    private List<string> A6 = new List<string>();
    private List<string> A7 = new List<string>();
    private List<string> A8 = new List<string>();

    private List<string> B0 = new List<string>();
    private List<string> B1 = new List<string>();
    private List<string> B2 = new List<string>();
    private List<string> B3 = new List<string>();
    private List<string> B4 = new List<string>();
    private List<string> B5 = new List<string>();
    private List<string> B6 = new List<string>();
    private List<string> B7 = new List<string>();
    private List<string> B8 = new List<string>();
    
    private int itCount, sampler, sampleGroup, sampleSize;  
    float time, totalTime;
    public int maxSampleSize;
    public float sampleRate;

   string TE3 = "t_0";
    string filename;

    // Start is called before the first frame update
    void Start()
    {
        itCount = 0;
        filename = "blob_sats.csv";
        sampleGroup = 0;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {

        
        totalTime = Mathf.Round(Time.time);
        
        time += Time.deltaTime;
        
        if (time >= sampleRate)
        {

            

            
            
             blobs  = GameObject.FindGameObjectsWithTag("Predator");
            if(blobs.Length <= 0){return;}
            if(blobs.Length >= 1)
            {
                if (blobs.Length < maxSampleSize){
                    sampleSize = blobs.Length;
                    }else{
                        sampleSize = maxSampleSize;
                        }
                        
                for (int i = 0; i < sampleSize; i++)
                {                                          
                    sampler = UnityEngine.Random.Range(0,sampleSize);
                    BrainBlobControls  sampledBlob = blobs[sampler].GetComponent<BrainBlobControls>();
                    BlobGenome sampledGenome = blobs[sampler].GetComponent<BlobGenome>();
                    string[] nucleotides = new string[27];
                    string[] bases = new string[sampledGenome.extA.GetLength(1)];
                    unitName.Add(sampledBlob.gameObject.name);
                    for(int a = 0; a < 27; a++)
                    {
                        nucleotides[a] = sampledBlob.superSatellite[0,a];
                    }
                    string sampleSat1 = String.Join("", nucleotides);
                    sat1.Add(sampleSat1);

                    for(int b = 0; b < 27; b++)
                    {
                        nucleotides[b] = sampledBlob.superSatellite[1,b];
                    }
                    string sampleSat2 = String.Join("", nucleotides);
                    sat2.Add(sampleSat2);

                    for(int c = 0; c < 27; c++)
                    {
                        nucleotides[c] = sampledBlob.superSatellite[2,c];
                    }
                    string sampleSat3 = String.Join("", nucleotides);
                    sat3.Add(sampleSat3);

                    for(int d = 0; d < 27; d++)
                    {
                        nucleotides[d] = sampledBlob.superSatellite[3,d];
                    }
                    string sampleSat4 = String.Join("", nucleotides);
                    sat4.Add(sampleSat4);


                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[0,e];
                    }
                    string sampleChrom = String.Join("", bases);
                    A0.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[1,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A1.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[2,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A2.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[3,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A3.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[4,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A4.Add(sampleChrom);


                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[5,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A5.Add(sampleChrom);


                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[6,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A6.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[7,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A7.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extA[8,e];
                    }
                     sampleChrom = String.Join("", bases);
                    A8.Add(sampleChrom);



                    
                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[0,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B0.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[1,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B1.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[2,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B2.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[3,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B3.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[4,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B4.Add(sampleChrom);


                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[5,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B5.Add(sampleChrom);


                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[6,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B6.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[7,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B7.Add(sampleChrom);

                    for(int e = 0; e < bases.Length; e++)
                    {
                        bases[e] = sampledGenome.extB[8,e];
                    }
                     sampleChrom = String.Join("", bases);
                    B8.Add(sampleChrom);
                    
                    

                }
            }   Save();
        }        
    }

            


            
        


        void Save(){
            itCount += 1;
            string[] rowDataTemp;
        if (itCount == 1){

            rowDataTemp = new string[26];
            rowDataTemp[0] ="time" ;
            rowDataTemp[1] ="name" ;
            rowDataTemp[2] = "sampleGroup";
            rowDataTemp[3] = "sample_number";
            rowDataTemp[4] = "sat1";
            rowDataTemp[5] = "sat2";
            rowDataTemp[6] = "sat3";
            rowDataTemp[7] = "sat4";
            rowDataTemp[8] = "A0";
            rowDataTemp[9] = "A1";
            rowDataTemp[10] = "A2";
            rowDataTemp[11] = "A3";
            rowDataTemp[12] = "A4";
            rowDataTemp[13] = "A5";
            rowDataTemp[14] = "A6";
            rowDataTemp[15] = "A7";
            rowDataTemp[16] = "A8";

            rowDataTemp[17] = "B0";
            rowDataTemp[18] = "B1";
            rowDataTemp[19] = "B2";
            rowDataTemp[20] = "B3";
            rowDataTemp[21] = "B4";
            rowDataTemp[22] = "B5";
            rowDataTemp[23] = "B6";
            rowDataTemp[24] = "B7";
            rowDataTemp[25] = "B8";
            

            rowData.Add(rowDataTemp);
            }


        for(int i = 0; i < sampleSize; i++)
        {   
            int n = 0;
            rowDataTemp = new string[26];
            rowDataTemp[0] = totalTime.ToString();
            rowDataTemp[1] = unitName[i];
            rowDataTemp[2] = sampleGroup.ToString();
            rowDataTemp[3] = i.ToString();
            rowDataTemp[4] = sat1[i].ToString();
            rowDataTemp[5] = sat2[i].ToString();
            rowDataTemp[6] = sat3[i].ToString();
            rowDataTemp[7] = sat4[i].ToString();

            rowDataTemp[8] = A0[i].ToString();
            rowDataTemp[9] = A1[i].ToString();
            rowDataTemp[10] = A2[i].ToString();
            rowDataTemp[11] = A3[i].ToString();
            rowDataTemp[12] = A4[i].ToString();
            rowDataTemp[13] = A5[i].ToString();
            rowDataTemp[14] = A6[i].ToString();
            rowDataTemp[15] = A7[i].ToString();
            rowDataTemp[16] = A8[i].ToString();

            rowDataTemp[17] = B0[i].ToString();
            rowDataTemp[18] = B1[i].ToString();
            rowDataTemp[19] = B2[i].ToString();
            rowDataTemp[20] = B3[i].ToString();
            rowDataTemp[21] = B4[i].ToString();
            rowDataTemp[22] = B5[i].ToString();
            rowDataTemp[23] = B6[i].ToString();
            rowDataTemp[24] = B7[i].ToString();
            rowDataTemp[25] = B8[i].ToString();
            
            
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
        A0.Clear();A1.Clear();A2.Clear();A3.Clear();A4.Clear();A5.Clear();A6.Clear();A7.Clear();A8.Clear();
        B0.Clear();B1.Clear();B2.Clear();B3.Clear();B4.Clear();B5.Clear();B6.Clear();B7.Clear();B8.Clear();
        unitName.Clear();
        Array.Clear(blobs,0,blobs.Length);
        time = 0f;
        sampleGroup += 1;


    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+filename;
        #elif UNITY_ANDROID
        return Application.persistentDataPath+filename;
        #elif UNITY_STANDALONE_OSX
        return Application.dataPath+"/"+filename;
        #else
        return Application.dataPath +"/"+filename;
        #endif
    }
}


