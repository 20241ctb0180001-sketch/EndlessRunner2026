using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float gravityModifier;
    private bool isOnGround;
    private static bool gameOver;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public int currentLifes;
    [SerializeField] int maxLifes;

    public InputActionAsset InputActions;
    private InputAction jumpAction;

    [SerializeField] HudManager hudManager;

    // Start is called before the first frame update
    private void Awake()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
    }
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        currentLifes = maxLifes;
        hudManager.UpdateLifes(currentLifes);
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
        private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }
    
    void Update()
    {
        if (/*Input.GetKeyDown(KeyCode.Space)*/jumpAction.WasPressedThisFrame() && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            isOnGround = true; 
            dirtParticle.Play();
        } else if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentLifes--;
            hudManager.UpdateLifes(currentLifes);
            if(currentLifes == 0)
            {
                processGameOver();
            }
        }     
    }

    private void processGameOver()
    {
        Debug.Log("Game Over");
        gameOver = true;
        playerAnim.SetInteger("DeathType_int", 1);
        playerAnim.SetBool("Death_b", true);
        dirtParticle.Stop();
        explosionParticle.Play();
    }

    public static bool IsGameOver()
    {
        return gameOver;
    }
}
