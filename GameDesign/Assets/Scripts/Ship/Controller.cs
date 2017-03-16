﻿using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    public static Controller control;
	public double maxSpeed, hydrogen, fuelPerTime;
    public const double maxH = 100;
    private float boostFuel,regFuel, sWidth, guiRatio;
	public float  forceAmount, currentSpeed, thrust, turn, shipRotationSpeed, shipThrust, boostThrust = 1.5f;
	public bool allowMovement;
	public Rigidbody rb;
    private bool full, high, mid, low, empty;

    public GUISkin guiSkin;
    //create a scale Vector3 with the above ratio  
    private Vector3 GUIsF;

    // Use this for initialization
    void Start () {
		hydrogen = 50;
		rb = transform.GetComponent<Rigidbody>();
        //calculating the fuel usage. it should come out to .09/sec
        regFuel = shipThrust / 100000;
        //the boost fuel usage is equal to the regular usage * the boost thrust squared
        boostFuel = regFuel * boostThrust;
	}


    //At this script initialization  
    void Awake()
    {

        
        
        //get the screen's width  
        sWidth = Screen.width;
        //calculate the scale ratio  
        guiRatio = sWidth / 1920;
        //create a scale Vector3 with the above ratio  
        GUIsF = new Vector3(guiRatio, guiRatio, 1);
    }
    void OnGUI()
    {
        //scale and position the GUI element to draw it at the screen's top left corner  
        if (hydrogen>80)
        {
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 45 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            //these labels should all be same
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[4]);
        }else
        {
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 45 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            //these labels should all be same
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[5]);
        }
        if (hydrogen>60)
        {
            //beneath the first bar
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 75 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            //draw GUI on the bottom right  
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[3]);
        }else
        {
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 75 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            //draw GUI on the bottom right  
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[5]);
        }
        if (hydrogen>40)
        {
            //beneath second bar
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 110 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[2]);
        }else
        {
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 110 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[5]);
        }
        if (hydrogen>20)
        {
            //beneath the third
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 145 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[1]);
        }else
        {
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 145 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[5]);
        }
        if (hydrogen > 0)
        {
            //beneath the fourth
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 180 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[0]);
        }else
        {
            GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width - 140 * GUIsF.x, 180 * GUIsF.y, 0), Quaternion.identity, GUIsF);
            GUI.Label(new Rect(0, 0, 100, 20), "", guiSkin.customStyles[5]);
        }

    }

    // Update is called once per frame
    void Update ()
	{
		rb.mass = 1 + (float)(hydrogen/100);
		if (hydrogen > 0) {
			allowMovement = true;
		} else {
			allowMovement = false;
		}

        
		currentSpeed = rb.velocity.magnitude;
		if (allowMovement) {
			if (Input.GetAxis ("Vertical") != 0) {
				thrust = Input.GetAxis ("Vertical") * shipThrust;
                // sutracting used fuel from hydrogen
                
				if (Input.GetKey(KeyCode.LeftShift)) {
					thrust *= boostThrust;
                    hydrogen -= boostFuel;
				}
                hydrogen -= regFuel;
            }
			if (Input.GetAxis ("Horizontal") != 0) {
				turn = Input.GetAxis ("Horizontal") * shipRotationSpeed;
                hydrogen -= (regFuel / 10);
			}
		

			rb.AddForce(thrust * transform.forward * Time.deltaTime);
			rb.AddRelativeTorque(transform.up * turn * Time.deltaTime);
		}
        #region sector switch
        if( transform.position.z <7000 && transform.position.z >-7000 &&transform.position.x >= 7000 )
        {
            loadData.data.secX++;
            transform.position = new Vector3((0 - transform.position.x) + 50, transform.position.y, transform.position.z);
        }else if (transform.position.z < 7000 && transform.position.z > -7000 && transform.position.x <= -7000)
        {
            loadData.data.secX--;
            transform.position = new Vector3((0 - transform.position.x) - 50, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < 7000 && transform.position.x > -7000 && transform.position.z >= 7000)
        {
            loadData.data.secZ++;
            transform.position = new Vector3(transform.position.x, transform.position.y, (0-transform.position.z) + 50);
        }else if (transform.position.x < 7000 && transform.position.x > -7000 && transform.position.z <= 7000)
        {
            loadData.data.secZ--;
            transform.position = new Vector3(transform.position.x, transform.position.y, (0-transform.position.z)-50);
        }
        else if (transform.position.z <=-7000 && transform.position.x <= -7000)
        {
            loadData.data.secX--;
            loadData.data.secZ--;
            transform.position = new Vector3((0 - transform.position.x) - 50, transform.position.y, (0-transform.position.z) -50);
        }
        else if (transform.position.z >= 7000 && transform.position.x >= 7000)
        {
            loadData.data.secX++;
            loadData.data.secZ++;
            transform.position = new Vector3((0 - transform.position.x) + 50, transform.position.y, (0 - transform.position.z) + 50);
        }
        else if (transform.position.z >= 7000 && transform.position.x <= -7000)
        {
            loadData.data.secX--;
            loadData.data.secZ++;
            transform.position = new Vector3((0 - transform.position.x) - 50, transform.position.y, (0 - transform.position.z) + 50);
        }
        else if (transform.position.z <= -7000 && transform.position.x >= 7000)
        {
            loadData.data.secX++;
            loadData.data.secZ--;
            transform.position = new Vector3((0 - transform.position.x) + 50, transform.position.y, (0 - transform.position.z) - 50);
        }
        #endregion
    }

    public bool vacuum ()
	{
        if(hydrogen < maxH)
        {
            hydrogen += .1;
            return true;
        }else
        {
            return false;
        }
	}
    public double getHydrogen()
    {
        return hydrogen;
    }
    
}
