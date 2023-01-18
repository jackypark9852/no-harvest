using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;


public class LeaderboardController : MonoBehaviour
{
    [SerializeField] string leaderboardKey = "player_leaderboard"; // This is the leaderboard ID from the LootLockers's dashboard
    [SerializeField] TMPro.TMP_InputField playerIDInputField; 
    [SerializeField] TMPro.TMP_Text[] scoreTexts; 
    [SerializeField] GameObject submitScorePanel;
    [SerializeField] GameObject leaderBoardPanel;
    int score = 0; 

    void Start()
    {
        
        LootLockerSDKManager.StartGuestSession((response) => 
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
    }

    public void SubmitScore()
    {
        score = ScoreManager.Instance.score; 
        Debug.Log(playerIDInputField.text);
        LootLockerSDKManager.SubmitScore(playerIDInputField.text, score, leaderboardKey, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    public void DisplayScores() {
        submitScorePanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        LootLockerSDKManager.GetScoreList(leaderboardKey, scoreTexts.Length, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successfully retrieved leaderboard");
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scoreTexts.Length; i++)
                {
                    scoreTexts[i].text = scores[i].member_id + " " + scores[i].score;
                }
            }
            else
            {
                Debug.Log("Failed to retrieve leaderboard");
            }
        });
    }
}
