using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Zenject;

public class PanelManager : MonoBehaviour
{
    [Inject] private DarkPanel _darkPanel;
    [Inject] private GameDataController _gameData;
    [Inject] private InputController _input;
    [Inject] private DiContainer _container;

    private bool _isLoading = false;

    public void Awake()
    {
        _input.InputActions.Player.Inventory.started += ctx =>
        {
            OnInputAction(_gameData.Panels.FirstOrDefault(x => x is InventoryPanel));
        };
    }

    public async void OnInputAction(PanelBase pnPrefab)
    {
        if (_isLoading) return;
        _isLoading = true;
        await _darkPanel.Transition(() => OpenPanel(pnPrefab));
        _isLoading = false;
    }

    public void OpenPanel(PanelBase pnPrefab)
    {
        var hie = transform.GetComponentInChildren<PanelBase>(true);

        if (hie == null)
        {
            var panel = _container.InstantiatePrefabForComponent<PanelBase>(pnPrefab.gameObject, transform);
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
