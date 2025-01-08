using UnityEngine;

public class LoadingScreen: MonoBehaviour
{
    [SerializeField] private GameObject _loadingGameObject;

#region MonoBehaviour
    private void Awake()
    {
        Deactivate();
    }
#endregion 

    public void Activate()
    {
        _loadingGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _loadingGameObject.SetActive(false);
    }

}