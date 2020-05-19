using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float swingingSpeed = 2f;
    public float cooldownSpeed = 2f;
    public float attackDuration = 0.35f;

    public float cooldownDuration = 0.5f;

    private Quaternion targetRotation;
    private float cooldownTimer;
    private bool isAttacking;

    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swingingSpeed);

        cooldownTimer -= Time.deltaTime;
    }
    public void Attack ()
    {
        if (cooldownTimer > 0f)
        {
            return;
        }
        targetRotation = Quaternion.Euler(-90, 0, 0);

        cooldownTimer = cooldownDuration;

        StartCoroutine(CooldownWait());
    }

    private IEnumerator CooldownWait()
    {
        yield return new WaitForSeconds(attackDuration);

        targetRotation = Quaternion.Euler(0, 0, 0);
    }
}
