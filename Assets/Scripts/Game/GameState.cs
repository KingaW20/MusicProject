using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameState
{
    [SerializeField] public List<int> ChoosedCategoryIds;
    [SerializeField] public MenuContext CurrentMenuContext;
    [SerializeField] public GameContext CurrentGameContext;

    //songs
    [SerializeField] public List<Category> SelectedCategories;
    [SerializeField] public Category CategoryForChange;
    [SerializeField] public Song HitSong;

    //answering
    [SerializeField] public int AnswerWordNumber;
    [SerializeField] public List<CorrectnessType> AnswersCorrectness;

    //help data
    [SerializeField] public bool[] HelpUsed;
    [SerializeField] public bool[] HelpJustUsed;
    [SerializeField] public string ChangeInfoText;
    [SerializeField] public List<int> ChoosedWordIds;

    [SerializeField] public string EnteredAnswer;

    public GameState()
    {
        ChoosedCategoryIds = new();
        CurrentMenuContext = MenuContext.MainContext;
        CurrentGameContext = GameContext.MainContext;

        AnswerWordNumber = Constants.FIRST_WORD_NUMBER;
        AnswersCorrectness = new();

        HelpUsed = new bool[3] { false, false, false };
        HelpJustUsed = new bool[3] { false, false, false };
        ChangeInfoText = "";
        ChoosedWordIds = new();
        EnteredAnswer = "";
    }

    public GameState(GameState gs)
    {
        ChoosedCategoryIds = gs.ChoosedCategoryIds.ToList();
        CurrentMenuContext = gs.CurrentMenuContext;
        CurrentGameContext = gs.CurrentGameContext;

        SelectedCategories = gs.SelectedCategories.Select(item => new Category(item)).ToList();
        CategoryForChange = new Category(gs.CategoryForChange);
        HitSong = new Song(gs.HitSong);

        AnswerWordNumber = gs.AnswerWordNumber;
        AnswersCorrectness = gs.AnswersCorrectness.ToList();

        HelpUsed = gs.HelpUsed.ToArray();
        HelpJustUsed = gs.HelpJustUsed.ToArray();
        ChangeInfoText = gs.ChangeInfoText;
        ChoosedWordIds = gs.ChoosedWordIds.ToList();
        EnteredAnswer = gs.EnteredAnswer;
    }
}
