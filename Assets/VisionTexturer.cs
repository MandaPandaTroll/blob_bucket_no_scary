using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisionTexturer : MonoBehaviour
{


    

   public GameObject m_gameObject;
   Smeller_Blub smeller;
   public int dims;
    // Start is called before the first frame update
     void Start()
     {  smeller = m_gameObject.GetComponent<Smeller_Blub>();
         // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
         //for(int i = 0; i <63; i ++){
            //previousX[i] = 0;
            //previousY[i] = 0;
                 texture =  new Texture2D(dims, dims, TextureFormat.ARGB32, false);
                  for(int i= 0; i < dims; i++){
                    for(int j= 0; j < dims; j++){
                    texture.SetPixel(i, j, new Color(0f,0f,0f,0.0f));
                    }
                }
         
         
     }
    public Vector2[] rawPositions = new Vector2[16];
    float maxDistance;
    Vector2 center = new Vector2();
    Vector2 forward = new Vector2();
    //Vector2 relativeVector = new Vector2();

    int[] previousX = new int[9]{0,0,0,0,0,0,0,0,0};
    int[] previousY = new int[9]{0,0,0,0,0,0,0,0,0};
   Texture2D texture; 
   float distanceColour;
    
    // Update is called once per frame
    void Update()
    {   
         //texture =  new Texture2D(dims, dims, TextureFormat.ARGB32, false);
        maxDistance = smeller.smellDistance;
        //float shifter = Mathf.Sqrt(2f);
        // texture = new Texture2D((int)32, (int)32, TextureFormat.ARGB32, false);
        center = m_gameObject.transform.position;
        
       // for(int i= 0; i < dims; i++){
           // for(int j= 0; j < dims; j++){
            //    texture.SetPixel(i, j, new Color(1f,1f,1f,1.0f));
         //   }
       // }
        
        
         
         for(int i = 0; i < 8; i++){
            
                rawPositions[i] = smeller.scaledPreyDistance[i];
                distanceColour = Mathf.Clamp01(1f / rawPositions[i].magnitude);
                texture.SetPixel(previousX[i], previousY[i], new Color(0.0f,0.0f,0.0f,0.0f));
                int x; int y;
                if(rawPositions[i].x == 0 && rawPositions[i].y == 0){
                    x = 0;
                    y = 0;
                }else{
                        x = Mathf.RoundToInt((1f+rawPositions[i].x)*(dims/2));
                        y = Mathf.RoundToInt((1f+rawPositions[i].y)*(dims/2));
                }
                
               if(x == 0 && y == 0){
                texture.SetPixel(x, y, new Color(0f,0f,0f,0f));
               }else{
                texture.SetPixel(x, y, new Color(1f,1f,1f,1f-distanceColour));
               }
                
                previousX[i] = x;
                previousY[i] = y; 
            
                //Debug.Log("("+x+","+y+")");
         }
         // set the pixel values
        
         
         // Apply all SetPixel calls
         texture.Apply();
         
         // connect texture to material of GameObject t$$anonymous$$s script is attached to
         gameObject.GetComponent<RawImage>().texture = texture;
        
    }
}
