using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSplitter : MonoBehaviour
{
    BoxCollider2D thisCollider;
    SpriteRenderer thisRenderer;
   public bool splitter_enabled;
    // Start is called before the first frame update
    void Start()
    {
        thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
        thisRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        if(splitter_enabled == false){
            thisCollider.enabled = false;
            thisRenderer.enabled = false;
        }
        else if(splitter_enabled == true){
            thisCollider.enabled = true;
            thisRenderer.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space") == true)
            {
                if(splitter_enabled == false){splitter_enabled = true;}
                else if(splitter_enabled == true){splitter_enabled = false;}

                if(splitter_enabled == true){
                    thisCollider.enabled = true;
                    thisRenderer.enabled = true;
                }
                else if(splitter_enabled == false){
                    thisCollider.enabled = false;
                    thisRenderer.enabled = false;
                }
            }
    }
}
