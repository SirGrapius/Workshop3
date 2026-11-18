using Unity.VisualScripting;
using UnityEngine;

public class BackgroundParalax : MonoBehaviour
{
    
    private float startPos;
    [SerializeField] GameObject cam;
    [SerializeField] float parallaxEffect;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        startPos = transform.position.x;
    }


    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
