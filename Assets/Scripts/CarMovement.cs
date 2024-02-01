using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Transform finalPoint;
    [SerializeField] float range = 20f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float zOffset = 1f;
    [SerializeField] float yOffset = 0.3f;
    [SerializeField] Transform nearestPoint;
    Transform target = null;
    Vector3 direction;
    Ray theRay;
    bool isClear = false;
    bool firstPoint = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isClear && !firstPoint)
        {  //target != null
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed* Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, nearestPoint.position, moveSpeed * Time.deltaTime);
            
            if (transform.position == nearestPoint.position)
                firstPoint = true;
        }
        else if (isClear && firstPoint) {
            transform.position = Vector3.MoveTowards(transform.position, finalPoint.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0f, 0.707f, 0f, 0.707f), moveSpeed * Time.deltaTime);
        }
    }

    public void LetsMoveTheCar() {
        Debug.Log("You clicked the " + gameObject.tag + " car");
        //CheckTheFront();
        CheckCollision(true);   //passing true for checking the front side first
    }

    void CheckCollision(bool isFront) {
        direction = Vector3.forward;
        if (isFront)
        {
            Vector3 origin = transform.position + new Vector3(0f, yOffset, zOffset);
            //Vector3 origin = transform.position + new Vector3(0f, 0.3f, 1f);
            theRay = new Ray(origin, transform.TransformDirection(direction *  range));
            Debug.DrawRay(origin, transform.TransformDirection(direction * range));
        }
        else {
            Vector3 origin = transform.position;// + new Vector3(0f, 0.3f, -1f);
            theRay = new Ray(origin, transform.TransformDirection(-direction * range));
            Debug.DrawRay(origin, transform.TransformDirection(-direction *range));
        }
        if (Physics.Raycast(theRay, out RaycastHit hit, range))
        {
            if (hit.collider.tag == "Road") {
                print("Yeah!! Good To Go");
                //target = hit.collider.gameObject.transform;
                isClear = true;
            }
            else if (hit.collider.tag != null)
            {
                print("You hit " + hit.collider.tag);
                //print("You can't move");
                if (isFront) {
                    print("Checking the back...");
                    CheckCollision(!isFront);
                }
            }
        }
        else
        {
            print("Good to GO!!!");
            
        }
    }



















    void CheckTheFront()
    {
        direction = Vector3.forward;
        Vector3 origin = transform.position + new Vector3(0f, 0.3f, 1f);
        theRay = new Ray(origin, transform.TransformDirection(direction * range));
        Debug.DrawRay(origin, transform.TransformDirection(direction * range));
        if (Physics.Raycast(theRay, out RaycastHit hit, range))
        {
            if (hit.collider.tag != null)
            {
                print("You hit " + hit.collider.tag);
                print("Checking the back...");
                CheckTheBack();
            }
        }
        else
        {
            print("Good to GO!!!");
        }
    }

    void CheckTheBack()
    {
        Vector3 origin = transform.position;// + new Vector3(0f, 0.3f, -1f);
        theRay = new Ray(origin, transform.TransformDirection(-direction * range));
        Debug.DrawRay(origin, transform.TransformDirection(-direction * range));
        if (Physics.Raycast(theRay, out RaycastHit hit, range))
        {
            if (hit.collider.tag != null)
            {
                print("You hit " + hit.collider.tag);
                print("You can't move");
            }
        }
        else
        {
            print("Good to GO!!!");
        }
    }
}
