using System;
using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] private LoadEventChannelSO _loadHub;
    [SerializeField] private LoadEventChannelSO _loadAvatarCreation;
    [SerializeField] private LoadEventChannelSO _loadRoom;
    [SerializeField] private LoadEventChannelSO _loadMenu;
    
    [SerializeField] private BoolEventChannelSO _onRoomLoadedEvent;
    [SerializeField] private BoolEventChannelSO _onHubLoadedEvent;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _toggleLoadingScreen;
    [SerializeField] private FadeChannelSO _fadeRequestChannel = default;
    
    private GameSceneSO _sceneToLoad;
    private GameSceneSO _currentlyLoadedScene;
    private bool _showLoadingScreen;
    
    private float _fadeDuration = .5f;
    private bool _isLoading = false;
    
    private static SceneLoader _instance;

    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneLoader>();
            }
            
            return _instance;
        }
    }
    
    private void OnEnable()
    {
        _loadHub.OnLoadingRequested += LoadScene;
        _loadAvatarCreation.OnLoadingRequested += LoadScene;
        _loadMenu.OnLoadingRequested += LoadScene;
        _loadRoom.OnLoadingRequested += LoadScene;
    }

    private void OnDisable()
    {
        _loadHub.OnLoadingRequested -= LoadScene;
        _loadAvatarCreation.OnLoadingRequested -= LoadScene;
        _loadMenu.OnLoadingRequested -= LoadScene;
        _loadRoom.OnLoadingRequested -= LoadScene;
    }
    
    private void LoadScene(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        if (_isLoading) return;

        _sceneToLoad = menuToLoad;
        _showLoadingScreen = showLoadingScreen;
        _isLoading = true;

        StartCoroutine(UnloadPreviousScene());
    }
    
    private IEnumerator UnloadPreviousScene()
    {
        _fadeRequestChannel.FadeOut(_fadeDuration);

        yield return new WaitForSeconds(_fadeDuration);

        if (_currentlyLoadedScene != null)
        {
            if (!string.IsNullOrEmpty(_currentlyLoadedScene.SceneReference))
            {
                SceneManager.UnloadSceneAsync(_currentlyLoadedScene.SceneReference);
            }
#if UNITY_EDITOR
            else
            {
                SceneManager.UnloadSceneAsync(_currentlyLoadedScene.SceneReference);
            }
#endif
        }

        LoadNewScene();
    }
    
    private void IsHubLoading()
    {
        if (_sceneToLoad.SceneType == GameSceneSO.GameSceneType.Hub)
        {
            _onHubLoadedEvent.RaiseEvent(true);
        }
    }

    private void IsRoomLoading()
    {
        if (_sceneToLoad.SceneType == GameSceneSO.GameSceneType.Room)
        {
            _onRoomLoadedEvent.RaiseEvent(true);
        }
    }
    
    private void LoadNewScene()
    {
        if (_showLoadingScreen)
        {
            _toggleLoadingScreen.RaiseEvent(true);
        }

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(_sceneToLoad.SceneReference, LoadSceneMode.Additive);
        loadingOperation.completed += OnNewSceneLoaded;
    }
    
    //TODO: не загружать комнату до ответа от сервака, оставить fadecontroller
    private void OnNewSceneLoaded(AsyncOperation obj)
    {
        _currentlyLoadedScene = _sceneToLoad;

        IsHubLoading();
        IsRoomLoading();

        Scene loadedScene = SceneManager.GetSceneByName(_sceneToLoad.SceneReference);
        SceneManager.SetActiveScene(loadedScene);

        _isLoading = false;

        if (_showLoadingScreen)
        {
            _toggleLoadingScreen.RaiseEvent(false);
        }

        _fadeRequestChannel.FadeIn(_fadeDuration);
    }
}