using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public float followSpeed = 1f;
    public float rotateSpeed = 5f;
    public float vfxTime = 0.2f;
    public GameObject rippleVFX, collisionVFX, redVFX, ball;

    public bool vfxPlaying = false, redPlaying = false, ripplePlaying = false;

    public static Controller instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int score = 0;
    public float time = 0;

    private void Awake() 
    {
        instance = this; 
        if(!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Time.timeScale = 1;
        if(PlayerPrefs.HasKey("HighScore"))
        {
            highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement Logic
        // Get the position of the mouse cursor in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.z));

        // Calculate the direction from the object to the mouse cursor
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0; // Ensure that the object is rotating only in the horizontal plane

        // Calculate the angle between the current object rotation and the direction to the mouse cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a quaternion rotation based on the calculated angle
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Rotate the object smoothly towards the mouse cursor
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        // Ensure that the object remains at its original Z position
        mousePosition.z = transform.position.z;

        // Move the object towards the mouse cursor
        transform.position = Vector3.Lerp(transform.position, mousePosition, followSpeed * Time.deltaTime);
        #endregion

        #region Input Logic
        if(Input.GetMouseButton(0))
        {
            StartCoroutine(RippleSound());
            StartCoroutine(PlayEffect(rippleVFX));
        }
        if(Input.GetMouseButtonDown(1))
        {
            if(GameObject.FindObjectOfType<Ball>() != null) return;
            StartCoroutine(RedEffect());
        }
        #endregion
    }

    private IEnumerator PlayEffect(GameObject vfx, bool gameOver = false)
    {
        if(gameOver)
        {
            Time.timeScale = 0.1f;
            yield return new WaitForSeconds(.3f);
            SceneManager.LoadScene(0);
        }
        if(!vfxPlaying)
        {
            vfxPlaying = true;
            Instantiate(vfx,  new Vector3(transform.position.x, transform.position.y, vfx.transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(vfxTime);
            vfxPlaying = false;
        }
    }

    private IEnumerator RedEffect()
    {      
        if(!redPlaying)
        {
            redPlaying = true;
            ParticleSystem tempPS = redVFX.GetComponent<ParticleSystem>();
            tempPS.Play();
            yield return new WaitForSeconds(tempPS.main.duration/2);
            Instantiate(ball,  Vector3.zero, Quaternion.identity);
            yield return new WaitForSeconds(tempPS.main.duration/2);
            tempPS.Stop();        
            redPlaying = false;            
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Ball"))
        {
            StartCoroutine(RippleSound());
            StartCoroutine(PlayEffect(rippleVFX));
            StartCoroutine(PlayEffect(collisionVFX));
        }
        if(other.collider.CompareTag("Wall"))
        {
            StartCoroutine(RippleSound());
            StartCoroutine(PlayEffect(collisionVFX, true));
        }
    }

    private IEnumerator RippleSound()
    {
        if(!ripplePlaying)
        {
            ripplePlaying = true;
            AudioManager.instance.Ripple();
            yield return new WaitForSeconds(1);
            ripplePlaying = false;
        }
    }
}