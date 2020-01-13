using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public int cameraSpeed;
    public GameObject background;

    private Camera cam;

    public float LeftB
    {
        get
        {
            return background.GetComponent<SpriteRenderer>().bounds.size.x * -0.5f + cam.orthographicSize * cam.aspect;
        }
    }

    public float RightB
    {
        get
        {
            return background.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f - cam.orthographicSize * cam.aspect;
        }
    }

    public float UpperB
    {
        get
        {
            return background.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f - cam.orthographicSize;
        }
    }

    public float BottomB
    {
        get
        {
            return background.GetComponent<SpriteRenderer>().bounds.size.y * -0.5f + cam.orthographicSize;
        }

    }

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        //прокрутка карты
        //left
        if (transform.position.x > LeftB && ((int)Input.mousePosition.x < 2 || Input.GetAxis("Horizontal") < 0))
        {
            transform.position -= transform.right * Time.deltaTime * cameraSpeed;
        }
        //right!
        if (transform.position.x < RightB && ((int)Input.mousePosition.x > Screen.width - 2 || Input.GetAxis("Horizontal") > 0))
        {
            transform.position += transform.right * Time.deltaTime * cameraSpeed;
        }
        //up
        if (transform.position.y < UpperB && ((int)Input.mousePosition.y > Screen.height - 2 || Input.GetAxis("Vertical") > 0))
        {
            transform.position += transform.up * Time.deltaTime * cameraSpeed;
        }
        //down!
        if (transform.position.y > BottomB && ((int)Input.mousePosition.y < 2 || Input.GetAxis("Vertical") < 0))
        {
            transform.position -= transform.up * Time.deltaTime * cameraSpeed;
        }

        //возврат камеры в границы
        if (transform.position.x < LeftB)
        {
            transform.position += transform.right * Time.deltaTime * cameraSpeed;
        }
        if (transform.position.x > RightB)
        {
            transform.position -= transform.right * Time.deltaTime * cameraSpeed;
        }
        if (transform.position.y > UpperB)
        {
            transform.position -= transform.up * Time.deltaTime * cameraSpeed;
        }
        if (transform.position.y < BottomB)
        {
            transform.position += transform.up * Time.deltaTime * cameraSpeed;
        }
    }
}