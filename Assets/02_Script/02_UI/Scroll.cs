using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 20;
    [SerializeField] private string stageName;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
        if (transform.position.y >= 500f)
        {
            SceneManager.LoadScene(stageName);
        }
    }

    public void SkipBtnClick()
    {
        SceneManager.LoadScene(stageName);
    }
}
