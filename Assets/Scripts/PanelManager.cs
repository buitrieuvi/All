using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Zenject;

public class PanelManager : MonoBehaviour
{
    [Inject] private InputController _inputCtrl;
    [Inject] private DarkPanel _darkPanel;
    [Inject] private GameDataController _gameData;

    [Inject] private DiContainer _container;

    private bool _isLoading = false;

    public void Awake()
    {
        _inputCtrl.InputActions.Player.Inventory.performed += ctx =>
        {
            OnInputAction<InventoryPanel>();
        };
    }

    public async void OnInputAction<T>() where T : PanelBase
    {
        if (_isLoading) return;
        _isLoading = true;
        await _darkPanel.Transition(OpenPanel<T>);
        _isLoading = false;
    }

    public void OpenPanel<T>() where T : PanelBase
    {
        var hie = transform.GetComponentInChildren<T>(true);

        if (hie == null)
        {
            var prefab = _gameData.Panels.FirstOrDefault(x => x is T).gameObject;
            var panel = _container.InstantiatePrefab(prefab, transform).GetComponent<T>();
            panel.Open();
        }
        else
        {
            if (hie.gameObject.activeSelf)
            {
                hie.Close();
            }
            else
            {
                hie.Open();
            }
        }
    }
}
