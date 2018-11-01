using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    [Header("Swipe Properties")]
    [SerializeField]
    private float _SwipeAccelerator = 4f;
    [SerializeField]
    private float _SwipeTreshold = 0.6f;

    private Rigidbody _Rigidbody;

    private void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        Input.multiTouchEnabled = false;
    }
    
    private void Update ()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.magnitude > _SwipeTreshold)
        {
            OnSwipe(Input.GetTouch(0).deltaPosition);
        }
    }

    private void OnSwipe(Vector3 swipeVector)
    {
        swipeVector.Normalize();
        _Rigidbody.angularVelocity += new Vector3(0.0f, - swipeVector.x * _SwipeAccelerator, 0.0f);
    }
}