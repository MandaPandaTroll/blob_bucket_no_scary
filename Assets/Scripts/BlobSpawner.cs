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
  public float initEnergy;

  GameObject box;
  Vector2 boxDims;
  public bool autoRespawn;

    // Start is called before the first frame update
    void Start()
    {   
        box = GameObject.Find("box");
        boxSize = box.transform.localScale.x;
        boxDims.x = box.transform.localScale.x;
        boxDims.y = box.transform.localScale.y;
        for(int i = 0; i < initBlob; i++){
        float x = (float)Random.Range(-boxDims.x/3,boxDims.x/3);
        float y = (float)Random.Range(-boxDims.y/3,boxDims.y/3);
       Instantiate(blob, new Vector3(x, y, 0), Quaternion.identity);
       BrainBlobControls bctrl = blob.GetComponent<BrainBlobControls>();
       bctrl.protein = initProtein;
       initEnergy = bctrl.maxEnergy*0.1f;
       bctrl.energy = initEnergy;
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
        float x = (float)Random.Range(-boxDims.x/3,boxDims.x/3);
        float y = (float)Random.Range(-boxDims.y/3,boxDims.y/3);
       Instantiate(blob, new Vector3(x, y, 0), Quaternion.identity);
       BrainBlobControls bctrl = blob.GetComponent<BrainBlobControls>();
       bctrl.protein = 0;
       initEnergy = bctrl.maxEnergy*0.1f;
       bctrl.energy = initEnergy;
       bctrl.age = Random.Range(0,bctrl.lifeLength*0.75f);
  }
  }

}
