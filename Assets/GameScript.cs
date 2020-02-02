using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    public GameObject enemy;
    public float maxEnemyDelay = 10;
    public float minEnemyDelay = 1;
    public int repairLength = 10;
    public float maxEnemyDelayEasy = 10;    
    public float minEnemyDelayEasy = 1;
    public int repairLengthEasy = 10;
    public float maxEnemyDelayHard = 10;
    public float minEnemyDelayHard = 1;
    public int repairLengthHard = 10;
    public GameObject perso;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject looseMenu;
    public GameObject beginMenu;
    public GameObject bigCloud1;
    public GameObject bigCloud2;
    public GameObject cloud1;
    public GameObject cloud2;
    public GameObject cloud3;
    public float bigCloud1Speed = 1;
    public float bigCloud2Speed = 1;
    public float cloud1Speed = 1;
    public float cloud2Speed = 1;
    public float cloud3Speed = 1;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        winMenu.gameObject.SetActive(false);
        looseMenu.gameObject.SetActive(false);
        Time.timeScale = 0;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > minEnemyDelay + maxEnemyDelay * (perso.GetComponent<PersoScript>().repairCompletion - perso.GetComponent<PersoScript>().repairProgression) / perso.GetComponent<PersoScript>().repairCompletion)
        {
            Instantiate(enemy, new Vector3((Random.value > 0.5 ? 7.5f : -7.5f), (Random.value > 0.5 ? 2f : -2f), 0f), Quaternion.identity);
            timer = 0;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || (Time.timeScale != 0 &&
            (perso.GetComponent<PersoScript>().repairProgression >= perso.GetComponent<PersoScript>().repairCompletion ||
            perso.GetComponent<PersoScript>().life <= 0)))
        {
            Pause();
        }
        bigCloud1.transform.Translate(Vector2.right * bigCloud1Speed * Time.deltaTime);
        bigCloud2.transform.Translate(Vector2.right * bigCloud2Speed * Time.deltaTime);
        cloud1.transform.Translate(Vector2.right * cloud1Speed * Time.deltaTime);
        cloud2.transform.Translate(Vector2.right * cloud2Speed * Time.deltaTime);
        cloud3.transform.Translate(Vector2.right * cloud3Speed * Time.deltaTime);
        if(bigCloud1.transform.localPosition.x > 12 || bigCloud1.transform.localPosition.x < -12)
        {
            bigCloud1.transform.localPosition = new Vector2(bigCloud1Speed / Mathf.Abs(bigCloud1Speed) * -12, bigCloud1.transform.localPosition.y);
        }
        if (bigCloud2.transform.localPosition.x > 12 || bigCloud2.transform.localPosition.x < -12)
        {
            bigCloud2.transform.localPosition = new Vector2(bigCloud2Speed / Mathf.Abs(bigCloud2Speed) * -12, bigCloud2.transform.localPosition.y);
        }
        if (cloud1.transform.localPosition.x > 10 || cloud1.transform.localPosition.x < -10)
        {
            cloud1.transform.localPosition = new Vector2(cloud1Speed / Mathf.Abs(cloud1Speed) * -10, cloud1.transform.localPosition.y);
        }
        if (cloud2.transform.localPosition.x > 10 || cloud2.transform.localPosition.x < -10)
        {
            cloud2.transform.localPosition = new Vector2(cloud2Speed / Mathf.Abs(cloud2Speed) * -10, cloud2.transform.localPosition.y);
        }
        if (cloud3.transform.localPosition.x > 10 || cloud3.transform.localPosition.x < -10)
        {
            cloud3.transform.localPosition = new Vector2(cloud2Speed / Mathf.Abs(cloud3Speed) * -10, cloud3.transform.localPosition.y);
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            if (perso.GetComponent<PersoScript>().repairProgression >= perso.GetComponent<PersoScript>().repairCompletion ||
            perso.GetComponent<PersoScript>().life <= 0)
            {
                Application.Quit();
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.gameObject.SetActive(false);
                beginMenu.gameObject.SetActive(false);
            }            
        }
        else
        {
            Time.timeScale = 0;
            if (perso.GetComponent<PersoScript>().repairProgression >= perso.GetComponent<PersoScript>().repairCompletion)
            {
                winMenu.gameObject.SetActive(true);
            }
            else if(perso.GetComponent<PersoScript>().life <= 0)
            {
                looseMenu.gameObject.SetActive(true);
            }
            else
            {
                pauseMenu.gameObject.SetActive(true);
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Begin(int level)
    {
        switch (level)
        {
            case 0:
                maxEnemyDelay = maxEnemyDelayEasy;
                minEnemyDelay = minEnemyDelayEasy;
                perso.GetComponent<PersoScript>().repairLength = repairLengthEasy;
                break;
            case 2:
                maxEnemyDelay = maxEnemyDelayHard;
                minEnemyDelay = minEnemyDelayHard;
                perso.GetComponent<PersoScript>().repairLength = repairLengthHard;
                break;
            default:
                break;
        }
        Time.timeScale = 1;
        beginMenu.gameObject.SetActive(false);
    }
}
