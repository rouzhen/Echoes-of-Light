using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    public void ButtonClick()
    {
        GameManager.instance.GameRestart();
    }
}