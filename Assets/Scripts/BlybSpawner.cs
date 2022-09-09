using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlybSpawner : MonoBehaviour
{
   
   int blybN;
  public int initBlyb;
  public int extraBlyb;
  public int minBlyb;
  public GameObject blyb;
  float boxSize;
  GameObject[] blybs;
  public int initProtein;

  GameObject box;

  public bool autoRespawn;

    // Start is called before the first frame update
    void Start()
    {   
        box = GameObject.Find("box");
        boxSize = box.transform.localScale.x;

        for(int i = 0; i < initBlyb; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(blyb, new Vector3(x, y, 0), Quaternion.identity);
       BrainBlybControls bctrl = blyb.GetComponent<BrainBlybControls>();
       bctrl.protein = initProtein;
       bctrl.age = Random.Range(0,bctrl.lifeLength*0.75f);
        }
    }

      void OnGUI()
    {
      autoRespawn = GUI.Toggle( new Rect(10,600,200,30),autoRespawn,"autoRespawn_blyb" );
    }
void LateUpdate()
{
  blybs = GameObject.FindGameObjectsWithTag("Predator2");
  blybN = blybs.Length;
  if (Input.GetKeyDown("y") == true  ){ extraSpawn();}
        if (autoRespawn == true && blybN < minBlyb){extraSpawn();}
        
    }
  

  void extraSpawn()
  {
        for(int i = 0; i < extraBlyb; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(blyb, new Vector3(x, y, 0), Quaternion.identity);
       BrainBlybControls bctrl = blyb.GetComponent<BrainBlybControls>();
       bctrl.protein = 0;
       bctrl.age = Random.Range(0,bctrl.lifeLength*0.75f);
  }
  }

}
