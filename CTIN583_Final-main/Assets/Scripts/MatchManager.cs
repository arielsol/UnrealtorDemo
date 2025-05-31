using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Numerics;
using Unity.VisualScripting;
using System.Globalization;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviour
{
    public LevelManager levelManager;
    public QuadSO debugQuad; // use this reference for debugging
    public Camera leftCamera;
    public Camera rightCamera;
    public TextMeshProUGUI matchText;
    public TextMeshProUGUI continueText;
    public MatchSpawner matchSpawner;
    public float matchResetTime = 5f;
    public float xThresholdPercent = 0.75f;
    public float yThresholdPercent = 1.2f;
    public float angleThreshold = 10f;
    public bool infiniteMatch = false;
    private float xThreshold = 10f;
    private float yThreshold = 20f;
    private bool matchable = true;
    public OpenDoor doorToOpen;
    [SerializeField] private List<string> matchTexts = new List<string>();
    [SerializeField] private List<QuadSOPair> quadPairs = new List<QuadSOPair>();
    [SerializeField] private List<GameObject> hideObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> showObjects = new List<GameObject>();

    private UnityEngine.Vector2 topLeftVertexScreenPoint;
    private UnityEngine.Vector2 bottomLeftVertexScreenPoint;

    private UnityEngine.Vector2 topRightVertexScreenPoint;
    private UnityEngine.Vector2 bottomRightVertexScreenPoint;

    void Start()
    {
        xThreshold = Screen.width * xThresholdPercent / 100;
        yThreshold = Screen.width * yThresholdPercent / 100;

        OnUnmatch();
    }


    void Update()
    {
        // Debug.Log(UnityEngine.Vector3.Distance(leftCamera.transform.position, debugQuad.topVertex));
        bool anyMatched = false; 

        foreach (var pair in quadPairs)
        {   
            // is it more efficient to get all the screen points and only check matches if players can see quads
            // or to get all distances and only track screen points within a distance threshold?

            // Only check if left player is within angleThreshold of left quad and right player within right quad
            
            if (CheckMatch(pair))
            {
                OnMatch(pair); // Run match effects
                anyMatched = true;
                break;
            }
        }

        if (!anyMatched)
        {
            OnUnmatch();
        }
    }

    public bool CheckMatch(QuadSOPair pair)
    {
        // Get the screen coordinates of vertices in each pair
        topLeftVertexScreenPoint = leftCamera.WorldToScreenPoint(pair.leftQuad.topVertex); // Left quad's right vertex
        bottomLeftVertexScreenPoint = leftCamera.WorldToScreenPoint(pair.leftQuad.bottomVertex); // Left quad's right vertex
        topRightVertexScreenPoint = rightCamera.WorldToScreenPoint(pair.rightQuad.topVertex); // Right quad's left vertex
        bottomRightVertexScreenPoint = rightCamera.WorldToScreenPoint(pair.rightQuad.bottomVertex); // Right quad's left vertex

        float topLeftDifference = topLeftVertexScreenPoint.x - (Screen.width/2);
        float bottomLeftDifference = bottomLeftVertexScreenPoint.x - (Screen.width/2);
        float topRightDifference = (Screen.width/2) - topRightVertexScreenPoint.x;
        float bottomRightDifference = (Screen.width/2) - bottomRightVertexScreenPoint.x;

        /*
        if (pair == quadPairs[0])
        {
            Debug.Log("TLD: " + topLeftDifference);
            Debug.Log("BLD: " + bottomLeftDifference);
            Debug.Log("TRD: " + topRightDifference);
            Debug.Log("BRD: " + bottomRightDifference);
        }
        */

        return 
        (
        Mathf.Abs(topLeftDifference) <= xThreshold && // Top left vertex near screen split
        Mathf.Abs(bottomLeftDifference) <= xThreshold && // Bottom left vertex near screen split on right
        Mathf.Abs(topRightDifference) <= xThreshold && // Top right vertex near screen split
        Mathf.Abs(bottomRightDifference) <= xThreshold && // Top right vertex near screen split

        Mathf.Abs(topLeftVertexScreenPoint.y - topRightVertexScreenPoint.y) <= yThreshold && // Top vertices horizontally aligned
        Mathf.Abs(bottomLeftVertexScreenPoint.y - bottomRightVertexScreenPoint.y) <= yThreshold // Bottom vertices horizontally aligned
        );
    }

    public void OnMatch(QuadSOPair pair)
    {
        matchText.text = matchTexts[quadPairs.IndexOf(pair)];
        doorToOpen.gotKey = true; //this is a fucked up way to do this and I know it
        if (quadPairs.IndexOf(pair) == 1)
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                foreach (var obj in hideObjects)
                {
                    obj.gameObject.SetActive(false);
                }
                foreach (var obj in showObjects)
                {
                    obj.gameObject.SetActive(true);
                }
            }
        }
        // Debug.Log(matchable);
        if (matchable && infiniteMatch)
        {
            matchSpawner.SpawnInfiniteMatch(pair);
        }

        if (matchable && !infiniteMatch)
        {
            matchable = false;
            // matchSpawner.SpawnMatch(pair);
        }

        // levelManager.SetComplete(true);   
        // continueText.gameObject.SetActive(true);
        // maybe a list of arrays of game objects to hide (and ones to show)
    }

    public void OnUnmatch()
    {
        matchText.text = "";
        matchable = true;
    }
}
