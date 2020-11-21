using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public enum SceneIndexes
    {
        //Title,
        SceneTest1 = 0,
        SceneTest2,
    }

    [SerializeField] private Animator[] _animators;
    [SerializeField] private SceneIndexes _scene;

    private static SceneHandler _instance;
    public static SceneHandler Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);

            _animators = GetComponentsInChildren<Animator>();
        }
    }

    // DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LoadScene(_scene);
        }
    }


    public void LoadScene(SceneIndexes sceneBuildIndex, int animationIndex = 0)
    {
        LoadScene((int)sceneBuildIndex, animationIndex);
    }

    public void LoadScene(int sceneBuildIndex, int animationIndex = 0)
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)sceneBuildIndex &&
            SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex) != string.Empty &&
            animationIndex >= 0 && animationIndex < _animators.Length)
            StartCoroutine(ILoadScene(sceneBuildIndex, animationIndex));
        else
            Debug.LogWarning("Trying to load identical scene or reach a scene that does not exist!", this);
    }

    private IEnumerator ILoadScene(int sceneBuildIndex, int animationIndex = 0)
    {
        Animator anim = _animators[animationIndex];
        float timer = 0;
        float minimumTransitionTime = 0;
        AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;

        for (int i = 0; i < animationClips.Length - 1; i++) // skip the last animation
        {
            minimumTransitionTime += animationClips[i].length;
        }

        // Start animation
        anim.SetTrigger("Start");

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        asyncLoadLevel.allowSceneActivation = false;


        while (!asyncLoadLevel.isDone)
        {
            // only allows a scene change if the first part of the transition has been completed
            timer += Time.deltaTime;
            if (timer > minimumTransitionTime)
            {
                asyncLoadLevel.allowSceneActivation = true;
            }

            yield return null;
        }

        anim.SetTrigger("End");
    }
}