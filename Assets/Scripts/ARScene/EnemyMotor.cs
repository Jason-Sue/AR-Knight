using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        anim.Play("die1");
        yield return new WaitForSeconds(2);
        Destroy(transform.gameObject);
    }
}
