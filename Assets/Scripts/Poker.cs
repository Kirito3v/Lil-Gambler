using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Poker : MonoBehaviour
{
    public Button Check_btn;
    public Button Fold_btn;
    public Button Raise_btn;
    public Button AllIn_btn;
    public Button Rest_btn;
    public Button Back_btn;

    public Text ScoreTxt;
    public Text BetTxt;

    public Text PlayerHandRank;
    public Text DealerHandRank;

    [SerializeField] private Deck2 deck;
    [SerializeField] private Player Player;

    // Start is called before the first frame update
    void Start()
    {
        Check_btn.onClick.AddListener(() => CheckClicked());
        Fold_btn.onClick.AddListener(() => FoldClicked());
        Raise_btn.onClick.AddListener(() => RaiseClicked());
        AllIn_btn.onClick.AddListener(() => BackClicked());
        Rest_btn.onClick.AddListener(() => RestClicked());
        Back_btn.onClick.AddListener(() => BackClicked());

        ScoreTxt.text = Player.Score.ToString();
        BetTxt.text = deck.bet.ToString();
    }

    private void CheckClicked() 
    {
        deck.Check();
    }

    private void FoldClicked()
    {
        deck.Fold();
    }

    private void RaiseClicked()
    {
        deck.Raise();
        BetTxt.text = deck.bet.ToString();
    }

    private void RestClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BackClicked()
    {
        SceneManager.LoadScene("Game");
    }
}
