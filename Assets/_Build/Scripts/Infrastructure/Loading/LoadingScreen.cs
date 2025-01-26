using UnityEngine;

public class LoadingScreen: MonoBehaviour
{
    [SerializeField] private GameObject _loadingGameObject;

#region MonoBehaviour
    private void Awake()
    {
        Hide();
    }
#endregion 

    public void Show()
    {
        _loadingGameObject.SetActive(true);
    }

    public void Hide()
    {
        _loadingGameObject.SetActive(false);
    }
}