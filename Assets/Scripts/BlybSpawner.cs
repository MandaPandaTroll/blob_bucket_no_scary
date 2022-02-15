using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlybSpawner : MonoBehaviour
{
   int blybN;
   GameObject[] blybs;
  public int initBlyb;
  public int extraBlyb;
  public GameObject blyb;
  float boxSize;
  public int minBlyb;

  GameObject box;

  private bool autoRespawn;

    // Start is called before the first frame update
    void Start()
    {   
        box = GameObject.Find("box");
        boxSize = box.transform.localScale.x;

        for(int i = 0; i < initBlyb; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(blyb, new Vector3(x, y, 0), Quaternion.identity);
        }
    }


      void OnGUI()
    {
      autoRespawn = GUI.Toggle( new Rect(10,600,200,30),autoRespawn,"autoRespawn_blyb" );
    }
void Update()
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
  }
  }

}
