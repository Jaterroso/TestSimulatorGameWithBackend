using UnityEngine;
using UnityEngine.UI;

public class BossCard : MonoBehaviour
{
    [Header("Stats")]
    public string bossName;
    public int winCount;
    public int bossHealth;
    public int bossId;

    [Header("UI")]
    public Text bossNameText;
    public Text winCountText;
    public Text bossHealthText;
    public Button button;

    public void Init()
    {
        bossNameText.text = bossName;
        winCountText.text = "Кол-во побед: " + winCount.ToString();
        bossHealthText.text = bossHealth.ToString();
        button.onClick.AddListener(() => AttackBoss(bossId));
    }

    private void AttackBoss(int id)
    {
        FindObjectOfType<Player>().AttackBoss(id);
    }
}
