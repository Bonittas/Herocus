using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingBIrd : MonoBehaviour
{
    public float speed = 5f;
    public float detectionRange = 10f; // Distance at which the bird starts flying
    public float flyDistance = 20f;    // Distance the bird will fly before disappearing
    [SerializeField] private float playerDamageAmount = 5f;

    private bool isFlying = false;
    private float distanceTraveled = 0f;
    private Vector3 startPosition;
    private Transform playerTransform;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPosition = transform.position; 
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= detectionRange)
            {
                StartFlying();
            }
        }

        if (isFlying)
        {
            float movementThisFrame = speed * Time.deltaTime;
            transform.Translate(Vector2.left * movementThisFrame);
            distanceTraveled += movementThisFrame;

            if (distanceTraveled >= flyDistance)
            {
                Destroy(gameObject);
            } 
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.HealthDamage(playerDamageAmount); 
            }
        }
    }

    private void StartFlying() 
    {
        if (!isFlying)
        {
            isFlying = true;
            startPosition = transform.position;  
            distanceTraveled = 0f;               
        }
    }
}