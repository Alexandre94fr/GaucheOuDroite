using UnityEngine;


public class DirectionButton : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] DirectionProperties.Direction _buttonDirection;

    
    public void OnButtonPressed()
    {
        EventHandler.OnDirectionChoiceInputEvent?.Invoke(_buttonDirection);
    }
}