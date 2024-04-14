using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TestAnimator : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WalkAndShoot());
    }

    IEnumerator WalkAndShoot()
    {
        yield return new WaitForSeconds(2);

        animator.SetTrigger("Shoot");

        StartCoroutine(WalkAndShoot());
    }
}
