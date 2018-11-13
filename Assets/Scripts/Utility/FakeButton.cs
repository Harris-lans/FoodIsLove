using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class FakeButton : MonoBehaviour
{

    [SerializeField]
    private Color _ClickTint;

    [SerializeField]
    private UnityEvent _OnMouseUp;

    [SerializeField]
    private SpriteRenderer _SpriteRenderer;

    private void OnMouseDown()
    {
        _SpriteRenderer.color = _ClickTint;
    }

    private void OnMouseUp()
    {
        _SpriteRenderer.color = Color.white;
        _OnMouseUp.Invoke();
    }

}
