using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [HideInInspector]
    public Vector2 leftAxis { get; private set; }

    [HideInInspector]
    public Vector2 rightAxis { get; private set; }

    [HideInInspector]
    public Vector2 mosueInput { get; private set; }

    [HideInInspector]
    public bool jumpInput { get; private set; }

    [HideInInspector]
    public bool runInput { get; private set; }


    private void Update()
    {
        leftAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rightAxis = new Vector2(Input.GetAxis("RightAxis X"), Input.GetAxis("RightAxis Y"));
        mosueInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        jumpInput = Input.GetButton("Jump");
        runInput = Input.GetButton("Fire1");
    }
}
