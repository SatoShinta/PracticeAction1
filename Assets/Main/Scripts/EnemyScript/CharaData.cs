using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Create StatusData")]
public class CharaData : ScriptableObject
{
    public List<Status> statusList = new List<Status>();

    // player�̏�񂪊i�[����Ă���C���f�b�N�X��\������ϐ�
    public int playerIndex = 0;

    // �L�����N�^�[�̃X�e�[�^�X�̎Q�ƌ�
    [System.Serializable]
    public class Status
    {
        public string charaName;
        public int maxHp;
        public int atk;
        public int agl;
        public int attackTime;
    }
   

}
