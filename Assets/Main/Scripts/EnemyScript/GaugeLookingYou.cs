using UnityEngine;

public class GaugeLookingYou : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    // �G��HP�Q�[�W��player�i���C���J�����j����Ɍ����悤�ɂ��鏈��
    private void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
    }
}
