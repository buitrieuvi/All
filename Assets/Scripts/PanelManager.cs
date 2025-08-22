using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private List<PanelBase> _panels = new();
    [Inject] private InputController _inputCtrl;
    [Inject] private DarkPanel _darkPanel;

    public void Awake()
    {
        LoadAllDataAsync().Forget();

        _inputCtrl.InputActions.Player.Inventory.performed += ctx => _darkPanel.Transition(OpenPanel<InventoryPanel>);
    }

    private async UniTask LoadPanelsAsync(string label)
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(label, obj =>
        {
            var panel = obj.GetComponent<PanelBase>();
            if (panel != null)
            {
                _panels.Add(panel);
            }
        });

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"✅ Loaded panels: {_panels.Count}");
        }
        else
        {
            Debug.LogError($"❌ Failed to load panels with label '{label}'");
        }
    }

    private async UniTask UpdateRemoteCatalogIfAvailable()
    {
        var checkHandle = Addressables.CheckForCatalogUpdates();
        var catalogs = await checkHandle.Task;

        if (catalogs != null && catalogs.Count > 0)
        {
            Debug.Log("📦 Catalog update found. Updating...");
            var updateHandle = Addressables.UpdateCatalogs(catalogs);
            await updateHandle.Task;
            Debug.Log("✅ Catalog updated.");
        }
        else
        {
            Debug.Log("ℹ️ No catalog updates found.");
        }
    }

    private async UniTask LoadAllDataAsync()
    {
        Caching.ClearCache();

        //await UpdateRemoteCatalogIfAvailable();

        await LoadPanelsAsync("panel");

        Debug.Log("🎉 Done loading panels!");
    }

    public void OpenPanel<T>() where T : PanelBase
    {
        
        var hie = transform.GetComponentInChildren<PanelBase>(true);

        if (hie == null)
        {
            Instantiate(_panels.FirstOrDefault(p => p is T).gameObject, transform).GetComponent<T>().Open();
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
