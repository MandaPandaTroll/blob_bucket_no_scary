        using System.Collections.Generic;
        using UnityEngine;

        /// <summary>
        ///Population graph
        /// Original code by
        /// Michael Hutton May 2020
        ///
        ///Modifications: tabacwoman November 2021
        /// </summary>
        public class Graph : MonoBehaviour
        {
            float e;
           public List <GameObject> blobList;
            GameObject[] blobs;

            GameObject[] blybs;
            public List <GameObject> blybList; 

            public  List <GameObject> blibList;

            public GameObject[] blibs;

            GameObject[] blubs;

            public List <GameObject> blubList; 
            Material mat;
            private Rect windowRect = new Rect(20, 20, 2048, 512);

            // A list of random values to draw
            private List<float> values;
            private List<float> values2;

            private List<float> values3;
            private List<float> values4;
            private List<float> values5;

            // The list the drawing function uses...
            private List<float> drawValues = new List<float>();
            private List<float> drawValues2 = new List<float>();
            private List<float> drawValues3 = new List<float>();
            private List<float> drawValues4 = new List<float>();
            private List<float> drawValues5 = new List<float>();

            // List of Windows
            private bool showWindow0 = false;
           int GraphScaler = 1;
           

            // Start is called before the first frame update
            void Start()
            {

                
                e = Mathf.Exp(1);
                mat = new Material(Shader.Find("Hidden/Internal-Colored"));
                // Should check for material but I'll leave that to you..

                // Fill a list with ten random values
                values = new List<float>();
                for (int i = 0; i < 10; i++)
                {
                    values.Add(Random.value);
                }

                values2 = new List<float>();
                    for (int i = 0; i < 10; i++)
                {
                    values2.Add(Random.value);
                }

                    values3 = new List<float>();
                    for (int i = 0; i < 10; i++)
                {
                    values3.Add(Random.value);
                }

                    values4 = new List<float>();
                    for (int i = 0; i < 10; i++)
                {
                    values4.Add(Random.value);
                }

                    values5 = new List<float>();
                    for (int i = 0; i < 10; i++)
                {
                    values5.Add(Random.value);
                }
            }

            int dbugCounter;
            void Update()
            {
                dbugCounter +=1;

               
                blubs = GameObject.FindGameObjectsWithTag("ApexPred");
                 blubList = new List <GameObject>(blubs);
               float blubCount = (float)blubList.Count;

                
                blobs = GameObject.FindGameObjectsWithTag("Predator");
                 blobList = new List <GameObject>(blobs);
               float blobCount = (float)blobList.Count;

                blybs = GameObject.FindGameObjectsWithTag("Predator2");
                 blybList = new List <GameObject>(blybs);
               float blybCount = (float)blybList.Count;

                blibs = GameObject.FindGameObjectsWithTag("Prey");
                 blibList = new List <GameObject>(blibs);
               float blibCount = (float)blibList.Count;
                float totalPop = blibCount + blobCount + blybCount + blubCount;
                 
                float scaledTotalPop = (Mathf.Log(totalPop+1,2)*windowRect.height/16f)-1f;
                //a + ( x - min(x) )*(b-a) / ( max(x)-min(x) ) 
                float scaledBlib = (Mathf.Log(blibCount+1,2)*windowRect.height/16f)-1f;
                float scaledBlob = (Mathf.Log(blobCount+1,2)*windowRect.height/16f)-1f;
                float scaledBlyb = (Mathf.Log(blybCount+1,2)*windowRect.height/16f)-1f;
                float scaledBlub = (Mathf.Log(blubCount+1,2)*windowRect.height/16f)-1f;
                
                
                
                // Keep adding values
                values.Add(scaledBlob);
                values2.Add(scaledBlib);
                values3.Add(scaledBlub);
                values4.Add(scaledBlyb);
                values5.Add(scaledTotalPop);
               
                if (dbugCounter >= 50){
                    Debug.Log(" scaledTotalPop "+scaledTotalPop+" scaledBlob "+scaledBlob+" scaledBlib "+scaledBlib+" scaledBlub "+scaledBlub+" scaledBlyb "+scaledBlyb);
                    dbugCounter = 0;
                }
            }

            private void OnGUI()
            {
                // Create a GUI.toggle to show graph window
                showWindow0 = GUI.Toggle(new Rect(10, 10, 100, 20), showWindow0, "Show N(t)");

                if (showWindow0)
                {
                    // Set out drawValue list equal to the values list 
                    drawValues = values;
                    drawValues2 = values2;
                    drawValues3 = values3;
                    drawValues4 = values4;
                    drawValues5 = values5;
                    windowRect = GUI.Window(0, windowRect, DrawGraph, "");
                }

            }


            void DrawGraph(int windowID)
            { 
                // Make Window Draggable
                GUI.DragWindow(new Rect(0, 0, 10000, 10000));

                // Draw the graph in the repaint cycle
                if (Event.current.type == EventType.Repaint)
                {
                    GL.PushMatrix();

                    GL.Clear(true, false, Color.black);
                    mat.SetPass(0);

                    // Draw a gray background Quad 
                    GL.Begin(GL.QUADS);
                    GL.Color(Color.black);
                    GL.Vertex3(4, 4, 0);
                    GL.Vertex3(windowRect.width - 4, 4, 0);
                    GL.Vertex3(windowRect.width - 4, windowRect.height - 4, 0);
                    GL.Vertex3(4, windowRect.height - 4, 0);



                    
                    GL.End();

                 
                    //Vertical Lines
                    GL.Begin(GL.LINES);
                    GL.Color(Color.gray);

                    GL.Vertex3(windowRect.width*(1f/8f), 4, 0);
                    GL.Vertex3(windowRect.width*(1f/8f), windowRect.height - 4, 0);

                    GL.Vertex3(windowRect.width*(2f/8f), 4, 0);
                    GL.Vertex3(windowRect.width*(2f/8f), windowRect.height - 4, 0);

                    GL.Vertex3(windowRect.width*(3f/8f), 4, 0);
                    GL.Vertex3(windowRect.width*(3f/8f), windowRect.height - 4, 0);

                    GL.Vertex3(windowRect.width*(4f/8f), 4, 0);
                    GL.Vertex3(windowRect.width*(4f/8f), windowRect.height - 4, 0);

                    GL.Vertex3(windowRect.width*(5f/8f), 4, 0);
                    GL.Vertex3(windowRect.width*(5f/8f), windowRect.height - 4, 0);

                    GL.Vertex3(windowRect.width*(6f/8f), 4, 0);
                    GL.Vertex3(windowRect.width*(6f/8f), windowRect.height - 4, 0);

                    GL.Vertex3(windowRect.width*(7f/8f), 4, 0);
                    GL.Vertex3(windowRect.width*(7f/8f), windowRect.height - 4, 0);

                    //Horizontal lines
                    GL.Vertex3(4, windowRect.height*(1f/8f), 0);
                    GL.Vertex3(windowRect.width-4, windowRect.height*(1f/8f), 0);

                    GL.Vertex3(4, windowRect.height*(2f/8f), 0);
                    GL.Vertex3(windowRect.width-4, windowRect.height*(2f/8f), 0);

                    GL.Vertex3(4, windowRect.height*(3f/8f), 0);
                    GL.Vertex3(windowRect.width-4, windowRect.height*(3f/8f), 0);

                    GL.Vertex3(4, windowRect.height*(4f/8f), 0);
                    GL.Vertex3(windowRect.width-4, windowRect.height*(4f/8f), 0);

                    GL.Vertex3(4, windowRect.height*(5f/8f), 0);
                    GL.Vertex3(windowRect.width-4, windowRect.height*(5f/8f), 0);

                    GL.Vertex3(4, windowRect.height*(6f/8f), 0);
                    GL.Vertex3(windowRect.width-4, windowRect.height*(6f/8f), 0);

                    GL.Vertex3(4, windowRect.height*(7f/8f), 0);
                    GL.Vertex3(windowRect.width-4, windowRect.height*(7f/8f), 0);



                    GL.End();
                   
                    // Draw the lines of the graph
                    GL.Begin(GL.LINES);
                    GL.Color(Color.magenta);

                    int valueIndex = drawValues.Count - 1;
                    for (int i = (int)windowRect.width - 4; i > 3; i--)
                    {
                        float y1 = 0;
                        float y2 = 0;
                        if (valueIndex > 0)
                        {
                            y2 = drawValues[valueIndex];
                            y1 = drawValues[valueIndex - 1];
                        }
                        GL.Vertex3(i, windowRect.height - 4 - y2, 0);
                        GL.Vertex3((i - 1), windowRect.height - 4 - y1, 0);
                        valueIndex -= 1;
                    }
                    GL.End();

                    

                    GL.Begin(GL.LINES);
                    GL.Color(Color.green);

                    int valueIndex2 = drawValues2.Count - 1;
                    for (int i = (int)windowRect.width - 4; i > 3; i--)
                    {
                        float y1 = 0;
                        float y2 = 0;
                        if (valueIndex2 > 0)
                        {
                            y2 = drawValues2[valueIndex2];
                            y1 = drawValues2[valueIndex2 - 1];
                        }
                        GL.Vertex3(i, windowRect.height - 4 - y2, 0);
                        GL.Vertex3((i - 1), windowRect.height - 4 - y1, 0);
                        valueIndex2 -= 1;
                    }
                    GL.End();

                    GL.Begin(GL.LINES);
                    GL.Color(Color.red);

                    int valueIndex3 = drawValues3.Count - 1;
                    for (int i = (int)windowRect.width - 4; i > 3; i--)
                    {
                        float y1 = 0;
                        float y2 = 0;
                        if (valueIndex3 > 0)
                        {
                            y2 = drawValues3[valueIndex3];
                            y1 = drawValues3[valueIndex3 - 1];
                        }
                        GL.Vertex3(i, windowRect.height - 4 - y2, 0);
                        GL.Vertex3((i - 1), windowRect.height - 4 - y1, 0);
                        valueIndex3 -= 1;
                    }
                    GL.End();

                    GL.Begin(GL.LINES);
                    GL.Color(Color.yellow);

                    int valueIndex4 = drawValues4.Count - 1;
                    for (int i = (int)windowRect.width - 4; i > 3; i--)
                    {
                        float y1 = 0;
                        float y2 = 0;
                        if (valueIndex4 > 0)
                        {
                            y2 = drawValues4[valueIndex4];
                            y1 = drawValues4[valueIndex4 - 1];
                        }
                        GL.Vertex3(i, windowRect.height - 4 - y2, 0);
                        GL.Vertex3((i - 1), windowRect.height - 4 - y1, 0);
                        valueIndex4 -= 1;
                    }
                    GL.End();

                    GL.Begin(GL.LINES);
                    GL.Color(Color.white);

                    int valueIndex5 = drawValues5.Count - 1;
                    for (int i = (int)windowRect.width - 4; i > 3; i--)
                    {
                        float y1 = 0;
                        float y2 = 0;
                        if (valueIndex5 > 0)
                        {
                            y2 = drawValues5[valueIndex5];
                            y1 = drawValues5[valueIndex5 - 1];
                        }
                        GL.Vertex3(i, windowRect.height - 4 - y2, 0);
                        GL.Vertex3((i - 1), windowRect.height - 4 - y1, 0);
                        valueIndex5 -= 1;
                    }
                    GL.End();


                    GL.PopMatrix();
                    
                    
                }
            }
        }