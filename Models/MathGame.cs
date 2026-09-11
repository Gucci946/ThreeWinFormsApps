namespace ThreeWinFormsApps.Models;

public sealed class MathGame
{
    private readonly Random _random = new();

    public MathQuestion CurrentQuestion { get; private set; } = MathQuestion.Create(new Random(), 1);
    public int Score { get; private set; }
    public int CorrectAnswers { get; private set; }
    public int WrongAnswers { get; private set; }
    public int QuestionsAsked { get; private set; }
    public int Difficulty { get; private set; } = 1;

    public void Start(int difficulty)
    {
        Difficulty = difficulty;
        Score = 0;
        CorrectAnswers = 0;
        WrongAnswers = 0;
        QuestionsAsked = 0;
        NextQuestion();
    }

    public bool CheckAnswer(int answer)
    {
        QuestionsAsked++;
        var isCorrect = answer == CurrentQuestion.CorrectAnswer;
        if (isCorrect)
        {
            CorrectAnswers++;
            Score += 10 * Difficulty;
        }
        else
        {
            WrongAnswers++;
            Score = Math.Max(0, Score - 2);
        }

        return isCorrect;
    }

    public void NextQuestion()
    {
        CurrentQuestion = MathQuestion.Create(_random, Difficulty);
    }
}
