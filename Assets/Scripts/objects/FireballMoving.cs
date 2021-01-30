using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMoving : MonoBehaviour
{
    Rigidbody2D rigid;

    [SerializeField]
    float Force;

    void Start()
    {
         rigid = GetComponent<Rigidbody2D>();
         Move();
    }
    
    // Update is called once per frame
    void Update() 
    {
        
    }

    void Move()
    {
            rigid.AddForce(new Vector2(0, Force));
            Invoke("Move", 2.00f);
    }
 }
