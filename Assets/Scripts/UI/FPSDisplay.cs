using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI FPS;
    private float delayTime = 1f;
    private float time;
    private int frameCount;


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if(time >= delayTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FPS.text = frameRate.ToString() + " FPS";

            time -= delayTime;
            frameCount = 0;
        }
    }
}
