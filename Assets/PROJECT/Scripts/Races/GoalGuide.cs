using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGuide : MonoBehaviour
{
    [SerializeField] GameObject spawnSphere;
    [SerializeField] GameObject initialSpawnSphere;
    [SerializeField] float timeDelay = 2;
    [SerializeField] GameObject container;

    List<Transform> guideSphereList;

    LevelManager myManager;
    bool timerIsRunning = false;

    private void Awake()
    {
        myManager = FindObjectOfType<LevelManager>();
        guideSphereList = new List<Transform>();
    }

    private void Update()
    {
        if (myManager.CurrentGameState == LevelManager.gameState.running)
        {
            if (!timerIsRunning)
            {
                timerIsRunning = true;
                StartCoroutine(spawnTimer());
            }
        }
    }

    private void OnDisable()
    {
        timerIsRunning = false;

    }
    private void OnEnable()
    {

        for (int i = 0; i < guideSphereList.Count; i++)
        {
            if(guideSphereList[i] != null)
            Destroy(guideSphereList[i].gameObject);
        }

        guideSphereList.Clear();
        spawnGameObject(initialSpawnSphere);
    }

    IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(timeDelay);
        spawnGameObject(spawnSphere);

        timerIsRunning = false;
    }

    private void spawnGameObject(GameObject objectToSpawn)
    {
        GameObject newSphere = Instantiate(objectToSpawn, this.transform.position, Quaternion.identity, container.transform);
        newSphere.GetComponent<GoalGuidanceSphere>().SetLevelManager(myManager);
        guideSphereList.Add(newSphere.transform);
    }
}
