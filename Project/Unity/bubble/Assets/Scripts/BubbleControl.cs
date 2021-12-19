using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BubbleControl : MonoBehaviour
{
    ParticleSystem bubble;
    public float speedValue = 1.0f;

    GameObject Bubbles;
    public SerialController serialController;
    int num = 0;


    // Start is called before the first frame update
    void Start()
    {
        Bubbles = GameObject.Find("Bubbles");
        serialController = Bubbles.GetComponent<SerialController>();

        bubble = GetComponent<ParticleSystem>();
    }


    // Convert string to int
    public static int IntParseFast(string message)
    {
        int num = 0;
        for (int i = 0; i < message.Length; i++)
        {
            char letter = message[i];
            num = 10 * num + (letter - 48);
        }
        return num;
    }


    // Update is called once per frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log("Message arrived: " + message);

        num = IntParseFast(message);


        var main = bubble.main;
        main.simulationSpeed = speedValue;
        speedValue = 0;

        var emission = bubble.emission;
        emission.rateOverTime = 0;


        bubble.Stop();
        if (num >= 400)
        {
            bubble.Play();
            speedValue = 1;
        }
        else
            bubble.Pause();


        // Control the number of bubbles & their speed
        if (num >= 450)
        {
            emission.rateOverTime = 3;
            if (num >= 800)
            {
                emission.rateOverTime = 6;
                if (num >= 1000)
                {
                    main.simulationSpeed = 2 * speedValue;
                    emission.rateOverTime = 12;
                    if (num >= 1500)
                    {
                        main.simulationSpeed = 3 * speedValue;
                        emission.rateOverTime = 18;
                    }
                }
            }
        }


        var sz = bubble.sizeOverLifetime;
        sz.enabled = true;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.0f);

        if (message.StartsWith("button"))
        {
            sz.size = new ParticleSystem.MinMaxCurve(0.0f, curve);
            bubble.Stop();
        }
    }
}