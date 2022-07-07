using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    private Piece selectedPieceScript = null;
    private RaycastHit2D hit2D;
    private bool inQuestion = false;
    private GameObject buttonOk;
    private IEnumerator coroutine;
    private GameObject question;
    private AudioSource completedAudioSource;
    private int pirp = 0;
    private float timePassed = 0;
    private Text timeText;

    void Start()
    {
        question = GameObject.Find("Question").gameObject;
        List<Sprite> questions = new List<Sprite>();

        for (int i = 0; i < 16; i++)
        {
            questions.Add(Resources.Load<Sprite>($"kazanım{i + 1}"));
        }

        string[] ansArr = {"765998135", "4373", "120", "115", "12", "4550", "112", "90", "33", "71", "50", "17", "28", "50",
                            "0,20<0,25<0,33", "Bir tam 6'da 3", "1<3", "3/5", "8/10", "312", "20/10", "300", "Yaşınıza göre yapınız"};
        List<string> answers = new List<string>();

        for (int i = 0; i < 16; i++)
        {
            answers.Add(ansArr[i]);
        }

        int index = 16;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                int random = Random.Range(0, index);
                GameObject.Find($"Pieces_{x}_{y}").GetComponent<Piece>().question = questions[random];
                GameObject.Find($"Pieces_{x}_{y}").GetComponent<Piece>().answer = answers[random];
                GameObject.Find($"answer_{x}_{y}").GetComponent<TextMesh>().text = answers[random].ToString();
                questions.Remove(questions[random]);
                answers.Remove(answers[random]);
                index--;
            }
        }

        buttonOk = GameObject.Find("ButtonOK");
        buttonOk.SetActive(false);
        completedAudioSource = GameObject.Find("PuzzleCompletedSound").GetComponent<AudioSource>();
        timeText = GameObject.Find("Zaman").GetComponent<Text>();
    }

    IEnumerator ScaleUpAnimation(float time)
    {
        inQuestion = true;

        float i = 0;
        float rate = 1 / time;

        question.GetComponent<SpriteRenderer>().sprite = hit2D.transform.gameObject.GetComponent<Piece>().question;

        buttonOk.SetActive(true);

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            hit2D.transform.gameObject.transform.localScale = Vector3.Lerp(hit2D.transform.gameObject.transform.localScale, new Vector3(7.0f, 7.0f, 0.0f), i);
            hit2D.transform.gameObject.transform.position = Vector3.Lerp(hit2D.transform.gameObject.transform.position, new Vector3(0, 0, -2), i);
            question.transform.localScale = Vector3.Lerp(question.GetComponent<SpriteRenderer>().transform.localScale, new Vector3(0.86f, 0.6f, 0.0f), i);
            question.transform.position = Vector3.Lerp(question.GetComponent<SpriteRenderer>().transform.position, new Vector3(hit2D.transform.gameObject.transform.position.x, hit2D.transform.gameObject.transform.position.y, -3.0f), i);
            
            if (hit2D.transform.gameObject.transform.localScale == hit2D.transform.gameObject.GetComponent<Piece>().originalScale)
            {
                StopCoroutine(coroutine);
            }

            yield return 0;
        }
    }

    IEnumerator ScaleDownAnimation(float time)
    {
        float i = 0;
        float rate = 1 / time;

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            hit2D.transform.gameObject.transform.localScale = Vector3.Lerp(hit2D.transform.gameObject.transform.localScale, hit2D.transform.gameObject.GetComponent<Piece>().originalScale, i);
            hit2D.transform.gameObject.transform.position = Vector3.Lerp(hit2D.transform.gameObject.transform.position, hit2D.transform.gameObject.GetComponent<Piece>().messyPos, i);
            question.transform.localScale = Vector3.Lerp(question.GetComponent<SpriteRenderer>().transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), i);
            question.transform.position = Vector3.Lerp(question.GetComponent<SpriteRenderer>().transform.position, new Vector3(hit2D.transform.gameObject.transform.position.x, hit2D.transform.gameObject.transform.position.y, -4.74f), i);

            if (hit2D.transform.gameObject.transform.localScale == hit2D.transform.gameObject.GetComponent<Piece>().originalScale)
            {
                question.GetComponent<SpriteRenderer>().sprite = null;
                inQuestion = false;
                StopCoroutine(coroutine);
            }

            yield return 0;
        }

    }

    public void ButtonOk()
    {
        buttonOk.SetActive(false);
        StopCoroutine(coroutine);
        coroutine = ScaleDownAnimation(20.0f);
        StartCoroutine(coroutine);
    }

    private void Completed()
    {
        int random = Random.Range(0, 7);
        GameObject.Find("puzzle").transform.position = new Vector3(GameObject.Find("puzzle").transform.position.x, GameObject.Find("puzzle").transform.position.y, -1.15f);
        GameObject.Find("CompletedPic").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"CompletedPuzzle/{random}");
        completedAudioSource.Play();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inQuestion)
        {
            hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (hit2D.transform != null)
            {
                selectedPieceScript = hit2D.transform.gameObject.GetComponent<Piece>();
            }
        }
        if (Input.GetMouseButtonUp(0) && !inQuestion)
        {
            if (selectedPieceScript != null)
            {
                if (selectedPieceScript.inRightPos && !selectedPieceScript.locked)
                {
                    pirp = pirp + 1;
                    selectedPieceScript.locked = true;
                }
                selectedPieceScript = null;
                if (pirp == 16)
                {
                    Completed();
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && !inQuestion)
        {
            hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit2D.transform != null)
            {
                if (hit2D.transform.gameObject.name.StartsWith("Pieces") && !hit2D.transform.gameObject.GetComponent<Piece>().inRightPos)
                {
                    coroutine = ScaleUpAnimation(20.0f);
                    question.transform.position = hit2D.transform.position;
                    StartCoroutine(coroutine);
                }
            }
        }

        if (selectedPieceScript != null && !selectedPieceScript.inRightPos && !inQuestion)
        {
            selectedPieceScript.gameObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -2.0f);
        }

        if (pirp < 16)
        {
            timePassed += Time.deltaTime;
            timeText.text = ((int)timePassed).ToString();
        }
    }
}
