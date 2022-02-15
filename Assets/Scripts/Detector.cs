using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

Vector3 blCorner, trCorner;
float maxDist;
float density;
public Vector2 mean1, mean2, mean3, mean4;

    float time;
   public int in1;
   public int in2;
   public int in3;
   public int in4;
    int blib_mask;
   Vector2 p = new Vector2 (0f,0f);
   Vector2 minP;
   Vector2 meanAll =new Vector2();
   public Vector2 outMeanAll =new Vector2();

    Vector2 boxAll;
    Vector2 boxA;
    Vector2 boxB;
    Vector2 boxC;
    Vector2 boxD;

   Collider2D[] detect1;
   Collider2D[] detect2;
   Collider2D[] detect3;

   Collider2D[] detect4;

   public int rDiceSizeA;
   public int rDiceSizeB;
   public int rDiceSizeC;
   public int rDiceSizeD;
   Transform boxTransform;
   float boxLength;

    // Start is called before the first frame update
    void Start()
    {
        meanAll = Vector2.zero;
        outMeanAll = Vector2.zero;
        mean1 = Vector2.zero;
        mean2 = Vector2.zero;
        mean3 = Vector2.zero;
        mean4 = Vector2.zero;
       blib_mask = LayerMask.GetMask("Prey");
       boxTransform = GameObject.Find("box").GetComponent<Transform>();
        minP = new Vector2(-boxTransform.lossyScale.x/2f, -boxTransform.lossyScale.y/2f);
        boxAll = new Vector2((boxTransform.lossyScale.x/2f),(boxTransform.lossyScale.y/2f));
        boxA = new Vector2((boxTransform.position.x - boxTransform.lossyScale.x/2f),(boxTransform.position.y + boxTransform.lossyScale.y/2f));
        boxB = new Vector2((boxTransform.position.x + boxTransform.lossyScale.x/2f),(boxTransform.position.y + boxTransform.lossyScale.y/2f));
        boxC = new Vector2((boxTransform.position.x - boxTransform.lossyScale.x/2f),(boxTransform.position.y - boxTransform.lossyScale.y/2f));
        boxD = new Vector2((boxTransform.position.x + boxTransform.lossyScale.x/2f),(boxTransform.position.y - boxTransform.lossyScale.y/2f));
        boxLength = boxTransform.lossyScale.x;

        blCorner = new Vector3(-boxTransform.lossyScale.x/2f, -boxTransform.lossyScale.y/2f, 0);
        trCorner = new Vector3(boxTransform.lossyScale.x/2f, boxTransform.lossyScale.y/2f, 0);
        maxDist = Vector3.Distance(blCorner,trCorner);
        Debug.Log(boxA);
        Debug.Log(boxB);
        Debug.Log(boxC);
        Debug.Log(boxD);
    }

    // Update is called once per frame
    void Update()
    {   time += Time.deltaTime;
       
        if(time >= 1){
        meanAll = Vector2.zero;
        outMeanAll = Vector2.zero;
        mean1 = Vector2.zero;
        mean2 = Vector2.zero;
        mean3 = Vector2.zero;
        mean4 = Vector2.zero;

        boxLength = boxTransform.lossyScale.x;

        Collider2D[] detectAll = Physics2D.OverlapAreaAll(minP, boxAll, blib_mask);

        Collider2D[] detect1 = Physics2D.OverlapAreaAll(p, boxA, blib_mask);
        in1 = detect1.Length;
        rDiceSizeA = (int)Mathf.Pow(2.0f, (float)0.25f*in1/Mathf.Sqrt(boxLength));

        Collider2D[] detect2 = Physics2D.OverlapAreaAll(p, boxB, blib_mask);
        in2 = detect2.Length;
        rDiceSizeB = (int)Mathf.Pow(2.0f, (float)0.25f*in2/Mathf.Sqrt(boxLength));

        Collider2D[] detect3 = Physics2D.OverlapAreaAll(p, boxC, blib_mask);
        in3 = detect3.Length;
        rDiceSizeC = (int)Mathf.Pow(2.0f, (float)0.25f*in3/Mathf.Sqrt(boxLength));

        Collider2D[] detect4 = Physics2D.OverlapAreaAll(p, boxD, blib_mask);
        in4 = detect4.Length;
        rDiceSizeD = (int)Mathf.Pow(2.0f, (float)0.25f*in4/Mathf.Sqrt(boxLength));

        boxA = new Vector2((boxTransform.position.x - boxTransform.lossyScale.x/2f),(boxTransform.position.y + boxTransform.lossyScale.y/2f));
        boxB = new Vector2((boxTransform.position.x + boxTransform.lossyScale.x/2f),(boxTransform.position.y + boxTransform.lossyScale.y/2f));
        boxC = new Vector2((boxTransform.position.x - boxTransform.lossyScale.x/2f),(boxTransform.position.y - boxTransform.lossyScale.y/2f));
        boxD = new Vector2((boxTransform.position.x + boxTransform.lossyScale.x/2f),(boxTransform.position.y - boxTransform.lossyScale.y/2f));
        
        density = (float)detectAll.Length/(boxLength*boxLength);

            foreach (Collider2D blib in detect1){
            float distx = blib.transform.position.x/(boxLength/2.0f);
            float disty = blib.transform.position.y /(boxLength/2.0f);
            
             
            mean1.x  += distx;;
            mean1.y += disty;
        }
            mean1.x = (mean1.x /detect1.LongLength);
            mean1.y = (mean1.y /detect1.LongLength);

            foreach (Collider2D blib in detect2){
            float distx = blib.transform.position.x/(boxLength/2.0f);
            float disty = blib.transform.position.y /(boxLength/2.0f);
            
             
            mean2.x  += distx;;
            mean2.y += disty;
        }
            mean2.x = (mean2.x /detect2.LongLength);
            mean2.y = (mean2.y /detect2.LongLength);


            foreach (Collider2D blib in detect3){
            float distx = blib.transform.position.x/(boxLength/2.0f);
            float disty = blib.transform.position.y /(boxLength/2.0f);
            
             
            mean3.x  += distx;;
            mean3.y += disty;
        }
            mean3.x = (mean3.x /detect3.LongLength);
            mean3.y = (mean3.y /detect3.LongLength);

            foreach (Collider2D blib in detect4){
            float distx = blib.transform.position.x/(boxLength/2.0f);
            float disty = blib.transform.position.y /(boxLength/2.0f);
            
             
            mean4.x  += distx;;
            mean4.y += disty;
        }
            mean4.x = (mean4.x /detect4.LongLength);
            mean4.y = (mean4.y /detect4.LongLength);

        meanAll.x  = (mean1.x + mean2.x + mean3.x + mean4.x)/4.0f;
        meanAll.y  = (mean1.y + mean2.y + mean3.y + mean4.y)/4.0f;

         
        outMeanAll = meanAll;
       Debug.Log("outMeanAll = " + outMeanAll.ToString("F4") + "mean1 = " + mean1.ToString("F4") + "mean2 = " + mean2.ToString("F4") + "mean3 = " + mean3.ToString("F4") + "mean4 = " + mean4.ToString("F4"));
        time = 0f;

        }


        

    }
 
           

           
  
    

    
}
