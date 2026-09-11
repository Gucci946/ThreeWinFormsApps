using ThreeWinFormsApps.Models;

namespace ThreeWinFormsApps;

public sealed class MemoryGameForm : Form
{
    private readonly MemoryGame _game = new();
    private readonly FlowLayoutPanel _board = new();
    private readonly ComboBox _sizeBox = new();
    private readonly Label _movesLabel = new();
    private readonly Label _pairsLabel = new();
    private readonly Label _timeLabel = new();
    private readonly Label _bestLabel = new();
    private readonly Label _scoreLabel = new();
    private readonly System.Windows.Forms.Timer _timer = new();
    private readonly List<string> _assetPaths = new();
    private MemoryCard? _firstCard;
    private MemoryCard? _secondCard;
    private PictureBox? _firstControl;
    private PictureBox? _secondControl;
    private bool _waiting;
    private int _elapsedSeconds;
    private int _bestScore;
    private int _currentScore;

    public MemoryGameForm()
    {
        Text = "Sarnaste piltide mäng";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(920, 760);
        MinimumSize = new Size(850, 700);
        BackColor = Color.FromArgb(245, 247, 251);
        Font = new Font("Segoe UI", 10F);

        LoadAssetPaths();
        BuildInterface();

        _timer.Interval = 1000;
        _timer.Tick += TimerTick;
        StartNewGame();
    }

    private void LoadAssetPaths()
    {
        var basePath = Path.Combine(AppContext.BaseDirectory, "Assets");
        if (Directory.Exists(basePath))
        {
            _assetPaths.AddRange(Directory.GetFiles(basePath, "*.png").OrderBy(Path.GetFileName));
        }
    }

    private void BuildInterface()
    {
        var top = new Panel
        {
            Dock = DockStyle.Top,
            Height = 90,
            BackColor = Color.FromArgb(124, 58, 237)
        };
        Controls.Add(top);

        var title = new Label
        {
            Text = "Sarnaste piltide mäng",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(24, 17)
        };
        top.Controls.Add(title);

        _sizeBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _sizeBox.Items.AddRange(new object[] { "4 × 4", "6 × 6" });
        _sizeBox.SelectedIndex = 0;
        _sizeBox.Location = new Point(555, 25);
        _sizeBox.Size = new Size(105, 30);
        top.Controls.Add(_sizeBox);

        var newGameButton = new Button
        {
            Text = "Uus mäng",
            Location = new Point(675, 23),
            Size = new Size(110, 34),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(91, 33, 182),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        newGameButton.FlatAppearance.BorderSize = 0;
        newGameButton.Click += (_, _) => StartNewGame();
        top.Controls.Add(newGameButton);

        var footer = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 55,
            BackColor = Color.White,
            Padding = new Padding(30, 12, 30, 8)
        };
        Controls.Add(footer);

        AddFooterLabel(_movesLabel, footer, "Käigud: 0", 0);
        AddFooterLabel(_pairsLabel, footer, "Paarid: 0/0", 150);
        AddFooterLabel(_timeLabel, footer, "Aeg: 0 s", 300);
        AddFooterLabel(_scoreLabel, footer, "Skoor: 0", 430);
        AddFooterLabel(_bestLabel, footer, "Parim skoor: 0", 555);
        _bestLabel.ForeColor = Color.FromArgb(91, 33, 182);
        _scoreLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

        _board.Dock = DockStyle.Fill;
        _board.WrapContents = true;
        _board.AutoScroll = true;
        _board.Padding = new Padding(20);
        _board.BackColor = Color.White;
        Controls.Add(_board);
    }

    private static void AddFooterLabel(Label label, Control parent, string text, int x)
    {
        label.Text = text;
        label.AutoSize = true;
        label.Location = new Point(x, 10);
        parent.Controls.Add(label);
    }

