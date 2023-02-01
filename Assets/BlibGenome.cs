// This script contains genetic information and converts the sequences to numeric values for the organism's phenotype. It also handles gene transfer and mutations.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;




/*
Mutation rate of cyanobacteria (Krasovec 2017)
4.4E-10 to 9.8E-10, per nucleotide per generation.

I choose the mean of these values...
base_u_blib = 7.1E-10 (u)

blib nucleotides= 2*2*9*486 (one strand not simulated, but I'll use it in the calculation, cuz I feel like it) 
= 17 496       (b)

base_num_mutations per genome per generation = 7.1E-10 * 1.7496E4 
= 1.242216E-5 (uG)

= 1/24221.6 = 0.00001242216
≈ 1/24000

This is ridiculously low, so I think my computer would be very sad if i used that.

maybe 1/2400 to 1/240 is a better value...








*/
public class BlibGenome : MonoBehaviour {
  List <int[]> dynamicGeneLocations = new List<int[]>();
 public bool evolveMutationRate_enabled = false;
 public bool dynamicGeneDuplication_enabled = false;

 public bool allowSelfing = false;
  //string[] testcopy = new string[8]{"A","B","C","D","E","F","G","H"};
  public float refSNP_A;
  public float refSNP_B;
  public float heteroZygosity;
  public float totalAminoAcids;
  public float maxAminoAcids;
  public float aminoAcidRatio;
  public string aa_A;
  public string aa_antiA;
  public string aa_B;
  public string aa_antiB;

 public string[,] offspringGenomeA = new string[9,486];
 public string[,] offspringGenomeB = new string[9,486];
  public string[,] producedGamete = new string[9,486];
public string[,] receivedGamete = new string[9,486];

public List <string[,]> gametes = new List<string[,]>();

public int mutCount;
int base_mutDice = 24000;
public int mutMultiplier = 1;
byte [] byteA;
byte [] byteB;
  char[] ABroll = new char[8]{'A','B','A','B','A','B','A','B'};
  //int[] pois = new int[80] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
 static int[] P_transversion = new int[20]{-1,-1,-1,-1,-1,-1,1,-1,-1,-1,-1,-1,1,-1,-1,-1,-1,-1,-1,-1};

  public bool translocation_enabled;
  public bool duplication_enabled;


  public List<string> lineageID = new List<string>();

  public float greenAllele1, greenAllele2,
               redAllele1, redAllele2,
               blueAllele1, blueAllele2,
               turnTorqueAllele1, turnTorqueAllele2,
               e2repAllele1, e2repAllele2,
               moveAllele1, moveAllele2,
               lifeLengthAllele1, lifeLengthAllele2, trackerAllele1, trackerAllele2;

  private string redSeq, greenSeq, blueSeq, moveSeq, turnSeq, repSeq, lifSeq, TRACKER;
  //Get relevant amino acid sequences from database.
  

  public bool mutate;
  public int numMutations;

  public string testA;
   public string testB;

  BlibControls blibControls;
  public BlibGenome mother; //Could be used to debug if genome is being properly transferred to offspring.
  float nGRN_A, nGRN_B,
      nRED_A, nRED_B,
      nLLY_A, nLLY_B,
      nMVV_A, nMVV_B,
      nTRN_A, nTRN_B,
      nREP_A, nREP_B,
      nLIF_A, nLIF_B,
      nTRACKER_A, nTRACKER_B;

  //Current chromosomal loci
  int[,] sites = new int[9, 19] {
        {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486},

        {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486},

        {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486} , {0,27,54,81,108,135,162,189,216,243,270,297,324,351,378,405,432,459,486},


        };
  public string[,] extA, extB;
  //Chromosome A 
  public string[,] A = new string[9,486];
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
  public string[,] B = new string[9,486];
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

public string[,] antisenseA = new string[9,486];
  public string[,] antisenseB = new string[9,486];

string sensebaseA;
string sensebaseB;


public float recomCooldown = 0;
  void Start() {
    recomCooldown = 0;
    if(mutMultiplier < 0){
      mutMultiplier = 0;
    }
    dynamicGeneLocations.Clear();
    gametes.Clear();
    //string debugString = "";
    //for(int i = 0; i < testcopy.Length;i++){
    //  debugString += NucleotideCopy(testcopy[i]);

    //}
    //Debug.Log(debugString);
    A = new string[9,486];
    B = new string[9,486];
    producedGamete = new string[9,486];
    receivedGamete = new string[9,486];
    offspringGenomeA = new string[9,486];
    offspringGenomeB = new string[9,486];
    firstTranslation = false;
    maxAminoAcids = (2f*2f*9f*486f)/3f;
    mutCount = 0;
    chromoPairs = 9;
    basePairs = 486;
    blibControls = this.gameObject.GetComponent<BlibControls>();
    final_mutsize = base_mutDice / mutMultiplier;
    int mutationroll = UnityEngine.Random.Range(0, final_mutsize);
      if(mutationroll == 1){
        mutate = true;
      }

        redSeq = GeneDatabase.red;
        greenSeq = GeneDatabase.green;
        blueSeq = GeneDatabase.blue;
        moveSeq = GeneDatabase.move;
        turnSeq = GeneDatabase.turnt;
        repSeq = GeneDatabase.rep;
        lifSeq = GeneDatabase.lifeL;
        TRACKER = GeneDatabase.TRACKER;
   
      
    
    



  
    





    tempS = null;

    codon = new string[3] { "", "", "" };
    allelesA = "";
    allelesB = "";
    thisAlleleA = "";
    thisAlleleB = "";

    nAllelesA = 0;
    stopA = "";
    nAllelesB = 0;
    stopB = "";
    isGene = false;



    if (randGenome == true) {
      for (int i = 0; i < chromoPairs; i++) {
        for (int j = 0; j < basePairs; j++) {
          int randBase = UnityEngine.Random.Range(1, 5);
          string randChar;
          if (randBase == 1) { randChar = "A"; } else if (randBase == 2) { randChar = "C"; } else if (randBase == 3) { randChar = "G"; } else { randChar = "T"; }
          A[i, j] = randChar;
          B[i, j] = randChar;

        }

      }


    }
    

   //TranslateGenome();
  }


  public List<string> giveSequenceA = new List<string>();
  public List<string> giveSequenceB = new List<string>();
  //public List<string> receiveSequenceA = new List<string>();
  //public List<string> receiveSequenceB = new List<string>();
  private int AorB;
  BlibGenome mateGenome;
  void OnCollisionEnter2D(Collision2D col) {

   if(recomCooldown <= 0){
    if (col.gameObject.tag == "prey" &&  blibControls.age > 5f && col.gameObject.GetComponent<BlibControls>().age > 5f) {
      
       mateGenome = col.gameObject.GetComponent<BlibGenome>();
       if(evolveMutationRate_enabled == true){
        int sumMutMult = mutMultiplier+mateGenome.mutMultiplier;
      mutMultiplier = sumMutMult/2;
       }
      
      if(mutMultiplier < 0){
      mutMultiplier = 0;
    }

      int snpCountA = 0, snpCountB = 0, snpCountAB = 0, snpCountBA = 0;
      bool isEqual;

      
      /*
      string debugChar;
      if (AorB == 0){debugChar ="A";}else{debugChar = "B";}
      string debugLocus = System.String.Join("",giveLocus);
     // //Debug.Log("Chromosome: " + debugChar + ":" + cNum + " Locus: " + locus + " -> " + debugLocus);
     */

      



      //A-Compare
      for(int p = 0; p < (9*486); p++){
        if(this.testA[p] != mateGenome.testA[p]){
          snpCountA +=1;
        }

      //B-Compare
        if(this.testB[p] != mateGenome.testB[p]){
          snpCountB +=1;
        }
      }

            //AB-Compare
      for(int p = 0; p < (9*486); p++){
        if(this.testA[p] != mateGenome.testB[p]){
          snpCountAB +=1;
        }

      //BA-Compare
        if(this.testB[p] != mateGenome.testA[p]){
          snpCountBA +=1;
        }
      }
     int[] snps = new int[4]{snpCountA,snpCountB,snpCountAB,snpCountBA};
     Array.Sort(snps);
      /*
      //A-Compare
      for (int i = 0; i < chromoPairs; i++) {
        for (int j = 0; j < basePairs; j++) {
          isEqual = System.String.Equals(this.A[i, j], mateGenome.A[i, j]);
          if (isEqual == false) {
            snpCountA += 1;
          }
        }
      }

      //B-Compare
      for (int i = 0; i < chromoPairs; i++) {
        for (int j = 0; j < basePairs; j++) {
          isEqual = System.String.Equals(this.B[i, j], mateGenome.B[i, j]);
          if (isEqual == false) {
            snpCountB += 1;
          }
        }
      }
      */
      //Genetic distance calculations
      float pairWise1_squared = Mathf.Pow(((float)snps[0] / 486f*9f), 2.0f);
      float pairWise2_squared = Mathf.Pow(((float)snps[1] / 486f*9f), 2.0f);

      float pythagDist = Mathf.Sqrt(pairWise1_squared + pairWise2_squared);
     

      
      //exchange genetic material
      bool derboug = true;
      //if(derboug == true){
      if (pythagDist < 0.1f && pythagDist > 0.001f) {
        //CreateGamete();

        for(int i = 0; i < 9; i++){
          for(int j = 0; j < 486; j++){
            receivedGamete[i,j] = NucleotideCopy(mateGenome.producedGamete[i,j]);
          }
        }
        
        has2Gametes = true;
        numRecoms = UnityEngine.Random.Range(0,256);
        AorB = -1;
        AorB = UnityEngine.Random.Range(0,2);
         recoChromos = 9;
         numRecoLoci = 19;
        

        
         whichChromo = UnityEngine.Random.Range(0,recoChromos);

          recoStart_locus =  UnityEngine.Random.Range(0,numRecoLoci-1);
         recoStart_site = sites[whichChromo, recoStart_locus];
         
         
         recoEnd_locus =  UnityEngine.Random.Range(recoStart_locus, numRecoLoci);
         recoEnd_site = sites[whichChromo, recoEnd_locus];
        

         recoLength = sites[whichChromo,recoEnd_locus]-sites[whichChromo,recoStart_locus];
          
            

        for(int n = 0; n < numRecoms; n++){
             

            for(int i = recoStart_site; i < recoEnd_site; i++){
            giveSequenceA.Add(NucleotideCopy(A[whichChromo,i]));
            giveSequenceB.Add(NucleotideCopy(B[whichChromo,i]));
            }

            //Give what where?
            int whereTo = -1;
            whereTo = UnityEngine.Random.Range(0,2);
    
                  if(whereTo == 0){ //A->A, B->B
                    for (int i = recoStart_site; i < recoEnd_site; i++){
                    
                    mateGenome.A[whichChromo,i] = NucleotideCopy(giveSequenceA[i - recoStart_site]);
                    mateGenome.B[whichChromo,i] = NucleotideCopy(giveSequenceB[i - recoStart_site]);
                    }
                }

                  else if(whereTo == 1){ //A->B, B->A
                    for (int i = recoStart_site; i < recoEnd_site; i++){
                          
                    mateGenome.B[whichChromo,i] = NucleotideCopy(giveSequenceA[i - recoStart_site]);
                    mateGenome.A[whichChromo,i] = NucleotideCopy(giveSequenceB[i - recoStart_site]);
                    }
                }
            
            
            giveSequenceA.Clear();
            giveSequenceB.Clear();
        }
        
        
      }
      
      
        

    recomCooldown = 5f;
    }
   }
    
  }





public int final_mutsize;


