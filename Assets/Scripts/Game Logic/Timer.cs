using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField]
    Text timerText;
    public bool isTimerOn = false;
    public float timeElapsed = 0;
    
    void Start()
    {
        timerText.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed > 0)
        {
            timerText.text = ConvertAndDisplayTime();
        }
        if (isTimerOn==true)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    public string ConvertAndDisplayTime()
    {
        var hours = timeElapsed / 3600;
        var minutes = (timeElapsed % 3600) / 60;
        var seconds = timeElapsed % 60;

        if (timeElapsed <= 60)
            return Mathf.Round(timeElapsed).ToString() + " s";
        else if (timeElapsed > 60)
        {
            return Mathf.Round( minutes).ToString() + " min " + Mathf.Round(seconds).ToString()+" s";
        }
        else if(timeElapsed>3600)
        {
            return Mathf.Round(hours).ToString()+ " hr "+ Mathf.Round(minutes).ToString() + "min" + Mathf.Round(seconds).ToString() + " s";
        }
        else return "";
    }
    public void StartTimer()
    {
        isTimerOn = true;
    }

    public void StopTimer()
    {
        isTimerOn = false;
    }
    public void ResetTimer()
    {
        timeElapsed = 0;
    }
    public void SetTimer(float t)
    {
        timeElapsed = t;
    }
}
