using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Android;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    public delegate void StartTouchEvent(Vector2 touchPos, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 touchPos, float time);
    public event EndTouchEvent OnEndTouch;

    private TouchControls touchControls;

    public SpriteRenderer spriteRenderer;

    public float countdownTime = 3;
    private float countdownStart;

    public float timeRemaining = 5;
    private float timeStart;

    public bool buttonHasBeenPressed = false;

    string filename = "";

    [System.Serializable]
    public class AccelerometerDatapoints
    {
        public Vector3 dir;
    }

    public AccelerometerDatapoints accelerometerDatapoints;
    private void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        touchControls.Touch.ButtonPress.started += ctx => StartTouch(ctx);
        touchControls.Touch.ButtonPress.canceled += ctx => EndTouch(ctx);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        filename = Application.dataPath + "/test.csv";
        timeStart = timeRemaining;
        countdownStart = countdownTime;
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("datapoints for x-axis , datapoints for y-axis , datapoints for z-axis");
        tw.Close();
    }

    void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Eow knappen er trykket" + context.ReadValue<float>());
    }
    void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Eow du slap knappen");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (countdownTime <= 0) //This would be "buttonHasBeenPressed" instead when using a button
        {
            if (timeRemaining > 0)
            {
                spriteRenderer.color = Color.green;
                
                //accelerometerDatapoints.dir.x = Input.acceleration.x;
                //accelerometerDatapoints.dir.y = Input.acceleration.y;
                //accelerometerDatapoints.dir.z = Input.acceleration.z;

                accelerometerDatapoints.dir = Input.acceleration;

                TextWriter tw = new StreamWriter(filename, true);
                tw.WriteLine(accelerometerDatapoints.dir.x + "," + accelerometerDatapoints.dir.y + "," + accelerometerDatapoints.dir.z);
                tw.Close();
                timeRemaining -= Time.deltaTime;
            }
            else
            {

                //countdownTime = countdownStart;
                //timeRemaining = timeStart;
                spriteRenderer.color= Color.red;
            }
        }
        else
        {
            countdownTime -= Time.deltaTime;
        }

    }

    //OnClick() Button input wouldn't work with Remote 5, so I used a timer with a countdown instead
    public void AccelerometerMeasurement()
    {
        buttonHasBeenPressed = true;
    }




}
