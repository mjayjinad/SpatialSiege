using GorillaZilla;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] OVRPassthroughLayer aliveLayer;
    [SerializeField] OVRPassthroughLayer winLayer;
    [SerializeField] OVRPassthroughLayer deadLayer;

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
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlivePlayerState();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        passthroughLayerController.SetActiveLayer(winLayer);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}
