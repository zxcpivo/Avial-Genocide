using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class learning : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ExecuteSomething());


    }
    IEnumerator ExecuteSomething()
    {

        yield return new WaitForSeconds(2f);

        Debug.Log("somerthing created");

        Debug.Log("123");

        Debug.Log("hello");
    }
}