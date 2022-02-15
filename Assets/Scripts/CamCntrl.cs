using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamCntrl : MonoBehaviour
{
public float camSpeed = 50f;
private float fixedDeltaTime;

void Awake()
{
    this.fixedDeltaTime = Time.fixedDeltaTime;
}

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()

    {

        
                if (Input.GetKey("w") == true)
        {

            transform.Translate(transform.up * Time.deltaTime * camSpeed);
        }

                if (Input.GetKey("a") == true)
        {

            transform.Translate(-transform.right * Time.deltaTime * camSpeed);
        }

                if (Input.GetKey("s") == true)
        {

            transform.Translate(-transform.up * Time.deltaTime * camSpeed);
        }

                if (Input.GetKey("d") == true)
        {

            transform.Translate(transform.right * Time.deltaTime * camSpeed);
        }

            if (Input.GetKey("escape") == true)
            {
                Application.Quit();
            }

            if(Input.GetKey("r") == true)
            {
                 SceneManager.LoadScene( SceneManager.GetActiveScene().name );
            }

            if(Input.GetKey("up") == true)
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize - 1*camSpeed*Time.deltaTime;
            }
                        if(Input.GetKey("down") == true)
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize + 1*camSpeed*Time.deltaTime;
            }


                        if(Input.GetKey("right") == true)
            {
                camSpeed += Mathf.Round(100f*Time.deltaTime);
            }
                        if(Input.GetKey("left") == true)
            {
                camSpeed -= Mathf.Round(100f*Time.deltaTime);
            }

            if(Input.GetKeyDown("n") == true)
            {
                Time.timeScale -= 0.1f;
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            }
            if(Input.GetKeyDown("m") == true)
            {
                Time.timeScale += 0.1f;
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            }
    }
}