  // Update is called once per frame
  void Update() {
    
    if(recomCooldown > 0){
      recomCooldown += -Time.deltaTime;
    }else if(recomCooldown < 0){
      recomCooldown = 0;
    }
    
    

   // (int)(mutMultiplier* 
    //(lifeLengthAllele1+lifeLengthAllele2/2)   )
      
    //);

    //if (mutationroll == 64) { mutate = true; }else{mutate = false;}
    extA = A;
    extB = B;
    /*
    if(firstTranslation == false){
      if(mutate == true && numMutations > 0){
        
        Mutate();
      }else{TranslateGenome();}
    }
    */
    if (mutate == true && firstTranslation == true) {
      
        
         Mutate();
    }else if (firstTranslation == false){
      TranslateGenome();
      
    }

    if(has2Gametes == true){
      CreateZygote();
    }
  }
  void Mutate() {
    mutate = false;
    if(mutMultiplier < 0){
      mutMultiplier = 0;
    }
      
    for (int q = 0; q < numMutations; q++) {
      if(evolveMutationRate_enabled == true){
        mutMultiplier += UnityEngine.Random.Range(-1,2)*UnityEngine.Random.Range(0,2);
      }
      

      mutCount+=1;
    //string randChar = "A";
    AorB = UnityEngine.Random.Range(0, 2);
    int duplicationRoll = UnityEngine.Random.Range(0, 2048);
    int transLocRoll = UnityEngine.Random.Range(0, 2048);
    

    string[] triNu = new string[3];
    string[] tranString_origin = new string[27];
    string[] tranString_destination = new string[27];
    int siteCroms = sites.GetLength(0);
    int siteLocs = sites.GetLength(1);

    int geneDuplicationRoll = UnityEngine.Random.Range(0,4096);
     //Proper-ish gene duplication
    if(dynamicGeneDuplication_enabled == true && geneDuplicationRoll == 1){
     
    int whichOrigin = UnityEngine.Random.Range(0,dynamicGeneLocations.Count);
    int whichDestination = UnityEngine.Random.Range(0,dynamicGeneLocations.Count);
    
    int originLength;
    
    int originSense; int originSet; int originChrom; int originSite_start; int originSite_end;
    int destinationSense; int destinationSet; int destinationChrom; int destinationSite_start; int destinationSite_end;

    originSense       = dynamicGeneLocations[whichOrigin][0];
    originSet         = dynamicGeneLocations[whichOrigin][1];
    originChrom       = dynamicGeneLocations[whichOrigin][2];
    originSite_start  = dynamicGeneLocations[whichOrigin][3];
    originSite_end    = dynamicGeneLocations[whichOrigin][4];

    originLength = originSite_end-originSite_start;

    destinationSense       = UnityEngine.Random.Range(0,2);
    destinationSet         = UnityEngine.Random.Range(0,2);
    destinationChrom       = UnityEngine.Random.Range(0,9);
    
    destinationSite_start  = UnityEngine.Random.Range(0,485-originLength);
    destinationSite_end    = destinationSite_start+originLength;

    
    string[] originSequence = new string[originLength];

    int s = 0;
    List<string[,]> tempArray = new List<string[,]>();
    switch(originSense){
      case 0:
        switch(originSet){
          case 0:
          tempArray.Add(A);
          break;

          case 1:
          tempArray.Add(B);
          break;
      }
      break;

      case 1:
        switch(originSet){
          case 0:
          tempArray.Add(antisenseA);
          break;

          case 1:
          tempArray.Add(antisenseB);
          break;
        }

      break;

    }

    switch(destinationSense){
      case 0:
        switch(destinationSet){
          case 0:
          tempArray.Add(A);
          break;
          case 1:
          tempArray.Add(B);
          break;
        }
      break;
      case 1:
        switch(destinationSet){
          case 0:
          tempArray.Add(antisenseA);
          break;
          case 1:
          tempArray.Add(antisenseB);
          break;
        }
      break;
    }



    for (int i = originSite_start; i < originSite_end; i++){
      originSequence[s] = NucleotideCopy(tempArray[0][originChrom,i]);
      s +=1;
    }
    s = 0;
  for(int i = destinationSite_start; i < destinationSite_end; i++){
      tempArray[1][destinationChrom,i] = NucleotideCopy(originSequence[s]);
      s += 1;
  }
  s = 0;
  tempArray.Clear();
    }
    
    
  

    

    int[] triIndex = new int[2] { UnityEngine.Random.Range(0, chromoPairs), UnityEngine.Random.Range(3, basePairs - 6) };

    if (transLocRoll == 1 && translocation_enabled == true) {
      string[] oNu = new string[3];
      //string origin = "", destination = "";
      int tAorB = UnityEngine.Random.Range(0, 2);

      int siteIndex_originI = UnityEngine.Random.Range(0, siteCroms);
      int siteIndex_originJ = sites[siteIndex_originI, UnityEngine.Random.Range(0, siteLocs - 1)];



      int siteIndex_destinationI = UnityEngine.Random.Range(0, siteCroms);
      int siteIndex_destinationJ = sites[siteIndex_destinationI, UnityEngine.Random.Range(0, siteLocs - 1)];

      /*
      public void CopyTo (int sourceIndex, char[] destination, int destinationIndex, int count);
      */

      
      if (AorB == 0) {
        //origin = "A";

        for (int i = 0; i < 27; i++) {
          tranString_origin[i] = NucleotideCopy(A[siteIndex_originI, siteIndex_originJ + i]) ;
          
        }

        if (tAorB == 0) {
          //destination = "A";


          for (int i = 0; i < 27; i++) {
            tranString_destination[i] = NucleotideCopy(A[siteIndex_destinationI, siteIndex_destinationJ + i]);
          }

          //Replace destination
          for (int i = 0; i < 27; i++) {
            A[siteIndex_destinationI, siteIndex_destinationJ + i] = NucleotideCopy(tranString_origin[i]);
          }
          //Replace origin
          for (int i = 0; i < 27; i++) {
            A[siteIndex_originI, siteIndex_originJ + i] = NucleotideCopy(tranString_destination[i]);
          }

        } else {
          //destination = "B";
          for (int i = 0; i < 27; i++) {
            tranString_destination[i] = NucleotideCopy(B[siteIndex_destinationI, siteIndex_destinationJ + i]);
          }

          //Replace destination
          for (int i = 0; i < 27; i++) {
            B[siteIndex_destinationI, siteIndex_destinationJ + i] = NucleotideCopy(tranString_origin[i]);
          }
          //Replace origin
          for (int i = 0; i < 27; i++) {
            A[siteIndex_originI, siteIndex_originJ + i] = NucleotideCopy(tranString_destination[i]);
          }

        }



      } else {
       // origin = "B";
        for (int i = 0; i < 27; i++) {
          tranString_origin[i] = NucleotideCopy(B[siteIndex_originI, siteIndex_originJ + i]);
        }

        if (tAorB == 0) {
         // destination = "A";
          for (int i = 0; i < 27; i++) {
            tranString_destination[i] = NucleotideCopy(A[siteIndex_destinationI, siteIndex_destinationJ + i]);
          }

          //Replace destination
          for (int i = 0; i < 27; i++) {
            A[siteIndex_destinationI, siteIndex_destinationJ + i] = NucleotideCopy(tranString_origin[i]);
          }

          //Replace origin
          for (int i = 0; i < 27; i++) {
            B[siteIndex_originI, siteIndex_originJ + i] = NucleotideCopy(tranString_destination[i]);
          }

        } else {
          //destination = "B";

          for (int i = 0; i < 27; i++) {
            tranString_destination[i] = NucleotideCopy(B[siteIndex_destinationI, siteIndex_destinationJ + i]);
          }
          //Replace destination
          for (int i = 0; i < 27; i++) {
            B[siteIndex_destinationI, siteIndex_destinationJ + i] = NucleotideCopy(tranString_origin[i]);
          }
          //Replace origin
          for (int i = 0; i < 27; i++) {
            B[siteIndex_originI, siteIndex_originJ + i] = NucleotideCopy(tranString_destination[i]);
          }

        }

      }


      //Debug.Log("Translocation :  " + " Origin: " + origin + "["+siteIndex_originI+","+siteIndex_originJ+"]" + "-[" + System.String.Join("", tranString_origin) +  "] " + " Destination: " + destination + "["+siteIndex_destinationI+","+siteIndex_destinationJ+"]" + "-[" + System.String.Join("", tranString_destination) +  "]");
    }

    if (duplicationRoll == 64 && duplication_enabled == true) {

      if (AorB == 0) {
        triNu[0] = NucleotideCopy(A[triIndex[0], triIndex[1]]);
        triNu[1] = NucleotideCopy(A[triIndex[0], triIndex[1] + 1]);
        triNu[2] = NucleotideCopy(A[triIndex[0], triIndex[1] + 2]);

        //string nextD = System.String.Join("", A[triIndex[0], triIndex[1] + 3] + A[triIndex[0], triIndex[1] + 4] + A[triIndex[0], triIndex[1] + 5]);
        //string thisD = System.String.Join("", triNu[0] + triNu[1] + triNu[2]);

        A[triIndex[0], triIndex[1] + 3] = NucleotideCopy(triNu[0]);
        A[triIndex[0], triIndex[1] + 4] = NucleotideCopy(triNu[1]);
        A[triIndex[0], triIndex[1] + 5] = NucleotideCopy(triNu[2]);
        //Debug.Log("Duplication! : " + "[" + triIndex[0] + "," + triIndex[1] + "] " + thisD + "-" + nextD);

      } else {
        triNu[0] = NucleotideCopy(B[triIndex[0], triIndex[1]]);
        triNu[1] = NucleotideCopy(B[triIndex[0], triIndex[1] + 1]);
        triNu[2] = NucleotideCopy(B[triIndex[0], triIndex[1] + 2]);
        //string nextD = System.String.Join("", B[triIndex[0], triIndex[1] + 3] + B[triIndex[0], triIndex[1] + 4] + B[triIndex[0], triIndex[1] + 5]);
        //string thisD = System.String.Join("", triNu[0] + triNu[1] + triNu[2]);

        B[triIndex[0], triIndex[1] + 3] = NucleotideCopy(triNu[0]);
        B[triIndex[0], triIndex[1] + 4] = NucleotideCopy(triNu[1]);
        B[triIndex[0], triIndex[1] + 5] = NucleotideCopy(triNu[2]);
        //Debug.Log("Duplication! : " + "[" + triIndex[0] + "," + triIndex[1] + "] " + thisD + "-" + nextD);

      }
    }

    //Point mutations
      
      
      
    

    string thisBase;
    string newBase;

    
      char AorB_new = ABroll[UnityEngine.Random.Range(0,ABroll.Length)];
      AorB = UnityEngine.Random.Range(0,2);
      int index0 = UnityEngine.Random.Range(0, 9);
      int index1 = UnityEngine.Random.Range(0, 486);
      int randBase = UnityEngine.Random.Range(0, 2);
      int pointTypeRoll = UnityEngine.Random.Range(0, 10);
      int transversionRoll = 0;
       transversionRoll = P_transversion[UnityEngine.Random.Range(0,P_transversion.Length)];
      //string pointType;
      //string pyrOrPur;


      //new point mutation code
     switch(AorB_new){
      case 'A':
      thisBase = A[index0,index1];
            switch(thisBase){
              case "A":
                  switch(transversionRoll){
                    case -1:
                    newBase = "G";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "T";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              case "G":
              switch(transversionRoll){
                    case -1:
                    newBase = "A";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "T";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              case "C":
              switch(transversionRoll){
                    case -1:
                    newBase = "T";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "G";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              case "T":
              switch(transversionRoll){
                    case -1:
                    newBase = "C";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "A";
                    A[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              }
              
      break;
      case 'B':
      thisBase = B[index0,index1];
            switch(thisBase){
              case "A":
                  switch(transversionRoll){
                    case -1:
                    newBase = "G";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "T";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              case "G":
              switch(transversionRoll){
                    case -1:
                    newBase = "A";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "T";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              case "C":
              switch(transversionRoll){
                    case -1:
                    newBase = "T";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "G";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              case "T":
              switch(transversionRoll){
                    case -1:
                    newBase = "C";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                    case 1:
                    newBase = "A";
                    B[index0,index1] = NucleotideCopy(newBase);
                    break;
                  }
              break;
              }
      break;
     }



      
      //Old point mutation code
      /*
      if (pointTypeRoll < 10) { pointType = "trv"; } else { pointType = "trt"; }




      if (AorB == 0) {
        if (A[index0, index1] == "A" || A[index0, index1] == "G") {
          pyrOrPur = "pur";
        } else { pyrOrPur = "pyr"; }

        if (pointType == "trt") {
          if (pyrOrPur == "pur") {
            if (randBase == 0) {
              randChar = "A";
            } else if (randBase == 1) {
              randChar = "G";
            }
          }
          if (pyrOrPur == "pyr") {
            if (randBase == 0) {
              randChar = "C";
            } else if (randBase == 1) {
              randChar = "T";
            }
          }
        }

        if (pointType == "trv") {
          if (pyrOrPur == "pur") {
            if (randBase == 0) {
              randChar = "C";
            } else if (randBase == 1) {
              randChar = "T";
            }
          }
          if (pyrOrPur == "pyr") {
            if (randBase == 0) {
              randChar = "A";
            } else if (randBase == 1) {
              randChar = "G";
            }
          }
        }

        A[index0, index1] = randChar;

      } else {
        if (B[index0, index1] == "A" || B[index0, index1] == "G") {
          pyrOrPur = "pur";
        } else { pyrOrPur = "pyr"; }


        if (pointType == "trt") {
          if (pyrOrPur == "pur") {
            if (randBase == 0) {
              randChar = "A";
            } else if (randBase == 1) {
              randChar = "G";
            }
          }
          if (pyrOrPur == "pyr") {
            if (randBase == 0) {
              randChar = "C";
            } else if (randBase == 1) {
              randChar = "T";
            }
          }
        }

        if (pointType == "trv") {
          if (pyrOrPur == "pur") {
            if (randBase == 0) {
              randChar = "C";
            } else if (randBase == 1) {
              randChar = "T";
            }
          }
          if (pyrOrPur == "pyr") {
            if (randBase == 0) {
              randChar = "A";
            } else if (randBase == 1) {
              randChar = "G";
            }
          }
        }

        B[index0, index1] = randChar;
      }
      */
    }

    mutate = false;
    numMutations = UnityEngine.Random.Range(1,2);
    final_mutsize = base_mutDice / mutMultiplier;
    
    TranslateGenome();
  }



  private string[] codon = new string[3];
  string[] anticodon = new string[3];
  string allelesA = "";
  string allelesB = "";
  string antiallelesA = "";
  string antiallelesB = "";
  string thisAlleleA = "";
  string thisAlleleB = "";
  string antithisAlleleA = "";
  string antithisAlleleB = "";

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

  int recoStart_locus;
  int recoStart_site;


  int recoEnd_locus;
  int recoEnd_site;


  int recoLength;
  List<string> recoBuffer_A = new List<string>();
  List<string> recoBuffer_B = new List<string>();
  List<string> donatorBuffer = new List<string>();
  
  //public List<string> problemSites = new List<string>();
  
  void TranslateGenome() {
    //mutate = false;
    if( final_mutsize < 0){
      final_mutsize = 0;
    }
    int mutationroll = UnityEngine.Random.Range(0, final_mutsize);
      if(mutationroll == 1){
        mutate = true;
      }
    //Debugging translation
/*    
    problemSites.Clear();
    
    
    for (int p = 0; p < A.GetLength(0); p++){
      for (int q = 0; q < A.GetLength(1); q++){
          if(A[p,q] != "A" && A[p,q] != "T" && A[p,q]  != "C" && A[p,q]  != "G"){
                problemSites.Add("A"+"("+p.ToString()+","+q.ToString()+")"+A[p,q]);
                
               }
        }
      }
      for (int p = 0; p < B.GetLength(0); p++){
      for (int q = 0; q < B.GetLength(1); q++){
          if(B[p,q] != "A" && B[p,q] != "T" && B[p,q]  != "C" && B[p,q]  != "G"){
                problemSites.Add("B"+"("+p.ToString()+","+q.ToString()+")"+B[p,q]);
               }
        }
      }
*/    
    
    
    
     if(firstTranslation == false ){ //Recombination
     firstTranslation = true;
     if(mother == null){
      if( blibControls.generation == 0){
        for (int i = 0; i < 9; i++){
          for (int j = 0; j < 486; j++){
            A[i,j] = NucleotideCopy(initGenomestatic.A_static[i,j]);
            B[i,j] = NucleotideCopy(initGenomestatic.B_static[i,j]);

            offspringGenomeA[i,j] = NucleotideCopy(initGenomestatic.A_static[i,j]);
            offspringGenomeB[i,j] = NucleotideCopy(initGenomestatic.B_static[i,j]);
            //producedGamete[i,j] = NucleotideCopy(initGenomestatic.A_static[i,j]);
            
          }
        }
        
      }else if(blibControls.generation > 0 && mother == null){
        A = null;
        B = null;
        testA = null;
        testB = null;
        Destroy(gameObject);
      }
      
      
    }else if(mother != null){
      for (int i = 0; i < 9; i++){
          for (int j = 0; j < 486; j++){
            A[i,j] = NucleotideCopy(mother.A[i,j]);
            B[i,j] = NucleotideCopy(mother.B[i,j]);
            //offspringGenomeA[i,j] = NucleotideCopy(mother.offspringGenomeA[i,j]);
            //offspringGenomeA[i,j] = NucleotideCopy(mother.offspringGenomeB[i,j]);
            //producedGamete[i,j] = NucleotideCopy(mother.offspringGenomeA[i,j]);
          }
        }
    }

    


        
         doConversion = false;
        
         numRecoms = UnityEngine.Random.Range(32, 64);

         recoChromos = 9;
         numRecoLoci = 19;
        

        
         whichChromo = UnityEngine.Random.Range(0,recoChromos);

          recoStart_locus =  UnityEngine.Random.Range(0,numRecoLoci-1);
         recoStart_site = sites[whichChromo, recoStart_locus];
         
         
         recoEnd_locus =  UnityEngine.Random.Range(recoStart_locus, numRecoLoci);
         recoEnd_site = sites[whichChromo, recoEnd_locus];
        

         recoLength = sites[whichChromo,recoEnd_locus]-sites[whichChromo,recoStart_locus];
          
          
        
          
          int conversionDice;
            int donator;

        for(int n = 0; n < numRecoms; n++){
             conversionDice = UnityEngine.Random.Range(0,2048);
             donator = -1;

            if(conversionDice == 1){
              donator = -1;

            donator = UnityEngine.Random.Range(0,2);
            doConversion = true;
            }else{doConversion = false;}

            for(int i = recoStart_site; i < recoEnd_site; i++){
            recoBuffer_A.Add(NucleotideCopy(A[whichChromo,i]));
            recoBuffer_B.Add(NucleotideCopy(B[whichChromo,i]));
            }

            for (int i = recoStart_site; i < recoEnd_site; i++){
                if(doConversion == false){
                    A[whichChromo,i] = NucleotideCopy(recoBuffer_B[i - recoStart_site]);
                    B[whichChromo,i] = NucleotideCopy(recoBuffer_A[i - recoStart_site]);
                    
                }else if(doConversion == true){
                    if(donator == 0){
                        B[whichChromo,i] = NucleotideCopy(recoBuffer_A[i - recoStart_site]);
                     }else if(donator == 1){
                        A[whichChromo,i] = NucleotideCopy(recoBuffer_B[i - recoStart_site]);
                    }
                }

            }
            recoBuffer_A.Clear();
            recoBuffer_B.Clear();
        }
            

        

    } 
    //CREATE ANTISENSE STRANDS
    for (int i = 0; i < A.GetLength(0); i++){
      for (int j = 0; j < A.GetLength(1); j++){
        sensebaseA = A[i,485-j];
        sensebaseB = B[i,485-j];
        switch (sensebaseA){
          case "A":
          antisenseA[i,j] = "T";
          break;
          case "T":
          antisenseA[i,j] = "A";
          break;
          case "C":
          antisenseA[i,j] = "G";
          break;
          case "G":
          antisenseA[i,j] = "C";
          break;
        }
        switch (sensebaseB){
          case "A":
          antisenseB[i,j] = "T";
          break;
          case "T":
          antisenseB[i,j] = "A";
          break;
          case "C":
          antisenseB[i,j] = "G";
          break;
          case "G":
          antisenseB[i,j] = "C";
          break;
        }
        
        
        
      }
    }
     
      //mutate = false;
      codon = new string[3] { "", "", "" };
      

      allelesA = "";
      allelesB = "";
      thisAlleleA = "";
      thisAlleleB = "";

      antiallelesA = "";
      antiallelesB = "";
     antithisAlleleA = "";
     antithisAlleleB = "";

      nAllelesA = 0;
      stopA = "";
      nAllelesB = 0;
      stopB = "";
      isGene = false;
      int numBasesA = 0;
      int numBasesB = 0;
      codonCount = 0;
      baseCount = 0;
      tempS = "";



      nGRN_A = 0; nGRN_B = 0; nRED_A = 0; nRED_B = 0;
      nLLY_A = 0; nLLY_B = 0; nMVV_A = 0; nMVV_B = 0;
      nTRN_A = 0; nTRN_B = 0; nREP_A = 0; nREP_B = 0;
      nTRACKER_A = 0; nTRACKER_B = 0;


      

      int[] tempDynamicGeneLocation = new int[5]; //[sense/antisense, ch. set, ch num, start, end]
      dynamicGeneLocations.Clear();
      for (int i = 0; i < A.GetLength(0); i++) {
        for (int j = 0; j < A.GetLength(1); j++) {
          if (i == 0 && j == 0) { allelesA = ""; allelesB = ""; thisAlleleA = ""; thisAlleleB = ""; }

          numBasesA += 1;


          if (baseCount == 3) {





            if (codon[0] == "A") {                     //A-

              if (codon[1] == "A") {                     //AA-
                if (codon[2] == "A") { tempS = "K"; }            //AAA
                else if (codon[2] == "T") { tempS = ("N"); }      //AAT
                else if (codon[2] == "C") { tempS = ("N"); }      //AAC
                else if (codon[2] == "G") { tempS = ("K"); }      //AAG

                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //AT-
                if (codon[2] == "A") { tempS = ("I"); }           //ATA
                else if (codon[2] == "T") { tempS = ("I"); }     //ATT
                else if (codon[2] == "C") { tempS = ("I"); }     //ATC
                else if (codon[2] == "G") {                    //ATG
                  if (thisAlleleA.Length > 0) { tempS = ("M"); }
                  if (thisAlleleA.Length == 0) { 
                    isGene = true; tempS = "*";
                  tempDynamicGeneLocation[0] = 0;  
                  tempDynamicGeneLocation[1] = 0; 
                  tempDynamicGeneLocation[2] = i;
                  tempDynamicGeneLocation[3] = j;
                  }

                }

                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //AC-
                if (codon[2] == "A") { tempS = ("T"); }           //ACA
                else if (codon[2] == "T") { tempS = ("T"); }     //ACT
                else if (codon[2] == "C") { tempS = ("T"); }     //ACC
                else if (codon[2] == "G") { tempS = ("T"); }     //ACG

                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //AG-
                if (codon[2] == "A") { tempS = ("R"); }           //AGA
                else if (codon[2] == "T") { tempS = ("S"); }     //AGT
                else if (codon[2] == "C") { tempS = ("S"); }     //AGC
                else if (codon[2] == "G") { tempS = ("R"); }     //AGG

                if (isGene == true) { thisAlleleA += tempS; }
              }
            } else if (codon[0] == "T") {              //T-
              if (codon[1] == "A") {                     //TA-
                if (codon[2] == "A") {
                  tempS = "*";
                  thisAlleleA += tempS;
                  codonCount = 0;                         //TAA
                  allelesA += thisAlleleA;
                  isGene = false;
                  nAllelesA += 1;
                  stopA += "-" + nAllelesA.ToString() + "TAA" + "-";
                  thisAlleleA = "";
                  tempS = "";
                  tempDynamicGeneLocation[4] = j;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("Y"); }     //TAT
                  else if (codon[2] == "C") { tempS = ("Y"); }     //TAC
                  else if (codon[2] == "G")                      //TAG
                  {
                  tempS = "*";
                  thisAlleleA += tempS;
                  codonCount = 0;
                  isGene = false;
                  allelesA += thisAlleleA;
                  nAllelesA += 1;
                  stopA += "-" + nAllelesA.ToString() + "TAG" + "-";
                  thisAlleleA = "";
                  tempS = "";
                  tempDynamicGeneLocation[4] = j;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);
                }
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //TT-
                if (codon[2] == "A") { tempS = ("L"); }           //TTA
                else if (codon[2] == "T") { tempS = ("F"); }     //TTT
                else if (codon[2] == "C") { tempS = ("F"); }     //TTC
                else if (codon[2] == "G") { tempS = ("L"); }     //TTG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //TC-
                if (codon[2] == "A") { tempS = ("S"); }           //TCA
                else if (codon[2] == "T") { tempS = ("S"); }     //TCT
                else if (codon[2] == "C") { tempS = ("S"); }     //TCC
                else if (codon[2] == "G") { tempS = ("S"); }     //TCG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //TG-
                if (codon[2] == "A")                             //TGA
                {
                  tempS = "*"; thisAlleleA += tempS;
                  codonCount = 0;
                  isGene = false;
                  allelesA += thisAlleleA;
                  nAllelesA += 1;
                  stopA += "-" + nAllelesA.ToString() + "TGA" + "-";
                  thisAlleleA = "";
                  tempS = "";
                  tempDynamicGeneLocation[4] = j;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("C"); }     //TGT
                  else if (codon[2] == "C") { tempS = ("C"); }     //TGC
                  else if (codon[2] == "G") { tempS = ("W"); }     //TGG
                if (isGene == true) { thisAlleleA += tempS; }
              }
            } else if (codon[0] == "C") {               //C-
              if (codon[1] == "A") {                     //CA-
                if (codon[2] == "A") { tempS = ("Q"); }           //CAA
                else if (codon[2] == "T") { tempS = ("H"); }     //CAT
                else if (codon[2] == "C") { tempS = ("H"); }     //CAC
                else if (codon[2] == "G") { tempS = ("Q"); }     //CAG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //CT-
                if (codon[2] == "A") { tempS = ("L"); }           //CTA
                else if (codon[2] == "T") { tempS = ("L"); }     //CTT
                else if (codon[2] == "C") { tempS = ("L"); }     //CTC
                else if (codon[2] == "G") { tempS = ("L"); }     //CTG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //CC-
                if (codon[2] == "A") { tempS = ("P"); }           //CCA
                else if (codon[2] == "T") { tempS = ("P"); }     //CCT
                else if (codon[2] == "C") { tempS = ("P"); }     //CCC
                else if (codon[2] == "G") { tempS = ("P"); }     //CCG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //CG-
                if (codon[2] == "A") { tempS = ("R"); }           //CGA
                else if (codon[2] == "T") { tempS = ("R"); }     //CGT
                else if (codon[2] == "C") { tempS = ("R"); }     //CGC
                else if (codon[2] == "G") { tempS = ("R"); }     //CGG
                if (isGene == true) { thisAlleleA += tempS; }
              }
            } else if (codon[0] == "G") {              //G-
              if (codon[1] == "A") {                     //GA-
                if (codon[2] == "A") { tempS = ("E"); }           //GAA
                else if (codon[2] == "T") { tempS = ("D"); }     //GAT
                else if (codon[2] == "C") { tempS = ("D"); }     //GAC
                else if (codon[2] == "G") { tempS = ("E"); }     //GAG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //GT-
                if (codon[2] == "A") { tempS = ("V"); }           //GTA
                else if (codon[2] == "T") { tempS = ("V"); }     //GTT
                else if (codon[2] == "C") { tempS = ("V"); }     //GTC
                else if (codon[2] == "G") { tempS = ("V"); }     //GTG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //GC-
                if (codon[2] == "A") { tempS = ("A"); }           //GCA
                else if (codon[2] == "T") { tempS = ("A"); }     //GCT
                else if (codon[2] == "C") { tempS = ("A"); }     //GCC
                else if (codon[2] == "G") { tempS = ("A"); }     //GCG
                if (isGene == true) { thisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //GG-
                if (codon[2] == "A") { tempS = ("G"); }           //GGA
                else if (codon[2] == "T") { tempS = ("G"); }     //GGT
                else if (codon[2] == "C") { tempS = ("G"); }     //GGC
                else if (codon[2] == "G") { tempS = ("G"); }     //GGG
                if (isGene == true) { thisAlleleA += tempS; }
              }
            }





            codonCount += 1;
            baseCount = 0;
          }

          codon[baseCount] = (A[i, j]);
          baseCount += 1;
        }
      }

    



      //Debug.Log("numBasesA = " + numBasesA + " nAllelesA = " + nAllelesA + " StopsA = " + stopA);


      codonCount = 0;
      baseCount = 0;
      int chromsize = B.GetLength(1);
      for (int x = 0; x < B.GetLength(0); x++) {
        for (int y = 0; y < B.GetLength(1); y++) {
          if (x == 0 && y == 0) { allelesB = ""; thisAlleleA = ""; thisAlleleB = ""; }
          numBasesB += 1;

          if (baseCount == 3) {




            if (codon[0] == "A") {                     //A-

              if (codon[1] == "A") {                     //AA-
                if (codon[2] == "A") { tempS = "K"; }            //AAA
                else if (codon[2] == "T") { tempS = ("N"); }      //AAT
                else if (codon[2] == "C") { tempS = ("N"); }      //AAC
                else if (codon[2] == "G") { tempS = ("K"); }      //AAG

                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //AT-
                if (codon[2] == "A") { tempS = ("I"); }           //ATA
                else if (codon[2] == "T") { tempS = ("I"); }     //ATT
                else if (codon[2] == "C") { tempS = ("I"); }     //ATC
                if (codon[2] == "G") {                        //ATG
                  if (thisAlleleB.Length > 0) { tempS = ("M"); }
                  if (thisAlleleB.Length == 0) { isGene = true; tempS = "*";
                  tempDynamicGeneLocation[0] = 0;  
                  tempDynamicGeneLocation[1] = 1; 
                  tempDynamicGeneLocation[2] = x;
                  tempDynamicGeneLocation[3] = y;
                  }
                }

                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //AC-
                if (codon[2] == "A") { tempS = ("T"); }           //ACA
                else if (codon[2] == "T") { tempS = ("T"); }     //ACT
                else if (codon[2] == "C") { tempS = ("T"); }     //ACC
                else if (codon[2] == "G") { tempS = ("T"); }     //ACG

                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //AG-
                if (codon[2] == "A") { tempS = ("R"); }           //AGA
                else if (codon[2] == "T") { tempS = ("S"); }     //AGT
                else if (codon[2] == "C") { tempS = ("S"); }     //AGC
                else if (codon[2] == "G") { tempS = ("R"); }     //AGG

                if (isGene == true) { thisAlleleB += tempS; }
              }
            } else if (codon[0] == "T") {              //T-
              if (codon[1] == "A") {                     //TA-
                if (codon[2] == "A") {
                  tempS = "*";
                  thisAlleleB += tempS;
                  codonCount = 0;                         //TAA
                  allelesB += thisAlleleB;
                  nAllelesB += 1;
                  stopB += "-" + nAllelesB.ToString() + "TAA" + "-";
                  thisAlleleB = "";
                  tempDynamicGeneLocation[4] = y;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("Y"); }     //TAT
                  else if (codon[2] == "C") { tempS = ("Y"); }     //TAC
                  else if (codon[2] == "G")                      //TAG
                  {
                  tempS = "*"; thisAlleleB += tempS;
                  codonCount = 0; isGene = false;
                  allelesB += thisAlleleB;
                  nAllelesB += 1;
                  stopB += "-" + nAllelesB.ToString() + "TAG" + "-";
                  thisAlleleB = "";

                }
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //TT-
                if (codon[2] == "A") { tempS = ("L"); }           //TTA
                else if (codon[2] == "T") { tempS = ("F"); }     //TTT
                else if (codon[2] == "C") { tempS = ("F"); }     //TTC
                else if (codon[2] == "G") { tempS = ("L"); }     //TTG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //TC-
                if (codon[2] == "A") { tempS = ("S"); }           //TCA
                else if (codon[2] == "T") { tempS = ("S"); }     //TCT
                else if (codon[2] == "C") { tempS = ("S"); }     //TCC
                else if (codon[2] == "G") { tempS = ("S"); }     //TCG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //TG-
                if (codon[2] == "A")                             //TGA
                {
                  tempS = "*"; thisAlleleB += tempS;
                  codonCount = 0; isGene = false;
                  allelesB += thisAlleleB;
                  nAllelesB += 1;
                  stopB += "-" + nAllelesB.ToString() + "TGA" + "-";
                  thisAlleleB = "";
                  tempDynamicGeneLocation[4] = y;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("C"); }     //TGT
                  else if (codon[2] == "C") { tempS = ("C"); }     //TGC
                  else if (codon[2] == "G") { tempS = ("W"); }     //TGG
                if (isGene == true) { thisAlleleB += tempS; }
              }
            } else if (codon[0] == "C") {               //C-
              if (codon[1] == "A") {                     //CA-
                if (codon[2] == "A") { tempS = ("Q"); }           //CAA
                else if (codon[2] == "T") { tempS = ("H"); }     //CAT
                else if (codon[2] == "C") { tempS = ("H"); }     //CAC
                else if (codon[2] == "G") { tempS = ("Q"); }     //CAG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //CT-
                if (codon[2] == "A") { tempS = ("L"); }           //CTA
                else if (codon[2] == "T") { tempS = ("L"); }     //CTT
                else if (codon[2] == "C") { tempS = ("L"); }     //CTC
                else if (codon[2] == "G") { tempS = ("L"); }     //CTG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //CC-
                if (codon[2] == "A") { tempS = ("P"); }           //CCA
                else if (codon[2] == "T") { tempS = ("P"); }     //CCT
                else if (codon[2] == "C") { tempS = ("P"); }     //CCC
                else if (codon[2] == "G") { tempS = ("P"); }     //CCG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //CG-
                if (codon[2] == "A") { tempS = ("R"); }           //CGA
                else if (codon[2] == "T") { tempS = ("R"); }     //CGT
                else if (codon[2] == "C") { tempS = ("R"); }     //CGC
                else if (codon[2] == "G") { tempS = ("R"); }     //CGG
                if (isGene == true) { thisAlleleB += tempS; }
              }
            } else if (codon[0] == "G") {              //G-
              if (codon[1] == "A") {                     //GA-
                if (codon[2] == "A") { tempS = ("E"); }           //GAA
                else if (codon[2] == "T") { tempS = ("D"); }     //GAT
                else if (codon[2] == "C") { tempS = ("D"); }     //GAC
                else if (codon[2] == "G") { tempS = ("E"); }     //GAG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //GT-
                if (codon[2] == "A") { tempS = ("V"); }           //GTA
                else if (codon[2] == "T") { tempS = ("V"); }     //GTT
                else if (codon[2] == "C") { tempS = ("V"); }     //GTC
                else if (codon[2] == "G") { tempS = ("V"); }     //GTG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //GC-
                if (codon[2] == "A") { tempS = ("A"); }           //GCA
                else if (codon[2] == "T") { tempS = ("A"); }     //GCT
                else if (codon[2] == "C") { tempS = ("A"); }     //GCC
                else if (codon[2] == "G") { tempS = ("A"); }     //GCG
                if (isGene == true) { thisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //GG-
                if (codon[2] == "A") { tempS = ("G"); }           //GGA
                else if (codon[2] == "T") { tempS = ("G"); }     //GGT
                else if (codon[2] == "C") { tempS = ("G"); }     //GGC
                else if (codon[2] == "G") { tempS = ("G"); }     //GGG
                if (isGene == true) { thisAlleleB += tempS; }
              }
            }





            codonCount += 1;
            baseCount = 0;
          }

          codon[baseCount] = (B[x, y]);
          baseCount += 1;
        }
      }


      //ANTISENSE READ


      nAllelesA = 0;
      stopA = "";
      nAllelesB = 0;
      stopB = "";
      isGene = false;
      numBasesA = 0;
      numBasesB = 0;
      codonCount = 0;
      baseCount = 0;
      tempS = "";

      for (int i = 0; i < antisenseA.GetLength(0); i++) {
        for (int j = 0; j < antisenseA.GetLength(1); j++) {
          if (i == 0 && j == 0) { antiallelesA = ""; antiallelesB = ""; antithisAlleleA = ""; antithisAlleleB = ""; }

          numBasesA += 1;


          if (baseCount == 3) {





            if (codon[0] == "A") {                     //A-

              if (codon[1] == "A") {                     //AA-
                if (codon[2] == "A") { tempS = "K"; }            //AAA
                else if (codon[2] == "T") { tempS = ("N"); }      //AAT
                else if (codon[2] == "C") { tempS = ("N"); }      //AAC
                else if (codon[2] == "G") { tempS = ("K"); }      //AAG

                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //AT-
                if (codon[2] == "A") { tempS = ("I"); }           //ATA
                else if (codon[2] == "T") { tempS = ("I"); }     //ATT
                else if (codon[2] == "C") { tempS = ("I"); }     //ATC
                else if (codon[2] == "G") {                    //ATG
                  if (antithisAlleleA.Length > 0) { tempS = ("M"); }
                  if (antithisAlleleA.Length == 0) { isGene = true; tempS = "*";
                    tempDynamicGeneLocation[0] = 1;  
                    tempDynamicGeneLocation[1] = 0; 
                    tempDynamicGeneLocation[2] = i;
                    tempDynamicGeneLocation[3] = j;
                   }

                }

                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //AC-
                if (codon[2] == "A") { tempS = ("T"); }           //ACA
                else if (codon[2] == "T") { tempS = ("T"); }     //ACT
                else if (codon[2] == "C") { tempS = ("T"); }     //ACC
                else if (codon[2] == "G") { tempS = ("T"); }     //ACG

                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //AG-
                if (codon[2] == "A") { tempS = ("R"); }           //AGA
                else if (codon[2] == "T") { tempS = ("S"); }     //AGT
                else if (codon[2] == "C") { tempS = ("S"); }     //AGC
                else if (codon[2] == "G") { tempS = ("R"); }     //AGG

                if (isGene == true) { antithisAlleleA += tempS; }
              }
            } else if (codon[0] == "T") {              //T-
              if (codon[1] == "A") {                     //TA-
                if (codon[2] == "A") {
                  tempS = "*";
                  antithisAlleleA += tempS;
                  codonCount = 0;                         //TAA
                  antiallelesA += antithisAlleleA;
                  isGene = false;
                  nAllelesA += 1;
                  stopA += "-" + nAllelesA.ToString() + "TAA" + "-";
                  antithisAlleleA = "";
                  tempS = "";
                  tempDynamicGeneLocation[4] = j;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("Y"); }     //TAT
                  else if (codon[2] == "C") { tempS = ("Y"); }     //TAC
                  else if (codon[2] == "G")                      //TAG
                  {
                  tempS = "*";
                  antithisAlleleA += tempS;
                  codonCount = 0;
                  isGene = false;
                  antiallelesA += antithisAlleleA;
                  nAllelesA += 1;
                  stopA += "-" + nAllelesA.ToString() + "TAG" + "-";
                  antithisAlleleA = "";
                  tempS = "";
                  tempDynamicGeneLocation[4] = j;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);
                }
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //TT-
                if (codon[2] == "A") { tempS = ("L"); }           //TTA
                else if (codon[2] == "T") { tempS = ("F"); }     //TTT
                else if (codon[2] == "C") { tempS = ("F"); }     //TTC
                else if (codon[2] == "G") { tempS = ("L"); }     //TTG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //TC-
                if (codon[2] == "A") { tempS = ("S"); }           //TCA
                else if (codon[2] == "T") { tempS = ("S"); }     //TCT
                else if (codon[2] == "C") { tempS = ("S"); }     //TCC
                else if (codon[2] == "G") { tempS = ("S"); }     //TCG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //TG-
                if (codon[2] == "A")                             //TGA
                {
                  tempS = "*"; antithisAlleleA += tempS;
                  codonCount = 0;
                  isGene = false;
                  antiallelesA += antithisAlleleA;
                  nAllelesA += 1;
                  stopA += "-" + nAllelesA.ToString() + "TGA" + "-";
                  antithisAlleleA = "";
                  tempS = "";
                  tempDynamicGeneLocation[4] = j;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("C"); }     //TGT
                  else if (codon[2] == "C") { tempS = ("C"); }     //TGC
                  else if (codon[2] == "G") { tempS = ("W"); }     //TGG
                if (isGene == true) { antithisAlleleA += tempS; }
              }
            } else if (codon[0] == "C") {               //C-
              if (codon[1] == "A") {                     //CA-
                if (codon[2] == "A") { tempS = ("Q"); }           //CAA
                else if (codon[2] == "T") { tempS = ("H"); }     //CAT
                else if (codon[2] == "C") { tempS = ("H"); }     //CAC
                else if (codon[2] == "G") { tempS = ("Q"); }     //CAG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //CT-
                if (codon[2] == "A") { tempS = ("L"); }           //CTA
                else if (codon[2] == "T") { tempS = ("L"); }     //CTT
                else if (codon[2] == "C") { tempS = ("L"); }     //CTC
                else if (codon[2] == "G") { tempS = ("L"); }     //CTG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //CC-
                if (codon[2] == "A") { tempS = ("P"); }           //CCA
                else if (codon[2] == "T") { tempS = ("P"); }     //CCT
                else if (codon[2] == "C") { tempS = ("P"); }     //CCC
                else if (codon[2] == "G") { tempS = ("P"); }     //CCG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //CG-
                if (codon[2] == "A") { tempS = ("R"); }           //CGA
                else if (codon[2] == "T") { tempS = ("R"); }     //CGT
                else if (codon[2] == "C") { tempS = ("R"); }     //CGC
                else if (codon[2] == "G") { tempS = ("R"); }     //CGG
                if (isGene == true) { antithisAlleleA += tempS; }
              }
            } else if (codon[0] == "G") {              //G-
              if (codon[1] == "A") {                     //GA-
                if (codon[2] == "A") { tempS = ("E"); }           //GAA
                else if (codon[2] == "T") { tempS = ("D"); }     //GAT
                else if (codon[2] == "C") { tempS = ("D"); }     //GAC
                else if (codon[2] == "G") { tempS = ("E"); }     //GAG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "T") {              //GT-
                if (codon[2] == "A") { tempS = ("V"); }           //GTA
                else if (codon[2] == "T") { tempS = ("V"); }     //GTT
                else if (codon[2] == "C") { tempS = ("V"); }     //GTC
                else if (codon[2] == "G") { tempS = ("V"); }     //GTG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "C") {               //GC-
                if (codon[2] == "A") { tempS = ("A"); }           //GCA
                else if (codon[2] == "T") { tempS = ("A"); }     //GCT
                else if (codon[2] == "C") { tempS = ("A"); }     //GCC
                else if (codon[2] == "G") { tempS = ("A"); }     //GCG
                if (isGene == true) { antithisAlleleA += tempS; }
              } else if (codon[1] == "G") {               //GG-
                if (codon[2] == "A") { tempS = ("G"); }           //GGA
                else if (codon[2] == "T") { tempS = ("G"); }     //GGT
                else if (codon[2] == "C") { tempS = ("G"); }     //GGC
                else if (codon[2] == "G") { tempS = ("G"); }     //GGG
                if (isGene == true) { antithisAlleleA += tempS; }
              }
            }





            codonCount += 1;
            baseCount = 0;
          }

          codon[baseCount] = (antisenseA[i, j]);
          baseCount += 1;
        }
      }

    tempS = "";



      //Debug.Log("numBasesA = " + numBasesA + " nAllelesA = " + nAllelesA + " StopsA = " + stopA);


      codonCount = 0;
      baseCount = 0;
      
      for (int x = 0; x < antisenseB.GetLength(0); x++) {
        for (int y = 0; y < antisenseB.GetLength(1); y++) {
          if (x == 0 && y == 0) { antiallelesB = ""; antithisAlleleA = ""; antithisAlleleB = ""; }
          numBasesB += 1;

          if (baseCount == 3) {




            if (codon[0] == "A") {                     //A-

              if (codon[1] == "A") {                     //AA-
                if (codon[2] == "A") { tempS = "K"; }            //AAA
                else if (codon[2] == "T") { tempS = ("N"); }      //AAT
                else if (codon[2] == "C") { tempS = ("N"); }      //AAC
                else if (codon[2] == "G") { tempS = ("K"); }      //AAG

                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //AT-
                if (codon[2] == "A") { tempS = ("I"); }           //ATA
                else if (codon[2] == "T") { tempS = ("I"); }     //ATT
                else if (codon[2] == "C") { tempS = ("I"); }     //ATC
                if (codon[2] == "G") {                        //ATG
                  if (antithisAlleleB.Length > 0) { tempS = ("M"); }
                  if (antithisAlleleB.Length == 0) { isGene = true; tempS = "*"; 
                    tempDynamicGeneLocation[0] = 1;  
                    tempDynamicGeneLocation[1] = 1; 
                    tempDynamicGeneLocation[2] = x;
                    tempDynamicGeneLocation[3] = y;
                  }
                }

                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //AC-
                if (codon[2] == "A") { tempS = ("T"); }           //ACA
                else if (codon[2] == "T") { tempS = ("T"); }     //ACT
                else if (codon[2] == "C") { tempS = ("T"); }     //ACC
                else if (codon[2] == "G") { tempS = ("T"); }     //ACG

                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //AG-
                if (codon[2] == "A") { tempS = ("R"); }           //AGA
                else if (codon[2] == "T") { tempS = ("S"); }     //AGT
                else if (codon[2] == "C") { tempS = ("S"); }     //AGC
                else if (codon[2] == "G") { tempS = ("R"); }     //AGG

                if (isGene == true) { antithisAlleleB += tempS; }
              }
            } else if (codon[0] == "T") {              //T-
              if (codon[1] == "A") {                     //TA-
                if (codon[2] == "A") {
                  tempS = "*";
                  antithisAlleleB += tempS;
                  codonCount = 0;                         //TAA
                  antiallelesB += antithisAlleleB;
                  nAllelesB += 1;
                  stopB += "-" + nAllelesB.ToString() + "TAA" + "-";
                  antithisAlleleB = "";
                  tempDynamicGeneLocation[4] = y;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("Y"); }     //TAT
                  else if (codon[2] == "C") { tempS = ("Y"); }     //TAC
                  else if (codon[2] == "G")                      //TAG
                  {
                  tempS = "*"; antithisAlleleB += tempS;
                  codonCount = 0; isGene = false;
                  antiallelesB += antithisAlleleB;
                  nAllelesB += 1;
                  stopB += "-" + nAllelesB.ToString() + "TAG" + "-";
                  antithisAlleleB = "";
                  tempDynamicGeneLocation[4] = y;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                }
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //TT-
                if (codon[2] == "A") { tempS = ("L"); }           //TTA
                else if (codon[2] == "T") { tempS = ("F"); }     //TTT
                else if (codon[2] == "C") { tempS = ("F"); }     //TTC
                else if (codon[2] == "G") { tempS = ("L"); }     //TTG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //TC-
                if (codon[2] == "A") { tempS = ("S"); }           //TCA
                else if (codon[2] == "T") { tempS = ("S"); }     //TCT
                else if (codon[2] == "C") { tempS = ("S"); }     //TCC
                else if (codon[2] == "G") { tempS = ("S"); }     //TCG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //TG-
                if (codon[2] == "A")                             //TGA
                {
                  tempS = "*"; antithisAlleleB += tempS;
                  codonCount = 0; isGene = false;
                  antiallelesB += antithisAlleleB;
                  nAllelesB += 1;
                  stopB += "-" + nAllelesB.ToString() + "TGA" + "-";
                  antithisAlleleB = "";
                  tempDynamicGeneLocation[4] = y;
                  dynamicGeneLocations.Add(tempDynamicGeneLocation);

                } else if (codon[2] == "T") { tempS = ("C"); }     //TGT
                  else if (codon[2] == "C") { tempS = ("C"); }     //TGC
                  else if (codon[2] == "G") { tempS = ("W"); }     //TGG
                if (isGene == true) { antithisAlleleB += tempS; }
              }
            } else if (codon[0] == "C") {               //C-
              if (codon[1] == "A") {                     //CA-
                if (codon[2] == "A") { tempS = ("Q"); }           //CAA
                else if (codon[2] == "T") { tempS = ("H"); }     //CAT
                else if (codon[2] == "C") { tempS = ("H"); }     //CAC
                else if (codon[2] == "G") { tempS = ("Q"); }     //CAG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //CT-
                if (codon[2] == "A") { tempS = ("L"); }           //CTA
                else if (codon[2] == "T") { tempS = ("L"); }     //CTT
                else if (codon[2] == "C") { tempS = ("L"); }     //CTC
                else if (codon[2] == "G") { tempS = ("L"); }     //CTG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //CC-
                if (codon[2] == "A") { tempS = ("P"); }           //CCA
                else if (codon[2] == "T") { tempS = ("P"); }     //CCT
                else if (codon[2] == "C") { tempS = ("P"); }     //CCC
                else if (codon[2] == "G") { tempS = ("P"); }     //CCG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //CG-
                if (codon[2] == "A") { tempS = ("R"); }           //CGA
                else if (codon[2] == "T") { tempS = ("R"); }     //CGT
                else if (codon[2] == "C") { tempS = ("R"); }     //CGC
                else if (codon[2] == "G") { tempS = ("R"); }     //CGG
                if (isGene == true) { antithisAlleleB += tempS; }
              }
            } else if (codon[0] == "G") {              //G-
              if (codon[1] == "A") {                     //GA-
                if (codon[2] == "A") { tempS = ("E"); }           //GAA
                else if (codon[2] == "T") { tempS = ("D"); }     //GAT
                else if (codon[2] == "C") { tempS = ("D"); }     //GAC
                else if (codon[2] == "G") { tempS = ("E"); }     //GAG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "T") {              //GT-
                if (codon[2] == "A") { tempS = ("V"); }           //GTA
                else if (codon[2] == "T") { tempS = ("V"); }     //GTT
                else if (codon[2] == "C") { tempS = ("V"); }     //GTC
                else if (codon[2] == "G") { tempS = ("V"); }     //GTG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "C") {               //GC-
                if (codon[2] == "A") { tempS = ("A"); }           //GCA
                else if (codon[2] == "T") { tempS = ("A"); }     //GCT
                else if (codon[2] == "C") { tempS = ("A"); }     //GCC
                else if (codon[2] == "G") { tempS = ("A"); }     //GCG
                if (isGene == true) { antithisAlleleB += tempS; }
              } else if (codon[1] == "G") {               //GG-
                if (codon[2] == "A") { tempS = ("G"); }           //GGA
                else if (codon[2] == "T") { tempS = ("G"); }     //GGT
                else if (codon[2] == "C") { tempS = ("G"); }     //GGC
                else if (codon[2] == "G") { tempS = ("G"); }     //GGG
                if (isGene == true) { antithisAlleleB += tempS; }
              }
            }





            codonCount += 1;
            baseCount = 0;
          }

          codon[baseCount] = (antisenseB[x, y]);
          baseCount += 1;
        }
      }
    
      //Debug.Log( "numBasesB = " + numBasesB + " nAllelesB = " + nAllelesB + " StopsB = " + stopB);


      tempS = "";

    aa_A = allelesA;
    aa_antiA = antiallelesA;
    aa_B = allelesB;
    aa_antiB = antiallelesB;


      string thisA = allelesA + antiallelesA;
      antiallelesA = null;
      allelesA = null;
      string thisB = allelesB + antiallelesB;
      antiallelesB = null;
      allelesB = null;
      totalAminoAcids = (float)(thisA.Length + thisB.Length);
      aminoAcidRatio = totalAminoAcids/maxAminoAcids;

      
      //debugAminoA.Add(thisGene);

      int grnCountA = Regex.Matches(thisA, greenSeq).Count;


      int bluCountA = Regex.Matches(thisA, blueSeq).Count;


      int redCountA = Regex.Matches(thisA, redSeq).Count;


      int moveCountA = Regex.Matches(thisA, moveSeq).Count;


      int turnCountA = Regex.Matches(thisA, turnSeq).Count;


      int repCountA = Regex.Matches(thisA, repSeq).Count;


      int lifCountA = Regex.Matches(thisA, lifSeq).Count;

      int trackerCountA = Regex.Matches(thisA, TRACKER).Count;

      nGRN_A = (float)grnCountA;
      nRED_A = (float)redCountA;
      nLLY_A = (float)bluCountA;
      nMVV_A = (float)moveCountA;
      nTRN_A = (float)turnCountA;
      nREP_A = (float)repCountA;
      nLIF_A = (float)lifCountA;
      nTRACKER_A = (float)trackerCountA;



      
      //debugBminoB.Bdd(thisGene);

      int grnCountB = Regex.Matches(thisB, greenSeq).Count;


      int bluCountB = Regex.Matches(thisB, blueSeq).Count;


      int redCountB = Regex.Matches(thisB, redSeq).Count;


      int moveCountB = Regex.Matches(thisB, moveSeq).Count;


      int turnCountB = Regex.Matches(thisB, turnSeq).Count;


      int repCountB = Regex.Matches(thisB, repSeq).Count;

      int lifCountB = Regex.Matches(thisB, lifSeq).Count;

      int trackerCountB = Regex.Matches(thisB, TRACKER).Count;
    
      nGRN_B = (float)grnCountB;
      nRED_B = (float)redCountB;
      nLLY_B = (float)bluCountB;
      nMVV_B = (float)moveCountB;
      nTRN_B = (float)turnCountB;
      nREP_B = (float)repCountB;
      nLIF_B = (float)lifCountB;
      nTRACKER_B = (float)trackerCountB;


      //Debug.Log("thisA : " + thisA);
      //Debug.Log("thisB : " + thisB);
      thisA = "";
      thisB = "";

      redAllele1 = nRED_A / 32f;
      redAllele2 = nRED_B / 32f;
      //redGene = Mathf.Clamp((redAllele1 + redAllele2) / 2.0f, 0.00f, 1.00f);


      greenAllele1 = nGRN_A / 32f;
      greenAllele2 = nGRN_B / 32f;
      //greenGene = Mathf.Clamp((greenAllele1 + greenAllele2) / 2.0f, 0.00f, 1.00f);

      blueAllele1 = nLLY_A / 32f;
      blueAllele2 = nLLY_B / 32f;
      //blueGene = Mathf.Clamp((blueAllele1 + blueAllele2) / 2.0f, 0.00f, 1.00f);

      moveAllele1 = nMVV_A * 3f;
      moveAllele2 = nMVV_B * 3f;
      //moveForce = (moveAllele1 + moveAllele2) / 2f;

      turnTorqueAllele1 = nTRN_A * 6f;
      turnTorqueAllele2 = nTRN_B * 6f;
      //turnTorque = (turnTorqueAllele1 + turnTorqueAllele2) / 2.0f;

      e2repAllele1 = 64f + Mathf.Pow(1.5f, nREP_A);
      e2repAllele2 = 64f + Mathf.Pow(1.5f, nREP_B);
      //energyToReproduce = (e2repAllele1 + e2repAllele2) / 2.0f;

      lifeLengthAllele1 = 16f + Mathf.Pow(2f, nLIF_A);
      lifeLengthAllele2 = 16f + Mathf.Pow(2f, nLIF_B);
      //lifeLength = (lifeLengthAllele1 + lifeLengthAllele2);
     
      trackerAllele1 = nTRACKER_A;
      trackerAllele2 = nTRACKER_B;

      
      final_mutsize = base_mutDice / mutMultiplier;






/*

string[] A0 = new string[486];
string[] A1 = new string[486];
string[] A2 = new string[486];
string[] A3 = new string[486];
string[] A4 = new string[486];
string[] A5 = new string[486];
string[] A6 = new string[486];
string[] A7 = new string[486];
string[] A8 = new string[486];

*/


float SNP_count = 0;

string[] tempchromo_A = new string[9];
string[] tempchromo_B = new string[9];
string[] tempBaseGather_A = new string[486];
string[] tempBaseGather_B = new string[486];

for(int i = 0; i < 9; i++){
  
  for(int j = 0; j < 486; j++){
    tempBaseGather_A[j] = NucleotideCopy(A[i,j]);
    tempBaseGather_B[j] = NucleotideCopy(B[i,j]);
    
    
  }
  tempchromo_A[i] = System.String.Join("",tempBaseGather_A);
  tempchromo_B[i] = System.String.Join("",tempBaseGather_B);
}

  
testA = System.String.Join("", tempchromo_A);
testB = System.String.Join("", tempchromo_B);

  float tempSNPA = 0;
  float tempSNPB = 0;
  for(int g = 0; g < 9*486;g++){
    if(!String.Equals(initGenomestatic.referenceA[g],testA[g])){
      tempSNPA += 1f;
    }
    if(!String.Equals(initGenomestatic.referenceB[g],testB[g])){
      tempSNPB += 1f;
    }
    if(!String.Equals(testA[g], testB[g])){
      SNP_count += 1.0f;
    }
  }
  heteroZygosity = SNP_count/(9f*486f);
    refSNP_A = tempSNPA;
    refSNP_B = tempSNPB;
      firstTranslation = true;

      int[] fromWhich = new int[9];
    for(int i = 0; i < 9; i++){
      fromWhich[i] = UnityEngine.Random.Range(0,2);
    }
    for(int i = 0; i < 9; i++){
      if(fromWhich[i] == 0){
        for(int j = 0; j < 486; j++){
          producedGamete[i,j] = NucleotideCopy(A[i,j]);
        }
      }else if(fromWhich[i] == 1){
        for(int j = 0; j < 486; j++){
          producedGamete[i,j] = NucleotideCopy(B[i,j]);
        }
      }
    }
      InitializePheno();
      
      

  }

  void OnDestroy(){
    this.blibControls = null;
    mother = null;
    A = null;
    B = null;
    extA = null;
    extB = null;
    antisenseA = null;
    antisenseB = null;
    aa_antiA = null;
    aa_antiB = null;
  }


  void InitializePheno(){

      blibControls.lifeLength = ((lifeLengthAllele1 + lifeLengthAllele2)/2.0f);
    
      blibControls.energyToReproduce =128f+ (1f+aminoAcidRatio)*(e2repAllele1 + e2repAllele2) / 2.0f;

      blibControls.moveForce = (moveAllele1 + moveAllele2) / 2f;

      blibControls.turnTorque = (turnTorqueAllele1 + turnTorqueAllele2) / 2.0f;

      blibControls.redGene = Mathf.Clamp((redAllele1 + redAllele2) / 2.0f, 0.00f, 1.00f);

      blibControls.greenGene = Mathf.Clamp((greenAllele1 + greenAllele2) / 2.0f, 0.00f, 1.00f);

      blibControls.blueGene = Mathf.Clamp((blueAllele1 + blueAllele2) / 2.0f, 0.00f, 1.00f);
      this.gameObject.GetComponent<SpriteRenderer>().color = new Color(Mathf.Clamp((redAllele1 + redAllele2) / 2.0f, 0.00f, 1.00f), Mathf.Clamp((greenAllele1 + greenAllele2) / 2.0f, 0.00f, 1.00f), Mathf.Clamp((blueAllele1 + blueAllele2) / 2.0f, 0.00f, 1.00f), 1f);
      //CreateGamete();
      
  }

  string NucleotideCopy(string input){
    
    char [] tempChar = new char[input.Length];
    input.CopyTo(0,tempChar,0,input.Length);
    string output = new string(tempChar);
    
    return output;
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
  void CreateGamete(){
    
    /*
      int originLength;
    
    int originSense = 0; int originSet; int originChrom; int originSite_start; int originSite_end;
    int destinationChrom; int destinationSite_start = 0; int destinationSite_end = 0;
    
    
   int numGenes = dynamicGeneLocations.Count;
   Debug.Log(numGenes);
     //Gene recombination for offspring
    List<string[,]> originList = new List<string[,]>();
    
     for(int q = 0; q < dynamicGeneLocations.Count/4; q++){
      
     
    
    //int whichDestination = UnityEngine.Random.Range(0,dynamicGeneLocations.Count);
    
    

    
    originSet         = dynamicGeneLocations[UnityEngine.Random.Range(0,2)][1];
    originChrom       = dynamicGeneLocations[q][2];
    originSite_start  = dynamicGeneLocations[q][3];
    originSite_end    = dynamicGeneLocations[q][4];

    originLength = originSite_end-originSite_start;
    
    
    
    
    destinationChrom       = originChrom;
    
    destinationSite_start  = originSite_start;
    destinationSite_end    = originSite_start + originLength;

    
    string[] originSequence = new string[originLength];

    int s = 0;

    if(originSet == 0){
      originList.Add(A);
    }else if(originSet == 1){
    originList.Add(B);
    }
    

    
        
      
      
    for (int i = originSite_start; i < originSite_end; i++){
      originSequence[s] = NucleotideCopy(originList[0][originChrom,i]);
      s +=1;
    }
    s = 0;
    for(int i = destinationSite_start; i < destinationSite_end; i++){
      producedGamete[destinationChrom,i] = NucleotideCopy(originSequence[s]);
      
      s += 1;
    }
    
    
    s = 0;
    originList.Clear();
    
    }
    
    */

    CreateZygote();
  }
public bool has2Gametes;
  void CreateZygote(){
    
    int whichSet = UnityEngine.Random.Range(0,2);

 
     
    
    
  
      if( allowSelfing == true && has2Gametes == false){
      for(int i = 0; i < 9; i++){
        for(int j = 0; j < 486; j++){
          receivedGamete[i,j] = NucleotideCopy(producedGamete[i,j]);
        }
      }
      has2Gametes = true;
    }

      

      if(has2Gametes == true){
        if(whichSet == 0){
      for(int i = 0; i < 9; i++){
        for(int j = 0; j < 486; j++){
          offspringGenomeA[i,j] = NucleotideCopy(producedGamete[i,j]);
          offspringGenomeB[i,j] = NucleotideCopy(receivedGamete[i,j]);
        }
      }
    }
    if(whichSet == 1){
      for(int i = 0; i < 9; i++){
        for(int j = 0; j < 486; j++){
          offspringGenomeB[i,j] = NucleotideCopy(producedGamete[i,j]);
          offspringGenomeA[i,j] = NucleotideCopy(receivedGamete[i,j]);
        }
      }
    }
      }
      
    
    

  }
  
}







