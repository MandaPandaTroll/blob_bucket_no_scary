//Behaviour and genetics
//tabacwoman march 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;


public class BlibControls : MonoBehaviour {

  RaycastHit2D rayResults;
     int proteinCost;

  CurriculumHandler curriculumHandler;
  public string[,] superSatellite = new string[4, 27] {
    //Chromosome 0
    {"A","T","G",  "A","A","A",  "A","A","A",
     "T","C","T",  "T","C","T",  "A","A","A",
     "A","T","G",  "A","T","G",  "T","G","A"},
    
    //Chromosome 1
    {"A","T","G",  "T","C","T",  "A","A","A",
     "T","C","T",  "A","A","A",  "T","C","T",
     "G","C","T",  "T","C","T",  "T","G","A"},
    //Chromosome 2
    {"A","T","G",  "C","A","T",  "G","A","A",
     "T","C","T",  "A","C","T",  "T","C","T",
     "A","A","A",  "T","C","T",  "T","G","A"},
    
    //Chromosome 3
    {"A","T","G",  "A","A","A",  "A","A","A",
     "A","T","T",  "A","A","A",  "A","A","A",
     "G","A","A",  "C","G","T",  "T","G","A"},
};

   BlibGenome genome_script;

  //Detector detector;
  //GameObject Alpha;

      public static float init_redGene = 0;
      public static float init_greenGene = 1f;
      public static float init_blueGene = 0;
      public static float init_moveForce = 60f;
      public static float init_turnTorque = 108f;
      public static float init_energyToReproduce = 1088f;
      public static float init_lifeLength = 160f;


  public float speciationDistance;
  public float energyToReproduce, e2repAllele1, e2repAllele2;
  private float colorR;
  private float colorG;
  private float colorB;
  private float colorA = 1f;

  private Color geneticColor;
  public float densityCo;
  Rigidbody2D rb;
  GameObject box;
  private bool bump;
  static GameObject[] blibs;
  LayerMask layer_mask;

  public int rDice;
  public float age;
  public float energy;
  public float statAge;
  System.Random rndA = new System.Random();





  // Selection parameters
  public float moveAllele1;
  public float moveAllele2;

  public float moveForce;
  public float turnTorque;
  public float turnTorqueAllele1;
  public float turnTorqueAllele2;

  public float[,] introns = new float[2, 2];


  public float redGene, greenGene, blueGene;
  public float redAllele1, redAllele2, greenAllele1, greenAllele2, blueAllele1, blueAllele2;
  public float initDiversity;
  public float lifeLength;
  int deathDice;
  public float turnDice;

  public float lookDistance;


  Transform boxTran;
  float boxLength;
  public int generation;

  private SpriteRenderer m_SpriteRenderer;


  float energyTick;
  float lNum;
  float lForce;

  public int nutLevel;
  int maxNut;
  float maxEnergy;
  public int nutToReproduce;

  float cellSize;

  nutGrid m_nutgrid;

  PopLogger popLogger;
  BlibSpawner blibSpawner;
  int posVal;

  //Amino acid count per chromosome. 

  int[] lys, asn, ile, met, thr, arg, ser, tyr, phe, leu, cys, trp, gln, his, pro, val, ala, asp, glu, gly; int nothing;
  string[,] A, B;
  int chromoPairs = 9; int basePairs = 486;

  List<string> codon;

