using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrainingPreySpawner : MonoBehaviour
{
   
   int blobN;
  public int initBlob;
  public int extraBlob;
  public GameObject trainblob;
  GameObject[] blobs;

  GameObject box;

 float boxSize;
    private bool autoRespawn;

    // Start is called before the first frame update
    void Start()
    {   
        box = GameObject.Find("box");
         boxSize = box.transform.localScale.x;

        for(int i = 0; i < initBlob; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(trainblob, new Vector3(x, y, 0), Quaternion.identity);

        }
    }

        void OnGUI()
    {
      autoRespawn = GUI.Toggle( new Rect(10,550,200,30),autoRespawn,"autoRespawn_Tblob" );
    }
void Update()
{
  blobs = GameObject.FindGameObjectsWithTag("Predator");
  blobN = blobs.Length;
  if (Input.GetKeyDown("o") == true  ){ extraSpawn();}
        if (autoRespawn == true && blobN < 500){extraSpawn();}
        
    }
  

  void extraSpawn()
  {
        for(int i = 0; i < extraBlob; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(trainblob, new Vector3(x, y, 0), Quaternion.identity);
  }
  }

}
