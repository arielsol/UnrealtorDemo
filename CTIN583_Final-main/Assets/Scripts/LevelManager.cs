using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TwoPlayerController twoPlayerController;
    [SerializeField] private MatchManager matchManager;
    [SerializeField] private float levelChargeTime = 1.5f;
    [SerializeField] private int currentLevel;
    private TwoPlayerInput controls;
    private bool player1Interact;
    private bool isLevelComplete;
    private float chargeAmount;
    private float chargeRatio;
    private float currentTime;

    private void Awake()
    {
        controls = new TwoPlayerInput();

        controls.Player1.Interact.performed += ctx => player1Interact = true;
        controls.Player1.Interact.canceled += ctx => player1Interact = false;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        isLevelComplete = false;
    }

    private void Update()
    {
        if(isLevelComplete)
        {
            ChargeNextLevel();
        }
    }

    public float GetChargeAmount()
    {
        return chargeAmount;
    }

    public void SetComplete(bool isComplete)
    {
        isLevelComplete = isComplete;
    }

    private void LoadNextLevel(int CurrentLevel)
    {
        int nextLevel = CurrentLevel + 1;
        SceneManager.LoadScene(nextLevel);
    }

    private void ChargeNextLevel()
    {
        if (player1Interact)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= levelChargeTime)
            {
                currentTime = levelChargeTime;
                Debug.Log("Loading Next Scene");
                LoadNextLevel(currentLevel);
            }
            chargeRatio = currentTime / levelChargeTime;
            chargeAmount = Mathf.Lerp(0f, -10f, chargeRatio);
        }
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f) 
            {
                currentTime = 0f;
            }
            chargeRatio = currentTime / levelChargeTime;
            chargeAmount = Mathf.Lerp(0f, -10f, chargeRatio); // Fades back up to 0 smoothly
        }
    }
}
