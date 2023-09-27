using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JackPot : MonoBehaviour
{
    // array of images
    [SerializeField] private Sprite[] sprites;

    // buttons
    public Button Roll_btn;
    public Button Stop_btn;
    public Button Back_btn;

    // images
    public Image img1;
    public Image img2;
    public Image img3;

    // texts
    public Text WinOrLose_txt;
    public Text score;

    // gameobject
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        score.text = player.Score.ToString();

        // add functions to buttons
        Roll_btn.onClick.AddListener(() => RollClicked());
        Stop_btn.onClick.AddListener(() => StopClicked());
        Back_btn.onClick.AddListener(() => BackClicked());
    }

    // when click roll button
    private void RollClicked()
    {
        WinOrLose_txt.text = "";

        Roll_btn.gameObject.SetActive(false);
        Stop_btn.gameObject.SetActive(true);

        InvokeRepeating("RollImg1", .1f, .01f);
        InvokeRepeating("RollImg2", .1f, .01f);
        InvokeRepeating("RollImg3", .1f, .01f);
    }

    // function to random images
    private void RandImg(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

    private void RollImg1() 
    {
        RandImg(img1.gameObject);
    }
    private void RollImg2()
    {
        RandImg(img2.gameObject);
    }
    private void RollImg3()
    {
        RandImg(img3.gameObject);
    }

    // when click stop button
    private void StopClicked() 
    {
        Stop_btn.gameObject.SetActive(false);
        Roll_btn.gameObject.SetActive(true);

        StartCoroutine(WaitForCancel());
    }

    // wait for images to stop
    IEnumerator WaitForCancel() 
    {
        for (int i = 1; i < 4; i++) 
        {
            yield return new WaitForSeconds(1.3f);
            CancelInvoke("RollImg" + i.ToString());
        }
        
        // check if images is equal
        if (img1.GetComponent<Image>().sprite == img2.GetComponent<Image>().sprite && img1.GetComponent<Image>().sprite == img3.GetComponent<Image>().sprite)
        {
            WinOrLose_txt.gameObject.GetComponent<Text>().text = "won";
            player.Score += 100;
            score.text = player.Score.ToString();
        }
        else
        {
            WinOrLose_txt.gameObject.GetComponent<Text>().text = "loser";
            player.Score -= 100;
            score.text = player.Score.ToString();
        }
    }
    // when click back button
    private void BackClicked()
    {
        SceneManager.LoadScene("Game");
    }
}
