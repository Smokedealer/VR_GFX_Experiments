using UnityEngine;
using UnityEngine.SceneManagement;

public class ObserverController : MonoBehaviour
{
    //initial speed
    public int baseSpeed = 10;
    private int currentSpeed;
    
    private void Start()
    {
        CursorLock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            MouseLook();
            Movement();
        }
        
    }

    
    /// <summary>
    /// Locks the cursor and makes it invisible
    /// </summary>
    public void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Handles the user imput and translates it into player movement.
    /// </summary>
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

    /// <summary>
    /// Very simple solution to camera rotation.
    /// </summary>
    private void MouseLook()
    {
        transform.eulerAngles += new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Rotation"));
    }
}