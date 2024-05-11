using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static JoystickController instance;
   
    [SerializeField] private RectTransform joystickBackground;
    private RectTransform joystickNub;
    private Vector2 inputDirection = Vector2.zero;

    public Vector2 InputDirection { get => inputDirection; set => inputDirection = value; }

    void Start()
    {
        instance = this;  
        joystickNub = transform.GetChild(0).GetComponent<RectTransform>(); // Assuming handle is the first child
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        joystickNub.anchoredPosition = Vector2.zero; // Reset handle position
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / joystickBackground.sizeDelta.x);
            position.y = (position.y / joystickBackground.sizeDelta.y);

            float x = (joystickBackground.rect.width / 2) * position.x;
            float y = (joystickBackground.rect.height / 2) * position.y;

            InputDirection = new Vector2(x, y).normalized;
            joystickNub.anchoredPosition = InputDirection * (joystickBackground.rect.width / 3); // Adjust the division value for handle sensitivity
        }
    }

  
}