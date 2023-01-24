using UnityEngine;
using UnityEngine.UI;

public class ScrollAreaDebug : MonoBehaviour
{
    ScrollRect _scrollRect;
    [SerializeField] float verticalNormalizedPosition;
    private void Awake() {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRect.onValueChanged.AddListener(OnScrollAreaChanged);
    }
    public void OnScrollAreaChanged(Vector2 value)
    {
        verticalNormalizedPosition = value.y;
    }
}