  float speedModifier;

  
  void Awake() {
    
      genome_script = gameObject.GetComponent<BlibGenome>();
     
    
    
  }
  void Start() {
      genome_script = gameObject.GetComponent<BlibGenome>();
    

    chromoPairs = 9;
    basePairs = 486;
    codon = new List<string>();
    lys = new int[5]; asn = new int[5]; ile = new int[5]; met = new int[5]; thr = new int[5]; arg = new int[5]; ser = new int[5]; tyr = new int[5]; phe = new int[5]; leu = new int[5]; cys = new int[5]; trp = new int[5]; gln = new int[5]; his = new int[5]; pro = new int[5]; val = new int[5]; ala = new int[5]; asp = new int[5]; glu = new int[5]; gly = new int[5];

    popLogger = GameObject.Find("StatisticsHandler").GetComponent<PopLogger>();
    blibSpawner = GameObject.Find("Spawner").GetComponent<BlibSpawner>();

    m_nutgrid = GameObject.Find("Testing").GetComponent<Testing>().nutgrid;

    int codonCount = 0;
    rb = gameObject.GetComponent<Rigidbody2D>();



    posVal = 0;
    //Alpha = GameObject.Find("Alpha");
    // detector = Alpha.GetComponent<Detector>();



    m_SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();





    layer_mask = LayerMask.GetMask("Prey", "Predator", "Predator2", "ApexPred", "Wall");

    box = GameObject.Find("box");
    boxTran = box.GetComponent<Transform>();
    boxLength = boxTran.localScale.x;
    cellSize = boxLength / 100f;








  //GetPheno();
    


    //geneticColor = new Color(redGene, greenGene, blueGene, 1f);
    //geneticColor.r = redGene;
    //geneticColor.g = greenGene;
    //geneticColor.b = blueGene;

    m_SpriteRenderer.color = new Color(redGene, greenGene, blueGene, 1f);


    for (int i = 0; i < superSatellite.GetLength(0); i++) {
      for (int j = 0; j < superSatellite.GetLength(1); j++) {
        if (codonCount == 3) {


          if (codon[0] == "A") {                     //A-
            if (codon[1] == "A") {                     //AA-
              if (codon[2] == "A") { lys[i] += 1; }           //AAA
              else if (codon[2] == "T") { asn[i] += 1; }     //AAT
              else if (codon[2] == "C") { asn[i] += 1; }     //AAC
              else if (codon[2] == "G") { lys[i] += 1; }     //AAG

            } else if (codon[1] == "T") {              //AT-
              if (codon[2] == "A") { ile[i] += 1; }           //ATA
              else if (codon[2] == "T") { ile[i] += 1; }     //ATT
              else if (codon[2] == "C") { ile[i] += 1; }     //ATC
              else if (codon[2] == "G") { met[i] += 1; }     //ATG
            } else if (codon[1] == "C") {               //AC-
              if (codon[2] == "A") { thr[i] += 1; }              //ACA
              else if (codon[2] == "T") { thr[i] += 1; }        //ACT
              else if (codon[2] == "C") { thr[i] += 1; }        //ACC
              else if (codon[2] == "G") { thr[i] += 1; }        //ACG
            } else if (codon[1] == "G") {               //AG-
              if (codon[2] == "A") { arg[i] += 1; }              //AGA
              else if (codon[2] == "T") { ser[i] += 1; }        //AGT
              else if (codon[2] == "C") { ser[i] += 1; }        //AGC
              else if (codon[2] == "G") { arg[i] += 1; }        //AGG
            }
          } else if (codon[0] == "T") {              //T-
            if (codon[1] == "A") {                     //TA-
              if (codon[2] == "A") { nothing += 1; }           //TAA
              else if (codon[2] == "T") { tyr[i] += 1; }     //TAT
              else if (codon[2] == "C") { tyr[i] += 1; }     //TAC
              else if (codon[2] == "G") { nothing += 1; }     //TAG
            } else if (codon[1] == "T") {              //TT-
              if (codon[2] == "A") { leu[i] += 1; }           //TTA
              else if (codon[2] == "T") { phe[i] += 1; }     //TTT
              else if (codon[2] == "C") { phe[i] += 1; }     //TTC
              else if (codon[2] == "G") { leu[i] += 1; }     //TTG
            } else if (codon[1] == "C") {               //TC-
              if (codon[2] == "A") { ser[i] += 1; }              //TCA
              else if (codon[2] == "T") { ser[i] += 1; }        //TCT
              else if (codon[2] == "C") { ser[i] += 1; }        //TCC
              else if (codon[2] == "G") { ser[i] += 1; }        //TCG
            } else if (codon[1] == "G") {               //TG-
              if (codon[2] == "A") { nothing += 1; }              //TGA
              else if (codon[2] == "T") { cys[i] += 1; }        //TGT
              else if (codon[2] == "C") { cys[i] += 1; }        //TGC
              else if (codon[2] == "G") { trp[i] += 1; }        //TGG
            }
          } else if (codon[0] == "C") {               //C-
            if (codon[1] == "A") {                     //CA-
              if (codon[2] == "A") { gln[i] += 1; }               //CAA
              else if (codon[2] == "T") { his[i] += 1; }        //CAT
              else if (codon[2] == "C") { his[i] += 1; }        //CAC
              else if (codon[2] == "G") { gln[i] += 1; }        //CAG
            } else if (codon[1] == "T") {              //CT-
              if (codon[2] == "A") { leu[i] += 1; }           //CTA
              else if (codon[2] == "T") { leu[i] += 1; }     //CTT
              else if (codon[2] == "C") { leu[i] += 1; }     //CTC
              else if (codon[2] == "G") { leu[i] += 1; }     //CTG
            } else if (codon[1] == "C") {               //CC-
              if (codon[2] == "A") { pro[i] += 1; }              //CCA
              else if (codon[2] == "T") { pro[i] += 1; }        //CCT
              else if (codon[2] == "C") { pro[i] += 1; }        //CCC
              else if (codon[2] == "G") { pro[i] += 1; }        //CCG
            } else if (codon[1] == "G") {               //CG-
              if (codon[2] == "A") { arg[i] += 1; }              //CGA
              else if (codon[2] == "T") { arg[i] += 1; }        //CGT
              else if (codon[2] == "C") { arg[i] += 1; }        //CGC
              else if (codon[2] == "G") { arg[i] += 1; }        //CGG
            }
          } else if (codon[0] == "G") {              //G-
            if (codon[1] == "A") {                     //GA-
              if (codon[2] == "A") { glu[i] += 1; }               //GAA
              else if (codon[2] == "T") { asp[i] += 1; }        //GAT
              else if (codon[2] == "C") { asp[i] += 1; }        //GAC
              else if (codon[2] == "G") { glu[i] += 1; }        //GAG
            } else if (codon[1] == "T") {              //GT-
              if (codon[2] == "A") { val[i] += 1; }              //GTA
              else if (codon[2] == "T") { val[i] += 1; }        //GTT
              else if (codon[2] == "C") { val[i] += 1; }        //GTC
              else if (codon[2] == "G") { val[i] += 1; }        //GTG
            } else if (codon[1] == "C") {               //GC-
              if (codon[2] == "A") { ala[i] += 1; }              //GCA
              else if (codon[2] == "T") { ala[i] += 1; }        //GCT
              else if (codon[2] == "C") { ala[i] += 1; }        //GCC
              else if (codon[2] == "G") { ala[i] += 1; }        //GCG
            } else if (codon[1] == "G") {               //GG-
              if (codon[2] == "A") { gly[i] += 1; }              //GGA
              else if (codon[2] == "T") { gly[i] += 1; }        //GGT
              else if (codon[2] == "C") { gly[i] += 1; }        //GGC
              else if (codon[2] == "G") { gly[i] += 1; }        //GGG
            }
          }






          codon.Clear(); codonCount = 0;
        }

        codon.Add(superSatellite[i, j]);
        codonCount += 1;
      }
    }

    
    maxEnergy = 10f + energyToReproduce * 2f;
  }

