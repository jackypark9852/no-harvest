using System.Collections;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardController : MonoBehaviour
{
    [Header("LootLocker API Settings")] 
    [SerializeField] string leaderboardKey = "player_leaderboard"; // This is the leaderboard ID from the LootLockers's dashboard


    [Header("Score Display Settings")]
    [SerializeField] int maxRows = 1000;
    [SerializeField] float scrollDelaysSeconds = 0.5f; // The delay before scrolling to the player's score
    [SerializeField] float scrollSpeed = 0.1f; // The speed of the scroll


    [Header("UI References")]
    [SerializeField] TMPro.TMP_InputField playerIDInputField; 
    [SerializeField] GameObject submitScorePanel;
    [SerializeField] GameObject leaderBoardPanel;
    [SerializeField] GameObject viewport; 
    [SerializeField] TMPro.TMP_Text scoreText;
    [Tooltip("This is the scroll rect that contains the leaderboard content")]
    [SerializeField] ScrollRect leaderboardScrollRect; 

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
                scoreText.text = "";
                for (int i = 0; i < scores.Length; i++) 
                {
                    string newText = string.Format("{0,4}.{1,-12}{2,8}\n", scores[i].rank, scores[i].member_id, scores[i].score);
                    scoreText.text += newText;
                }
                StartCoroutine(ScrollToPlayerScore(playerRank, response.items.Length));
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard");
            }
        });
    }

    // This method will scroll the leaderboard to the player's score
    IEnumerator ScrollToPlayerScore(int playerRank, int rankCount) {
        if(playerRank == -1) {
            throw new System.Exception("Player rank not set, please make sure player rank is set before calling this method");
        }
        yield return new WaitForSeconds(scrollDelaysSeconds); // Wait for the score text to be updated and rect transform to be updated
        float targetVerticalScrollPosition = CalculateVerticalScrollPosition(playerRank, rankCount);
        
        while(Mathf.Abs(leaderboardScrollRect.verticalNormalizedPosition - targetVerticalScrollPosition) > 0.01f) {
            leaderboardScrollRect.verticalNormalizedPosition = Mathf.SmoothStep(leaderboardScrollRect.verticalNormalizedPosition, targetVerticalScrollPosition, scrollSpeed);
            yield return new WaitForEndOfFrame();
        }
        // Debug.Log("Scrolling to position: " + targetVerticalScrollPosition);
    }

    // This method will calculate the normalized scroll position of the viewport
    float CalculateVerticalScrollPosition(int rank, int rankCount) {
        float viewportHeight = viewport.GetComponent<RectTransform>().rect.height; // The height of the viewport
        float totalHeight = scoreText.GetComponent<RectTransform>().rect.height; // The height of each score row
        float scoreRowHeight = totalHeight / rankCount; // The height of each score row 
        float scrollableHeight = totalHeight - viewportHeight; // The total height of the scroll view minus the height of the viewport
        float scrollPosition =  1 - Mathf.Min((((float)rank - 0.7f) * scoreRowHeight) / scrollableHeight, 1f); // The normalized scroll position of the viewport 

        Debug.Log("rank: " + rank);
        Debug.Log("viewportHeight: " + viewportHeight);
        Debug.Log("scoreRowHeight: " + scoreRowHeight);
        Debug.Log("totalHeight: " + totalHeight);
        Debug.Log("scrollableHeight: " + scrollableHeight);
        Debug.Log("scrollPosition: " + scrollPosition);
        return scrollPosition;
    }
}
