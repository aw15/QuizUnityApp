using Firebase;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
    public FirebaseApp app;
    public UnityEvent OnFirebaseInitialize = new UnityEvent();
    // Start is called before the first frame update

    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Debug.Log("Firebase Init Success");
                OnFirebaseInitialize.Invoke();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
}
