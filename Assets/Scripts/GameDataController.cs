using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameDataController : MonoBehaviour
{
    public List<PanelBase> Panels = new();

    public void Awake()
    {
        LoadAllDataAsync().Forget();
    }

    public async UniTask LoadAllDataAsync()
    {
        Caching.ClearCache();

        // 🔄 Kiểm tra và cập nhật catalog nếu có bản mới
        //await UpdateRemoteCatalogIfAvailable();

        var panels = LoadAssetsAsync<GameObject>("panel", so => Panels.Add(so.GetComponent<PanelBase>()));

        await UniTask.WhenAll(panels).ContinueWith(() =>
        {

        });
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

    private async UniTask LoadAssetsAsync<T>(string label, System.Action<T> onAssetLoaded)
    {
        var handle = Addressables.LoadAssetsAsync<T>(label, onAssetLoaded);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"✅ Loaded {typeof(T).Name}: {handle.Result.Count}");
        }
        else
        {
            Debug.LogError($"❌ Failed to load {typeof(T).Name} with label '{label}'");
        }
    }
}
