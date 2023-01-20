using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;


public class LeaderboardController : MonoBehaviour
{
    [SerializeField] string leaderboardKey = "player_leaderboard"; // This is the leaderboard ID from the LootLockers's dashboard
    [SerializeField] TMPro.TMP_InputField playerIDInputField; 
    [SerializeField] GameObject scoreRowsParent;
    [SerializeField] GameObject scoreRowsPrefab;
    [SerializeField] int maxRows = 500;
    [SerializeField] GameObject submitScorePanel;
    [SerializeField] GameObject leaderBoardPanel;
    int score = 0; 
    string member_id = null; 

    void Start()
    {
        
        LootLockerSDKManager.StartGuestSession((response) => 
        {
            if (!response.success)
            {
                // Debug.Log("error starting LootLocker session");

                return;
            }

            // Debug.Log("successfully started LootLocker session");
        });
    }

    public void SubmitScore()
    {
        score = ScoreManager.Instance.score; 
        member_id = playerIDInputField.text;
        LootLockerSDKManager.SubmitScore(playerIDInputField.text, score, leaderboardKey, (response) =>
        {
            if (response.statusCode == 200) {
                // Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    public void DisplayScores() {
        submitScorePanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        DisplayTopScores();
    }

    public void DisplayTopScores() {
        LootLockerSDKManager.GetScoreList(leaderboardKey, maxRows, (response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++) 
                {
                    GameObject scoreRow = Object.Instantiate(scoreRowsPrefab, scoreRowsParent.transform);
                    scoreRow.GetComponent<ScoreRow>().SetData(scores[i].rank, scores[i].member_id, scores[i].score);
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard");
            }
        });
    }
}
