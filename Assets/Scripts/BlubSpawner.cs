using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlubSpawner : MonoBehaviour
{
   
  public int initBlub;
  public int extraBlub;
  public GameObject blub;
  GameObject[] blubs;
  GameObject box;
 float boxSize;
  int blubN;
  private bool autoRespawn;
  public int minBlub;

    // Start is called before the first frame update
    void Start()
    {   
        box = GameObject.Find("box");
         boxSize = box.transform.localScale.x;

        for(int i = 0; i < initBlub; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(blub, new Vector3(x, y, 0), Quaternion.identity);
        }
    }

        void OnGUI()
    {
      autoRespawn = GUI.Toggle( new Rect(10,650,200,30),autoRespawn,"autoRespawn_blub" );
    }

void Update()
{
  blubs = GameObject.FindGameObjectsWithTag("ApexPred");
  blubN = blubs.Length;
  if (Input.GetKeyDown("u") == true  ){ extraSpawn();}
        if (autoRespawn == true && blubN < minBlub){extraSpawn();}
        
    }
  

  void extraSpawn()
  {
        for(int i = 0; i < extraBlub; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(blub, new Vector3(x, y, 0), Quaternion.identity);
  }
  }

}
