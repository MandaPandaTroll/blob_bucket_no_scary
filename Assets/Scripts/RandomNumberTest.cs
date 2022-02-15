
//prints random numbers to .csv file
//original code by smkplus
//modified by tabacwoman november 2021


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class RandomNumberTest : MonoBehaviour
{

GameObject[] blibs;

private List<string[]> rowData = new List<string[]>();

    
    float time;


   public int sampleSize;
    public float sampleRate;
    
   



    public List <int> rand1;
    public List <int> rand2;

    public List <int> rand3;
    public List <int> rand4;
    public List <int> randSeries;
    System.Random rnd = new System.Random();
    
    int seriesIndex = 0;
    int iter;
    
 

    // Start is called before the first frame update
    void Start()
    {

        sampleSize = 1;
        sampleRate = 0.1f;


    }

    // Update is called once per frame
    void Update()
    {
        
        
        do{
            randSeries.Add(rnd.Next(0,128));
        
        iter += 1;
        }while(iter < 10000);
        
        time += Time.deltaTime;
        
            if (time >= sampleRate){
               
                



                
                    
                
                rand1.Add(randSeries[seriesIndex]);
                seriesIndex += 1;  
                rand2.Add(randSeries[seriesIndex]);
                seriesIndex += 1;  
                rand3.Add(randSeries[seriesIndex]);
                seriesIndex += 1;  
                rand4.Add(randSeries[seriesIndex]);
                seriesIndex += 1;  
                


                        
                
            
             


            



        
            
            
            Save();
            


            }
        


    }
        void Save(){
           

        
             string[] rowDataTemp = new string[4];
            

            rowDataTemp[0] = rand1[0].ToString();
            rowDataTemp[1] = rand2[0].ToString();
            rowDataTemp[2] = rand3[0].ToString();
            rowDataTemp[3] = rand4[0].ToString();

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
        

        rand1.Clear();
        rand2.Clear();
        rand3.Clear();
        rand4.Clear();


        time = 0f;



    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Random_numbers.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Random_numbers.csv";
        #elif UNITY_STANDALONE_OSX
        return Application.dataPath+"/"+"Random_numbers.csv";
        #else
        return Application.dataPath +"/"+"Random_numbers.csv";
        #endif
    }
}



