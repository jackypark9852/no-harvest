using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;


public class LeaderboardController : MonoBehaviour
{
    [SerializeField] string leaderboardKey = "player_leaderboard"; // This is the leaderboard ID from the LootLockers's dashboard
    [SerializeField] TMPro.TMP_InputField playerIDInputField; 
    [SerializeField] List<ScoreRow> scoreRows; 
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
        DisplayTopScores(scoreRows.GetRange(0, scoreRows.Count - 1));
        DisplayPlayerScore(scoreRows[scoreRows.Count - 1]); 
        
    }

    public void DisplayTopScores(List<ScoreRow> scoreRows) {
        LootLockerSDKManager.GetScoreList(leaderboardKey, scoreRows.Count, (response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scoreRows.Count; i++) 
                {
                    ScoreRow scoreRow = scoreRows[i]; 
                    scoreRow.rank.text  = string.Format("{0}.", i + 1); 
                    scoreRow.name.text = scores[i].member_id;
                    scoreRow.score.text = string.Format("{0}", scores[i].score);
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard");
            }
        });
    }

    public void DisplayPlayerScore(ScoreRow scoreRow) {
        if(member_id != null) {
            // This is to get the rank of the player
            LootLockerSDKManager.GetMemberRank(leaderboardKey, member_id, (response) =>
            {
                if (response.statusCode == 200) {
                    scoreRow.rank.text = string.Format("{0}.", response.rank);
                    scoreRow.name.text = member_id;
                    scoreRow.score.text = string.Format("{0}", response.score);
                } else {
                    Debug.LogError("Failed to retrieve player rank: " + response.Error);
                }
            });
        } else { // If the player hasn't submitted a score yet
            scoreRow.rank.text = "";
            scoreRow.name.text = "";
            scoreRow.score.text = "";
        }
    }
}


[System.Serializable]
public struct ScoreRow {
    public TMPro.TMP_Text rank;
    public TMPro.TMP_Text name;
    public TMPro.TMP_Text score;
}