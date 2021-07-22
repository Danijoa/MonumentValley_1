using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// 데이터 저장 + 불러오기
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

    // 싱글톤으로
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
                // 씬 전환되어도 유지되는 오브젝트
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    // 게임 데이터 파일이름 설정
    public string GameDataFileName = "MonuStageData.json";
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            // 게임이 시작되면 자동으로 실행되도록
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    private void Start()
    {
        LoadGameData();
        SaveGameData();
    }

    // 저장된 게임 불러오기
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            Debug.Log("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        // 저장된 게임이 없다면
        else
        {
            Debug.Log("새로운 파일 생성");
            _gameData = new GameData();
        }
    }

    // 게임 저장하기
    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        // 이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, ToJsonData);

        // 올바르게 저장됐는지 확인 
        Debug.Log("저장완료");
        Debug.Log("Stage 1 " + gameData.isClear1);
        Debug.Log("Stage 2 " + gameData.isClear2);
    }

    // 게임을 종료하면 자동저장되도록
    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private bool once = true;
    private void Update()
    {
        if (once && gameData.isClear1)
        {
            CubeState.cubeNumber = 0;
            SceneManager.LoadScene("Level2Scene");
            once = false;
        }
    }
}
