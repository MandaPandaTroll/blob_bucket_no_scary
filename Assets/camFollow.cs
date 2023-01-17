using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    Transform parentTransform;
    Camera cam;
   public BrainBlubControls bctrl;
   
    
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = gameObject.GetComponentInParent<Transform>();
        cam = gameObject.GetComponent<Camera>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        cam.orthographicSize = bctrl.lookDistance;
        
        Vector3 parentPosition = parentTransform.position;
        //transform.rotation = parentTransform.rotation;
        transform.rotation = Quaternion.identity;
        Vector3 newPosition = new Vector3(parentPosition.x,parentPosition.y,-1f);
        transform.position = newPosition;
        
    }
}
