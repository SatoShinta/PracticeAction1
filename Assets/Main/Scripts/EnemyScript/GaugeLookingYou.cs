using UnityEngine;

public class GaugeLookingYou : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    // 敵のHPゲージがplayer（メインカメラ）を常に向くようにする処理
    private void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
    }
}
