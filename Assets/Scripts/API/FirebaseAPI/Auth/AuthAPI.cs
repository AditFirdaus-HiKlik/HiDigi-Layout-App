using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Storage;
using Firebase.Firestore;

public class AuthAPI
{
    public static FirebaseStorage db => FirebaseStorage.GetInstance(FirebaseAPI.app);

    static GlobalMonoBehaviour monoBehaviour = GlobalMonoBehaviour.instance;

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoBehaviour.StartCoroutine(coroutine);
    }

    // Coroutine
    public static Coroutine Login(string username, string password, UnityAction<UserData> callback)
    {
        return StartCoroutine(LoginAsync(username, password, callback));
    }

    // Async
    static IEnumerator LoginAsync(string username, string password, UnityAction<UserData> callback)
    {
        DocumentSnapshot documentSnapshot = null;

        DebugApp.Log("Getting user data");

        DebugApp.Log("username: " + username);
        DebugApp.Log("password: " + password);

        yield return FirestoreAPI.GetDocument("users", username, (result) => documentSnapshot = result);

        if (documentSnapshot.Exists)
        {
            UserData userData = documentSnapshot.ConvertTo<UserData>();

            if (userData.password == password)
            {
                callback(userData);
            }
            else
            {
                DebugApp.Log("Wrong password");
            }
        }
        else
        {
            DebugApp.Log("User not found");
        }
    }
}