using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2;
    private float repeatRate = 2;
    [SerializeField] PlayerController playerController;
    void Start()
    {
        //playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstable", startDelay, repeatRate);
    }

    void SpawnObstable ()
    {
        if (!PlayerController.IsGameOver()) {
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
            MoveLeft moveLeftScript = obstacle.GetComponent<MoveLeft>();
            moveLeftScript.Init(playerController);
            //Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }
    } 
}
