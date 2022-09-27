using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DNAbin{
    public static byte   Adenosine = 0x88;
    public static byte   Guanine = 0x48;
    public static byte   Cytosine = 0x28;
    public static byte   Thymine = 0x18;
    public static byte unknown = 0x2;
   

    public static byte[] GetDNAbin(string input){
      byte[] output = new byte[input.Length];
      for(int i = 0; i < input.Length; i++){
            switch(input[i]){

            case 'A':
            output[i] = Adenosine;
            break;

            case 'G':
            output[i] = Guanine;
            break;

            case 'C':
            output[i] = Cytosine;
            break;

            case 'T':
            output[i] = Thymine;
            break;

            default:
            output[i] = unknown;
            break;
      }

      }
      
      
      return(output);
    }
}


public class GeneDatabase 
{

// Database for amino acid sequences of phenotypic relevance.

/* 
—————————————————————————Codon List————————————————————————–––––|
                                                                |
Isoleucine	    I (Ile)	    ATT, ATC, ATA                       |
Leucine 	    L (Leu)	    CTT, CTC, CTA, CTG, TTA, TTG        |
Valine	        V (Val)	    GTT, GTC, GTA, GTG                  |
Phenylalanine	F (Phe)	    TTT, TTC                            |
Methionine  	M (Met)	    ATG                                 |
Cysteine	    C (Cys)	    TGT, TGC                            |
Alanine	        A (Ala)	    GCT, GCC, GCA, GCG                  |
Glycine	        G (Gly)	    GGT, GGC, GGA, GGG                  |
Proline	        P (Pro)	    CCT, CCC, CCA, CCG                  |
Threonine	    T (Thr)	    ACT, ACC, ACA, ACG                  |
Serine	        S (Ser)	    TCT, TCC, TCA, TCG, AGT, AGC        |
Tyrosine	    Y (Tyr)	    TAT, TAC                            |
Tryptophan	    W (Trp)	    TGG                                 |
Glutamine	    Q (Gln)	    CAA, CAG                            |
Asparagine	    N (Asn)	    AAT, AAC                            |
Histidine	    H (His)	    CAT, CAC                            |
Glutamic acid	E (Glu)	    GAA, GAG                            |
Aspartic acid	D (Asp)	    GAT, GAC                            |
Lysine	        K (Lys)	    AAA, AAG                            |
Arginine	    R (Arg)	    CGT, CGC, CGA, CGG, AGA, AGG        |
stop  codons	     	    TAA, TAG, TGA                       |
Start codon	    Start	    ATG                                 |
—————————————————————––––––––––––––––––––––—————————————————————|


TEMPLATE:
0                  1                 2                 3                 4                 5
"X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , "X" , 

XXX = ––– - ––– - –––
      ––– - ––– - –––
      ––– - ––– - –––
      ––– - ––– - –––
      ––– - ––– - –––
      ––– - ––– - –––


RED = CGT - GAA - GAT
      CGC - GAG - GAC
      CGA  
      CGG
      AGA
      AGG

      "C", "G", "C", "G", "A", "A", "G", "A", "T"

GRN = GGT - CGT - AAT
      GGC - CGC - AAC
      GGA - CGA   
      GGG - CGG
            AGA
            AGG

     "G", "G", "T", "C", "G", "T", "A", "A", "T"

LLY = CTT - CTT - TAT
      CTC - CTC - TAC
      CTA - CTA - –––
      CTG - CTG - –––
      TTA - TTA - –––
      TTG - TTG - –––

      "C", "T", "T", "C", "T", "T", "T", "A", "T"


MVV = ATG - GTT - GTT
      ––– - GTC - GTC
      ––– - GTA - GTA
      ––– - GTG - GTG
      ––– - ––– - –––
      ––– - ––– - –––
      "A", "T", "G", "G", "T", "T", "G", "T", "T"

TRN = ACT - CGT - AAT
      ACC - CGC - AAC
      ACA - CGA   
      ACG - CGG
            AGA
            AGG
    "A", "C", "T", "C", "G", "T", "A", "A", "T"
REP = CGT - GAA - CCT
      CGC - GAG - CCC
      CGA - ––– - CCA
      CGG - ––– - CCG
      AGA
      AGG

 "A", "T", "G", "C", "G", "T", "G", "A", "A", "C", "C", "T", "C", "G", "T", "G", "A", "A", "C", "C", "T", "T", "C", "T", "T", "G", "A"
LIF = CTT - ATT - TTT
      CTC - ATC - TTC
      CTA - ATA - –––
      CTG - ––– - –––
      TTA - ––– - –––
      TTG - ––– - –––

"A", "T", "G", "C", "T", "T", "A", "T", "T", "T", "T", "T", "C", "T", "T", "A", "T", "T", "T", "T", "T", "A", "A", "A", "T", "G", "A"

           L     K     D     I     S     T     A
LKDISTA = CTT - AAA - GAT - ATT - TCT - ACT - CGT
          CTC - AAG - GAC - ATC - TCC - ACC - GCC
          CTA - ––– - ––– - ATA - TCA - ACA - GCA
          CTG - ––– - ––– - ––– - TCG - ACG - GCG
          TTA - ––– - ––– - ––– - AGT - ––– - –––
          TTG - ––– - ––– - ––– - AGC - ––– - –––


    "A", "T", "G", "C", "T", "G", "A", "A", "A", "G", "A", "T", "A", "T", "T", "A", "G", "C", "A", "C", "C", "G", "C", "G", "T", "G", "A"
    atg ctg aaa gat att agc acc gcg tga
         T     H     I     C     C
THICC = ACT - CAT - ATT - TGT - TGT
        ACC - CAC - ATC - TGC - TGC
        ACA - ––– - ATA - ––– - –––
        ACG - ––– - ––– - ––– - –––
        ––– - ––– - ––– - ––– - –––
        ––– - ––– - ––– - ––– - –––
    "A", "T", "G", "A", "C", "T", "C", "A", "T", "A", "T", "T", "T", "G", "T", "T", "G", "T", "T", "G", "A", "A", "A", "A", "A", "A", "A"





TRACKER = ––– - ––– - ––– - ––– - ––– - ––– - –––
          ––– - ––– - ––– - ––– - ––– - ––– - –––
          ––– - ––– - ––– - ––– - ––– - ––– - –––
          ––– - ––– - ––– - ––– - ––– - ––– - –––
          ––– - ––– - ––– - ––– - ––– - ––– - –––
          ––– - ––– - ––– - ––– - ––– - ––– - –––
0                  1                 2                 3                 4                 5                 6
"A", "C", "C", "C", "G", "C", "G", "C", "G", "T", "G", "C", "A", "A", "A", "G", "A", "A", "C", "G", "C"


*/



public static string red = "RED", green = "GRN",  blue = "LLY", move = "MVV", turnt = "TRN", rep = "REP", lifeL = "LIF", lookD = "LKDISTA", thicc = "THICC", TRACKER = "TRACKER";


 

        


public int [,] loci = new int[8,10] {
        {0,27,54,81,108,135,162,189,216,243} , {0,27,54,81,108,135,162,189,216,243},

        {0,27,54,81,108,135,162,189,216,243} , {0,27,54,81,108,135,162,189,216,243},

        {0,27,54,81,108,135,162,189,216,243} , {0,27,54,81,108,135,162,189,216,243},

        {0,27,54,81,108,135,162,189,216,243} , {0,27,54,81,108,135,162,189,216,243},
        };






public int GetSites ( int chromosome, int locus){

    return loci[chromosome,locus];

}









}
