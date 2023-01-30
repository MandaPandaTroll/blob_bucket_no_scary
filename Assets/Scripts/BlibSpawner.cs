using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlibSpawner : MonoBehaviour {
  public int initialMutations;
  public initGenomeTest_blib initgen;
  private string[,] initA = new string[9,486];
  private string[,] initB = new string[9,486];
  public char[] linChars;
  public char[] initLineage;
  public List<char[]> initLineages = new List<char[]>();
  public int initBlib;
  public bool autoRespawn;
  public int extraBlib;

  public int minBlib;
  public  GameObject blib;
  GameObject[] blibs;
  BlibGenome thisGenome;

  GameObject box;
  int blibN;
  float boxSize;
  Vector2 boxDims;
  PopLogger popLogger;
  public bool speedModifier_enabled;
  public float speedModifier;
  private float vSliderValue;
  public int initProtein;

  // Start is called before the first frame update

  
  void Start() {

    
    string[,,] owo = initGenomestatic.GetInitGeno();
    for(int i = 0; i < 9; i++){
              for(int j = 0; j < 486; j++){
                initA[i,j] = owo[0,i,j];
                initB[i,j] = owo[1,i,j];
              }
    }
    
    //initA = (string[,])initGenomestatic.A_static.Clone();
    //initB = (string[,])initGenomestatic.B_static.Clone();
    
    linChars = new char[93];
    initLineage = new char[32];
    for (int i = 0; i < linChars.Length; i++) {
      linChars[i] = (char)(i + 33);
    }

    popLogger = GameObject.Find("StatisticsHandler").GetComponent<PopLogger>();

    box = GameObject.Find("box");
    
    boxDims.x = box.transform.localScale.x;
    boxDims.y = box.transform.localScale.y;
    blib.name = "blib";
    for (int i = 0; i < initBlib; i++) {

      for (int k = 0; k < initLineage.Length; k++) {
        initLineage[k] = linChars[Random.Range(0, linChars.Length)];

      }
      initLineages.Add(initLineage);

      float x = (float)Random.Range(-boxDims.x / 3, boxDims.x / 3);
      float y = (float)Random.Range(-boxDims.y / 3, boxDims.y / 3);
      GameObject thisBlib = Instantiate(blib, new Vector3(x, y, 0), Quaternion.identity);
      thisGenome = thisBlib.GetComponent<BlibGenome>();
      thisGenome.lineageID.Add(System.String.Join("", initLineages[i]));
      //thisGenome.A = new string[9,486];
      //thisGenome.B = new string[9,486];
      //thisGenome.A = initB.Clone() as string[,];
      //thisGenome .B = initB.Clone() as string[,];
      thisGenome.numMutations = initialMutations;
      thisGenome.mutate = true;
      thisGenome.mother = null;

      thisBlib.name = popLogger.GetName("blib");
      thisBlib.name = thisBlib.name.Replace("(Clone)", "");
      BlibControls thisBlibControls = thisBlib.GetComponent<BlibControls>();
      //thisBlibControls.energy = 100f;
      thisBlibControls.nutLevel = initProtein;
      //thisBlibControls.age = Random.Range(0f, thisBlibControls.lifeLength * 0.8f);
      //thisBlibControls.energy = Random.Range(thisBlibControls.energyToReproduce / 16f, thisBlibControls.energyToReproduce);
      

    }
  }


  void OnGUI() {
    autoRespawn = GUI.Toggle(new Rect(10, 500, 200, 30), autoRespawn, "autoRespawn_blib");
    speedModifier_enabled = GUI.Toggle(new Rect(10, 200, 200, 30), speedModifier_enabled, "blib_speed_mod");

    vSliderValue = GUI.HorizontalSlider(new Rect(25, 250, 200, 30), vSliderValue, 0.0f, 1.0f);
  }

  void LateUpdate() {


    blibs = GameObject.FindGameObjectsWithTag("Prey");
    blibN = blibs.Length;
    if (Input.GetKeyDown("i") == true) { extraSpawn(); }
    if (autoRespawn == true && blibN <= minBlib) { extraSpawn(); }

    if (speedModifier_enabled == true) {
      speedModifier = vSliderValue;
    } else { speedModifier = 1f; }
  }


  void extraSpawn() {

    for (int i = 0; i < extraBlib; i++) {
      float x = (float)Random.Range(-boxDims.x / 3, boxDims.x / 3);
      float y = (float)Random.Range(-boxDims.y / 3, boxDims.y / 3);
      GameObject thisBlib = Instantiate(blib, new Vector3(x, y, 0), Quaternion.identity);
      thisGenome = thisBlib.GetComponent<BlibGenome>();
      
      //thisGenome.A = new string[9,486];
      //thisGenome.B = new string[9,486];
      //thisGenome.A = initB.Clone() as string[,];
      //thisGenome .B = initB.Clone() as string[,];
      thisGenome.numMutations = initialMutations;
      thisGenome.mutate = true;
      thisGenome.mother = null;

      thisBlib.name = popLogger.GetName("blib");
      thisBlib.name = thisBlib.name.Replace("(Clone)", "");
      BlibControls thisBlibControls = thisBlib.GetComponent<BlibControls>();
      //thisBlibControls.energy = 100f;
      thisBlibControls.nutLevel = initProtein;

    }
  }
/* 
string[,,] createAntiSenseStrand(BlibGenome thisGenome){
    string[,,] antis = new string[2,9,486];
    
    string sensebaseA;
    string sensebaseB;
    //CREATE ANTISENSE STRANDS
    for (int i = 0; i < thisGenome.A.GetLength(0); i++){
      for (int j = 0; j < thisGenome.A.GetLength(1); j++){
        sensebaseA = thisGenome.A[i,485-j];
        sensebaseB = thisGenome.B[i,485-j];
        switch (sensebaseA){
          case "A":
          antis[0,i,j] = "T";
          break;
          case "T":
          antis[0,i,j] = "A";
          break;
          case "C":
          antis[0,i,j] = "G";
          break;
          case "G":
          antis[0,i,j] = "C";
          break;
        }
        switch (sensebaseB){
          case "A":
          antis[1,i,j] = "T";
          break;
          case "T":
          antis[1,i,j] = "A";
          break;
          case "C":
          antis[1,i,j] = "G";
          break;
          case "G":
          antis[1,i,j] = "C";
          break;
        }
        
        
        
      }
    }
    return antis;
  }
  */
}
