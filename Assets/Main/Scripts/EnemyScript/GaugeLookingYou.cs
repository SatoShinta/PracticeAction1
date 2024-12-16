using UnityEngine;

public class GaugeLookingYou : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
    }
}
