using UnityEngine;
using UnityEngine.UI;

public class DamageFriendCard : MonoBehaviour
{
    public string playerName;
    public int damage;
    public int id;
    public Text nameText;
    public Text damageText;

    public void Init()
    {
        nameText.text = playerName;
        damageText.text = damage.ToString();
    }
}
