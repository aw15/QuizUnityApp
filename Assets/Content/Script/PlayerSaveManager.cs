using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class PlayerSaveManager : MonoBehaviour
{
    private const string PLAYER_KEY = "PLAYER_KEY";
    private FirebaseDatabase database;
    // Start is called before the first frame update
    void Start()
    {
        database = FirebaseDatabase.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(player));
    }
}
