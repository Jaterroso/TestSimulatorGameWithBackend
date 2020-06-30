using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class Player : MonoBehaviour
{
    public int playerId
    {
        get
        {
            return PlayerPrefs.GetInt("id");
        }
        set
        {
            PlayerPrefs.SetInt("id", value);
        }
    }

    [Header("Player's stats")]
    public int playerLevel;
    public int playerCoins;
    public string playerName;
    public int playerFistDamage;
    public int playerFlaskDamage;

    [Header("UI Stats")]
    public Text playerLevelText;
    public Text playerCoinsText;
    public Text playerNameText;
    public Text playerIdText;

    [Header("UI Boss fight")]
    public BossSlider bossSlider;
    public GameObject bossFightPanel;
    public GameObject bossWinPanel;
    public Text bossNameText;
    public Text bossHealthText;
    public Text playerFlaskText;

    [Header("UI boss win panel")]
    public Text winBossNameText;
    public Text winBossMaxHealthText;

    [Header("UI boss table")]
    public GameObject bossTablePanel;
    public Transform cardRoot;
    public GameObject bossCardPref;

    [Header("UI Friends")]
    public Transform friendRoot;
    public GameObject friendCardPref;

    private void Awake()
    {
        if (CheckUser())
        {
            LoadData();
            string fUrl = $"http://127.0.0.1:8000/get_friends/{playerId}/";
            StartCoroutine(LoadFriendsList(fUrl));
        }
        else
        {
            string url = $"http://127.0.0.1:8000/take_id/";
            StartCoroutine(TakeId(url));
        }
    }

    public bool CheckUser()
    {
        if (PlayerPrefs.GetInt("id") == 0)
        {
            return false;
        }
        return true;
    }

    public IEnumerator TakeId(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        playerId = data;
        LoadData();
    }

    public void AttackBoss(int bossId)
    {
        string url = $"http://127.0.0.1:8000/attack_boss/{playerId}/{bossId}/";
        StartCoroutine(AttackToBoss(url));
    }

    public void LoadData()
    {
        string url = $"http://127.0.0.1:8000/check_user/{playerId}/";
        StartCoroutine(LoadPlayerData(url));
    }

    public void OpenBossTable()
    {
        string url = $"http://127.0.0.1:8000/try_open_boss_table/{playerId}/";
        StartCoroutine(OpenBossTableIen(playerId, url));
    }

    public void AttackBossByFist()
    {
        string getBossUrl = $"http://127.0.0.1:8000/get_boss_id/{playerId}/";
        string url = $"http://127.0.0.1:8000/to_damage/{playerId}/{playerFistDamage}/";
        StartCoroutine(AttackBossByDamage(url));
    }

    public void RefreshBossFight()
    {
        StartCoroutine(RefreshBossFightIen());
    }

    public void ShowWinPanel()
    {
        string url = $"http://127.0.0.1:8000/get_boss/{playerId}/";
        StartCoroutine(GetBoss(playerId, url));
    }

    public void OpenTable()
    {
        string url = $"http://127.0.0.1:8000/get_bosses_table/";
        StartCoroutine(GetBossesTable(url));
    }

    public void TryToUseFlask()
    {
        string url = $"http://127.0.0.1:8000/try_to_use_flask/{playerId}/";
        StartCoroutine(TryToUseFlask(url));
    }

    public IEnumerator TryToUseFlask(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        if (data == "true")
        {
            string fUrl = $"http://127.0.0.1:8000/use_flask/{playerId}/";
            StartCoroutine(UseFlask(fUrl));
        }
        else if (data == "false")
        {

        }
    }

    public IEnumerator UseFlask(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        if (data == "UseFlask")
        {
            string dUrl = $"http://127.0.0.1:8000/to_damage/{playerId}/{playerFlaskDamage}/";
            StartCoroutine(AttackBossByDamage(dUrl));
        }
    }

    public IEnumerator RefreshBossFightIen()
    {
        string bUrl = $"http://127.0.0.1:8000/show_boss_fight/{playerId}/";
        var bReq = UnityWebRequest.Get(bUrl);
        yield return bReq.SendWebRequest();
        var bossData = JSON.Parse(bReq.downloadHandler.text);
        Debug.Log(bossData);
        bossNameText.text = bossData[0]["name"];
        var bossHealth = bossData[0]["health"];
        var bossMaxHealth = bossData[0]["max_health"];
        var flaskCount = bossData[1]["flask_count"].ToString();
        bossHealthText.text = $"{bossHealth} / {bossMaxHealth}";
        playerFlaskText.text = flaskCount;
        bossSlider.health = bossHealth;
        bossSlider.maxHealth = bossMaxHealth;
        bossSlider.Init();
        bossFightPanel.SetActive(true);
        if (bossHealth <= 0)
        {
            bossFightPanel.SetActive(false);
            ShowWinPanel();
        }
    }

    IEnumerator LoadPlayerData(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        Debug.Log(data);
        playerLevel = data["level"];
        playerCoins = data["coins"];
        playerName = data["name"].Value;
        playerFistDamage = data["fist_damage"];
        playerFlaskDamage = data["flask_damage"];
        playerLevelText.text = playerLevel.ToString();
        playerCoinsText.text = playerCoins.ToString();
        playerNameText.text = playerName;
        playerIdText.text = data["id"].ToString();
    }

    IEnumerator LoadFriendsList(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        Debug.Log(data);
    }

    IEnumerator OpenBossTableIen(int playerId, string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        if (data == "Fighting")
        {
            StartCoroutine(RefreshBossFightIen());
        }
        else if (data == "Win")
        {
            ShowWinPanel();
        }
        else if (data == "OpenTable")
        {
            OpenTable();
        }
    }

    IEnumerator AttackBossByDamage(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        if (data == "Damage")
        {
            RefreshBossFight();
        }
        else if (data == "Win")
        {
            bossFightPanel.SetActive(false);
            ShowWinPanel();
        }
    }

    IEnumerator GetBoss(int playerId, string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        var name = data["name"];
        var maxHealth = data["max_health"].ToString();
        winBossMaxHealthText.text = maxHealth;
        winBossNameText.text = name;
        bossWinPanel.SetActive(true);
    }

    IEnumerator GetBossesTable(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        if (cardRoot.childCount == 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var card = Instantiate(bossCardPref, cardRoot);
                card.GetComponent<BossCard>().bossName = data[i]["name"];
                card.GetComponent<BossCard>().bossHealth = data[i]["max_health"];
                card.GetComponent<BossCard>().bossId = data[i]["id"];
                card.GetComponent<BossCard>().Init();
            }
        }
        bossTablePanel.SetActive(true);
    }

    IEnumerator AttackToBoss(string url)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        var data = JSON.Parse(req.downloadHandler.text);
        Debug.Log(data);
        if (data == "StartFight")
        {
            StartCoroutine(RefreshBossFightIen());
            bossTablePanel.SetActive(false);
            bossFightPanel.SetActive(true);         
        }
    }
}
