using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct InventoryEvent
{
    public string itemId;
    public string eventType;
}
public class WebRequestSystem
{
    private const string URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string AUTH_TOKEN = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    public async void SendInventoryEvent(string itemId, string eventType)
    {
        var inventoryEvent = new InventoryEvent
        {
            itemId = itemId,
            eventType = eventType
        };
        
        var jsonData = JsonUtility.ToJson(inventoryEvent);
        
        try
        {
            var response = await PostRequestAsync(jsonData);
            Debug.Log("Response: " + response);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    private async UniTask<string> PostRequestAsync(string jsonData)
    {
        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AUTH_TOKEN);
            
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            await request.SendWebRequest().ToUniTask();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }

            throw new System.Exception(request.error);
        }
    }
}