  void FixedUpdate() {
   
    tempNut = nutLevel;

    if (nutLevel < nutToReproduce) {
      posVal = m_nutgrid.GetValue(transform.position);
      if (posVal > 0) {
        nutLevel += 1;
        m_nutgrid.SetValue(transform.position, (int)(posVal - 1));

        int feedTurn = Random.Range(0, (int)turnDice / 8);
        if (feedTurn == 1) {
          rb.AddTorque(turnTorque * Random.Range(-1, 2));
        }
      }
    }
  }
  public bool doInitDiversifier = false;
 
  void LateUpdate() {
    speedModifier = blibSpawner.speedModifier;
    tempProtein = nutLevel;
    

//owowo


    energyTick += Time.deltaTime;
    if (energyTick > 1.0f) {
        maxEnergy = energyToReproduce * 2f;
        proteinCost = (int)Mathf.Round(genome_script.aminoAcidRatio);
        if(nutLevel >= proteinCost+1){
          posVal = m_nutgrid.GetValue(transform.position);
          nutLevel += -1*proteinCost;
        m_nutgrid.SetValue(transform.position, (posVal + proteinCost));
        }
        //GetPheno();
        /*
      redAllele1 = genome_script.redAllele1;
      redAllele2 = genome_script.redAllele2;
      redGene = Mathf.Clamp((redAllele1 + redAllele2) / 2.0f, 0.00f, 1.00f);
      greenAllele1 = genome_script.greenAllele1;
      greenAllele2 = genome_script.greenAllele2;

      blueAllele1 = genome_script.blueAllele1;
      blueAllele2 = genome_script.blueAllele2;

      moveAllele1 = genome_script.moveAllele1;
      moveAllele2 = genome_script.moveAllele2;

      turnTorqueAllele1 = genome_script.turnTorqueAllele1;
      turnTorqueAllele2 = genome_script.turnTorqueAllele2;

      e2repAllele1 = genome_script.e2repAllele1;
      e2repAllele2 = genome_script.e2repAllele2;

      

      lifeLength = (genome_script.lifeLengthAllele1 + genome_script.lifeLengthAllele2);

      energyToReproduce = (e2repAllele1 + e2repAllele2) / 2.0f;

      moveForce = (moveAllele1 + moveAllele2) / 2f;

      turnTorque = (turnTorqueAllele1 + turnTorqueAllele2) / 2.0f;

      

      greenGene = Mathf.Clamp((greenAllele1 + greenAllele2), 0.00f, 1.00f);

      blueGene = Mathf.Clamp((blueAllele1 + blueAllele2) / 2.0f, 0.00f, 1.00f);

      */
        
      geneticColor = new Color(redGene, greenGene, blueGene, 1f);
      

      
      m_SpriteRenderer.color = new Color(redGene, greenGene, blueGene, 1f);
      if (energy < maxEnergy) {
        energy += 32f * greenGene + 24f * redGene + 16*blueGene;
        if (energy > maxEnergy) { energy = maxEnergy; }
      }
      energyTick = 0.0f;
    }



    if (doInitDiversifier == true) {

      InitDiversifier();
    }





    int dC = (int)((lifeLength * Mathf.Pow((4f * lifeLength / age), 2f)) - (8f * lifeLength));
    deathDice = Random.Range(1, dC);

    age += Time.deltaTime;



    // rAgeC = 10 + (L/a)^2 
    int rAgeC = 1 + (int)(Mathf.Pow((lifeLength / (age * 2f)), 2f));
    int rAgeDice = Random.Range(0, rAgeC + 1);


    statAge = age;

    if (rAgeDice == 1 && energy >= energyToReproduce && nutLevel >= nutToReproduce) {
      
        /*
      redAllele1 = genome_script.redAllele1;
      redAllele2 = genome_script.redAllele2;

      greenAllele1 = genome_script.greenAllele1;
      greenAllele2 = genome_script.greenAllele2;

      blueAllele1 = genome_script.blueAllele1;
      blueAllele2 = genome_script.blueAllele2;

      moveAllele1 = genome_script.moveAllele1;
      moveAllele2 = genome_script.moveAllele2;

      turnTorqueAllele1 = genome_script.turnTorqueAllele1;
      turnTorqueAllele2 = genome_script.turnTorqueAllele2;

      e2repAllele1 = genome_script.e2repAllele1;
      e2repAllele2 = genome_script.e2repAllele2;

      lifeLength = (genome_script.lifeLengthAllele1 + genome_script.lifeLengthAllele2);

      energyToReproduce = (e2repAllele1 + e2repAllele2) / 2.0f;

      moveForce = (moveAllele1 + moveAllele2) / 2f;

      turnTorque = (turnTorqueAllele1 + turnTorqueAllele2) / 2.0f;

      redGene = Mathf.Clamp((redAllele1 + redAllele2) / 2.0f, 0.00f, 1.00f);

      greenGene = Mathf.Clamp((greenAllele1 + greenAllele2) / 2.0f, 0.00f, 1.00f);

      blueGene = Mathf.Clamp((blueAllele1 + blueAllele2) / 2.0f, 0.00f, 1.00f);
        */

      Reproduce();

    }


    if (bump == false) {

      GoForward();
    }


    if (deathDice == 1 && age > (lifeLength * (3f / 4f)) || age >= lifeLength || energy <= 0) {

      m_nutgrid.SetValue(transform.position, (int)(posVal + nutLevel));
      nutLevel = 0;
      Destroy(gameObject);

    }





  }


