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
        OnGameStart?.Invoke(); // Trigger the event
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
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
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