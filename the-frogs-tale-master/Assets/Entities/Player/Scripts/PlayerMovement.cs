using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private float velocity;
    public float runSpeed = 40f;
    private Vector2 horizontalMove;
    private Vector2 verticalMove;
    private bool lastSide; // false: left   true: right
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = new Vector2(Input.GetAxisRaw("Horizontal"),0).normalized * runSpeed;
        verticalMove = new Vector2(Input.GetAxisRaw("Vertical"), 0).normalized * runSpeed;
        
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove.magnitude * runSpeed));
        animator.SetFloat("Speed2", Mathf.Abs(verticalMove.magnitude * runSpeed));
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            lastSide = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            lastSide = true;
        }

        bool flipped = horizontalMove.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f,0f));
        float x, y;
        
        
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(x * velocity, y * velocity);

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, lastSide ? 360f : 180f, 0f));

    }

}
