using System.Collections;
using UnityEngine;

public class StillBird : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));

        GetComponent<Animator>().enabled = true;
    }
}