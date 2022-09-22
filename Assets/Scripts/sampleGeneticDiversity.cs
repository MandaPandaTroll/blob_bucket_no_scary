using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sampleGeneticDiversity : MonoBehaviour
{
    GameObject[] blibs;
    GameObject[] blobs;
    GameObject[] blybs;
    GameObject[] blubs;

    public float    sampleRate;
    public int      maxSampleSize;
           int      sampleSize;
           float    t;

           float totHeterozygosity_blib;
    public float meanHeterozygosity_blib;
           float nPolymorphisms_blib;

           float totHeterozygosity_blob;
    public float meanHeterozygosity_blob;
           float nPolymorphisms_blob;

           float totHeterozygosity_blyb;
    public float meanHeterozygosity_blyb;
           float nPolymorphisms_blyb;

           //float totHeterozygosity_blub;
    //public float meanHeterozygosity_blub;
           //float nPolymorphisms_blub;


    string[,] currentGenome_A = new string[9,486];
    string[,] currentGenome_B = new string[9,486];
    
    int numSites;
    
    // Start is called before the first frame update
    void Start()
    {
        numSites = 9 * 486;
        nPolymorphisms_blib = 0;
        totHeterozygosity_blib = 0;
        meanHeterozygosity_blib = 0;

        nPolymorphisms_blob = 0;
        totHeterozygosity_blob = 0;
        meanHeterozygosity_blob = 0;

        nPolymorphisms_blyb = 0;
        totHeterozygosity_blyb = 0;
        meanHeterozygosity_blyb = 0;

        //nPolymorphisms_blub = 0;
        //totHeterozygosity_blub = 0;
        //meanHeterozygosity_blub = 0;

        t = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        t += Time.deltaTime;
        if(t >= sampleRate){

            blibs  = GameObject.FindGameObjectsWithTag("Prey");
            blobs  = GameObject.FindGameObjectsWithTag("Predator");
            blybs  = GameObject.FindGameObjectsWithTag("Predator2");
            //blubs  = GameObject.FindGameObjectsWithTag("ApexPred");
            totHeterozygosity_blib = 0;
            totHeterozygosity_blob = 0;
            totHeterozygosity_blyb = 0;
            //totHeterozygosity_blub = 0;
            

            
            if(blibs.Length >= 1){

                if(blibs.Length < maxSampleSize){sampleSize = blibs.Length;}
                else if(blibs.Length >= maxSampleSize){sampleSize = maxSampleSize;}

                for (int i = 0; i < sampleSize; i++){
                    int sampler = Random.Range(0,blibs.Length);
                    currentGenome_A = blibs[sampler].GetComponent<BlibGenome>().A;
                    currentGenome_B = blibs[sampler].GetComponent<BlibGenome>().B;
                            for(int a = 0; a < 8; a++){
                                for(int b = 0; b < 485; b++){
                                    if(System.String.Equals(currentGenome_A[a,b], currentGenome_B[a,b]) == false){
                                        nPolymorphisms_blib += 1.0f;
                                    }
                                }
                                
                            }
                    totHeterozygosity_blib += nPolymorphisms_blib/numSites;
                    nPolymorphisms_blib = 0;
                }
                meanHeterozygosity_blib = totHeterozygosity_blib/sampleSize;
                //Debug.Log("meanHeterozygosity_blib = " + meanHeterozygosity_blib );
            }

            if(blobs.Length >= 1){

                if(blobs.Length < maxSampleSize){sampleSize = blobs.Length;}
                else if(blobs.Length >= maxSampleSize){sampleSize = maxSampleSize;}

                for (int i = 0; i < sampleSize; i++){
                    int sampler = Random.Range(0,blobs.Length);
                    currentGenome_A = blobs[sampler].GetComponent<BlobGenome>().A;
                    currentGenome_B = blobs[sampler].GetComponent<BlobGenome>().B;
                            for(int a = 0; a < 8; a++){
                                for(int b = 0; b < 485; b++){
                                    if(System.String.Equals(currentGenome_A[a,b], currentGenome_B[a,b]) == false){
                                        nPolymorphisms_blob += 1.0f;
                                    }
                                }
                                
                            }
                    totHeterozygosity_blob += nPolymorphisms_blob/numSites;
                    nPolymorphisms_blob = 0;
                }
                meanHeterozygosity_blob = totHeterozygosity_blob/sampleSize;
                //Debug.Log("meanHeterozygosity_blob = " + meanHeterozygosity_blob );
            }

            if(blybs.Length >= 1){

                if(blybs.Length < maxSampleSize){sampleSize = blybs.Length;}
                else if(blybs.Length >= maxSampleSize){sampleSize = maxSampleSize;}

                for (int i = 0; i < sampleSize; i++){
                    int sampler = Random.Range(0,blybs.Length);
                    currentGenome_A = blybs[sampler].GetComponent<BlybGenome>().A;
                    currentGenome_B = blybs[sampler].GetComponent<BlybGenome>().B;
                            for(int a = 0; a < 8; a++){
                                for(int b = 0; b < 485; b++){
                                    if(System.String.Equals(currentGenome_A[a,b], currentGenome_B[a,b]) == false){
                                        nPolymorphisms_blyb += 1.0f;
                                    }
                                }
                                
                            }
                    totHeterozygosity_blyb += nPolymorphisms_blyb/numSites;
                    nPolymorphisms_blyb = 0;
                }
                meanHeterozygosity_blyb = totHeterozygosity_blyb/sampleSize;
                //Debug.Log("meanHeterozygosity_blyb = " + meanHeterozygosity_blyb );
            }
            /*
            if(blubs.Length >= 1){

                if(blubs.Length < maxSampleSize){sampleSize = blubs.Length;}
                else if(blubs.Length >= maxSampleSize){sampleSize = maxSampleSize;}

                for (int i = 0; i < sampleSize; i++){
                    int sampler = Random.Range(0,blubs.Length);
                    currentGenome_A = blubs[sampler].GetComponent<BlubGenome>().A;
                    currentGenome_B = blubs[sampler].GetComponent<BlubGenome>().B;
                            for(int a = 0; a < 8; a++){
                                for(int b = 0; b < 485; b++){
                                    if(System.String.Equals(currentGenome_A[a,b], currentGenome_B[a,b]) == false){
                                        nPolymorphisms_blub += 1.0f;
                                    }
                                }
                                
                            }
                    totHeterozygosity_blub += nPolymorphisms_blub/numSites;
                    nPolymorphisms_blub = 0;
                }
                meanHeterozygosity_blub = totHeterozygosity_blub/sampleSize;
                Debug.Log("meanHeterozygosity_blub = " + meanHeterozygosity_blub );
            }
               */     
            t = 0;
        }
        
    }
}