  public int tempProtein;
  void OnCollisionEnter2D(Collision2D col) {
    bump = true;
    GameObject booper = col.gameObject;

    if (booper.tag == "Predator") {
      BrainBlobControls contactor = booper.GetComponent<BrainBlobControls>();
      if (contactor.tryPhagocytise == true) {

        contactor.protein += nutLevel;
        nutLevel = 0;
        contactor.energy += energy;
        energy = 0;

        Destroy(gameObject, 0.2f);
      }

    }

    if (booper.tag == "Predator2") {
      BrainBlybControls contactor = booper.GetComponent<BrainBlybControls>();
      if (contactor.tryPhagocytise == true) {

        contactor.protein += nutLevel;
        nutLevel = 0;
        contactor.energy += energy;
        energy = 0;

        Destroy(gameObject, 0.2f);
      }


    }
    /*
    if (booper.tag == "Prey_noDothis") {
      BlibControls mate;
      mate = booper.GetComponent<BlibControls>();
      float geneticDistance =
      (Mathf.Abs((introns[0, 0] - mate.introns[0, 0])) +
      Mathf.Abs((introns[1, 0] - mate.introns[1, 0])) +
      Mathf.Abs((introns[0, 1] - mate.introns[0, 1])) +
      Mathf.Abs((introns[1, 1] - mate.introns[1, 1]))) / 4.0f;
      //if(geneticDistance > speciationDistance){Debug.Log("blibGenDist = " + geneticDistance);}
      if (geneticDistance < speciationDistance) {



        turnDice = (turnDice + mate.turnDice) / 2;
        lifeLength = (lifeLength + mate.lifeLength) / 2.0f;

        introns[0, 0] = (introns[0, 0] - mate.introns[0, 0]) / 2.0f;
        introns[1, 0] = (introns[1, 0] - mate.introns[1, 0]) / 2.0f;
        introns[0, 1] = (introns[0, 1] - mate.introns[0, 1]) / 2.0f;
        introns[1, 1] = (introns[1, 1] - mate.introns[1, 1]) / 2.0f;

        
        redGene = Mathf.Clamp(((redAllele1 + redAllele2) / 2.0f), 0.00f, 1.00f);
        greenGene = Mathf.Clamp(((greenAllele1 + greenAllele2) / 2.0f), 0.00f, 1.00f);
        blueGene = Mathf.Clamp(((blueAllele1 + blueAllele2) / 2.0f), 0.00f, 1.00f);
        


        geneticColor.r = redGene;
        geneticColor.g = greenGene;
        geneticColor.b = blueGene;
        m_SpriteRenderer.color = geneticColor;


      }
    }
    */

    ContactPoint2D contact = col.GetContact(0);
    Vector2 norm = contact.normal.normalized;


    Quaternion turney = new Quaternion();
    turney.Set(0.0f, 0.0f, norm.x, norm.y);
    transform.rotation = turney;



  }

