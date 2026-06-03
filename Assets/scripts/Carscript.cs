using UnityEngine;

public class Carscript : MonoBehaviour
{
    public float speed = 3f;
    private bool isMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }
    public void StartMoving()
    {
        isMoving = true;
    }
}
