using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardController : MonoBehaviour
{
    [Header("LootLocker API Settings")] 
    [Tooltip("Leaderboard key from the LootLockers's dashboard")]
    [SerializeField] private List<Tuple<leaderboardType, string>> _keys = new List<Tuple<leaderboardType, string>>(); // This is the leaderboard ID from the LootLockers's dashboard
    private Dictionary<leaderboardType, string> _keysDict = new Dictionary<leaderboardType, string>();

    [Header("Score Display Settings")]
    [SerializeField] private int _maxRows = 1000;
    [SerializeField] private float _scrollYoffset = 0.7f; // The offset of the scroll rect from the player's score
    [SerializeField] private float _scrollDelaysSeconds = 0.5f; // The delay before scrolling to the player's score
    [SerializeField] private float _scrollSpeed = 0.1f; // The speed of the scroll
    [SerializeField] private Color _columnLabelColor = Color.gray; // The color of the column labels
    [SerializeField] private Color _sortColumnLabelColor = Color.yellow; // The color of the column labels
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
    private string _submissionID = null;
    private Dictionary<leaderboardType,string> _scoresTextDict = new Dictionary<leaderboardType, string>();
    private Dictionary<leaderboardType, int> _playerRankDict = new Dictionary<leaderboardType, int>(); 
    private Dictionary<leaderboardType, bool> _submissionDoneFlags = new Dictionary<leaderboardType, bool>(); // The column that the leaderboard is sorted by
    private Coroutine _scrollCoroutine; 
    private bool _sdkInitialized = false;
    private void Awake() {
        // Initialize the SDK
        LootLockerSDKManager.StartGuestSession((response) =>  
        {
            if (response.success)
            {
                _sdkInitialized = true;
            } else {
                Debug.LogError("failed: " + response.Error);
                return; 
            }
        });
    }
    void Start()
    {
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
        if(!_sdkInitialized) {
            Debug.LogWarning("SDK not initialized");
            return;
        }

        _submissionID = System.Guid.NewGuid().ToString(); // Generate a unique ID for the submission 

        LeaderboardMetadata metadata = new LeaderboardMetadata();
        metadata.PlayerID = _playerIDInputField.text; 
        metadata.Score = ScoreManager.Instance.score;
        metadata.Day = RoundManager.Instance.roundNum;
        metadata.SubmissionID = _submissionID;
        metadata.SubmissionTime = System.DateTime.Now.ToString();
        string metaDataJSON = JsonUtility.ToJson(metadata);  // Convert the metadata to JSON string

        _submissionDoneFlags[leaderboardType.DayScore] = false; // Submission started
        LootLockerSDKManager.SubmitScore(metadata.SubmissionID, metadata.Day,  _keysDict[leaderboardType.DayScore], metaDataJSON, (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                _playerRankDict[leaderboardType.DayScore] = response.rank; // Cache the player's rank in the leaderboard
                _submissionDoneFlags[leaderboardType.DayScore] = true; // Submission done
            } else {
                Debug.LogError("failed: " + response.Error);
            }
        }));

        _submissionDoneFlags[leaderboardType.ScoreDay] = false; // Submission started
        LootLockerSDKManager.SubmitScore(metadata.SubmissionID, metadata.Score, _keysDict[leaderboardType.ScoreDay], metaDataJSON, (System.Action<LootLockerSubmitScoreResponse>)((response) =>
        {
            if (response.statusCode == 200) {
                _playerRankDict[leaderboardType.ScoreDay] = response.rank; // Cache the player's rank in the leaderboard
                _submissionDoneFlags[leaderboardType.ScoreDay] = true; // Submission done
            } else {
                Debug.LogError("failed: " + response.Error);
            }
        }));
    }
    public void SwitchToLeaderboardPanel() {
        _submitScorePanel.SetActive(false);
        _leaderBoardPanel.SetActive(true);
        DisplayScoreDay();
    }

    public void DisplayScoreDay() {
        StartCoroutine(Display(leaderboardType.ScoreDay));
    }

    public void DisplayDayScore() {
        StartCoroutine(Display(leaderboardType.DayScore));
    }

    IEnumerator Display(leaderboardType type) {
        if(_scrollCoroutine != null) {
            StopCoroutine(_scrollCoroutine); // Stop scrolling to player's score if it's still scrolling
        }

        if(_scoresTextDict.ContainsKey(type)) { // If the leaderboard text is already cached, display it
            _scoresText.text = _scoresTextDict[type];
            yield break;
        }

        // Check if all submissions are done, if not wait until they are before retrieving the leaderboard
        while(!_submissionDoneFlags.All(x => x.Value)) {
            yield return null;
        }

        LootLockerSDKManager.GetScoreList(_keysDict[type], _maxRows, (System.Action<LootLockerGetScoreListResponse>)((response) =>
        {
            if (response.statusCode == 200)
            {; 
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
        yield break; 
    }
    // This method will generate the leaderboard text
    string GenerateText(leaderboardType type, LootLockerLeaderboardMember[] scores) {
        string columnHex = ColorUtility.ToHtmlStringRGB(_columnLabelColor);
        string sortColumnColorHex = ColorUtility.ToHtmlStringRGB(_sortColumnLabelColor);
        string leaderboardText;
        switch (type)
        {
            case leaderboardType.DayScore:
                leaderboardText = string.Format("<color=#{0}> RANK  PLAYER ID     SCORE  <color=#{1}>DAY</color>\n</color>", columnHex, sortColumnColorHex);
                break;
            case leaderboardType.ScoreDay:
                leaderboardText = string.Format("<color=#{0}> RANK  PLAYER ID     <color=#{1}>SCORE</color>  DAY\n</color>", columnHex, sortColumnColorHex);
                break;
            default:
                leaderboardText = string.Format("<color=#{0}> RANK  PLAYER ID     SCORE  DAY\n</color>", columnHex); // Hardcoded for now
                break;
        }
        for (int i = 0; i < scores.Length; i++) 
        {
            LeaderboardMetadata metadata = JsonUtility.FromJson<LeaderboardMetadata>(scores[i].metadata); // Convert the metadata string to a LeaderboardMetadata object
            string newText = ""; 
            switch(type) {
                case leaderboardType.DayScore:
                    newText = string.Format("{0,4}.  {1,-13}{3,6}  {2,3}\n", scores[i].rank, metadata.PlayerID, metadata.Score, metadata.Day);
                    break;
                case leaderboardType.ScoreDay:
                    newText = string.Format("{0,4}.  {1,-13}{3,6}  {2,3}\n", scores[i].rank, metadata.PlayerID, metadata.Score, metadata.Day);
                    break;
                default:
                    throw new System.Exception("Leaderboard type not set");
            }
            
            if (metadata.SubmissionID == _submissionID) { 
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
            leaderboardScrollRect.verticalNormalizedPosition = Mathf.Lerp(leaderboardScrollRect.verticalNormalizedPosition, targetVerticalScrollPosition, _scrollSpeed);
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

// This is the metadata that will be stored in the LootLocker leaderboard
public struct LeaderboardMetadata { 
    public string SubmissionID;
    public string SubmissionTime; 
    public string PlayerID;
    public int Day;
    public int Score;
}