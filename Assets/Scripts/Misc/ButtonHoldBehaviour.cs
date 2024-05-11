using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class ButtonHoldBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isButtonHeld = false;
    public UnityEvent OnButtonHold = new UnityEvent();
    Button button;
    float timer = 0;
    float delay = .1f;

    private void Start()
    {
        button = GetComponent<Button>();
    }


    void Update()
    {
        if (isButtonHeld)
        {
            //add a slight delay
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                OnButtonHold?.Invoke();
                timer = 0f;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        timer = 0;
        isButtonHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {        
        isButtonHeld = false;
    }
}