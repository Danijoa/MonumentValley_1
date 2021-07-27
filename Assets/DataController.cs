using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ���� + �ҷ�����
public class DataController : MonoBehaviour
{
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    // �̱�������
    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                // �� ��ȯ�Ǿ �����Ǵ� ������Ʈ
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    // ���� ������ �����̸� ����
    public string GameDataFileName = "MonuStageData.json";
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            // ������ ���۵Ǹ� �ڵ����� ����ǵ���
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    private Scene scene;
    private void Start()
    {
        scene = SceneManager.GetActiveScene();

        LoadGameData();
        SaveGameData();
    }

    // ����� ���� �ҷ�����
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        // ����� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            Debug.Log("�ҷ����� ����");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        // ����� ������ ���ٸ�
        else
        {
            Debug.Log("���ο� ���� ����");
            _gameData = new GameData();
        }
    }

    // ���� �����ϱ�
    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        // �̹� ����� ������ �ִٸ� �����
        File.WriteAllText(filePath, ToJsonData);

        // �ùٸ��� ����ƴ��� Ȯ�� 
        Debug.Log("����Ϸ�");
        Debug.Log("Stage 1 " + gameData.isClear1);
        Debug.Log("Stage 2 " + gameData.isClear2);
    }

    // ������ �����ϸ� �ڵ�����ǵ���
    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void Update()
    {
        if (scene.name == "StartScene")
        {
            gameData.isClear1 = false;
            gameData.isClear2 = false;
        }

        if (gameData.isClear1 && scene.name != "Level2Scene")
        {
            SceneManager.LoadScene("Level2Scene");
        }
    }
}
