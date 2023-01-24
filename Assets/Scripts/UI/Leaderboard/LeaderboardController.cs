using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardController : MonoBehaviour
{
    [Header("LootLocker API Settings")] 
    [SerializeField] private List<Tuple<leaderboardType, string>> _keys = new List<Tuple<leaderboardType, string>>(); // This is the leaderboard ID from the LootLockers's dashboard
    private Dictionary<leaderboardType, string> _keysDict = new Dictionary<leaderboardType, string>();

    [Header("Score Display Settings")]
    [SerializeField] private int _dayScoreMultiplier = 1000000; // The multiplier for the day score
    [SerializeField] private int _scoreDayMultiplier = 100; // The multiplier for the score day
    [SerializeField] private int _maxRows = 1000;
    [SerializeField] private float _scrollYoffset = 0.7f; // The offset of the scroll rect from the player's score
    [SerializeField] private float _scrollDelaysSeconds = 0.5f; // The delay before scrolling to the player's score
    [SerializeField] private float _scrollSpeed = 0.1f; // The speed of the scroll
    [SerializeField] private Color _columnLabelColor = Color.gray; // The color of the column labels
    [SerializeField] private Color _playerScoreColor = Color.red; // The color of the player's score
 

    [Header("UI References")]
    [SerializeField] private TMPro.TMP_InputField _playerIDInputField; 
    [SerializeField] private GameObject _submitScorePanel;
    [SerializeField] private GameObject _leaderBoardPanel;
    [SerializeField] private GameObject _viewport; 
    [SerializeField] private TMPro.TMP_Text _scoresText;
    [Tooltip("This is the scroll rect that contains the leaderboard content")]
    [SerializeField] private ScrollRect _scrollRect; 

    // cached values, used to scroll to the player's score
    private string _playerID = null; 
    private int _playerScore = -1;  
    private int _playerDay = -1; // The number of days survived by the player
    private Dictionary<leaderboardType,string> _scoresTextDict = new Dictionary<leaderboardType, string>();
    private Dictionary<leaderboardType, int> _playerRankDict = new Dictionary<leaderboardType, int>(); 
    private Coroutine _scrollCoroutine; 
    
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) => 
        {
            if (!response.success)
            {
                return;
            }
        });
        initializeLeaderboardKeyDict();
    }
    void initializeLeaderboardKeyDict() {
        foreach(Tuple<leaderboardType, string> leaderboardKey in _keys) {
            _keysDict.Add(leaderboardKey.Item1, leaderboardKey.Item2);
        }
    }
    // Submits score, days, and game result to leaderboards
    public void SubmitScore()
    {
        _playerID = _playerIDInputField.text;
        if(_playerID == "") { // Hardcoded for now
            // generate a random uid for playerID limited to 12 characters 
            _playerID = System.Guid.NewGuid().ToString().Substring(0,8);
        }
        _playerScore = ScoreManager.Instance.score; 
        _playerDay = RoundManager.Instance.roundNum;

        int day_score = _playerDay*_dayScoreMultiplier + _playerScore; 
        LootLockerSDKManager.SubmitScore(_playerID, day_score, _keysDict[leaderboardType.DayScore], (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                _playerRankDict[leaderboardType.DayScore] = response.rank; // Cache the player's rank in the leaderboard
            } else {
                Debug.Log("failed: " + response.Error);
            }
        }));

        int score_day = _playerScore*_scoreDayMultiplier + _playerDay; 
        LootLockerSDKManager.SubmitScore(_playerID, score_day, _keysDict[leaderboardType.ScoreDay], (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                _playerRankDict[leaderboardType.ScoreDay] = response.rank; // Cache the player's rank in the leaderboard
            } else {
                Debug.Log("failed: " + response.Error);
            }
        }));
    }
    public void SwitchToLeaderboardPanel() {
        _submitScorePanel.SetActive(false);
        _leaderBoardPanel.SetActive(true);
        DisplayScoreDay();
    }

    public void DisplayScoreDay() {
        Display(leaderboardType.ScoreDay);
    }

    public void DisplayDayScore() {
        Display(leaderboardType.DayScore);
    }

    void Display(leaderboardType type) {
        if(_scrollCoroutine != null) {
            StopCoroutine(_scrollCoroutine); // Stop scrolling to player's score if it's still scrolling
        }

        if(_scoresTextDict.ContainsKey(type)) { // If the leaderboard text is already cached, display it
            _scoresText.text = _scoresTextDict[type];
            return;
        }

        LootLockerSDKManager.GetScoreList(_keysDict[type], _maxRows, (System.Action<LootLockerGetScoreListResponse>)((response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] scores = response.items;
                _scoresText.text = GenerateText(type, scores);
                _scoresTextDict[type] = _scoresText.text;
                _scrollCoroutine = StartCoroutine(ScrollToPlayerScore(_playerRankDict[type], response.items.Length, _scrollRect));
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard");
            }
        }));
    }
    // This method will generate the leaderboard text
    string GenerateText(leaderboardType type, LootLockerLeaderboardMember[] scores) {
        string colorHex = ColorUtility.ToHtmlStringRGB(_columnLabelColor);
        string leaderboardText = string.Format("<color=#{0}> RANK  PLAYER ID     DAY   SCORE\n</color>", colorHex); // Hardcoded for now
        for (int i = 0; i < scores.Length; i++) 
        {
            string newText = ""; 
            switch(type) {
                case leaderboardType.DayScore:
                    newText = string.Format("{0,4}.  {1,-13} {2,3} {3,6}\n", scores[i].rank, scores[i].member_id, scores[i].score/_dayScoreMultiplier, scores[i].score%_dayScoreMultiplier);
                    break;
                case leaderboardType.ScoreDay:
                    newText = string.Format("{0,4}.  {1,-13} {2,3} {3,6}\n", scores[i].rank, scores[i].member_id, scores[i].score%_scoreDayMultiplier, scores[i].score/_scoreDayMultiplier);
                    break;
                default:
                    throw new System.Exception("Leaderboard type not set");
            }
            
            if (scores[i].member_id == _playerID) {
                newText = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(_playerScoreColor) ,newText); // Highlight the player's score
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
        yield return new WaitForSeconds(_scrollDelaysSeconds); // Wait for the score text to be updated and rect transform to be updated
        float targetVerticalScrollPosition = CalculateVerticalScrollPosition(playerRank, rankCount);
        
        while(Mathf.Abs(leaderboardScrollRect.verticalNormalizedPosition - targetVerticalScrollPosition) > 0.01f) {
            leaderboardScrollRect.verticalNormalizedPosition = Mathf.SmoothStep(leaderboardScrollRect.verticalNormalizedPosition, targetVerticalScrollPosition, _scrollSpeed);
            yield return new WaitForEndOfFrame();
        }
        _scrollCoroutine = null; 
        // Debug.Log("Scrolling to position: " + targetVerticalScrollPosition);
    }
    // This method will calculate the normalized scroll position of the viewport
    float CalculateVerticalScrollPosition(int rank, int rankCount) {
        float viewportHeight = _viewport.GetComponent<RectTransform>().rect.height; // The height of the viewport
        float totalHeight = _scoresText.GetComponent<RectTransform>().rect.height; // The height of each score row
        float scoreRowHeight = totalHeight / (rankCount + 1); // The height of each score row, +1 accounts for the column labels
        float scrollableHeight = totalHeight - viewportHeight; // The total height of the scroll view minus the height of the viewport
        // The normalized scroll position of the viewport
        float scrollPosition =  1 - Mathf.Min((((float)rank + _scrollYoffset) * scoreRowHeight) / scrollableHeight, 1f); 
        return scrollPosition;
    }
}

public enum leaderboardType{
    DayScore,
    ScoreDay,
    Uninitialized,
}
