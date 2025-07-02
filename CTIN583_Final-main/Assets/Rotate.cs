using UnityEngine;

public class Rotate : MonoBehaviour
{
    private GameObject Parent;
    private GameObject RotTarget;


    private bool key;
    private bool entered;

    private Transform CurrentRot;
    private Transform NewRot;
    private Transform OldRot;

    private float timeCount = 0.0f;

    [Range(0.01f, 1f)]
    public float speed;


    private void Awake()
    {
        Parent = this.transform.parent.gameObject;

        NewRot = this.gameObject.GetComponent<Transform>();

        CurrentRot = Parent.transform.Find("RotatingObjs");
        OldRot = CurrentRot;
    }

    private void OnTriggerEnter(Collider other)
    {
        entered = true;
    }

    //get transforms of gameobjects
    //once collision
    //mult over time until == rotation 

    



    // Update is called once per frame
    void Update()
    {
        if ( entered )
        {
            //while (CurrentRot.rotation.z < NewRot.rotation.z)
           // {
                CurrentRot.rotation = Quaternion.Lerp(OldRot.rotation, NewRot.rotation, timeCount * speed);
                timeCount = timeCount + Time.deltaTime;
           // }
        }
    }
}
