using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open = false;
        public Transform player;
        public float interactionDistance = 3f;

        void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                float dist = Vector3.Distance(player.position, transform.position);
                if (dist < interactionDistance)
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
            }
        }

        IEnumerator opening()
        {
            Debug.Log("Opening the door");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing()
        {
            Debug.Log("Closing the door");
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