  void OnCollisionStay2D(Collision2D col) {

    GameObject booper = col.gameObject;

    if (booper.tag == "Predator") {
      BrainBlobControls contactor = booper.GetComponent<BrainBlobControls>();
      if (contactor.tryPhagocytise == true) {

        contactor.protein += nutLevel;
        nutLevel = 0;
        contactor.energy += energy;
        energy = 0;

        Destroy(gameObject, 0.4f);
      } else {
        ContactPoint2D contact = col.GetContact(0);
        float thisDir = rb.rotation * Mathf.Deg2Rad;
        Vector2 norm = contact.normal;


        rb.AddForce(contact.normal * moveForce * 5f);

        rb.AddTorque((norm.y + norm.x) * Mathf.Rad2Deg);
      }

    }

    if (booper.tag == "Predator2") {
      BrainBlybControls contactor = booper.GetComponent<BrainBlybControls>();
      if (contactor.tryPhagocytise == true) {

        contactor.protein += nutLevel;
        nutLevel = 0;
        contactor.energy += energy;
        energy = 0;

        Destroy(gameObject, 0.4f);
      } else {
        ContactPoint2D contact = col.GetContact(0);
        float thisDir = rb.rotation * Mathf.Deg2Rad;
        Vector2 norm = contact.normal;


        rb.AddForce(contact.normal * moveForce * 5f);

        rb.AddTorque((norm.y + norm.x) * Mathf.Rad2Deg);

      }


    }
    if (booper.tag == "Wall" || booper.tag == "ApexPred") {

      ContactPoint2D contact = col.GetContact(0);
      float thisDir = rb.rotation * Mathf.Deg2Rad;
      Vector2 norm = contact.normal;


      rb.AddForce(contact.normal * moveForce * 5f);

      rb.AddTorque((norm.y + norm.x) * Mathf.Rad2Deg);
    }





    GoForward();




  }

  void OnCollisionExit2D(Collision2D col) {

    bump = false;
    GoForward();
  }



