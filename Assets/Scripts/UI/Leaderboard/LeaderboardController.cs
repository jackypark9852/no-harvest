using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;


public class LeaderboardController : MonoBehaviour
{
    [Header("LootLocker API Settings")] 
    [SerializeField] string leaderboardKey = "player_leaderboard"; // This is the leaderboard ID from the LootLockers's dashboard


    [Header("Score Rows")]
    [SerializeField] GameObject scoreRowsParent;
    [SerializeField] GameObject scoreRowsPrefab;
    [SerializeField] int maxRows = 500;

    [Header("UI References")]
    [SerializeField] TMPro.TMP_InputField playerIDInputField; 
    [SerializeField] GameObject submitScorePanel;
    [SerializeField] GameObject leaderBoardPanel;

    int score = 0; 
    string playerID = null; 
    int playerRank = -1;
    float viewportHeight; 
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
        playerID = playerIDInputField.text;
        LootLockerSDKManager.SubmitScore(playerIDInputField.text, score, leaderboardKey, (response) =>
        {
            if (response.statusCode == 200) {
                playerRank = response.rank;
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
        Debug.Log("Displaying top scores");
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
                ScrollToPlayerScore();
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard");
            }
        });
    }

    void ScrollToPlayerScore() {
        if(playerRank == -1) {
            throw new System.Exception("Player rank not set, please make sure player rank is set before calling this method");
        }

        float targetVerticalScrollPosition = CalculateVerticalScrollPosition(playerRank);
        Debug.Log("Scrolling to position: " + targetVerticalScrollPosition);
    }

    float CalculateVerticalScrollPosition(int rank) {
        float viewportHeight = scoreRowsParent.transform.parent.GetComponent<RectTransform>().rect.height; // The height of the viewport
        float scoreRowHeight = scoreRowsPrefab.GetComponent<RectTransform>().rect.height; // The height of each score row
        float totalHeight = scoreRowsParent.transform.childCount * scoreRowHeight; // The total height of the scroll view
        float scrollableHeight = totalHeight - viewportHeight; // The total height of the scroll view minus the height of the viewport
        float scrollPosition =  1 - Mathf.Min((rank * scoreRowHeight) / totalHeight, 1f); // The normalized scroll position of the viewport 

        Debug.Log("rank: " + rank);
        Debug.Log("viewportHeight: " + viewportHeight);
        Debug.Log("scoreRowHeight: " + scoreRowHeight);
        Debug.Log("totalHeight: " + totalHeight);
        Debug.Log("scrollableHeight: " + scrollableHeight);
        Debug.Log("scrollPosition: " + scrollPosition);
        return scrollPosition;
    }
}
