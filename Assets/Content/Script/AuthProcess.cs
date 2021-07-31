using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.Events;
using UltimateClean;
using System.Text.RegularExpressions;
using TMPro;
using Firebase;
using System.Threading.Tasks;

public class AuthProcess : MonoBehaviour
{
    public UnityEvent<bool, string> HandleRegisterationSuccess;
    public UnityEvent<bool, string> HandleLoginSuccess;
    private Coroutine registerationCoroutine;
    private Coroutine loginCoroutine;
    // Start is called before the first frame update
    private static AuthProcess instance;
    public static AuthProcess Ins
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(AuthProcess)) as AuthProcess;

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        Ins = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        loginCoroutine = null;
        registerationCoroutine = null;
    }

    void OnRegisterResult(bool result, string reason)
    {
        if (result)
        {
            UIManager.Ins.ShowNotification("���� ��� ����", "", Notification.InfoType.Success);
        }
        else
        {
            UIManager.Ins.ShowNotification("���� ��� ����", reason, Notification.InfoType.Error);
        }
    }
    void OnLoginResult(bool result, string reason)
    {
        if (result)
        {
            UIManager.Ins.ShowNotification("�α��� ����", "", Notification.InfoType.Success);
        }
        else
        {
            UIManager.Ins.ShowNotification("�α��� ����", reason, Notification.InfoType.Error);
        }
    }
    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }
    public void Login(string email, string pw)
    {
        loginCoroutine = StartCoroutine(LoginImplement(email, pw));
    }

    private IEnumerator LoginImplement(string email, string pw)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, pw);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Failed to login task with {loginTask.Exception}");

            FirebaseException firebaseEx = loginTask.Exception.InnerExceptions[0] as FirebaseException;
            string reason = string.Empty;
            if (firebaseEx != null)
            {
                var errorCode = (AuthError)firebaseEx.ErrorCode;
                GetErrorMessage(errorCode);
            }
            HandleLoginSuccess.Invoke(false, reason);
            OnLoginResult(false, reason);
        }
        else
        {
            HandleLoginSuccess.Invoke(true, string.Empty);
            OnLoginResult(true, string.Empty);
        }

        loginCoroutine = null;
    }

    public void Register(string email, string pw)
    {
        registerationCoroutine = StartCoroutine(RegisterUser(email, pw));
    }

    private IEnumerator RegisterUser(string email, string pw)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, pw);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if(registerTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {registerTask.Exception}");

            FirebaseException firebaseEx = registerTask.Exception.InnerExceptions[0] as FirebaseException;
            string reason = string.Empty;
            if (firebaseEx != null)
            {
                var errorCode = (AuthError)firebaseEx.ErrorCode;
                GetErrorMessage(errorCode);
            }

            HandleRegisterationSuccess.Invoke(false, reason);
            OnRegisterResult(false, reason);
        }
        else
        {
            Debug.Log($"Successfully registered user {registerTask.Result.Email}");
            HandleRegisterationSuccess.Invoke(true, string.Empty);
            OnRegisterResult(true, string.Empty);
        }

        registerationCoroutine = null;
    }

    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "������ �̹� �����մϴ�.";
                break;
            case AuthError.MissingPassword:
                message = "��й�ȣ�� �ʿ��մϴ�.";
                break;
            case AuthError.WeakPassword:
                message = "��й�ȣ�� �ʹ� �����ϴ�.";
                break;
            case AuthError.WrongPassword:
                message = "�߸��� ��й�ȣ�Դϴ�.";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "�̹� ���� �̸����Դϴ�.";
                break;
            case AuthError.InvalidEmail:
                message = "�̸����� ��ȿ���� �ʽ��ϴ�.";
                break;
            case AuthError.MissingEmail:
                message = "�̸����� �ʿ��մϴ�.";
                break;
            default:
                message = "���� �߻�";
                break;
        }
        return message;
    }

    public Task<bool> FindPassword(string email)
    {
        var auth = FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            return auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("[FindPassword] SendPasswordResetEmailAsync was canceled.");
                    return false;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("[FindPassword] SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return false;
                }

                Debug.Log("Password reset email sent successfully.");
                return true;
            });
        }
        else
        {
            return new Task<bool>(() => { Debug.LogError("[FindPassword] no user");  return false; });
        }
    }
}