  float energyMoveMod;
  void GoForward() {
    if (energy < energyToReproduce / 32f) { energyMoveMod = 0; } else { energyMoveMod = 1f; }
    Vector2 origin = transform.position + transform.up;
    Vector2 direction = transform.up;
    float distance = 64f;
    rayResults = Physics2D.Raycast(origin, direction, distance, layer_mask, -Mathf.Infinity, Mathf.Infinity);
    if (rayResults.collider != null && rayResults.collider.gameObject.tag != "Prey") {
      rb.AddTorque(turnTorque * Random.Range(-1f, 1f));
    }

    rb.AddForce((transform.up * moveForce * rb.mass * speedModifier) * energyMoveMod);
    energy -= (moveForce / 256) * energyMoveMod;
    int randTurner = Random.Range(0, (int)turnDice);

    if (randTurner == 0) {
      rb.AddTorque(turnTorque * Random.Range(-1f, 1f));

    }


  }

  private int SatChunk, SatIndex, pointmutation;
  private string pointString;
  int tempNut;
  void Reproduce() {
    
    int mutationroll = UnityEngine.Random.Range(0, genome_script.final_mutsize);
    SatChunk = Random.Range(0, 4);
    SatIndex = Random.Range(0, 27);
    pointmutation = rndA.Next(0, 4);

    if (pointmutation == 0) { pointString = "A"; } else if (pointmutation == 1) { pointString = "T"; } else if (pointmutation == 2) { pointString = "C"; } else { pointString = "G"; }

    superSatellite[SatChunk, SatIndex] = pointString;











    

 
    introns[0, 0] += (float)rndA.Next(-2, 3);
    introns[1, 0] += (float)rndA.Next(-2, 3);
    introns[0, 1] += (float)rndA.Next(-2, 3);
    introns[1, 1] += (float)rndA.Next(-2, 3);



    





    //geneticColor.r = redGene;
    //geneticColor.g = greenGene;
    //geneticColor.b = blueGene;

   this.gameObject.GetComponent<SpriteRenderer>().color = new Color(redGene, greenGene, blueGene, 1f);



    energy = (energy / 2.0f);
    //bool odd = false;
    int quotient = 0; int remainder = 0; int daughterNutes = 0; int motherNutes = 0;
    if (nutLevel > 0) {
      if(nutLevel == 1){
        motherNutes = 1; daughterNutes = 0;
      }

      if(nutLevel > 1){
        quotient = nutLevel/2;
        remainder = nutLevel%2;
        motherNutes = quotient+remainder;
        daughterNutes = quotient;
      }
       

     /* if (nutLevel == 1) {
        odd = true;
        tempNut = 0;
      } else if (nutLevel > 1) {

        if (nutLevel % 2 == 0) {
          odd = false;
          tempNut = (nutLevel / 2);
        } else {
          odd = true;
          tempNut = (nutLevel - 1) / 2;
        }

      }*/
      nutLevel = 0;
    }


    
    GameObject daughter = Instantiate(this.gameObject);
    BlibGenome daughterGenome = daughter.GetComponent<BlibGenome>();
    daughterGenome.mother = this.genome_script;
    BlibControls daughter_controls = daughter.GetComponent<BlibControls>();
    daughter_controls.generation = generation + 1;
    daughter_controls.age = 0f;
    daughter.name = popLogger.GetName("blib");
    char[] daughterID = new char[8];
    for (int i = 0; i < daughterID.Length; i++) {
      daughterID[i] = blibSpawner.linChars[UnityEngine.Random.Range(0, blibSpawner.linChars.Length)];
    }
    
    
   daughterGenome.lineageID.Add(System.String.Join("", daughterID));
   //daughterGenome.A = this.gameObject.GetComponent<BlibGenome>().A.Clone() as string[,];
   //daughterGenome.B = this.gameObject.GetComponent<BlibGenome>().B.Clone() as string[,];
   
   //daughterGenome.numMutations = 1;
   if(mutationroll == 1){
    daughterGenome.mutate = true;
    
    
   }else{daughterGenome.mutate = false;}
    
    
      daughter_controls.nutLevel = daughterNutes;
      nutLevel = motherNutes;

    //nutLevel = tempNut;





  }



