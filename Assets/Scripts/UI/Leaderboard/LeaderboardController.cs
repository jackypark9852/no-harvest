using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardController : MonoBehaviour
{
    [Header("LootLocker API Settings")] 
    [SerializeField] List<Tuple<leaderboardType, string>> leaderboardKeys = new List<Tuple<leaderboardType, string>>(); // This is the leaderboard ID from the LootLockers's dashboard
    Dictionary<leaderboardType, string> leaderboardKeyDict = new Dictionary<leaderboardType, string>();

    [Header("Score Display Settings")]
    [SerializeField] int maxRows = 1000;
    [SerializeField] float scrollYoffset = 0.7f; // The offset of the scroll rect from the player's score
    [SerializeField] float scrollDelaysSeconds = 0.5f; // The delay before scrolling to the player's score
    [SerializeField] float scrollSpeed = 0.1f; // The speed of the scroll
    [SerializeField] Color columnlabelColor = Color.gray; // The color of the column labels
    [SerializeField] Color playerScoreColor = Color.red; // The color of the player's score
 

    [Header("UI References")]
    [SerializeField] TMPro.TMP_InputField playerIDInputField; 
    [SerializeField] GameObject submitScorePanel;
    [SerializeField] GameObject leaderBoardPanel;
    [SerializeField] GameObject viewport; 
    [SerializeField] TMPro.TMP_Text leaderboardScoresText;
    [Tooltip("This is the scroll rect that contains the leaderboard content")]
    [SerializeField] ScrollRect leaderboardScrollRect; 

    // cached values, used to scroll to the player's score
    string playerID = null; 
    float viewportHeight; 
    Dictionary<leaderboardType,string> leaderboardTextDict = new Dictionary<leaderboardType, string>();
    Dictionary<leaderboardType, int> playerScoreDict = new Dictionary<leaderboardType, int>(); 
    Dictionary<leaderboardType, int> playerRankDict = new Dictionary<leaderboardType, int>(); 
    Coroutine scrollCoroutine = null;
    
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
        initializeLeaderboardKeyDict();
    }
    void initializeLeaderboardKeyDict() {
        foreach(Tuple<leaderboardType, string> leaderboardKey in leaderboardKeys) {
            leaderboardKeyDict.Add(leaderboardKey.Item1, leaderboardKey.Item2);
        }
    }
    // Submits score, days, and game result to leaderboards
    public void SubmitScore()
    {
        if(playerIDInputField.text == "") { // Hardcoded for now
            // generate a random uid for playerID limited to 12 characters 
            playerIDInputField.text = System.Guid.NewGuid().ToString().Substring(0,8);
        }

        playerScoreDict.Add(leaderboardType.SCORES, ScoreManager.Instance.score);
        playerID = playerIDInputField.text;
        LootLockerSDKManager.SubmitScore(playerIDInputField.text, playerScoreDict[leaderboardType.SCORES], leaderboardKeyDict[leaderboardType.SCORES], (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                playerRankDict.Add(leaderboardType.SCORES,response.rank); 
                // Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.Error);
            }
        }));

        playerScoreDict[leaderboardType.DAYS] = RoundManager.Instance.roundNum;
        LootLockerSDKManager.SubmitScore(playerIDInputField.text, playerScoreDict[leaderboardType.DAYS], leaderboardKeyDict[leaderboardType.DAYS], (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                playerRankDict.Add(leaderboardType.DAYS,response.rank); 
                // Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.Error);
            }
        }));

        // Submit days and score to leaderboard for analytics
        // Generate a random UID for memberid to avoid duplicate submissions
        string memberUID = System.Guid.NewGuid().ToString();
        string gameResultData = string.Format("{{\"score\":{0},\"days\":{1}}}", playerScoreDict[leaderboardType.SCORES], playerScoreDict[leaderboardType.DAYS]); // submitted as metadata field in LootLocker API
        LootLockerSDKManager.SubmitScore(memberUID, 0, leaderboardKeyDict[leaderboardType.DAYS_SCORES], gameResultData, (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                // Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.Error);
            }
        }));
    
    }
    public void SwitchToLeaderboardPanel() {
        submitScorePanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        DisplayScoresLeaderboard();
    }
    public void DisplayScoresLeaderboard() {
        DisplayLeaderboard(leaderboardType.SCORES);
    }
    public void DisplayDaysLeaderboard() {
        DisplayLeaderboard(leaderboardType.DAYS);
    }
    void DisplayLeaderboard(leaderboardType type) {
        if(scrollCoroutine != null) {
            StopCoroutine(scrollCoroutine); // Stop scrolling to player's score if it's still scrolling
        }

        if(leaderboardTextDict.ContainsKey(type)) { // If the leaderboard text is already cached, display it
            leaderboardScoresText.text = leaderboardTextDict[type];
            return;
        }

        LootLockerSDKManager.GetScoreList(leaderboardKeyDict[type], maxRows, (System.Action<LootLockerGetScoreListResponse>)((response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] scores = response.items;
                leaderboardScoresText.text = GetLeaderboardText(scores, type.ToString());
                leaderboardTextDict[type] = leaderboardScoresText.text;
                scrollCoroutine = StartCoroutine(ScrollToPlayerScore(playerRankDict[type], response.items.Length, leaderboardScrollRect));
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard");
            }
        }));
    }
    // This method will generate the leaderboard text
    string GetLeaderboardText(LootLockerLeaderboardMember[] scores, string scoreColumnLabel) {
        string colorHex = ColorUtility.ToHtmlStringRGB(columnlabelColor);
        string leaderboardText = string.Format("<color=#{0}>{1,4}. {2,-12}{3,8}\n</color>", colorHex, "RANK", "PLAYER ID", scoreColumnLabel); // Set the column labels
        for (int i = 0; i < scores.Length; i++) 
        {
            string newText = string.Format("{0,4}. {1,-12}{2,8}\n", scores[i].rank, scores[i].member_id, scores[i].score);
            if (scores[i].member_id == playerID) {
                newText = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(playerScoreColor) ,newText); // Highlight the player's score
            }
            leaderboardText += newText;
        }
        return leaderboardText;
    }
    // This method will scroll the leaderboard to the player's score
    IEnumerator ScrollToPlayerScore(int playerRank, int rankCount, ScrollRect leaderboardScrollRect) {
        if(playerRank == -1) {
            throw new System.Exception("Player rank not set, please make sure player rank is set before calling this method");
        }
        yield return new WaitForSeconds(scrollDelaysSeconds); // Wait for the score text to be updated and rect transform to be updated
        float targetVerticalScrollPosition = CalculateVerticalScrollPosition(playerRank, rankCount);
        
        while(Mathf.Abs(leaderboardScrollRect.verticalNormalizedPosition - targetVerticalScrollPosition) > 0.005f) {
            leaderboardScrollRect.verticalNormalizedPosition = Mathf.SmoothStep(leaderboardScrollRect.verticalNormalizedPosition, targetVerticalScrollPosition, scrollSpeed);
            yield return new WaitForEndOfFrame();
        }
        // Debug.Log("Scrolling to position: " + targetVerticalScrollPosition);
    }
    // This method will calculate the normalized scroll position of the viewport
    float CalculateVerticalScrollPosition(int rank, int rankCount) {
        float viewportHeight = viewport.GetComponent<RectTransform>().rect.height; // The height of the viewport
        float totalHeight = leaderboardScoresText.GetComponent<RectTransform>().rect.height; // The height of each score row
        float scoreRowHeight = totalHeight / (rankCount + 1); // The height of each score row, +1 accounts for the column labels
        float scrollableHeight = totalHeight - viewportHeight; // The total height of the scroll view minus the height of the viewport
        // The normalized scroll position of the viewport
        float scrollPosition =  1 - Mathf.Min((((float)rank + scrollYoffset) * scoreRowHeight) / scrollableHeight, 1f); 
        return scrollPosition;
    }
}

public enum leaderboardType{
    SCORES, 
    DAYS,
    DAYS_SCORES,
    UNINITIALIZED,
}
