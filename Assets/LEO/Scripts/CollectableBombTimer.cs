using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CollectableBombTimer : MonoBehaviour
{
    [SerializeField] int collectTime = 10;
    private float timer;
    public float Timer
    {
        get
        {
            return timer;
        }
        private set
        {
            timer = value;
            float roundedValue = Mathf.RoundToInt(value);
            textMP.text = UItext + " " + roundedValue;
        }

    }
    bool collectableIsWaiting;
    [SerializeField] TextMeshProUGUI textMP;
    [SerializeField] string UItext;

    private void Start()
    {
        timer = collectTime;
        StartTimer();
    }

    private void Update()
    {
        if (collectableIsWaiting)
            RunCollectTimer();
    }

    void RunCollectTimer()
    {
        Timer -= Time.deltaTime;
        Timer = Mathf.Clamp(Timer, 0, collectTime);
    }

    public void ResetTimer()
    {
        Timer = collectTime;
        textMP.gameObject.SetActive(false);
        collectableIsWaiting = false;
    }

    public void StartTimer()
    {
        textMP.gameObject.SetActive(true);
        collectableIsWaiting = true;
    }
}
