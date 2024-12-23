using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Create StatusData")]
public class CharaData : ScriptableObject
{
    public List<Status> statusList = new List<Status>();

    // playerの情報が格納されているインデックスを表示する変数
    public int playerIndex = 0;

    // キャラクターのステータスの参照元
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
