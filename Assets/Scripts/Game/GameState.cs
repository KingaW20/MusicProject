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
    [SerializeField] public List<bool> AnswersCorrectness;

    //help data
    [SerializeField] public bool[] HelpUsed;

    [SerializeField] public string EnteredAnswer;

    public GameState()
    {
        ChoosedCategoryIds = new();
        CurrentMenuContext = MenuContext.MainContext;
        CurrentGameContext = GameContext.MainContext;

        AnswerWordNumber = Constants.FIRST_WORD_NUMBER;
        AnswersCorrectness = new();

        HelpUsed = new bool[3] { false, false, false };
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
        EnteredAnswer = gs.EnteredAnswer;
    }
}
