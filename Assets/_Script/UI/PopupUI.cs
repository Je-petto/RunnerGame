using UnityEngine;
using MoreMountains.Feedbacks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopupUI : MonoBehaviour
{
    [SerializeField] GameObject quit;
    [SerializeField] MMF_Player quitOpen;
    [SerializeField] MMF_Player quitClose;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        quit.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quit.activeSelf)
                quitClose.PlayFeedbacks();
            else
                quitOpen.PlayFeedbacks();

        }
    }

    
    public void QuitOk()
    {
        //에디터 모드에서 Quit 작동
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        
        // 빌드 후 런타임에서 작동
#else
            Application.Quit();
#endif
    }

    public void QuitCancel()
    {
        quitClose?.PlayFeedbacks();
    }

}

