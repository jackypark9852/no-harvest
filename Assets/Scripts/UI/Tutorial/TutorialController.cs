using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] Transform tutorialScreenParent;
    List<GameObject> tutorialScreens;

    private int currentScreen = 0;

    void Awake()
    {
        tutorialScreens = GetChildGameObjects(tutorialScreenParent);
    }

    void Start()
    {
        if (GameManager.numPlays > 1)
        {
            GameManager.Instance.StartGame();
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("here");
            NextScreen();
        }
    }

    public void NextScreen()
    {
        tutorialScreens[currentScreen].SetActive(false);
        currentScreen++;
        if (currentScreen < tutorialScreens.Count)
        {
            tutorialScreens[currentScreen].SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            GameManager.Instance.StartGame();
        }
    }

    public List<GameObject> GetChildGameObjects(Transform parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform childTransform in parent)
        {
            children.Add(childTransform.gameObject);
        }
        return children;
    }
}
