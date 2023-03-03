using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public string username { get; set; }
    [FirestoreProperty]
    public string password { get; set; }

    public UserData() { }

    public UserData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
