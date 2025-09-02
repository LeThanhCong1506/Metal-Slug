using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void HandleKeyboardInput()
    {
        bool jump = Input.GetButtonDown("Jump");
        bool fireKey = Input.GetButtonDown("Fire1");
        bool grenadeKey = Input.GetButtonDown("Fire2");
        int horizontalKey = (int)Input.GetAxisRaw("Horizontal");
        int verticalKey = (int)Input.GetAxis("Vertical");

    }

}
