using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlobSpawner : MonoBehaviour
{
   
   int blobN;
  public int initBlob;
  public int extraBlob;
  public int minBlob;
  public GameObject blob;
  float boxSize;
  GameObject[] blobs;
  public int initProtein;

  GameObject box;

  public bool autoRespawn;

    // Start is called before the first frame update
    void Start()
    {   
        box = GameObject.Find("box");
        boxSize = box.transform.localScale.x;

        for(int i = 0; i < initBlob; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(blob, new Vector3(x, y, 0), Quaternion.identity);
       BrainBlobControls bctrl = blob.GetComponent<BrainBlobControls>();
       bctrl.protein = initProtein;
       bctrl.age = Random.Range(0,bctrl.lifeLength*0.75f);
        }
    }

        void OnGUI()
    {
      autoRespawn = GUI.Toggle( new Rect(10,550,200,30),autoRespawn,"autoRespawn_blob" );
    }
void LateUpdate()
{
  blobs = GameObject.FindGameObjectsWithTag("Predator");
  blobN = blobs.Length;
  if (Input.GetKeyDown("o") == true  ){ extraSpawn();}
        if (autoRespawn == true && blobN < minBlob){extraSpawn();}
        
    }
  

  void extraSpawn()
  {boxSize = box.transform.localScale.x;
        for(int i = 0; i < extraBlob; i++){
        float x = (float)Random.Range(-boxSize/3,boxSize/3);
        float y = (float)Random.Range(-boxSize/3,boxSize/3);
       Instantiate(blob, new Vector3(x, y, 0), Quaternion.identity);
       BrainBlobControls bctrl = blob.GetComponent<BrainBlobControls>();
       bctrl.protein = 0;
       bctrl.age = Random.Range(0,bctrl.lifeLength*0.75f);
  }
  }

}
