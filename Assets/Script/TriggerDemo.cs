using Unity.VisualScripting;
using UnityEngine;
public class TriggerDemo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }
}