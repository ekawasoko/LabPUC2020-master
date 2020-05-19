using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject model;
    public float rotatingSpeed = 2f;
    [Header("Movement")]
    public float movingVelocity;
    public float jumpingVelocity;
    public float knockbackVelocity;

    [Header("Equipment")]
    public int health = 3;
    public Sword sword;
    public Bow bow;
    public int arrowAmount = 15;
    public GameObject bombPrefab;
    public float throwingSpeed;
    public int bombAmount = 5;


    private Rigidbody playerRigidBody;
    private bool canJump;
    private Quaternion targetModelRotation;

    // Start is called before the first frame update
    void Start()
    {
        bow.gameObject.SetActive(false);

        playerRigidBody = GetComponent<Rigidbody>();
        targetModelRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        playerRigidBody.velocity = new Vector3
            (0,
            playerRigidBody.velocity.y,
            0
            );

        if (Input.GetKey("d"))
        {
            playerRigidBody.velocity = new Vector3(
                movingVelocity,
                playerRigidBody.velocity.y,
                playerRigidBody.velocity.z
                );
            targetModelRotation = Quaternion.Euler(0, 270, 0);

        }
        if (Input.GetKey("a"))
        {
            playerRigidBody.velocity = new Vector3(
                -movingVelocity,
                playerRigidBody.velocity.y,
                playerRigidBody.velocity.z
        );
            targetModelRotation = Quaternion.Euler(0, 90, 0);

        }
        if (Input.GetKey("w"))
        {
            playerRigidBody.velocity = new Vector3(
            playerRigidBody.velocity.x,
            playerRigidBody.velocity.y,
            movingVelocity);

            targetModelRotation = Quaternion.Euler(0, 180, 0);

        }
        if (Input.GetKey("s"))
        {
            playerRigidBody.velocity = new Vector3(
            playerRigidBody.velocity.x,
            playerRigidBody.velocity.y,
            -movingVelocity);

            targetModelRotation = Quaternion.Euler(0, 0, 0);

        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.01f));
            {
                canJump=true;
            }

        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, targetModelRotation, Time.deltaTime * rotatingSpeed);

        if (canJump && Input.GetKeyDown("space"))
        {
            canJump = false;
            playerRigidBody.velocity = new Vector3(
                playerRigidBody.velocity.x,
                jumpingVelocity,
                playerRigidBody.velocity.z);
        }
        if(Input.GetKeyDown("g"))
        {
            ThrowBomb();
        }

        if (Input.GetKeyDown("z"))
        {
            sword.gameObject.SetActive(true);
            bow.gameObject.SetActive(false);
            sword.Attack();
        }

        if(Input.GetKeyDown("c"))
        {
            if(arrowAmount > 0)
            {
                sword.gameObject.SetActive(false);
                bow.gameObject.SetActive(true);

                bow.Attack();
                arrowAmount--;
            }
            
        }

    }
    private void ThrowBomb()
    {
        if (bombAmount <= 0)
        {
            return;
        }
        GameObject bombObject = Instantiate(bombPrefab);
        bombObject.transform.position = transform.position + model.transform.forward;

        Vector3 throwingDirection = (model.transform.forward + Vector3.up).normalized;
        bombObject.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed);

        bombAmount--;
    }

    void OnTriggerEnter (Collider otherCollider)
    {
        if (otherCollider.GetComponent<EnemyBullet>() != null)
        {
            Hit((transform.position - otherCollider.transform.position).normalized);
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Hit((transform.position - collision.transform.position).normalized);
        }
    }
    private void Hit(Vector3 direction)
    {
        Vector3 knockbackDirection = (direction + Vector3.up).normalized;
        playerRigidBody.AddForce (knockbackDirection * knockbackVelocity);

        health--;
        if(health <= 0)
        {
            Destroy(gameObject);

        }
    }
}
