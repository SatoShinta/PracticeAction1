using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Create StatusData")]
public class CharaData : ScriptableObject
{
    public List<Status> statusList = new List<Status>();

    [System.Serializable]
    public class Status
    {
        public string charaName;
        public int maxHp;
        public int atk;
        public int agl;
    }
   

}
