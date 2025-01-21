using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public GameObject winTextObject;
    private bool gameStarted = false;
    public event System.Action OnGameStart;
    public bool GameStarted
    {
        get { return gameStarted; }
    }


    void Start()
    {
        winTextObject.SetActive(false);
        count = 0;
        rb = GetComponent <Rigidbody>();
        SetCountText();
    }
    void OnMove (InputValue movementValue)
    {
        if (!gameStarted)
        {
        gameStarted = true;
        OnGameStart?.Invoke();
        }
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    void SetCountText()
    {
        countText.text = "Count: "+count.ToString();
    }
    private void FixedUpdate()
    {
        if (gameStarted)
        {
            // Get the camera's forward and right directions
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            // Project forward and right directions onto the horizontal plane (y = 0)
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Calculate the movement direction based on camera orientation
            Vector3 movement = forward * movementY + right * movementX;

            rb.AddForce(movement * speed);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            if (count >= 15)
                {
                    winTextObject.SetActive(true);
                    Destroy(GameObject.FindGameObjectWithTag("Enemy"));
                }
        }
    }
    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MiniGame");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            StartCoroutine(RestartGameAfterDelay(1.0f));
        
        }
    }
}