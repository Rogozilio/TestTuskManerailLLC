using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class InventoryEvent
{
    public string item_id; // Идентификатор предмета
    public string event_type; // Событие (например, "add" или "remove")
}
public class RequstSystem
{
    private const string URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string AUTH_TOKEN = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    public async void SendInventoryEvent(string itemId, string eventType)
    {
        // Создаем объект с данными
        InventoryEvent inventoryEvent = new InventoryEvent
        {
            item_id = itemId,
            event_type = eventType
        };

        // Преобразуем объект в JSON
        string jsonData = JsonUtility.ToJson(inventoryEvent);

        // Отправляем запрос и ждем ответа
        try
        {
            string response = await PostRequestAsync(jsonData);
            Debug.Log("Response: " + response);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    private async UniTask<string> PostRequestAsync(string jsonData)
    {
        // Создаем UnityWebRequest
        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            // Устанавливаем заголовки
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AUTH_TOKEN);

            // Прикрепляем JSON-данные к телу запроса
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправляем запрос и ждем завершения
            await request.SendWebRequest().ToUniTask();

            // Обрабатываем ответ
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                throw new System.Exception(request.error);
            }
        }
    }
}
