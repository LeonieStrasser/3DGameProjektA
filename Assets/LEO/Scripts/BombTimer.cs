using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

public class BombTimer : MonoBehaviour
{
    [SerializeField] float maxTime = 10f;
    [Tooltip("Unterhalb dieser Geschwindigkeit blinkt der Timer Rot. Achtung! " +
        "Das muss im Animator per Hand nachgestellt werden!")]
    [SerializeField] float speedLimit = 150f;
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
            float roundedValue = Mathf.Round(value * 100.0f) * 0.01f;
            textMP.text = roundedValue.ToString();
            uiAnim.SetFloat("Magnitude", myRigidbody.velocity.magnitude);
        }

    }
    Rigidbody myRigidbody;
    [SerializeField] bool bombIsActive;


    [SerializeField] TextMeshProUGUI textMP;
    [SerializeField] Animator uiAnim;
    [SerializeField] GameObject bombObject;
    [SerializeField] ParticleSystem bombVFX1;
    [SerializeField] ParticleSystem bombVFX2;

    List<Vector3> collectableSpawnPoints;
    [SerializeField] GameObject collectiblePrefab;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        collectableSpawnPoints = new List<Vector3>();
    }
    private void Start()
    {
        SpawnPoint[] allSpawnpoints = FindObjectsOfType<SpawnPoint>();
        foreach (var item in allSpawnpoints)
        {
            collectableSpawnPoints.Add(item.gameObject.transform.position);
        }

        Timer = maxTime;
        StopBomb();
    }

    private void Update()
    {
        if (bombIsActive)
        {
            RunBombTimer();
            RunBombVFX();
        }
    }

    void RunBombTimer()
    {
        Timer -= Time.deltaTime / myRigidbody.velocity.magnitude; // Wenn die Geschwindigkeit hoch ist, wird weniger abgezogen
        Timer = Mathf.Clamp(Timer, 0, maxTime);
    }

    void RunBombVFX()
    {
        if (myRigidbody.velocity.magnitude < speedLimit)
        {
            bombVFX1.gameObject.SetActive(true);
        }else
        {
            bombVFX1.gameObject.SetActive(false);
        }
    }

    void spawnNextBomb()
    {
        int x = Random.Range(0, collectableSpawnPoints.Count);
        Vector3 spawnPosition = collectableSpawnPoints[x];
        Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
    }

    public void StopBomb()
    {
        textMP.gameObject.SetActive(false);
        bombObject.SetActive(false);
        bombIsActive = false;
        Timer = maxTime;
        spawnNextBomb();
    }

    public void PickUpBomb()
    {
        Timer = maxTime;
        textMP.gameObject.SetActive(true);
        bombObject.SetActive(true);
        bombIsActive = true;
    }

}
