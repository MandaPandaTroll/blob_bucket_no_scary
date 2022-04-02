using UnityEngine;
using UnityEngine.UI;

//Displays data and time
//tabacwoman march 2022
public class TimeDisplay : MonoBehaviour
{

    Text m_Text;
    RectTransform m_RectTransform;

    float time;
    float tScale;
    int grandNutes;
    int totNutes;
    GameObject cam;
    CamCntrl camCntrl;
    float camSpeed;     
    Testing testing;
    int gridX, gridY;
     int initConc;
     public float refreshRate;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Text = GetComponent<Text>();
        m_RectTransform = GetComponent<RectTransform>();
        cam = GameObject.Find("Main Camera");
        camCntrl = cam.GetComponent<CamCntrl>();
        testing = GameObject.Find("Testing").GetComponent<Testing>();
        grandNutes = testing.statNutes;
        totNutes = testing.statTot;
        gridX = testing.gridX;
        gridY = testing.gridY;
        initConc = testing.initConc;
    }
       public int blibs;
        public int blobs;
        public int blybs;
        public int blubs;
           
            float timeToDisplay;
    void FixedUpdate()
    {   
       
        totNutes = testing.statTot;
        grandNutes = testing.statNutes;
        initConc = testing.initConc;
        camSpeed = camCntrl.camSpeed;
        
         timeToDisplay = Mathf.Round(Time.time);
        tScale = Time.timeScale;
        time += Time.deltaTime;
        if(time >= refreshRate){
             blibs = FindObjectsOfType<BlibControls>().Length;
             blobs = FindObjectsOfType<BrainBlob>().Length;
             blybs = FindObjectsOfType<BrainBlyb>().Length;
             blubs = FindObjectsOfType<BrainBlub>().Length;
        

                    string timeString = timeToDisplay.ToString();
                    string blibString = blibs.ToString();
                    string blobString = blobs.ToString();
                    string blybString = blybs.ToString();
                    string blubString = blubs.ToString();
                    string camSpeedString = camSpeed.ToString();
                    string timeScaleString = tScale.ToString();
                    string nuteString = ((float)grandNutes/(float)totNutes).ToString();
                    string expNuteString = initConc.ToString();
                    
                 //Change the m_Text text to the message below
                 m_Text.text = "t = " + timeString + "\n"  + 
                 "Blibs = " + blibString + "\n" + 
                 "Blobs = " + blobString + "\n" + 
                 "Blybs = " + blybString + "\n" + 
                 "Blubs = " + blubString + "\n" + 
                 "Nutes: "  +              "\n" + 
                 "Expected = "  + totNutes + "\n" + 
                 "Measured = "      + grandNutes     + "\n" + 
                 "Free = "      + testing.freeNutes     + "\n" + 
                 "Locked = "    + testing.lockedNutes    + "\n" + 
                 "measured/expected = " + nuteString    + "\n" + 
                 
                 "camSpeed = " + camSpeedString + "\n" + 
                 "timeScale = " + timeScaleString+"x" ;
                 
                 time = 0f;

        }
    }
}
