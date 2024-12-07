using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
   [SerializeField,Header("ダメージを受けた回数")] int damageCounter = 0;

    private void Update()
    {
        if(damageCounter > 4)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerAttackCollider")
        {
            damageCounter++;
        }
    }
}
