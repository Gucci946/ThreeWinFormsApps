using ThreeWinFormsApps.Models;

namespace ThreeWinFormsApps;

public sealed class MathGameForm : Form
{
    private readonly MathGame _game = new();
    private readonly Label _questionLabel = new();
    private readonly Label _scoreLabel = new();
    private readonly Label _timeLabel = new();
    private readonly Label _statsLabel = new();
    private readonly TextBox _answerBox = new();
    private readonly ComboBox _difficultyBox = new();
    private readonly Button _startButton = new();
    private readonly Button _answerButton = new();
    private readonly System.Windows.Forms.Timer _timer = new();
    private int _timeLeft;
    private bool _gameRunning;

    public MathGameForm()
    {
        Text = "Matemaatiline mäng";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(760, 570);
        MinimumSize = new Size(700, 540);
        BackColor = Color.FromArgb(245, 247, 251);
        Font = new Font("Segoe UI", 10F);

        BuildInterface();
        _timer.Interval = 1000;
        _timer.Tick += TimerTick;
        SetGameControlsEnabled(false);
        UpdateLabels();
    }

    private void BuildInterface()
    {
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 90,
            BackColor = Color.FromArgb(16, 185, 129)
        };
        Controls.Add(header);

        var title = new Label
        {
            Text = "Matemaatiline mäng",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 18)
        };
        header.Controls.Add(title);

        var subtitle = new Label
        {
            Text = "Kiirus + täpsus = rohkem punkte",
            ForeColor = Color.FromArgb(209, 250, 229),
            AutoSize = true,
            Location = new Point(34, 57)
        };
        header.Controls.Add(subtitle);

        var card = new Panel
        {
            Location = new Point(40, 115),
            Size = new Size(680, 355),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };
        Controls.Add(card);

        var difficultyLabel = new Label
        {
            Text = "Raskus:",
            AutoSize = true,
            Location = new Point(30, 28),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };
        card.Controls.Add(difficultyLabel);

        _difficultyBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _difficultyBox.Items.AddRange(new object[] { "1 - lihtne", "2 - keskmine", "3 - raske" });
        _difficultyBox.SelectedIndex = 0;
        _difficultyBox.Location = new Point(92, 24);
        _difficultyBox.Size = new Size(170, 30);
        card.Controls.Add(_difficultyBox);

        _timeLabel.AutoSize = true;
        _timeLabel.Location = new Point(500, 28);
        _timeLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        _timeLabel.ForeColor = Color.FromArgb(220, 38, 38);
        card.Controls.Add(_timeLabel);

        _questionLabel.TextAlign = ContentAlignment.MiddleCenter;
        _questionLabel.Font = new Font("Segoe UI", 34F, FontStyle.Bold);
        _questionLabel.Location = new Point(30, 85);
        _questionLabel.Size = new Size(620, 90);
        _questionLabel.ForeColor = Color.FromArgb(15, 23, 42);
        card.Controls.Add(_questionLabel);

        _answerBox.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        _answerBox.TextAlign = HorizontalAlignment.Center;
        _answerBox.Location = new Point(195, 190);
        _answerBox.Size = new Size(290, 50);
        _answerBox.KeyDown += AnswerBoxKeyDown;
        card.Controls.Add(_answerBox);

        _answerButton.Text = "Kontrolli vastust";
        _answerButton.Location = new Point(195, 255);
        _answerButton.Size = new Size(290, 45);
        _answerButton.BackColor = Color.FromArgb(16, 185, 129);
        _answerButton.ForeColor = Color.White;
        _answerButton.FlatStyle = FlatStyle.Flat;
        _answerButton.FlatAppearance.BorderSize = 0;
        _answerButton.Click += (_, _) => CheckAnswer();
        card.Controls.Add(_answerButton);

        _startButton.Text = "Alusta mängu";
        _startButton.Location = new Point(40, 485);
        _startButton.Size = new Size(210, 45);
        _startButton.BackColor = Color.FromArgb(37, 99, 235);
        _startButton.ForeColor = Color.White;
        _startButton.FlatStyle = FlatStyle.Flat;
        _startButton.FlatAppearance.BorderSize = 0;
        _startButton.Click += (_, _) => StartGame();
        Controls.Add(_startButton);

        _scoreLabel.AutoSize = true;
        _scoreLabel.Location = new Point(285, 499);
        _scoreLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        Controls.Add(_scoreLabel);

        _statsLabel.AutoSize = true;
        _statsLabel.Location = new Point(285, 525);
        _statsLabel.ForeColor = Color.FromArgb(100, 116, 139);
        Controls.Add(_statsLabel);
    }

    private void StartGame()
    {
        _game.Start(_difficultyBox.SelectedIndex + 1);
        _timeLeft = 60;
        _gameRunning = true;
        _timer.Start();
        SetGameControlsEnabled(true);
        _difficultyBox.Enabled = false;
        _answerBox.Clear();
        _answerBox.BackColor = Color.White;
        Text = "Matemaatiline mäng";
        UpdateLabels();
        _answerBox.Focus();
    }

    private void CheckAnswer()
    {
        if (!_gameRunning)
            return;

        if (!int.TryParse(_answerBox.Text.Trim(), out var answer))
        {
            MessageBox.Show("Sisesta täisarv.", "Vastuse kontroll", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _answerBox.Focus();
            return;
        }

        var correct = _game.CheckAnswer(answer);
        _answerBox.Clear();
        _game.NextQuestion();
        UpdateLabels();

        _answerBox.BackColor = correct
            ? Color.FromArgb(220, 252, 231)
            : Color.FromArgb(254, 226, 226);
        Text = correct ? "Matemaatiline mäng • Õige!" : "Matemaatiline mäng • Proovi järgmist!";

        var resetColorTimer = new System.Windows.Forms.Timer { Interval = 350 };
        resetColorTimer.Tick += (_, _) =>
        {
            resetColorTimer.Stop();
            resetColorTimer.Dispose();
            _answerBox.BackColor = Color.White;
            Text = "Matemaatiline mäng";
        };
        resetColorTimer.Start();
        _answerBox.Focus();
    }

    private void AnswerBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        CheckAnswer();
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        if (!_gameRunning)
            return;

        _timeLeft--;
        UpdateLabels();

        if (_timeLeft <= 0)
            EndGame();
    }

    private void EndGame()
    {
        if (!_gameRunning)
            return;

        _gameRunning = false;
        _timer.Stop();
        SetGameControlsEnabled(false);
        _difficultyBox.Enabled = true;
        Text = "Matemaatiline mäng";

        MessageBox.Show(
            $"Aeg sai otsa!\n\nPunktid: {_game.Score}\nÕigeid vastuseid: {_game.CorrectAnswers}\nValesid vastuseid: {_game.WrongAnswers}",
            "Mäng läbi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void SetGameControlsEnabled(bool enabled)
    {
        _answerBox.Enabled = enabled;
        _answerButton.Enabled = enabled;
    }

    private void UpdateLabels()
    {
        _questionLabel.Text = _game.CurrentQuestion.ToString();
        _timeLabel.Text = $"Aeg: {_timeLeft:00} s";
        _scoreLabel.Text = $"Punktid: {_game.Score}";
        _statsLabel.Text = $"Õiged: {_game.CorrectAnswers}    Valed: {_game.WrongAnswers}    Ülesanded: {_game.QuestionsAsked}";
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _timer.Stop();
        base.OnFormClosed(e);
    }
}
