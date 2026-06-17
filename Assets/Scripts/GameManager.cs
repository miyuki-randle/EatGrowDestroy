using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerPrefab;
    public GameObject player;
    public float score;
    //public float lives;
    //public int size;
    public int stage;
    public bool running;

    //private int lastSizeCheck;

    [SerializeField] private SizeHUD sizeHUD;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TextMeshProUGUI finalSize;
    [SerializeField] private TextMeshProUGUI highScore;

    [SerializeField] private AudioSource gameOverAudio;
    [SerializeField] private AudioSource backgroundMusic;

    [SerializeField] private AudioClip[] bgMusicClips;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartGame()
    {
        player = Instantiate(playerPrefab);
        score = 0;
        stage = 0;
        running = true;
        gameOverAudio = GetComponent<AudioSource>();
        backgroundMusic.clip = bgMusicClips[1];
        backgroundMusic.Play();

        foreach (var obj in spawners)
        {
            obj.gameObject.SetActive(true);
            obj.GetComponent<Spawner>().active = true;
        }
        EventBus.Instance.DispatchEvent(GameEvent.GameStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            UpdateUI();
            if (sizeHUD != null && sizeHUD.curSize > sizeHUD.baseSize) score += Time.deltaTime * (sizeHUD.curSize - sizeHUD.baseSize);
            else score += Time.deltaTime;
        }

    }

    void UpdateUI()
    {
        scoreText.text = Mathf.RoundToInt(score).ToString();
    }

    public IEnumerator GameOver()
    {
        gameOverAudio.Play();
        running = false;

        backgroundMusic.clip = bgMusicClips[0];
        backgroundMusic.Play();

        // Disable spawners
        foreach (var obj in spawners)
        {
            obj.gameObject.SetActive(false);
            obj.GetComponent<Spawner>().active = false;
        }
        EventBus.Instance.DispatchEvent(GameEvent.PlayerDeath);

        yield return new WaitForSeconds(3);
        
        gameOverPanel.SetActive(true);
        finalScore.text = Mathf.RoundToInt(score).ToString();
        finalSize.text = sizeHUD.curSize.ToString();

        if (score > PlayerPrefs.GetInt("HIGH_SCORE"))
        {
            PlayerPrefs.SetInt("HIGH_SCORE", Mathf.RoundToInt(score));
            highScore.text = finalScore.text;
            highScore.color = Color.yellow;
        }
        else highScore.text = PlayerPrefs.GetInt("HIGH_SCORE").ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
