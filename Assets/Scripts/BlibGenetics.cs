//prints genetic data to .csv file
//original code by smkplus
//modified by tabacwoman november 2021


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class BlibGenetics : MonoBehaviour
{


GameObject[] blibs;

private List<string[]> rowData = new List<string[]>();
    int itCount;
    
    float time;
    float totalTime;

    public int maxSampleSize;
    int sampleSize;
    public float sampleRate;
    
   

    int sampler;
    private List<string> unitName = new List<string>();
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

    public List <float> turnDice;
    public List <float> turnTorqueAllele1;
    public List <float> turnTorqueAllele2;

    public List <float> e2repA;
    public List <float> e2repB;
    public List <float> exonRatio;
    float[] heteroZygosity;

    int sampleGroup;

    

    GameObject sampledBlib;
    BlibControls sampbctrl;
    BlibGenome sampbgn;

    // Start is called before the first frame update
    void Start()
    {
        unitName.Clear();
        heteroZygosity = new float[maxSampleSize];
        itCount = 0;
        sampleGroup = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        
        totalTime = Mathf.Round(Time.time);
        time += Time.deltaTime;
        
            if (time >= sampleRate){
               
                blibs  = GameObject.FindGameObjectsWithTag("Prey");
                
                
                if(blibs.Length <= 0){return;}
                if(blibs.Length >= 1){
                    if(blibs.Length < maxSampleSize){sampleSize = blibs.Length;}
                    if(blibs.Length >= maxSampleSize){sampleSize = maxSampleSize;}
                for (int i = 0; i < sampleSize; i++)
                {   
                    sampler = UnityEngine.Random.Range(0,blibs.Length);
                    sampledBlib = blibs[sampler];
                    sampbctrl = sampledBlib.GetComponent<BlibControls>();
                    sampbgn = sampledBlib.GetComponent<BlibGenome>();
                        
                    intron1.Add(sampbctrl.introns[0,0]);
                    intron2.Add(sampbctrl.introns[1,0]);
                    intron3.Add(sampbctrl.introns[0,1]);
                    intron4.Add(sampbctrl.introns[1,1]);

                    moveAllele1.Add(sampbgn.moveAllele1);
                    moveAllele2.Add(sampbgn.moveAllele2); 

                    redAllele1.Add(sampbgn.redAllele1);
                    redAllele2.Add(sampbgn.redAllele2);

                    greenAllele1.Add(sampbgn.greenAllele1);
                    greenAllele2.Add(sampbgn.greenAllele2);

                    blueAllele1.Add(sampbgn.blueAllele1);
                    blueAllele2.Add(sampbgn.blueAllele2);
                    lifeLengthAllele1.Add(sampbgn.lifeLengthAllele1);
                    lifeLengthAllele2.Add(sampbgn.lifeLengthAllele2);
                    turnDice.Add(sampbctrl.turnDice);
                    turnTorqueAllele1.Add(sampbgn.turnTorqueAllele1);
                    turnTorqueAllele2.Add(sampbgn.turnTorqueAllele2);
                    generation.Add(sampbctrl.generation);
                    e2repA.Add(sampbgn.e2repAllele1);
                    e2repB.Add(sampbgn.e2repAllele2);
                    exonRatio.Add(sampbgn.aminoAcidRatio);
                    heteroZygosity[i] = sampbgn.heteroZygosity;
                    unitName.Add(sampledBlib.gameObject.name);
                    
                }           
                
            
             


            



        
            
            
            Save();
                }
            


            }
        


    }
        void Save(){
            itCount += 1;
            string[] rowDataTemp;
        if (itCount == 1){
            rowDataTemp = new string[25];
            rowDataTemp[0] ="time" ;
            rowDataTemp[1] ="name";
            rowDataTemp[2] ="sampleGroup";
            rowDataTemp[3] ="generation";
            rowDataTemp[4] = "intron1";
            rowDataTemp[5] = "intron2";
            rowDataTemp[6] = "intron3";
            rowDataTemp[7] = "intron4";
            rowDataTemp[8] = "moveAllele1";
            rowDataTemp[9] = "moveAllele2";
            rowDataTemp[10] = "redAllele1";
            rowDataTemp[11] = "redAllele2";
            rowDataTemp[12] = "greenAllele1";
            rowDataTemp[13] = "greenAllele2";
            rowDataTemp[14] = "blueAllele1";
            rowDataTemp[15] = "blueAllele2";
            rowDataTemp[16] = "lifeLengthAllele1";
            rowDataTemp[17] = "lifeLengthAllele2";
            rowDataTemp[18] = "turnDice";
            rowDataTemp[19] = "turnTorqueAllele1";
            rowDataTemp[20] = "turnTorqueAllele2";
            rowDataTemp[21] = "e2repA";
            rowDataTemp[22] = "e2repB";
            rowDataTemp[23] = "exon_intron_ratio";
            rowDataTemp[24] = "Heterozygosity";
            rowData.Add(rowDataTemp);
        }


        // You can add up the values in as many cells as you want.
        for(int i = 0; i < sampleSize; i++){
            rowDataTemp = new string[25];
            rowDataTemp[0] = totalTime.ToString();
            rowDataTemp[1] = unitName[i];
            rowDataTemp[2] = sampleGroup.ToString();
            rowDataTemp[3] = generation[i].ToString();
            rowDataTemp[4] = intron1[i].ToString();
            rowDataTemp[5] = intron2[i].ToString();
            rowDataTemp[6] = intron3[i].ToString();
            rowDataTemp[7] = intron4[i].ToString();
            rowDataTemp[8] = moveAllele1[i].ToString();
            rowDataTemp[9] = moveAllele2[i].ToString();
            rowDataTemp[10] = redAllele1[i].ToString();
            rowDataTemp[11] = redAllele2[i].ToString();
            rowDataTemp[12] = greenAllele1[i].ToString();
            rowDataTemp[13] = greenAllele2[i].ToString();
            rowDataTemp[14] = blueAllele1[i].ToString();
            rowDataTemp[15] = blueAllele2[i].ToString();
            rowDataTemp[16] = lifeLengthAllele1[i].ToString();
            rowDataTemp[17] = lifeLengthAllele2[i].ToString();
            rowDataTemp[18] = turnDice[i].ToString();
            rowDataTemp[19] = turnTorqueAllele1[i].ToString();
            rowDataTemp[20] = turnTorqueAllele2[i].ToString();
            rowDataTemp[21] = e2repA[i].ToString();
            rowDataTemp[22] = e2repB[i].ToString();
            rowDataTemp[23] = exonRatio[i].ToString();
            rowDataTemp[24] = heteroZygosity[i].ToString();

            

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
        outStream.Flush();
        outStream.Close();
        outStream.Dispose();
        

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
        turnTorqueAllele1.Clear();
        turnTorqueAllele2.Clear();
        turnDice.Clear();
        generation.Clear();
        e2repA.Clear();
        e2repB.Clear();
        exonRatio.Clear();
        sampleGroup += 1;
        unitName.Clear();

        Array.Clear(blibs,0,blibs.Length);
        time = 0f;



    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Blib_genetics.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Blib_genetics.csv";
        #elif UNITY_STANDALONE_OSX
        return Application.dataPath+"/"+"Blib_genetics.csv";
        #else
        return Application.dataPath +"/"+"Blib_genetics.csv";
        #endif
    }
}



