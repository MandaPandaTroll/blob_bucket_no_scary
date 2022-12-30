//prints genetic data to .csv file
//original code by smkplus
//modified by tabacwoman january 2022


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class BlibSatLogger : MonoBehaviour
{

/*
byte[] asDNAbin = new byte [486*9];
asDNAbin = DNAbin.GetDNAbin(testA);
string bytestring = "";
for(int v = 0; v < asDNAbin.Length; v++){
bytestring += ","+((uint)asDNAbin[v]).ToString() ;
}
Debug.Log(bytestring);
*/


GameObject[] blibs;
    private long internal_fileSize;
    public  long fileSize_MB;
    private List<string[]> rowData = new List<string[]>();
    private List<string> unitName = new List<string>();
    
    private List<string> testA = new List<string>();
    private List<string> testB = new List<string>();
    private List<string> aa_A = new List<string>();
    private List<string> aa_B = new List<string>();
    private List<string> aa_antiA = new List<string>();
    private List<string> aa_antiB = new List<string>();
    private List<float> snpratA = new List<float>();
    private List<float> snpratB = new List<float>();
    private List<int> generation = new List<int>();

    
    private int itCount, sampler, sampleGroup, sampleSize;  
    float time, totalTime;
    public int maxSampleSize;
    public float sampleRate;
    BlibGenome sampledGenome;
    BlibControls  sampledBlib;
   
    string filename;
    int filenum;
    string og_filename = "blib_sats";
    // Start is called before the first frame update
    void Start()
    {   blibs  = GameObject.FindGameObjectsWithTag("Prey");
        
        unitName.Clear();testA.Clear();testB.Clear();aa_A.Clear();aa_antiA.Clear();aa_B.Clear();aa_antiB.Clear();
        Array.Clear(blibs,0,blibs.Length);
        itCount = 0;
        filenum = 0;
        internal_fileSize = fileSize_MB * 1000000;
        filename =  og_filename + filenum.ToString() + ".csv";
        sampleGroup = 0;
        time = 0;

        
    }
    string sampleChrom;
    // Update is called once per frame
    void FixedUpdate()
    {

        
        totalTime = Mathf.Round(Time.time);
        
        time += Time.deltaTime;
        
        if (time >= sampleRate)
        {

            
            
             blibs  = GameObject.FindGameObjectsWithTag("Prey");
            if(blibs.Length <= 0){return;}
            if(blibs.Length >= 1)
            {
                if (blibs.Length < maxSampleSize){
                    sampleSize = blibs.Length;
                    }else{
                        sampleSize = maxSampleSize;
                        }
                        
                for (int i = 0; i < sampleSize; i++)
                {       sampledGenome = null;                                   
                    sampler = UnityEngine.Random.Range(0,sampleSize);
                    sampledBlib = blibs[sampler].GetComponent<BlibControls>();
                    sampledGenome = blibs[sampler].GetComponent<BlibGenome>();
                    //string[] nucleotides = new string[27];
                    //string[] bases = new string[sampledGenome.A.GetLength(1)];
                    unitName.Add(sampledBlib.gameObject.name);
                    //aa_A.Add(sampledGenome.aa_A);
                    //aa_antiA.Add(sampledGenome.aa_antiA);
                    //aa_B.Add(sampledGenome.aa_B);
                    //aa_antiB.Add(sampledGenome.aa_antiB);
                    testA.Add(sampledGenome.testA);
                    testB.Add(sampledGenome.testB);
                    snpratA.Add(sampledGenome.refSNP_A);
                    snpratB.Add(sampledGenome.refSNP_B);
                    generation.Add(sampledBlib.generation);
                }
            }   Save();
        }        
    }

            


            
        


        void Save(){
            itCount += 1;
            string[] rowDataTemp;
        if (itCount == 1){

            rowDataTemp = new string[9];
            rowDataTemp[0] ="time" ;
            rowDataTemp[1] ="name" ;
            rowDataTemp[2] = "sampleGroup";
            rowDataTemp[3] = "sample_number";
            rowDataTemp[4] = "testA";
            rowDataTemp[5] = "testB";
            rowDataTemp[6] = "divergence_ratio_A";
            rowDataTemp[7] = "divergence_ratio_B";
            rowDataTemp[8] = "generation";
            //rowDataTemp[6] = "aa_A";
            //rowDataTemp[7] = "aa_antiA";
            //rowDataTemp[8] = "aa_B";
            //rowDataTemp[9] = "aa_antiB";

            

            rowData.Add(rowDataTemp);
            }


        for(int i = 0; i < sampleSize; i++)
        {   
            
            rowDataTemp = new string[9];
            rowDataTemp[0] = totalTime.ToString();
            rowDataTemp[1] = unitName[i];
            rowDataTemp[2] = sampleGroup.ToString();
            rowDataTemp[3] = i.ToString();
            rowDataTemp[4] = testA[i].ToString();
            rowDataTemp[5] = testB[i].ToString();
            rowDataTemp[6] = snpratA[i].ToString();
            rowDataTemp[7] = snpratB[i].ToString();
            rowDataTemp[8] = generation[i].ToString();
            //rowDataTemp[6] = aa_A[i].ToString();
            //rowDataTemp[7] = aa_antiA[i].ToString();
            //rowDataTemp[8] = aa_B[i].ToString();
            //rowDataTemp[9] = aa_antiB[i].ToString();
            
            
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
            

        
        filename = og_filename + filenum.ToString() + ".csv";
        string filePath = getPath();

        

        StreamWriter outStream = File.CreateText(filePath);
         
        outStream.WriteLine(sb);

        

        outStream.Flush();
        outStream.Close();
        outStream.Dispose();
        Resources.UnloadUnusedAssets();
        FileInfo fi = new FileInfo(filePath);  
        long size = fi.Length;  
        if(size > internal_fileSize){
            filenum += 1;
            filename = og_filename + filenum.ToString() + ".csv";
            sb.Clear();
            rowData.Clear();
            itCount = 0;
            
        }
        
        
        unitName.Clear();testA.Clear();testB.Clear();aa_A.Clear();aa_antiA.Clear();aa_B.Clear();aa_antiB.Clear();snpratA.Clear();snpratB.Clear();generation.Clear();
        Array.Clear(blibs,0,blibs.Length);
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


