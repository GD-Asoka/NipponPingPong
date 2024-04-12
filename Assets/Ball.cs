using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnEnable() 
    {
        Controller.instance.score = 0;
        Controller.instance.time = 0;
    }

    private void OnDisable() 
    {
        
            if(Controller.instance.score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", Controller.instance.score);
                Controller.instance.highScoreText.text = Controller.instance.score.ToString();
            }
        
        
            
        
    }

    private void Update() 
    {        
        Controller.instance.scoreText.text = Controller.instance.score.ToString();
        Controller.instance.time += Time.deltaTime;
        Controller.instance.score = (int)Controller.instance.time;
        GetComponent<Rigidbody2D>().gravityScale += Time.deltaTime * 0.01f;
        GetComponent<Rigidbody2D>().mass += Time.deltaTime * 0.01f;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.CompareTag("Wall"))
        {
            AudioManager.instance.Ripple();
            gameObject.SetActive(false);
        }
    }
}
