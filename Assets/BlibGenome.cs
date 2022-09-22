// This script contains genetic information and converts the sequences to numeric values for the organism's phenotype. It also handles gene transfer and mutations.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;


public struct PhenoStruct{
    
    
    
    
    
    
    
    public PhenoStruct(float redGene, float greenGene, float blueGene, float moveForce, float turnTorque, float energyToReproduce, float lifeLength){
            
        }
    }
    
    public struct GenoPhenoStruct{
    public float redAllele1;
    public float redAllele2;
    public float redGene;
    public float greenAllele1;
    public float greenAllele2;
    public float greenGene;
    public float blueAllele1;
    public float blueAllele2;
    public float blueGene;
    public float moveAllele1;
    public float moveAllele2;
    public float moveForce;
    public float turnTorqueAllele1;
    public float turnTorqueAllele2;
    public float turnTorque;
    public float e2repAllele1;
    public float e2repAllele2;
    public float energyToReproduce;
    public float lifeLengthAllele1;
    public float lifeLengthAllele2;
    public float lifeLength;
     GenoPhenoStruct(
                float redAllele1,
                float redAllele2,
                float greenAllele1,
                float greenAllele2,
                float blueAllele1,
                float blueAllele2,
                float moveAllele1,
                float moveAllele2,
                float turnTorqueAllele1,
                float turnTorqueAllele2,
                float e2repAllele1,
                float e2repAllele2,
                float lifeLengthAllele1,
                float lifeLengthAllele2){
                    
            
            
            
            
            
            
                this.redAllele1 = redAllele1;
                this.redAllele2 = redAllele2;
                this.redGene = redGene;
                this.greenAllele1 = greenAllele1;
                this.greenAllele2 = greenAllele2;
                this.greenGene = greenGene;
                this.blueAllele1 = blueAllele1;
                this.blueAllele2 = blueAllele2;
                this.blueGene = blueGene;
                this.moveAllele1 = moveAllele1;
                this.moveAllele2 = moveAllele2;
                this.moveForce = moveForce;
                this.turnTorqueAllele1 = turnTorqueAllele1;
                this.turnTorqueAllele2 = turnTorqueAllele2;
                this.turnTorque = turnTorque;
                this.e2repAllele1 = e2repAllele1;
                this.e2repAllele2 = e2repAllele2;
                this.energyToReproduce = energyToReproduce;
                this.lifeLengthAllele1 = lifeLengthAllele1;
                this.lifeLengthAllele2 = lifeLengthAllele2;
                this.lifeLength = lifeLength;
        }
    }


public class BlibGenome : MonoBehaviour
{
     PhenoStruct  phenotype_struct = new PhenoStruct();
     GenoStruct  genotype_struct = new GenoStruct();
    int[] pois = new int[20]{0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0};
   public float[] phenotype = new float[7];
    public initGenomeTest_blib initGenomeTest_blib;
    public bool translocation_enabled;
    public bool duplication_enabled;
    public string[,] A = new string[9,486];
    public string[,] B = new string[9,486];
    
    public List<string> lineageID = new List<string>();
   public static List <BlibGenome> blib_genomes = new List <BlibGenome>();
    public float greenAllele1, greenAllele2, greenGene,
                 redAllele1, redAllele2, redGene,
                 blueAllele1, blueAllele2, blueGene,
                 turnTorqueAllele1, turnTorqueAllele2, turnTorque,
                 e2repAllele1, e2repAllele2, energyToReproduce,
                 moveAllele1, moveAllele2, moveForce, 
                 lifeLengthAllele1, lifeLengthAllele2, lifeLength;
    
    //private string redSeq, greenSeq, blueSeq, moveSeq, turnSeq, repSeq, lifSeq;
    //Get relevant amino acid sequences from database.
       string redSeq = GeneDatabase.red;
       string greenSeq = GeneDatabase.green;
       string blueSeq = GeneDatabase.blue;
       string moveSeq = GeneDatabase.move;
       string turnSeq = GeneDatabase.turnt;
       string repSeq = GeneDatabase.rep;
       string lifSeq = GeneDatabase.lifeL;
 public bool mutate = false;
   public int numMutations;
       public string[] giveLocus = new string[27];
       public string[] receiveLocus = new string[27];

    BlibControls blibControls;
    public BlibGenome mother; //Could be used to debug if genome is being properly transferred to offspring.
    float nGRN_A, nGRN_B,
        nRED_A,nRED_B,
        nLLY_A, nLLY_B, 
        nMVV_A, nMVV_B,
        nTRN_A, nTRN_B,
        nREP_A, nREP_B,
        nLIF_A, nLIF_B;

         //Current chromosomal loci
    int [,] sites = new int[9,19] {
        {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486},

        {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486},

        {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486},

        
        };
public string[,] extA, extB;
//Chromosome A 
    //public string[,] A = new string[9,486]
    /*
        // 0: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
           {{"A", "T", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "T", "A", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "A", "G", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "G", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "A", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 1: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 2: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "T", "G", "A", "A", "T", "G", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "T", "G", "A", "A", "T", "G", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "T", "G", "A", "A", "T", "G", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "T", "G", "A", "A", "T", "G", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "T", "G", "A", "A", "T", "G", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "T", "G", "A", "A", "T", "G", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "T", "G", "A", "A", "T", "G", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "T", "G", "A", "A", "T", "G", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 3: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 4: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 5: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 6: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" },
        // 7: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "C", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" },
        // 8: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" }};
        */

