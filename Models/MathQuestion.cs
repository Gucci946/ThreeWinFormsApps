namespace ThreeWinFormsApps.Models;

public sealed class MathQuestion
{
    public int FirstNumber { get; }
    public int SecondNumber { get; }
    public char Operator { get; }
    public int CorrectAnswer { get; }

    private MathQuestion(int firstNumber, int secondNumber, char @operator, int correctAnswer)
    {
        FirstNumber = firstNumber;
        SecondNumber = secondNumber;
        Operator = @operator;
        CorrectAnswer = correctAnswer;
    }

    public static MathQuestion Create(Random random, int difficulty)
    {
        int max = difficulty switch
        {
            1 => 20,
            2 => 50,
            _ => 100
        };

        var operators = new[] { '+', '-', '×', '÷' };
        var op = operators[random.Next(operators.Length)];

        int a;
        int b;
        int answer;

        switch (op)
        {
            case '+':
                a = random.Next(1, max + 1);
                b = random.Next(1, max + 1);
                answer = a + b;
                break;
            case '-':
                a = random.Next(1, max + 1);
                b = random.Next(1, max + 1);
                if (b > a) (a, b) = (b, a);
                answer = a - b;
                break;
            case '×':
                a = random.Next(1, Math.Max(3, max / 2) + 1);
                b = random.Next(1, 13);
                answer = a * b;
                break;
            default:
                b = random.Next(1, 13);
                answer = random.Next(1, Math.Max(3, max / 2) + 1);
                a = b * answer;
                break;
        }

        return new MathQuestion(a, b, op, answer);
    }

    public override string ToString() => $"{FirstNumber}  {Operator}  {SecondNumber} = ?";
}
