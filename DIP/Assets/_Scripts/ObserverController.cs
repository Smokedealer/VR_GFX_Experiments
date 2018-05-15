using UnityEngine;

public class ObserverController : MonoBehaviour
{
    //initial speed
    public int baseSpeed = 10;
    private int currentSpeed;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    
    private float rotY; // rotation around the up/y axis
    private float rotX; // rotation around the right/x axis

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            MouseLook();
            Movement();
        }
    }

    public void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Movement()
    {
        
        //press shift to move faster
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = baseSpeed * 2;
        }
        else
        {
            //if shift is not pressed, reset to default speed
            currentSpeed = baseSpeed;
        }

        //move camera to the left
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + transform.right * -1 * currentSpeed * Time.deltaTime;
        }

        //move camera backwards
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + transform.forward * -1 * currentSpeed * Time.deltaTime;
        }

        //move camera to the right
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + transform.right * currentSpeed * Time.deltaTime;
        }

        //move camera forward
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + transform.forward * currentSpeed * Time.deltaTime;
        }

        //move camera upwards
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = transform.position + transform.up * currentSpeed * Time.deltaTime;
        }

        //move camera downwards
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            transform.position = transform.position + transform.up * -1 * currentSpeed * Time.deltaTime;
        }
    }

    private void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }
}