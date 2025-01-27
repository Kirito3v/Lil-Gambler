using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class Player : MonoBehaviour
{
    // components
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anime;
    [SerializeField] private Collider2D coll;

    // layers
    [SerializeField] private LayerMask Ground;

    // inputs
    [SerializeField] private InputActionReference act;

    // movement
    [SerializeField] private float speed = 0f;
    private float movementX;
    private float movementY;

    // animation
    private enum State { idle, walk}
    private State state = State.idle;

    // init values
    public int Score = 0;
    public int Bet = 0;
    bool isJackPot = false;
    bool isBlackJack = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        act.action.performed += ActionPressF;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        SetState();
        anime.SetInteger("State", (int)state);
    }

    // when press F
    private void ActionPressF(InputAction.CallbackContext obj) 
    {
        if (isJackPot) 
        {
            SceneManager.LoadScene("JackPot");
        }
        else if (isBlackJack) 
        {
            SceneManager.LoadScene("BlackJack");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JackPot")
        {
            isJackPot = true;
            act.action.Enable();
        }
        else if (collision.gameObject.tag == "BlackJack")
        {
            isBlackJack = true;
            act.action.Enable();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JackPot")
        {
            isJackPot = false;
            act.action.Disable();
        }
        else if (collision.gameObject.tag == "BlackJack")
        {
            isBlackJack = false;
            act.action.Disable();
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "JackPot")
    //    {
    //        isJackPot = true;
    //        act.action.Enable();
    //    }
    //    else if (collision.gameObject.tag == "BlackJack")
    //    {
    //        isBlackJack = true;
    //        act.action.Enable();
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "JackPot")
    //    {
    //        isJackPot = false;
    //        act.action.Disable();
    //    }
    //    else if (collision.gameObject.tag == "BlackJack")
    //    {
    //        isBlackJack = false;
    //        act.action.Disable();
    //    }
    //}

    // movement input
    private void OnMove(InputValue MV)
    {
        Vector2 move = MV.Get<Vector2>();

        movementX = move.x;
        movementY = move.y;
    }
    
    // movement controll
    void Movement()
    {
        Vector2 movement = new Vector2(movementX, movementY);

        if (movementX == -1)
        {
            transform.localScale = new Vector2(-1f, 1f);
            //rb.AddForce(movement * speed);
        }
        if (movementX == 1)
        {
            transform.localScale = new Vector2(1f, 1f);
            //rb.velocity = movement * Time.deltaTime * speed;
            //rb.AddForce(movement * speed);
        }
            rb.velocity = Vector2.Lerp(rb.velocity,new Vector2( movement.x * speed,rb.velocity.y), 0.1f);
    }

    // animation states
    private void SetState()
    {
        if (Mathf.Abs(movementX) > Mathf.Epsilon && coll.IsTouchingLayers(Ground))
        {
            state = State.walk;
        }
        else
        {
            state = State.idle;
        }
    }
}
