using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] TextMeshProUGUI doorText;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private float textTime = 2f;
    public TextMeshProUGUI matchText;
    public Animator openandclose;
    public bool open = false;

    //private Quaternion initialRotation;
    //private Quaternion targetRotation;
    //private bool isOpening = false;
    public bool gotKey = false;

    /*
    void Start()
    {
        if (door != null)
        {
            initialRotation = door.transform.rotation;
            targetRotation = Quaternion.Euler(door.transform.eulerAngles.x, door.transform.eulerAngles.y + 135f, door.transform.eulerAngles.z);
        }
    }
    */

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryOpenDoor();
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            gotKey = true;
            matchText.text = "Got the key!";
            StartCoroutine(ShowGotText());
        }
    }

    public void TryOpenDoor()
    {
        if (gotKey)
        {
            OpenCloseDoor();
        }
        else
        {
            StartCoroutine(ShowLockedText());
        }
    }

    public void OpenCloseDoor()
    {
        if (!open)
        {
            StartCoroutine(opening());
        }
        else
        {
            StartCoroutine(closing());
        }
    }
    IEnumerator opening()
    {
        // Debug.Log("Opening the door");
        openandclose.Play("Opening");
        open = true;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator closing()
    {
        // Debug.Log("Closing the door");
        openandclose.Play("Closing");
        open = false;
        yield return new WaitForSeconds(0.5f);
    }

    /*
    public void RotateDoor()
    {
        if (!isOpening && door != null)
        {
            // Recalculate initial and target rotation right before rotating
            initialRotation = door.transform.rotation;
            targetRotation = Quaternion.Euler(
                door.transform.eulerAngles.x,
                door.transform.eulerAngles.y + 135f,
                door.transform.eulerAngles.z
            );

            StartCoroutine(RotateDoorCoroutine());
        }
    }

    private IEnumerator RotateDoorCoroutine()
    {
        isOpening = true;

        float timeElapsed = 0f;
        while (timeElapsed < rotationDuration)
        {
            door.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed / rotationDuration);
            Debug.Log("Door Y Rotation: " + door.transform.eulerAngles.y);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("rotated door with coroutine");
        door.transform.rotation = targetRotation;
        isOpening = false;
    }
    */

    private IEnumerator ShowLockedText()
    {
        doorText.gameObject.SetActive(true);

        yield return new WaitForSeconds(textTime);

        doorText.gameObject.SetActive(false);

    }

    private IEnumerator ShowGotText()
    {
        matchText.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);

        matchText.gameObject.SetActive(false);

    }
}