    //Chromosome B
    //public string[,] B = new string[9,486]
    /*
        // 0: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
           {{"A", "T", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "T", "A", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "A", "G", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "G", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "A", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "G", "A", "G", "G", "A", "G", "A", "A", "T", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 1: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "G", "G", "T", "C", "G", "T", "A", "A", "T", "G", "G", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 2: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "T", "G", "A", "A", "T", "G", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "T", "G", "A", "A", "T", "G", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "G", "C", "T", "T", "G", "A", "A", "T", "G", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "T", "G", "A", "A", "T", "G", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "T", "G", "A", "A", "T", "G", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "G", "G", "T", "T", "G", "A", "A", "T", "G", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "T", "G", "A", "A", "T", "G", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "C", "T", "T", "T", "G", "A", "A", "T", "G", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "A", "T", "T", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 3: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 4: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "C", "T", "C", "G", "T", "A", "A", "T", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 5: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A"  },
        // 6: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" },
        // 7: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "G", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" },
        // 8: L0 ->                                                                                                                             27 | L1 ->                                                                                                                             54 | L2 ->                                                                                                                             81 | L3 ->                                                                                                                            108 | L4 ->                                                                                                                            135 | L5 ->                                                                                                                            162 | L6 ->                                                                                                                            189 | L7 ->                                                                                                                            216 | L8 ->                                                                                                                            243 | L9 ->                                                                                                                             270| L10 ->                                                                                                                            297| L11 ->                                                                                                                            324| L12 ->                                                                                                                           351 | L13 ->                                                                                                                           378 | L14 ->                                                                                                                           405 | L15 ->                                                                                                                           432 | L16 ->                                                                                                                           459 | L17 ->                                                                                                                           486 |
            {"A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "T", "G", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "T", "G", "G", "T", "T", "G", "T", "T", "A", "A", "A", "T", "G", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" }};
        */
   public int chromoPairs, basePairs;
    public bool randGenome;
    int baseCount = 0;
    bool firstTranslation;
    


   
    
    void Awake(){
        blibControls = this.gameObject.GetComponent<BlibControls>();
         
        Mutate();
        TranslateGenome();

        
        

    }
   
    void Start()
    {
        Debug.Log("struct_start" + " redGene" + phenotype_struct.redGene+ " greenGene " + phenotype_struct.greenGene+ " blueGene " + phenotype_struct.blueGene+ " moveForce " + phenotype_struct.moveForce+ " turnTorque " + phenotype_struct.turnTorque+ " energyToReproduce " + phenotype_struct.energyToReproduce+ " lifeLength"+ phenotype_struct.lifeLength);
        Array.Clear(A,0,A.Length);
        Array.Clear(B,0,B.Length);
        
        firstTranslation = false;
        

        
        
        chromoPairs = A.GetLength(0);
        basePairs = A.GetLength(1);
        

        

        if(mother != null){
            A = mother.A;
            B = mother.B;
        }else if(Time.time < 1.0f){
            A = initGenomeTest_blib.A;
            B = initGenomeTest_blib.B;
        }

        tempS = null;
        
        codon = new string[3]{"","",""}; 
         allelesA = "";
         allelesB = "";
         thisAlleleA = "";
         thisAlleleB = "";

         nAllelesA = 0;
         stopA = "";
         nAllelesB = 0;
         stopB = "";
         isGene = false;


        
        if(randGenome == true){
            for(int i = 0; i < chromoPairs;i++){
                for(int j = 0; j < basePairs;j++){
                    int randBase = UnityEngine.Random.Range(1,5);
                    string randChar;
                    if (randBase == 1){randChar = "A";}
                    else if (randBase == 2){randChar = "C";}
                    else if (randBase == 3){randChar = "G";}
                    else {randChar = "T";}
                    A[i,j] = randChar;
                    B[i,j] = randChar; 
                    
                }

            }

            
        }
        
       
       
     
    }


public int loSite, cNum, locus;
private int AorB, mate_loSite,mate_cNum;
void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Prey"){

         cNum = UnityEngine.Random.Range(0,A.GetLength(0));
         AorB = UnityEngine.Random.Range(0,2);
        locus = UnityEngine.Random.Range(0,sites.GetLength(1)-1);
        loSite = sites[cNum,locus];
        
        int snpCountA = 0,snpCountB = 0;
        bool isEqual;
        
        for (int i = 0 ; i < 27; i++)
        {   if (AorB == 0){
                giveLocus[i] = A[cNum,loSite+i];
            }else{
                giveLocus[i] = B[cNum,loSite+i];
            }
            
        }
        /*
        string debugChar;
        if (AorB == 0){debugChar ="A";}else{debugChar = "B";}
        string debugLocus = System.String.Join("",giveLocus);
       // //Debug.Log("Chromosome: " + debugChar + ":" + cNum + " Locus: " + locus + " -> " + debugLocus);
       */
       
        BlibGenome mateGenome = col.gameObject.GetComponent<BlibGenome>();
        
        
        
        //A-Compare
        for(int i = 0; i < chromoPairs; i++){
            for(int j = 0; j < basePairs; j++){
                isEqual = System.String.Equals(this.A[i,j], mateGenome.A[i,j]);
                if(isEqual == false){
                    snpCountA +=1;
                }
                }
            }

        //B-Compare
        for(int i = 0; i < chromoPairs; i++){
            for(int j = 0; j < basePairs; j++){
                isEqual = System.String.Equals(this.B[i,j], mateGenome.B[i,j]);
                if(isEqual == false){
                    snpCountB +=1;
                }
                }
            }

            //Genetic distance calculations
            float pairWiseA_squared = Mathf.Pow(((float)snpCountA/1944.0f),2.0f);
            float pairWiseB_squared = Mathf.Pow(((float)snpCountA/1944.0f),2.0f);

            float pythagDist = Mathf.Sqrt( pairWiseA_squared + pairWiseB_squared );
            if( pythagDist > 0.01){
           // //Debug.Log("SNP-A = " + snpCountA + ", SNP-B = " + snpCountB + " PWA = " + pairWiseA_squared + " PWB = " + pairWiseB_squared +
           // " Pythagorean distance (BP) = " + pythagDist);
            }
        

