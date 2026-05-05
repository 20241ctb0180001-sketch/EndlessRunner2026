using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 25;
    private float leftBound = -10;
    [SerializeField] private PlayerController playerController;

    public void Init(PlayerController script)
    {
        playerController = script;
    }
    void Start()
    {
        //playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        if (!PlayerController.IsGameOver())
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);    

            if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
        }
    }
}
