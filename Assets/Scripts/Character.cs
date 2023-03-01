using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string CharacterName = "Kaelen";
    public GameObject character;

    [Header("Movement")]
    public Vector3Int position;
    public float speed = 10.0f;
    public float jumpSpeed = 2.0f;
    private Vector2 playerInput;
    public bool canJump;
    public bool shouldJump;
    private int doubleJump=1;

    Rigidbody2D r2d;
    // Start is called before the first frame update
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown("space") && canJump && doubleJump>0)
        {
            r2d.transform.Translate(Vector3.up * jumpSpeed * Time.fixedDeltaTime * 10);
            doubleJump-=1;
        }
       
    }
    void FixedUpdate() {
        r2d.MovePosition(transform.position + (new Vector3(Input.GetAxisRaw("Horizontal"), 0) )* Time.fixedDeltaTime * speed); 

         

    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        canJump=true;
        doubleJump=2;
           
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        
        

    }

}
