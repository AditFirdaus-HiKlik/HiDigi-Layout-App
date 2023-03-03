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

    static CoroutineHandler coroutineHandler = CoroutineHandler.instance;

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return coroutineHandler.StartCoroutine(coroutine);
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

        Debug.Log("Getting user data");

        Debug.Log("username: " + username);
        Debug.Log("password: " + password);

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
                Debug.LogWarning("Wrong password");
            }
        }
        else
        {
            Debug.LogWarning("User not found");
        }
    }
}