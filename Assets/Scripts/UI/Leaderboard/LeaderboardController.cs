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
    [SerializeField] int dayScoreMultiplier = 1000000; // The multiplier for the day score
    [SerializeField] int scoreDayMultiplier = 100; // The multiplier for the score day
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
    int playerScore = -1;  
    int playerDay = -1; // The number of days survived by the player
    float viewportHeight; 
    Dictionary<leaderboardType,string> leaderboardTextDict = new Dictionary<leaderboardType, string>();
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
        playerID = playerIDInputField.text;
        if(playerID == "") { // Hardcoded for now
            // generate a random uid for playerID limited to 12 characters 
            playerID = System.Guid.NewGuid().ToString().Substring(0,8);
        }
        playerScore = ScoreManager.Instance.score; 
        playerDay = RoundManager.Instance.roundNum;

        int day_score = playerDay*dayScoreMultiplier + playerScore; 
        LootLockerSDKManager.SubmitScore(playerID, day_score, leaderboardKeyDict[leaderboardType.DAY_SCORE], (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                playerRankDict[leaderboardType.DAY_SCORE] = response.rank; // Cache the player's rank in the leaderboard
                // Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.Error);
            }
        }));

        int score_day = playerScore*scoreDayMultiplier + playerDay; 
        LootLockerSDKManager.SubmitScore(playerID, score_day, leaderboardKeyDict[leaderboardType.SCORE_DAY], (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                playerRankDict[leaderboardType.SCORE_DAY] = response.rank; // Cache the player's rank in the leaderboard
                // Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.Error);
            }
        }));
    }
    public void SwitchToLeaderboardPanel() {
        submitScorePanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        DisplayScoreDayLeaderboard();
    }

    public void DisplayScoreDayLeaderboard() {
        DisplayLeaderboard(leaderboardType.SCORE_DAY);
    }

    public void DisplayDayScoreLeaderboard() {
        DisplayLeaderboard(leaderboardType.DAY_SCORE);
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
                leaderboardScoresText.text = GetLeaderboardText(type, scores);
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
    string GetLeaderboardText(leaderboardType type, LootLockerLeaderboardMember[] scores) {
        string colorHex = ColorUtility.ToHtmlStringRGB(columnlabelColor);
        string leaderboardText = string.Format("<color=#{0}> RANK  PLAYER ID     DAY   SCORE\n</color>", colorHex); // Hardcoded for now
        for (int i = 0; i < scores.Length; i++) 
        {
            string newText = ""; 
            switch(type) {
                case leaderboardType.DAY_SCORE:
                    newText = string.Format("{0,4}.  {1,-13} {2,3} {3,6}\n", scores[i].rank, scores[i].member_id, scores[i].score/dayScoreMultiplier, scores[i].score%dayScoreMultiplier);
                    break;
                case leaderboardType.SCORE_DAY:
                    newText = string.Format("{0,4}.  {1,-13} {2,3} {3,6}\n", scores[i].rank, scores[i].member_id, scores[i].score%scoreDayMultiplier, scores[i].score/scoreDayMultiplier);
                    break;
                default:
                    throw new System.Exception("Leaderboard type not set");
            }
            
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
    DAY_SCORE,
    SCORE_DAY,
    UNINITIALIZED,
}