            //Genetic recombination
            if(pythagDist < 0.1f){

                receiveLocus = mateGenome.giveLocus;
                mate_cNum = mateGenome.cNum;
                mate_loSite = mateGenome.loSite;
                
                AorB = UnityEngine.Random.Range(0,2);

                if(AorB == 0){
                    for(int i = 0; i < 27; i++){
                    A[mate_cNum,mate_loSite+i] = receiveLocus[i];
                    }
                }

                if(AorB == 1){
                    for(int i = 0; i < 27; i++){
                    A[mate_cNum,mate_loSite+i] = receiveLocus[i];
                    }
                }
                
            }

        }
    }


    
    
    



    // Update is called once per frame
    void Update()
    {   
        int mutationroll = UnityEngine.Random.Range(0,4096*2);

        if (mutationroll == 64){mutate = true;}
        extA = A;
        extB = B;
        if(firstTranslation == false){
            TranslateGenome();
        }
        if(this.gameObject.GetComponent<BlibControls>().age > 0.5f ){
        if ( mutate == true)
        {
            Mutate();
            
        }
        }
                
    }
    void Mutate() {
    string randChar = "A";
    AorB = UnityEngine.Random.Range(0,2);
    int duplicationRoll = UnityEngine.Random.Range(0,1024);
    int transLocRoll = UnityEngine.Random.Range(0,1024);

    string[] triNu = new string[3];
    string[] tranString_origin = new string[27];
    string[] tranString_destination = new string[27];
    int siteCroms = sites.GetLength(0);
    int siteLocs = sites.GetLength(1);
    
    int[] triIndex = new int[2]{UnityEngine.Random.Range(0,chromoPairs),UnityEngine.Random.Range(3,basePairs-6)};
    
    if(transLocRoll == 64 && translocation_enabled == true){
        string [] oNu = new string[3];
        string origin ="", destination ="";
        int tAorB = UnityEngine.Random.Range(0,2);

        int siteIndex_originI = UnityEngine.Random.Range(0,siteCroms);
        int siteIndex_originJ = sites[siteIndex_originI, UnityEngine.Random.Range(0,siteLocs-1)];

       

        int siteIndex_destinationI = UnityEngine.Random.Range(0,siteCroms);
        int siteIndex_destinationJ = sites[siteIndex_destinationI, UnityEngine.Random.Range(0,siteLocs-1)];

        


        if(AorB == 0){
            origin = "A";

        for(int i = 0; i < 27; i++){
            tranString_origin[i] = A[siteIndex_originI, siteIndex_originJ +i];
        }

        if (tAorB == 0){
            destination = "A";


            for(int i = 0; i < 27; i++){
            tranString_destination[i] = A[siteIndex_destinationI, siteIndex_destinationJ +i];
            }

            //Replace destination
            for(int i = 0; i < 27; i++){
            A[siteIndex_destinationI,siteIndex_destinationJ +i] = tranString_origin[i];
            }
            //Replace origin
            for(int i = 0; i < 27; i++){
            A[siteIndex_originI,siteIndex_originJ +i] = tranString_destination[i];
            }

            }
        else{
            destination = "B";
            for(int i = 0; i < 27; i++){
            tranString_destination[i] = B[siteIndex_destinationI, siteIndex_destinationJ +i];
            }

            //Replace destination
            for(int i = 0; i < 27; i++){
            B[siteIndex_destinationI,siteIndex_destinationJ +i] = tranString_origin[i];
            }
            //Replace origin
            for(int i = 0; i < 27; i++){
            A[siteIndex_originI,siteIndex_originJ +i] = tranString_destination[i];
            }

            }



        }
        else{
            origin = "B";
        for(int i = 0; i < 27; i++){
            tranString_origin[i] = B[siteIndex_originI, siteIndex_originJ +i];
        }

        if (tAorB == 0){
            destination = "A";
            for(int i = 0; i < 27; i++){
            tranString_destination[i] = A[siteIndex_destinationI, siteIndex_destinationJ +i];
            }
            
            //Replace destination
            for(int i = 0; i < 27; i++){
            A[siteIndex_destinationI,siteIndex_destinationJ +i] = tranString_origin[i];
            }

            //Replace origin
            for(int i = 0; i < 27; i++){
            B[siteIndex_originI,siteIndex_originJ +i] = tranString_destination[i];
            }

            }
        else{
            destination = "B";

            for(int i = 0; i < 27; i++){
            tranString_destination[i] = B[siteIndex_destinationI, siteIndex_destinationJ +i];
            }
            //Replace destination
            for(int i = 0; i < 27; i++){
            B[siteIndex_destinationI,siteIndex_destinationJ +i] = tranString_origin[i];
            }
            //Replace origin
            for(int i = 0; i < 27; i++){
            B[siteIndex_originI,siteIndex_originJ +i] = tranString_destination[i];
            }
            
            }

        }

        
    //Debug.Log("Translocation :  " + " Origin: " + origin + "["+siteIndex_originI+","+siteIndex_originJ+"]" + "-[" + System.String.Join("", tranString_origin) +  "] " + " Destination: " + destination + "["+siteIndex_destinationI+","+siteIndex_destinationJ+"]" + "-[" + System.String.Join("", tranString_destination) +  "]");
    }

    if(duplicationRoll == 64 && duplication_enabled == true){
        
        if(AorB == 0){
            triNu[0] = A[triIndex[0],triIndex[1]];
            triNu[1] = A[triIndex[0],triIndex[1]+1];
            triNu[2] = A[triIndex[0],triIndex[1]+2];

            string nextD = System.String.Join("",A[triIndex[0],triIndex[1]+3]+A[triIndex[0],triIndex[1]+4]+A[triIndex[0],triIndex[1]+5]);
            string thisD = System.String.Join("", triNu[0]+triNu[1]+triNu[2]);
            
                A[triIndex[0],triIndex[1]+3] = triNu[0];
                A[triIndex[0],triIndex[1]+4] = triNu[1];
                A[triIndex[0],triIndex[1]+5] = triNu[2];
                //Debug.Log("Duplication! : " + "[" + triIndex[0] + "," + triIndex[1] + "] " + thisD + "-" + nextD);
                
            } 

        else{
            triNu[0] = B[triIndex[0],triIndex[1]];
            triNu[1] = B[triIndex[0],triIndex[1]+1];
            triNu[2] = B[triIndex[0],triIndex[1]+2];
            string nextD = System.String.Join("",B[triIndex[0],triIndex[1]+3]+B[triIndex[0],triIndex[1]+4]+B[triIndex[0],triIndex[1]+5]);
            string thisD = System.String.Join("", triNu[0]+triNu[1]+triNu[2]);
            
                B[triIndex[0],triIndex[1]+3] = triNu[0];
                B[triIndex[0],triIndex[1]+4] = triNu[1];
                B[triIndex[0],triIndex[1]+5] = triNu[2];
                //Debug.Log("Duplication! : " + "[" + triIndex[0] + "," + triIndex[1] + "] " + thisD + "-" + nextD);
                
            } 
    }
     
    
    numMutations = pois[UnityEngine.Random.Range(0,pois.Length)];
    for (int i = 0; i < numMutations; i++){
    
    int index0 = UnityEngine.Random.Range(0, 8);
    int index1 = UnityEngine.Random.Range(0, 485);
    int randBase = UnityEngine.Random.Range(0,2);
    int pointTypeRoll = UnityEngine.Random.Range(1,101);
    string pointType;
    string pyrOrPur;
        if (pointTypeRoll < 10){pointType = "trv";}else{pointType = "trt";}

        
       

        if (AorB == 0){
            if(A[index0,index1] == "A" || A[index0,index1] == "G"){
                pyrOrPur ="pur";
            }else{pyrOrPur = "pyr";}

        if(pointType == "trt"){
            if(pyrOrPur == "pur"){
                if(randBase == 0){
                    randChar = "A";
                }
                else if(randBase == 1){
                    randChar = "G";
                }
            }
            if(pyrOrPur == "pyr"){
                if(randBase == 0){
                    randChar = "C";
                }
                else if(randBase == 1){
                    randChar = "T";
                }
            }
        }

        if(pointType == "trv"){
            if(pyrOrPur == "pur"){
                if(randBase == 0){
                    randChar = "C";
                }
                else if(randBase == 1){
                    randChar = "T";
                }
            }
            if(pyrOrPur == "pyr"){
                if(randBase == 0){
                    randChar = "A";
                }
                else if(randBase == 1){
                    randChar = "G";
                }
            }
        }

        A[index0,index1] = randChar; 
        
        }

        else{
            if(B[index0,index1] == "A" || B[index0,index1] == "G"){
                pyrOrPur ="pur";
            }else{pyrOrPur = "pyr";}


        if(pointType == "trt"){
            if(pyrOrPur == "pur"){
                if(randBase == 0){
                    randChar = "A";
                }
                else if(randBase == 1){
                    randChar = "G";
                }
            }
            if(pyrOrPur == "pyr"){
                if(randBase == 0){
                    randChar = "C";
                }
                else if(randBase == 1){
                    randChar = "T";
                }
            }
        }

        if(pointType == "trv"){
            if(pyrOrPur == "pur"){
                if(randBase == 0){
                    randChar = "C";
                }
                else if(randBase == 1){
                    randChar = "T";
                }
            }
            if(pyrOrPur == "pyr"){
                if(randBase == 0){
                    randChar = "A";
                }
                else if(randBase == 1){
                    randChar = "G";
                }
            }
        }

             B[index0,index1] = randChar; 
    }
}

        mutate = false;
        
}

    