  void InitDiversifier() {
    /*
    redAllele1 = genome_script.redAllele1;
    redAllele2 = genome_script.redAllele2;
    redGene = Mathf.Clamp((redAllele1 + redAllele2) / 2.0f, 0.00f, 1.00f);

    greenAllele1 = genome_script.greenAllele1;
    greenAllele2 = genome_script.greenAllele2;
    greenGene = Mathf.Clamp((greenAllele1 + greenAllele2) / 2.0f, 0.00f, 1.00f);

    blueAllele1 = genome_script.blueAllele1;
    blueAllele2 = genome_script.blueAllele2;
    blueGene = Mathf.Clamp((blueAllele1 + blueAllele2) / 2.0f, 0.00f, 1.00f);

    moveAllele1 = genome_script.moveAllele1;
    moveAllele2 = genome_script.moveAllele2;
    moveForce = (moveAllele1 + moveAllele2) / 2f;

    turnTorqueAllele1 = genome_script.turnTorqueAllele1;
    turnTorqueAllele2 = genome_script.turnTorqueAllele2;
    turnTorque = (turnTorqueAllele1 + turnTorqueAllele2) / 2.0f;

    e2repAllele1 = genome_script.e2repAllele1;
    e2repAllele2 = genome_script.e2repAllele2;
    energyToReproduce = (e2repAllele1 + e2repAllele2) / 2.0f;
    lifeLength = (genome_script.lifeLengthAllele1 + genome_script.lifeLengthAllele2);

    */





    /*
    geneticColor = new Color(redGene, greenGene, blueGene, 1f);
    geneticColor.r = redGene;
    geneticColor.g = greenGene;
    geneticColor.b = blueGene;

    m_SpriteRenderer.color = new Color(redGene, greenGene, blueGene, 1f);;
    */
    if (initDiversity != 0) {
      //Mutation





        /*
      turnDice += (float)(rndA.Next(-1, 2) * initDiversity);
      introns[0, 0] += (float)rndA.Next(-1, 2) * rndA.Next(2);
      introns[1, 0] += (float)rndA.Next(-1, 2) * rndA.Next(2);
      introns[0, 1] += (float)rndA.Next(-1, 2) * rndA.Next(2);
      introns[1, 1] += (float)rndA.Next(-1, 2) * rndA.Next(2);
      lifeLength += (float)(rndA.Next(-1, 2) * initDiversity);
      energyToReproduce += (float)(rndA.Next(-1, 2) * initDiversity);





      redGene = Mathf.Clamp(((redAllele1 + redAllele2) / 2.0f), 0.00f, 1.00f);
      greenGene = Mathf.Clamp(((greenAllele1 + greenAllele2) / 2.0f), 0.00f, 1.00f);
      blueGene = Mathf.Clamp(((blueAllele1 + blueAllele2) / 2.0f), 0.00f, 1.00f);
        */
    } else { return; }
    doInitDiversifier = false;
  }

    void GetPheno(){
    redAllele1 = genome_script.redAllele1;
    redAllele2 = genome_script.redAllele2;
    redGene = Mathf.Clamp((redAllele1 + redAllele2) / 2.0f, 0.00f, 1.00f);

    greenAllele1 = genome_script.greenAllele1;
    greenAllele2 = genome_script.greenAllele2;
    greenGene = Mathf.Clamp((greenAllele1 + greenAllele2) / 2.0f, 0.00f, 1.00f);

    blueAllele1 = genome_script.blueAllele1;
    blueAllele2 = genome_script.blueAllele2;
    blueGene = Mathf.Clamp((blueAllele1 + blueAllele2) / 2.0f, 0.00f, 1.00f);

    moveAllele1 = genome_script.moveAllele1;
    moveAllele2 = genome_script.moveAllele2;
    moveForce = (moveAllele1 + moveAllele2) / 2f;

    turnTorqueAllele1 = genome_script.turnTorqueAllele1;
    turnTorqueAllele2 = genome_script.turnTorqueAllele2;
    turnTorque = (turnTorqueAllele1 + turnTorqueAllele2) / 2.0f;

    e2repAllele1 = genome_script.e2repAllele1;
    e2repAllele2 = genome_script.e2repAllele2;
    energyToReproduce = 128f + (e2repAllele1 + e2repAllele2) / 2.0f;
    lifeLength = (genome_script.lifeLengthAllele1 + genome_script.lifeLengthAllele2);

  }
    

        void OnDestroy()
    {
        genome_script = null;
        
    }



}
