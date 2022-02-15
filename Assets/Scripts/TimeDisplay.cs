using UnityEngine;
using UnityEngine.UI;

//Displays pops and time
//tabacwoman november 2021
public class TimeDisplay : MonoBehaviour
{

    Text m_Text;
    RectTransform m_RectTransform;
    GameObject PopGraph;
    Graph Graph;
    int blubs;
    GameObject StatisticsHandler;
    Logger Logger;

    int blibs;
    int blobs;
    int blybs;
    float ratio;
    float time;
    float tScale;

    GameObject cam;
    CamCntrl camCntrl;
    float camSpeed;     
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_Text = GetComponent<Text>();
        m_RectTransform = GetComponent<RectTransform>();
        PopGraph = GameObject.Find("PopGraph");
        Graph = PopGraph.GetComponent<Graph>();
        cam = GameObject.Find("Main Camera");
        camCntrl = cam.GetComponent<CamCntrl>();
        

    }

    // Update is called once per frame
    void Update()
    {   
       
        
        
        camSpeed = camCntrl.camSpeed;
        
        float timeToDisplay = Mathf.Round(Time.time);
        tScale = Time.timeScale;
        time += Time.deltaTime;
        if(time >= 1.0f){
        blibs = GameObject.FindGameObjectsWithTag("Prey").Length;
        blobs = GameObject.FindGameObjectsWithTag("Predator").Length;
        blybs = GameObject.FindGameObjectsWithTag("Predator2").Length;
        blubs = GameObject.FindGameObjectsWithTag("ApexPred").Length;
        

                    string timeString = timeToDisplay.ToString();
                    string blibString = blibs.ToString();
                    string blobString = blobs.ToString();
                    string blybString = blybs.ToString();
                    string blubString = blubs.ToString();
                    string camSpeedString = camSpeed.ToString();
                    string timeScaleString = tScale.ToString();
                 //Change the m_Text text to the message below
                 m_Text.text = "t = " + timeString + "\n"  + 
                 "Blibs = " + blibString + "\n" + 
                 "Blobs = " + blobString + "\n" + 
                 "blybs = " + blybString + "\n" + 
                 "Blubs = " + blubString + "\n" + 
                 
                 "camSpeed = " + camSpeedString + "\n" + 
                 "timeScale = " + timeScaleString+"x" ;
                 
                 time = 0f;

        }
    }
}