string[] codon= new string[3];
string allelesA = "";
string allelesB = "";
string thisAlleleA = "";
string thisAlleleB = "";

     int nAllelesA = 0;
     string stopA = "";
     int nAllelesB = 0;
     string stopB = "";
     bool isGene = false;


string tempS = "";


int codonCount = 0;
bool doConversion;
int numRecoms;
int recoChromos;
int numRecoLoci;
int whichChromo;

        int  recoStart_locus;
        int recoStart_site;
         
         
        int recoEnd_locus;
        int recoEnd_site;
        

        int recoLength;
        List <string> recoBuffer_A = new List<string>(); 
        List <string> recoBuffer_B = new List<string>(); 
        List <string> donatorBuffer = new List<string>(); 

 void TranslateGenome() {

     bool translating = true;
     
     if(firstTranslation == false ){ //Recombination
        
         doConversion = false;
        
         numRecoms = UnityEngine.Random.Range(1, 16);

         recoChromos = 9;
         numRecoLoci = 19;
        

        
         whichChromo = UnityEngine.Random.Range(0,recoChromos);

          recoStart_locus =  UnityEngine.Random.Range(0,numRecoLoci-1);
         recoStart_site = sites[whichChromo, recoStart_locus];
         
         
         recoEnd_locus =  UnityEngine.Random.Range(recoStart_locus, numRecoLoci);
         recoEnd_site = sites[whichChromo, recoEnd_locus];
        

         recoLength = sites[whichChromo,recoEnd_locus]-sites[whichChromo,recoStart_locus];
          
          /*
          recoBuffer_A = new string[recoLength];
          recoBuffer_B = new string[recoLength];
          donatorBuffer = new string[recoLength];
          */
          int conversionDice;
            int donator;

        for(int n = 0; n < numRecoms; n++){
             conversionDice = UnityEngine.Random.Range(0,4096);
             donator = -1;

            if(conversionDice == 64){
            donator = UnityEngine.Random.Range(0,2);
            doConversion = true;
            }else{doConversion = false;}

            for(int i = recoStart_site; i < recoEnd_site; i++){
            recoBuffer_A.Add(A[whichChromo,i]);
            recoBuffer_B.Add(B[whichChromo,i]);
            }

            for (int i = recoStart_site; i < recoEnd_site; i++){
                if(doConversion == false){
                    A[whichChromo,i] = recoBuffer_B[i - recoStart_site];
                    B[whichChromo,i] = recoBuffer_A[i - recoStart_site];
                }else if(doConversion == true){
                    if(donator == 0){
                        B[whichChromo,i] = recoBuffer_A[i - recoStart_site];
                     }else if(donator == 1){
                        A[whichChromo,i] = recoBuffer_B[i - recoStart_site];
                    }
                }

            }
            recoBuffer_A.Clear();
            recoBuffer_B.Clear();
        }
            

        

    }

do{
mutate = false;
        codon = new string[3]{"","",""};
         allelesA = "";
         allelesB = "";
         thisAlleleA = "";
         thisAlleleB = "";

         nAllelesA = 0;
         stopA = "";
         nAllelesB = 0;
         stopB = "";
         isGene = false;




    nGRN_A = 0; nGRN_B = 0; nRED_A = 0; nRED_B = 0;
     nLLY_A = 0; nLLY_B = 0; nMVV_A = 0; nMVV_B = 0;
      nTRN_A = 0; nTRN_B = 0; nREP_A = 0; nREP_B = 0;

            
        int numBasesA = 0;
        int numBasesB = 0;
        codonCount = 0;
        baseCount = 0;
        for (int i = 0; i < A.GetLength(0); i++){
        for (int j = 0; j < basePairs;j++)
        {   
            if(i == 0 && j == 0){allelesA = ""; allelesB = ""; thisAlleleA = ""; thisAlleleB = "";}
            
            numBasesA +=1;
            
 
                        if (baseCount == 3){
                            
                            
                                
                                
                                
                                if(codon[0] == "A"){                     //A-

                                if(codon[1] == "A"){                     //AA-
                                    if(codon[2] == "A" ) {tempS = "K";}            //AAA
                                    else if (codon[2] == "T" ) {tempS = ("N");}      //AAT
                                    else if (codon[2] == "C" ) {tempS = ("N");}      //AAC
                                    else if (codon[2] == "G" ) {tempS = ("K");}      //AAG
                                    
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "T"){              //AT-
                                    if(codon[2] == "A" ) {tempS = ("I");}           //ATA
                                    else if (codon[2] == "T" ) {tempS = ("I");}     //ATT
                                    else if (codon[2] == "C" ) {tempS = ("I");}     //ATC
                                    else if (codon[2] == "G" ) {                    //ATG
                                        if ( thisAlleleA.Length > 0){tempS = ("M");}
                                        if (thisAlleleA.Length == 0){isGene = true; tempS = "<";}
                                         
                                        }
                                   
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "C"){               //AC-
                                    if(codon[2] == "A" ) {tempS = ("T");}           //ACA
                                    else if (codon[2] == "T" ) {tempS = ("T");}     //ACT
                                    else if (codon[2] == "C" ) {tempS = ("T");}     //ACC
                                    else if (codon[2] == "G" ) {tempS = ("T");}     //ACG
                                    
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                                else if (codon[1] == "G"){               //AG-
                                    if(codon[2] == "A" ) {tempS = ("R");}           //AGA
                                    else if (codon[2] == "T" ) {tempS = ("S");}     //AGT
                                    else if (codon[2] == "C" ) {tempS = ("S");}     //AGC
                                    else if (codon[2] == "G" ) {tempS = ("R");}     //AGG
                                    
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                            }
                            else if (codon[0] == "T"){              //T-
                                if(codon[1] == "A"){                     //TA-
                                    if(codon[2] == "A" ) { 
                                        tempS = ">";
                                        thisAlleleA += tempS; 
                                        codonCount = 0;                         //TAA
                                         allelesA += thisAlleleA;
                                         isGene = false;
                                         nAllelesA +=1;
                                         stopA +=  "-" + nAllelesA.ToString()+ "TAA" + "-";
                                         thisAlleleA = "";
                                         tempS = "";
                                         
                                        }  

                                    else if (codon[2] == "T" ) {tempS = ("Y");}     //TAT
                                    else if (codon[2] == "C" ) {tempS = ("Y");}     //TAC
                                    else if (codon[2] == "G" )                      //TAG
                                    {tempS = ">"; 
                                    thisAlleleA += tempS;     
                                            codonCount = 0; 
                                            isGene = false;
                                         allelesA += thisAlleleA;
                                         nAllelesA +=1;
                                         stopA +=  "-" + nAllelesA.ToString()+ "TAG" + "-";
                                         thisAlleleA = "";
                                        tempS = "";
                                        }  
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "T"){              //TT-
                                    if(codon[2] == "A" ) {tempS = ("L");}           //TTA
                                    else if (codon[2] == "T" ) {tempS = ("F");}     //TTT
                                    else if (codon[2] == "C" ) {tempS = ("F");}     //TTC
                                    else if (codon[2] == "G" ) {tempS = ("L");}     //TTG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "C"){               //TC-
                                    if(codon[2] == "A" ) {tempS = ("S");}           //TCA
                                    else if (codon[2] == "T" ) {tempS = ("S");}     //TCT
                                    else if (codon[2] == "C" ) {tempS = ("S");}     //TCC
                                    else if (codon[2] == "G" ) {tempS = ("S");}     //TCG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                                else if (codon[1] == "G"){               //TG-
                                    if(codon[2] == "A" )                             //TGA
                                    {tempS = ">"; thisAlleleA += tempS;     
                                            codonCount = 0; 
                                            isGene = false;
                                         allelesA += thisAlleleA;
                                         nAllelesA +=1;
                                         stopA +=  "-" + nAllelesA.ToString()+ "TGA" + "-";
                                         thisAlleleA = "";
                                         tempS = "";
                                         
                                        }            
                                    else if (codon[2] == "T" ) {tempS = ("C");}     //TGT
                                    else if (codon[2] == "C" ) {tempS = ("C");}     //TGC
                                    else if (codon[2] == "G" ) {tempS = ("W");}     //TGG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                            }
                            else if (codon[0] == "C"){               //C-
                                if(codon[1] == "A"){                     //CA-
                                    if(codon[2] == "A" ) {tempS = ("Q");}           //CAA
                                    else if (codon[2] == "T" ) {tempS = ("H");}     //CAT
                                    else if (codon[2] == "C" ) {tempS = ("H");}     //CAC
                                    else if (codon[2] == "G" ) {tempS = ("Q");}     //CAG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "T"){              //CT-
                                    if(codon[2] == "A" ) {tempS = ("L");}           //CTA
                                    else if (codon[2] == "T" ) {tempS = ("L");}     //CTT
                                    else if (codon[2] == "C" ) {tempS = ("L");}     //CTC
                                    else if (codon[2] == "G" ) {tempS = ("L");}     //CTG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "C"){               //CC-
                                    if(codon[2] == "A" ) {tempS = ("P");}           //CCA
                                    else if (codon[2] == "T" ) {tempS = ("P");}     //CCT
                                    else if (codon[2] == "C" ) {tempS = ("P");}     //CCC
                                    else if (codon[2] == "G" ) {tempS = ("P");}     //CCG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                                else if (codon[1] == "G"){               //CG-
                                    if(codon[2] == "A" ) {tempS = ("R");}           //CGA
                                    else if (codon[2] == "T" ) {tempS = ("R");}     //CGT
                                    else if (codon[2] == "C" ) {tempS = ("R");}     //CGC
                                    else if (codon[2] == "G" ) {tempS = ("R");}     //CGG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                            }
                            else if (codon[0] == "G"){              //G-
                                if(codon[1] == "A"){                     //GA-
                                    if(codon[2] == "A" ) {tempS = ("E");}           //GAA
                                    else if (codon[2] == "T" ) {tempS = ("D");}     //GAT
                                    else if (codon[2] == "C" ) {tempS = ("D");}     //GAC
                                    else if (codon[2] == "G" ) {tempS = ("E");}     //GAG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "T"){              //GT-
                                    if(codon[2] == "A" ) {tempS = ("V");}           //GTA
                                    else if (codon[2] == "T" ) {tempS = ("V");}     //GTT
                                    else if (codon[2] == "C" ) {tempS = ("V");}     //GTC
                                    else if (codon[2] == "G" ) {tempS = ("V");}     //GTG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }

                                else if (codon[1] == "C"){               //GC-
                                    if(codon[2] == "A" ) {tempS = ("A");}           //GCA
                                    else if (codon[2] == "T" ) {tempS = ("A");}     //GCT
                                    else if (codon[2] == "C" ) {tempS = ("A");}     //GCC
                                    else if (codon[2] == "G" ) {tempS = ("A");}     //GCG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                                else if (codon[1] == "G"){               //GG-
                                    if(codon[2] == "A" ) {tempS = ("G");}           //GGA
                                    else if (codon[2] == "T" ) {tempS = ("G");}     //GGT
                                    else if (codon[2] == "C" ) {tempS = ("G");}     //GGC
                                    else if (codon[2] == "G" ) {tempS = ("G");}     //GGG
                                    if (isGene == true ){ thisAlleleA += tempS;}
                                }
                            }
                            




                            codonCount += 1;
                             baseCount = 0;
                            }

            codon[baseCount]=(A[i,j]);
            baseCount +=1;
        }
     }
            
            //Debug.Log("numBasesA = " + numBasesA + " nAllelesA = " + nAllelesA + " StopsA = " + stopA);
         
        
        codonCount = 0;
        baseCount = 0;
        for (int x = 0; x < B.GetLength(0); x++){
        for (int y = 0; y < basePairs;y++)
        {   
            if(x == 0 && y == 0){ allelesB = ""; thisAlleleA = ""; thisAlleleB = "";}
            numBasesB +=1;
 
                        if (baseCount == 3){
                            
                            
                            
                                
                                if(codon[0] == "A"){                     //A-

                                if(codon[1] == "A"){                     //AA-
                                    if(codon[2] == "A" ) {tempS = "K";}            //AAA
                                    else if (codon[2] == "T" ) {tempS = ("N");}      //AAT
                                    else if (codon[2] == "C" ) {tempS = ("N");}      //AAC
                                    else if (codon[2] == "G" ) {tempS = ("K");}      //AAG
                                    
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "T"){              //AT-
                                    if(codon[2] == "A" ) {tempS = ("I");}           //ATA
                                    else if (codon[2] == "T" ) {tempS = ("I");}     //ATT
                                    else if (codon[2] == "C" ) {tempS = ("I");}     //ATC
                                     if (codon[2] == "G" ) {                        //ATG
                                            if ( thisAlleleB.Length > 0){tempS = ("M");}
                                        if (thisAlleleB.Length == 0){isGene = true; tempS = "<";}
                                        }
                                   
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "C"){               //AC-
                                    if(codon[2] == "A" ) {tempS = ("T");}           //ACA
                                    else if (codon[2] == "T" ) {tempS = ("T");}     //ACT
                                    else if (codon[2] == "C" ) {tempS = ("T");}     //ACC
                                    else if (codon[2] == "G" ) {tempS = ("T");}     //ACG
                                    
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                                else if (codon[1] == "G"){               //AG-
                                    if(codon[2] == "A" ) {tempS = ("R");}           //AGA
                                    else if (codon[2] == "T" ) {tempS = ("S");}     //AGT
                                    else if (codon[2] == "C" ) {tempS = ("S");}     //AGC
                                    else if (codon[2] == "G" ) {tempS = ("R");}     //AGG
                                    
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                            }
                            else if (codon[0] == "T"){              //T-
                                if(codon[1] == "A"){                     //TA-
                                    if(codon[2] == "A" ) { 
                                        tempS = ">";
                                        thisAlleleB += tempS; 
                                        codonCount = 0;                         //TAA
                                         allelesB += thisAlleleB;
                                         nAllelesB +=1;
                                         stopB +=  "-" + nAllelesB.ToString()+ "TAA" + "-";
                                         thisAlleleB = "";
                                         
                                        }  

                                    else if (codon[2] == "T" ) {tempS = ("Y");}     //TAT
                                    else if (codon[2] == "C" ) {tempS = ("Y");}     //TAC
                                    else if (codon[2] == "G" )                      //TAG
                                    {tempS = ">"; thisAlleleB += tempS;     
                                            codonCount = 0; isGene = false;
                                         allelesB += thisAlleleB;
                                         nAllelesB +=1;
                                         stopB +=  "-" + nAllelesB.ToString()+ "TAG" + "-";
                                         thisAlleleB = "";
                                        
                                        }  
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "T"){              //TT-
                                    if(codon[2] == "A" ) {tempS = ("L");}           //TTA
                                    else if (codon[2] == "T" ) {tempS = ("F");}     //TTT
                                    else if (codon[2] == "C" ) {tempS = ("F");}     //TTC
                                    else if (codon[2] == "G" ) {tempS = ("L");}     //TTG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "C"){               //TC-
                                    if(codon[2] == "A" ) {tempS = ("S");}           //TCA
                                    else if (codon[2] == "T" ) {tempS = ("S");}     //TCT
                                    else if (codon[2] == "C" ) {tempS = ("S");}     //TCC
                                    else if (codon[2] == "G" ) {tempS = ("S");}     //TCG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                                else if (codon[1] == "G"){               //TG-
                                    if(codon[2] == "A" )                             //TGA
                                    {tempS = ">"; thisAlleleB += tempS;     
                                            codonCount = 0; isGene = false;
                                         allelesB += thisAlleleB;
                                         nAllelesB +=1;
                                         stopB +=  "-" + nAllelesB.ToString()+ "TGA" + "-";
                                         thisAlleleB = "";
                                         
                                        }            
                                    else if (codon[2] == "T" ) {tempS = ("C");}     //TGT
                                    else if (codon[2] == "C" ) {tempS = ("C");}     //TGC
                                    else if (codon[2] == "G" ) {tempS = ("W");}     //TGG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                            }
                            else if (codon[0] == "C"){               //C-
                                if(codon[1] == "A"){                     //CA-
                                    if(codon[2] == "A" ) {tempS = ("Q");}           //CAA
                                    else if (codon[2] == "T" ) {tempS = ("H");}     //CAT
                                    else if (codon[2] == "C" ) {tempS = ("H");}     //CAC
                                    else if (codon[2] == "G" ) {tempS = ("Q");}     //CAG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "T"){              //CT-
                                    if(codon[2] == "A" ) {tempS = ("L");}           //CTA
                                    else if (codon[2] == "T" ) {tempS = ("L");}     //CTT
                                    else if (codon[2] == "C" ) {tempS = ("L");}     //CTC
                                    else if (codon[2] == "G" ) {tempS = ("L");}     //CTG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "C"){               //CC-
                                    if(codon[2] == "A" ) {tempS = ("P");}           //CCA
                                    else if (codon[2] == "T" ) {tempS = ("P");}     //CCT
                                    else if (codon[2] == "C" ) {tempS = ("P");}     //CCC
                                    else if (codon[2] == "G" ) {tempS = ("P");}     //CCG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                                else if (codon[1] == "G"){               //CG-
                                    if(codon[2] == "A" ) {tempS = ("R");}           //CGA
                                    else if (codon[2] == "T" ) {tempS = ("R");}     //CGT
                                    else if (codon[2] == "C" ) {tempS = ("R");}     //CGC
                                    else if (codon[2] == "G" ) {tempS = ("R");}     //CGG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                            }
                            else if (codon[0] == "G"){              //G-
                                if(codon[1] == "A"){                     //GA-
                                    if(codon[2] == "A" ) {tempS = ("E");}           //GAA
                                    else if (codon[2] == "T" ) {tempS = ("D");}     //GAT
                                    else if (codon[2] == "C" ) {tempS = ("D");}     //GAC
                                    else if (codon[2] == "G" ) {tempS = ("E");}     //GAG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "T"){              //GT-
                                    if(codon[2] == "A" ) {tempS = ("V");}           //GTA
                                    else if (codon[2] == "T" ) {tempS = ("V");}     //GTT
                                    else if (codon[2] == "C" ) {tempS = ("V");}     //GTC
                                    else if (codon[2] == "G" ) {tempS = ("V");}     //GTG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }

                                else if (codon[1] == "C"){               //GC-
                                    if(codon[2] == "A" ) {tempS = ("A");}           //GCA
                                    else if (codon[2] == "T" ) {tempS = ("A");}     //GCT
                                    else if (codon[2] == "C" ) {tempS = ("A");}     //GCC
                                    else if (codon[2] == "G" ) {tempS = ("A");}     //GCG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                                else if (codon[1] == "G"){               //GG-
                                    if(codon[2] == "A" ) {tempS = ("G");}           //GGA
                                    else if (codon[2] == "T" ) {tempS = ("G");}     //GGT
                                    else if (codon[2] == "C" ) {tempS = ("G");}     //GGC
                                    else if (codon[2] == "G" ) {tempS = ("G");}     //GGG
                                    if (isGene == true ){ thisAlleleB += tempS;}
                                }
                            }
                            




                            codonCount += 1;
                             baseCount = 0;
                            }

            codon[baseCount]=(B[x,y]);
            baseCount +=1;
        }
     }

    //Debug.Log( "numBasesB = " + numBasesB + " nAllelesB = " + nAllelesB + " StopsB = " + stopB);


    
    string thisA = allelesA;
    allelesA = null;
        
        //debugAminoA.Add(thisGene);
        
        int grnCountA = Regex.Matches(thisA, greenSeq).Count;

         
         int bluCountA = Regex.Matches(thisA, blueSeq).Count;

         
         int redCountA = Regex.Matches(thisA, redSeq).Count;

         
         int moveCountA = Regex.Matches(thisA, moveSeq).Count;

         
         int turnCountA = Regex.Matches(thisA, turnSeq).Count;

         
         int repCountA = Regex.Matches(thisA, repSeq).Count;


         int lifCountA = Regex.Matches(thisA, lifSeq).Count;
        
        nGRN_A = (float)grnCountA;
        nRED_A = (float)redCountA;
        nLLY_A = (float)bluCountA;
        nMVV_A = (float)moveCountA;
        nTRN_A = (float)turnCountA;
        nREP_A = (float)repCountA;
        nLIF_A = (float)lifCountA;

    
    
   string thisB = allelesB;
        allelesB = null;
        //debugBminoB.Bdd(thisGene);
        
        int grnCountB = Regex.Matches(thisB, greenSeq).Count;

         
         int bluCountB = Regex.Matches(thisB, blueSeq).Count;

         
         int redCountB = Regex.Matches(thisB, redSeq).Count;

         
         int moveCountB = Regex.Matches(thisB, moveSeq).Count;

         
         int turnCountB = Regex.Matches(thisB, turnSeq).Count;

         
         int repCountB = Regex.Matches(thisB, repSeq).Count;

         int lifCountB = Regex.Matches(thisB, lifSeq).Count;
        
        nGRN_B = (float)grnCountB;
        nRED_B = (float)redCountB;
        nLLY_B = (float)bluCountB;
        nMVV_B = (float)moveCountB;
        nTRN_B = (float)turnCountB;
        nREP_B = (float)repCountB;
        nLIF_B = (float)lifCountB;

    

    //Debug.Log("thisA : " + thisA);
    //Debug.Log("thisB : " + thisB);
    thisA = null;
    thisB = null;

    genotype_struct.redAllele1 = nRED_A/12f;
    genotype_struct.redAllele2 = nRED_B/12f;
    phenotype_struct.redGene = Mathf.Clamp((genotype_struct.redAllele1 + genotype_struct.redAllele2)/2.0f, 0.00f,1.00f);
    

    genotype_struct.greenAllele1 = nGRN_A/12f;
    genotype_struct.greenAllele2 = nGRN_B/12f;
    phenotype_struct.greenGene = Mathf.Clamp((genotype_struct.greenAllele1 + genotype_struct.greenAllele2)/2.0f, 0.00f,1.00f);

    genotype_struct.blueAllele1 = nLLY_A/12f;
    genotype_struct.blueAllele2 = nLLY_B/12f;
    phenotype_struct.blueGene = Mathf.Clamp((genotype_struct.blueAllele1 + blueAllele2)/2.0f, 0.00f,1.00f);

    genotype_struct.moveAllele1 = nMVV_A*3f;
    genotype_struct.moveAllele2 = nMVV_B*3f;
    phenotype_struct.moveForce = (moveAllele1+moveAllele2)/2f;

    genotype_struct.turnTorqueAllele1 = nTRN_A*6f;
    genotype_struct.turnTorqueAllele2 = nTRN_B*6f;
    phenotype_struct.turnTorque = (turnTorqueAllele1 + turnTorqueAllele2)/2.0f;

    genotype_struct.e2repAllele1 = 64f +  Mathf.Pow(2f,nREP_A);
    genotype_struct.e2repAllele2 = 64f + Mathf.Pow(2f,nREP_B);
    phenotype_struct.energyToReproduce = (e2repAllele1+e2repAllele2)/2.0f;

    genotype_struct.lifeLengthAllele1 = 16f + Mathf.Pow(2f,nLIF_A);
    genotype_struct.lifeLengthAllele2 = 16f +Mathf.Pow(2f,nLIF_B);
    phenotype_struct.lifeLength = (lifeLengthAllele1 + lifeLengthAllele2);
    
     

     phenotype_struct.redGene = redGene;
     phenotype_struct.greenGene = greenGene;
     phenotype_struct.blueGene = blueGene;
     phenotype_struct.moveForce = moveForce;
     phenotype_struct.turnTorque = turnTorque;
     phenotype_struct.energyToReproduce = energyToReproduce;
     phenotype_struct.lifeLength = lifeLength;
    
    Debug.Log("struct_aftertranslate" + " redGene" + phenotype_struct.redGene+ " greenGene " + phenotype_struct.greenGene+ " blueGene " + phenotype_struct.blueGene+ " moveForce " + phenotype_struct.moveForce+ " turnTorque " + phenotype_struct.turnTorque+ " energyToReproduce " + phenotype_struct.energyToReproduce+ " lifeLength"+ phenotype_struct.lifeLength);
    

            
    

            
            

            
            

            
            

            
            
            
    

    

        firstTranslation = true;
        translating = false;
}while(translating == true);

}






/* 
—————————————————————————Codon List————————————————————————–––––|
                                                                |
Isoleucine	    I(Ile)	    ATT, ATC, ATA                       |
Leucine 	    L(Leu)	    CTT, CTC, CTA, CTG, TTA, TTG        |
Valine	        V(Val)	    GTT, GTC, GTA, GTG                  |
Phenylalanine	F(Phe)	    TTT, TTC                            |
Methionine  	M(Met)	    ATG                                 |
Cysteine	    C(Cys)	    TGT, TGC                            |
Alanine	        A(Ala)	    GCT, GCC, GCA, GCG                  |
Glycine	        G(Gly)	    GGT, GGC, GGA, GGG                  |
Proline	        P(Pro)	    CCT, CCC, CCA, CCG                  |
Threonine	    T(Thr)	    ACT, ACC, ACA, ACG                  |
Serine	        S(Ser)	    TCT, TCC, TCA, TCG, AGT, AGC        |
Tyrosine	    Y(Tyr)	    TAT, TAC                            |
Tryptophan	    W(Trp)	    TGG                                 |
Glutamine	    Q(Gln)	    CAA, CAG                            |
Asparagine	    N(Asn)	    AAT, AAC                            |
Histidine	    H(His)	    CAT, CAC                            |
Glutamic acid	E(Glu)	    GAA, GAG                            |
Aspartic acid	D(Asp)	    GAT, GAC                            |
Lysine	        K(Lys)	    AAA, AAG                            |
Arginine	    R(Arg)	    CGT, CGC, CGA, CGG, AGA, AGG        |
  codons	     	TAA, TAG, TGA                           |
Start codon	    Start	ATG                                     |
—————————————————————––––––––––––––––––––––—————————————————————|
|—————————————————————————Codon Table————————————————————————–––|

    2nd      A              T               C              G
––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––
   1st                                                                    3rd
    A   |   AAA            ATA             ACA            AGA              A
    A   |   AAT            ATT             ACT            AGT              T
    A   |   AAC            ATC             ACC            AGC              C
    A   |   AAG            ATG             ACG            AGG              G

    T   |   TAA            TTA             TCA            TGA              A
    T   |   TAT            TTT             TCT            TGT              T
    T   |   TAC            TTC             TCC            TGC              C
    T   |   TAG            TTG             TCG            TGG              G

    C   |   CAA            CTA             CCA            GGA              A
    C   |   CAT            CTT             CCT            GGT              T
    C   |   CAC            CTC             CCC            GGC              C
    C   |   CAG            CTG             CCG            GGG              G

    G   |   GAA            GTA             GCA            GGA              A
    G   |   GAT            GTT             GCT            GGT              T
    G   |   GAC            GTC             GCC            GGC              C
    G   |   GAG            GTG             GCG            GGG              G

*/



}







