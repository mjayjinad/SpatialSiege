using GorillaZilla;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] OVRPassthroughLayer aliveLayer;
    [SerializeField] OVRPassthroughLayer winLayer;
    [SerializeField] OVRPassthroughLayer deadLayer;

    [SerializeField] private Transform player;
    [SerializeField] private OrbSpawner orbSpawner;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject homepageUI;
    [SerializeField] private GameObject gun;
    [SerializeField] private AudioSource speaker;
    [SerializeField] private AudioClip gameSoundClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip gameWinClip;

    private PassthroughLayerController passthroughLayerController;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        passthroughLayerController = GetComponent<PassthroughLayerController>();
        AlivePlayerState();
    }

    private void AlivePlayerState()
    {
        passthroughLayerController.SetActiveLayer(aliveLayer);
    }

    public void DeadPlayerState()
    {
        passthroughLayerController.SetActiveLayer(deadLayer);

    }

    public void WinPlayerState()
    {
        PlaySound(gameWinClip, false);
        gun.SetActive(false);
        passthroughLayerController.SetActiveLayer(winLayer);
        winUI.SetActive(true);
    }

    public void GameOver()
    {
        DeadPlayerState();
        gameOverUI.SetActive(true);
        gun.SetActive(false);
        PlaySound(gameOverClip, false);
        orbSpawner.GameOver();
        EnemySpawnerHandler.Instance.GameOver();
    }

    public void RestartGame()
    {
        homepageUI.SetActive(true);
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
        AlivePlayerState();
        player.tag = "Player";
        player.GetComponent<PlayerHealthManager>().isPlayerDead = false;
        EnemySpawnerHandler.Instance.waveInitialized = false;
        EnemySpawnerHandler.Instance.isGameOver = false;
    }

    public void StartGame()
    {
        orbSpawner.InitializeOrb();
        EnemySpawnerHandler.Instance.InitializeWave();
        PlaySound(gameSoundClip, true);
    }

    private void PlaySound(AudioClip clip, bool value)
    {
        speaker.Stop();
        speaker.loop = value;
        speaker.clip = clip;
        speaker.Play();
    }
}