    private void StartNewGame()
    {
        var size = _sizeBox.SelectedIndex == 1 ? 6 : 4;
        if (_assetPaths.Count < size * size / 2)
        {
            MessageBox.Show(
                $"Assets-kaustas peab olema vähemalt {size * size / 2} PNG-pilti.",
                "Viga",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        _timer.Stop();
        _game.Start(_assetPaths, size, size);
        _elapsedSeconds = 0;
        _currentScore = 10000;
        _firstCard = null;
        _secondCard = null;
        _firstControl = null;
        _secondControl = null;
        _waiting = false;

        BuildBoard(size);
        _timer.Start();
        UpdateLabels();
    }

    private void BuildBoard(int size)
    {
        _board.SuspendLayout();
        ClearBoardImages();
        _board.Controls.Clear();

        var gap = 8;
        var availableWidth = Math.Max(200, _board.ClientSize.Width - _board.Padding.Horizontal - gap * (size - 1) - 20);
        var availableHeight = Math.Max(200, _board.ClientSize.Height - _board.Padding.Vertical - gap * (size - 1) - 20);
        var cardSize = Math.Max(70, Math.Min(availableWidth / size, availableHeight / size));

        foreach (var card in _game.Cards)
        {
            var picture = CreateCardControl(card, cardSize);
            picture.Tag = card;
            picture.Click += CardClicked;
            _board.Controls.Add(picture);
        }

        _board.ResumeLayout(true);
    }

    private static PictureBox CreateCardControl(MemoryCard card, int size)
    {
        return new PictureBox
        {
            Size = new Size(size, size),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(37, 99, 235),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(5),
            Cursor = Cursors.Hand,
            Image = CreateBackImage(size)
        };
    }

    private static Bitmap CreateBackImage(int size)
    {
        var bitmap = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.FromArgb(37, 99, 235));
        using var pen = new Pen(Color.FromArgb(147, 197, 253), 3);
        graphics.DrawRectangle(pen, new Rectangle(10, 10, size - 20, size - 20));
        using var font = new Font("Segoe UI", Math.Max(18, size / 4), FontStyle.Bold);
        using var brush = new SolidBrush(Color.White);
        const string text = "?";
        var textSize = graphics.MeasureString(text, font);
        graphics.DrawString(text, font, brush, (size - textSize.Width) / 2, (size - textSize.Height) / 2);
        return bitmap;
    }

    private static Image LoadImageWithoutLock(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var temporaryImage = Image.FromStream(stream);
        return new Bitmap(temporaryImage);
    }

    private void CardClicked(object? sender, EventArgs e)
    {
        if (_waiting || sender is not PictureBox picture || picture.Tag is not MemoryCard card)
            return;
        if (card.IsMatched || card.IsRevealed)
            return;

        card.IsRevealed = true;
        RevealCard(picture, card);

        if (_firstCard is null)
        {
            _firstCard = card;
            _firstControl = picture;
            return;
        }

        _secondCard = card;
        _secondControl = picture;
        _game.RegisterMove();

        if (_game.ArePair(_firstCard, _secondCard))
        {
            _firstCard.IsMatched = true;
            _secondCard.IsMatched = true;
            _game.RegisterMatch();
            _firstControl!.BackColor = Color.FromArgb(220, 252, 231);
            _secondControl!.BackColor = Color.FromArgb(220, 252, 231);
            ResetSelection();

            if (_game.IsComplete)
                FinishGame();
        }
        else
        {
            _waiting = true;
            var hideTimer = new System.Windows.Forms.Timer { Interval = 750 };
            hideTimer.Tick += (_, _) =>
            {
                hideTimer.Stop();
                hideTimer.Dispose();
                HideCard(_firstControl!, _firstCard!);
                HideCard(_secondControl!, _secondCard!);
                ResetSelection();
            };
            hideTimer.Start();
        }

        UpdateLabels();
    }

    private void RevealCard(PictureBox picture, MemoryCard card)
    {
        picture.Image?.Dispose();
        picture.Image = LoadImageWithoutLock(card.ImagePath);
        picture.BackColor = Color.White;
    }

    private static void HideCard(PictureBox picture, MemoryCard card)
    {
        card.IsRevealed = false;
        picture.Image?.Dispose();
        picture.Image = CreateBackImage(picture.Width);
        picture.BackColor = Color.FromArgb(37, 99, 235);
    }

    private void ResetSelection()
    {
        _firstCard = null;
        _secondCard = null;
        _firstControl = null;
        _secondControl = null;
        _waiting = false;
        UpdateLabels();
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        _elapsedSeconds++;
        _currentScore = _game.CalculateScore(_elapsedSeconds);
        UpdateLabels();
    }

    private void FinishGame()
    {
        _timer.Stop();
        _currentScore = _game.CalculateScore(_elapsedSeconds);
        if (_currentScore > _bestScore)
            _bestScore = _currentScore;

        UpdateLabels();
        MessageBox.Show(
            $"Kõik paarid leitud!\n\nKäigud: {_game.Moves}\nAeg: {_elapsedSeconds} sekundit\nSkoor: {_currentScore}",
            "Mäng läbi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void UpdateLabels()
    {
        _movesLabel.Text = $"Käigud: {_game.Moves}";
        _pairsLabel.Text = $"Paarid: {_game.MatchedPairs}/{_game.PairCount}";
        _timeLabel.Text = $"Aeg: {_elapsedSeconds} s";
        _scoreLabel.Text = $"Skoor: {_currentScore}";
        _bestLabel.Text = $"Parim skoor: {_bestScore}";
    }

    private void ClearBoardImages()
    {
        foreach (Control control in _board.Controls)
        {
            if (control is PictureBox picture)
            {
                picture.Image?.Dispose();
                picture.Image = null;
            }
            control.Dispose();
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _timer.Stop();
        ClearBoardImages();
        base.OnFormClosed(e);
    }
}
