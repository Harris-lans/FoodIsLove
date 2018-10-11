using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class FakeButton : MonoBehaviour
{

    [SerializeField]
    private Color _ClickTint;

    [SerializeField]
    private UnityEvent _OnMouseUp;

    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void OnMouseDown()
    {
        material.color = _ClickTint;
    }

    private void OnMouseUp()
    {
        material.color = Color.white;
        _OnMouseUp.Invoke();
    }

}
