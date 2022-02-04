using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : MonoBehaviour
{
    [SerializeField] float speed = 2;

    private void Start()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
