using UnityEngine;
using UnityEngine.UI;

public class FriendCard : MonoBehaviour
{
    [Header("Stats")]
    public string friendName;
    public int friendLevel;

    [Header("UI")]
    public Text friendNameText;
    public Text friendLevelText;

    public void Init()
    {
        friendLevelText.text = friendLevel.ToString();
        friendNameText.text = friendName;
    }
}
