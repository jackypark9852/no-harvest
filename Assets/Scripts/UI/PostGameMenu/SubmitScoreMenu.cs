using UnityEngine;

public class SubmitScoreMenu : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text scoreText;
    [SerializeField] TMPro.TMP_Text scoreText2;
    private void OnEnable() {
        try {
            scoreText.text = $"Days: {RoundManager.Instance.roundNum}";
            scoreText2.text = $"Score: {ScoreManager.Instance.score}";
        } catch(System.Exception e) {
            Debug.Log(e);
        }
    }
}
