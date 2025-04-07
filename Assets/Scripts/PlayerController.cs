using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jumpForce = 5f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;


    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    private int jumpCount = 0;
    private bool isGround = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue) 
    { 
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    void OnJump() {
        if (isGround || jumpCount < 2) {

            rb.angularVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
            isGround = false;

        }
    }

    void SetCountText() { 
        countText.text = "Count: " + count.ToString();
        if (count >= 10) {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);


        rb.AddForce(movement * speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText() ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) { 
            isGround = true;    
            jumpCount = 0; 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            isGround = false;
        }
    }
}
