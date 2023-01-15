using Cysharp.Threading.Tasks;
using UnityEngine;

public enum PlantAnimationTrigger
{
    Spawn, 
    Idle, 
    Destroyed, 
}

public class PlantAnimation : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer; 

    Vector3 startingRotation;
    public float swayPeriod = 1.0f;
    public float swayAmount = 50f;
    float elapsedTime = 0.0f;

    public Material blinkMaterial;
    public int blinkCount = 2;
    public int blinkIntervalMillieSecond = 50;
    public float startingRotationZ;
    int score = 100; 

    [SerializeField] GameObject immuneSpriteGO;

    [SerializeField] GameObject scoreSpriteGO; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; 
        float sway = Mathf.Sin(elapsedTime * Mathf.PI * 2 / swayPeriod) * swayAmount;
        float x = transform.rotation.eulerAngles.x;
        float y = transform.rotation.eulerAngles.y; 
        transform.eulerAngles = new Vector3(x, y, startingRotationZ + sway);
    }

    public void PlaySpawnAnimation()
    {
        animator.SetTrigger(PlantAnimationTrigger.Spawn.ToString());
    }

    public void PlayDeathAnimation() 
    {
        PlayDeathBlinks();
    }

    async void PlayDeathBlinks()
    {
        Material original = spriteRenderer.material;
        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.material = blinkMaterial;
            await UniTask.Delay(blinkIntervalMillieSecond);
            spriteRenderer.material = original;
            await UniTask.Delay(blinkIntervalMillieSecond); 
        }
        InstantiateScoreSprite(100); 
    }

    public void InstantiateImmuneSprite()
    {
        Instantiate(immuneSpriteGO, transform.position, immuneSpriteGO.transform.rotation);

    }

    public void InstantiateScoreSprite(int score)
    {
        GameObject scoreSprite = Instantiate(scoreSpriteGO, transform.position, scoreSpriteGO.transform.rotation); 
    }
}
