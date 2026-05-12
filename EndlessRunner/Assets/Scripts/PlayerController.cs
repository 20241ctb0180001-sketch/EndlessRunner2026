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

    [SerializeField] InputActionAsset inputActions;
    private InputAction jumpAction;

    public Renderer rend;
    public Color flash = Color.red;
    public float flashDuration = 0.5f;
    private Color originalColor;

    [SerializeField] HudManager hudManager;

    // Start is called before the first frame update
    private void Awake()
    {
        jumpAction = inputActions.FindAction("Jump");
    }
    void Start()
    {
        originalColor = rend.material.color;
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        currentLifes = maxLifes;
        hudManager.UpdateLifes(currentLifes);
    }

    private void OnEnable()
    {
        jumpAction.Enable();
    }
        private void OnDisable()
    {
        jumpAction.Disable();
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
            StartCoroutine(FlashRed());
            hudManager.UpdateLifes(currentLifes);
            if(currentLifes == 0)
            {
                //processGameOver();
                StartCoroutine(Game0(2.5f));
            }
        }     
    }

    /*private void processGameOver()
    {
        Debug.Log("Game Over");
        gameOver = true;
        playerAnim.SetInteger("DeathType_int", 1);
        playerAnim.SetBool("Death_b", true);
        dirtParticle.Stop();
        explosionParticle.Play();
        QuitApplication();
    }*/

    public static bool IsGameOver()
    {
        return gameOver;
    }

    private IEnumerator Game0(float wait)
    {
        Debug.Log("Game Over");
        gameOver = true;
        playerAnim.SetInteger("DeathType_int", 1);
        playerAnim.SetBool("Death_b", true);
        dirtParticle.Stop();
        explosionParticle.Play();
        yield return new WaitForSeconds(wait);
        QuitApplication();
    }

    private IEnumerator FlashRed()
    {
        rend.material.color = flash;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
    }
    public void QuitApplication()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
