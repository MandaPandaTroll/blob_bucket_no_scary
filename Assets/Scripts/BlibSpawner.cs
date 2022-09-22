using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlibSpawner : MonoBehaviour
{
   
  public char[] linChars;
  public char[] initLineage;
  public List<char[]> initLineages = new List<char[]>();
  public int initBlib;
  public bool autoRespawn;
  public int extraBlib;

  public int minBlib;
  public GameObject blib;
  GameObject[] blibs;

  GameObject box;
  int blibN;
  float boxSize;
  PopLogger popLogger;
  public bool speedModifier_enabled;
  public float speedModifier;
  private float vSliderValue;
  public int initProtein;

    // Start is called before the first frame update
    void Start()
    {   

      linChars = new char[93];
      initLineage = new char[32];
      for (int i = 0; i <  linChars.Length; i++){
        linChars[i] = (char)(i+33);
      }

      popLogger = GameObject.Find("StatisticsHandler").GetComponent<PopLogger>();
      
        box = GameObject.Find("box");
         boxSize = box.transform.localScale.x;
        blib.name = "blib";
        for(int i = 0; i < initBlib; i++){

            for (int k = 0; k < initLineage.Length;k++){
                initLineage[k] = linChars[Random.Range(0,linChars.Length)];

            }
            initLineages.Add(initLineage);

        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       GameObject thisBlib = Instantiate(blib, new Vector3(x, y, 0), Quaternion.identity);
       thisBlib.GetComponent<BlibGenome>().lineageID.Add (System.String.Join("", initLineages[i]));
       thisBlib.name = popLogger.GetName("blib");
       thisBlib.name = thisBlib.name.Replace("(Clone)", "");
       BlibControls thisBlibControls = thisBlib.GetComponent<BlibControls>();
       thisBlibControls.energy = 100f;
       thisBlibControls.nutLevel = initProtein;
       thisBlibControls.age = Random.Range(0f, thisBlibControls.lifeLength*0.8f);
       thisBlibControls.energy = Random.Range(thisBlibControls.energyToReproduce/16f, thisBlibControls.energyToReproduce);
       thisBlibControls.doInitDiversifier = true;
          
        }
    }


    void OnGUI()
    {
      autoRespawn = GUI.Toggle( new Rect(10,500,200,30),autoRespawn,"autoRespawn_blib" );
      speedModifier_enabled = GUI.Toggle (new Rect (10, 200, 200, 30), speedModifier_enabled, "blib_speed_mod");
            
                vSliderValue = GUI.HorizontalSlider (new Rect (25, 250, 200, 30), vSliderValue, 0.0f, 1.0f);
    }

void LateUpdate()
{
  

  blibs = GameObject.FindGameObjectsWithTag("Prey");
  blibN = blibs.Length;
  if (Input.GetKeyDown("i") == true  ){ extraSpawn();}
        if (autoRespawn == true && blibN <= minBlib){extraSpawn();}
  
  if(speedModifier_enabled == true){
    speedModifier = vSliderValue;
      }else{speedModifier = 1f;}
    }
  

  void extraSpawn()
  {
    
        for(int i = 0; i < extraBlib; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       GameObject thisBlib = Instantiate(blib, new Vector3(x, y, 0), Quaternion.identity);
       thisBlib.GetComponent<BlibControls>().energy = 100f;
       thisBlib.name = popLogger.GetName("blib");
       thisBlib.name = thisBlib.name.Replace("(Clone)", "");
       
      }
  }

}
