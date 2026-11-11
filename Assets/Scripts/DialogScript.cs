using TMPro;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public TextMeshPro text;
    void Start()
    {
        text =  GetComponent<TextMeshPro>();
    }
}
