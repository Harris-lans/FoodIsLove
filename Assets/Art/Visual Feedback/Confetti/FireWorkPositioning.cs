using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorkPositioning : MonoBehaviour
{



    [SerializeField]
    private Camera Maincamera;
    [SerializeField]
    private Vector3 Coordenates;



    // Use this for initialization
    void Start()
    {
		Debug.Log("bla");
       gameObject.transform.localPosition = Maincamera.transform.position + Coordenates;
    }

}
