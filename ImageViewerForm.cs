namespace ThreeWinFormsApps;

public sealed class ImageViewerForm : Form
{
    private readonly PictureBox _pictureBox = new();
    private readonly Label _statusLabel = new();
    private readonly System.Windows.Forms.Timer _slideTimer = new();
    private readonly List<string> _imageFiles = new();
    private int _currentIndex = -1;
    private bool _slideshowRunning;

    public ImageViewerForm()
    {
        Text = "Pildi vaatamise programm";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1000, 700);
        MinimumSize = new Size(820, 600);
        BackColor = Color.FromArgb(245, 247, 251);
        Font = new Font("Segoe UI", 10F);

        BuildInterface();
        _slideTimer.Interval = 2200;
        _slideTimer.Tick += (_, _) => ShowNextImage();
    }

    private void BuildInterface()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = Color.FromArgb(31, 41, 55) };
        Controls.Add(top);

        var openButton = CreateButton("Ava pildid", Color.FromArgb(37, 99, 235));
        openButton.Click += (_, _) => OpenImages();
        top.Controls.Add(openButton);

        var previousButton = CreateButton("‹ Eelmine", Color.FromArgb(71, 85, 105));
        previousButton.Location = new Point(120, 12);
        previousButton.Click += (_, _) => ShowPreviousImage();
        top.Controls.Add(previousButton);

        var nextButton = CreateButton("Järgmine ›", Color.FromArgb(71, 85, 105));
        nextButton.Location = new Point(230, 12);
        nextButton.Click += (_, _) => ShowNextImage();
        top.Controls.Add(nextButton);

        var slideButton = CreateButton("▶ Slaidiesitlus", Color.FromArgb(16, 185, 129));
        slideButton.Location = new Point(345, 12);
        slideButton.Click += (_, _) => ToggleSlideshow(slideButton);
        top.Controls.Add(slideButton);

        var bgButton = CreateButton("Taustavärv", Color.FromArgb(124, 58, 237));
        bgButton.Location = new Point(495, 12);
        bgButton.Click += (_, _) => ChangeBackgroundColor();
        top.Controls.Add(bgButton);

        var saveButton = CreateButton("Salvesta kui", Color.FromArgb(234, 88, 12));
        saveButton.Location = new Point(605, 12);
        saveButton.Click += (_, _) => SaveCurrentImage();
        top.Controls.Add(saveButton);

        var helpButton = CreateButton("Abi", Color.FromArgb(71, 85, 105));
        helpButton.Location = new Point(715, 12);
        helpButton.Click += (_, _) => MessageBox.Show(
            "Vali mitu pilti korraga, liigu nuppe kasutades nende vahel või käivita slaidiesitlus.\n\n" +
            "Parendus 1: taustavärv.\nParendus 2: automaatne slaidiesitlus.\nParendus 3: pildi salvestamine uude formaati.",
            "Abi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        top.Controls.Add(helpButton);

        _pictureBox.Dock = DockStyle.Fill;
        _pictureBox.BackColor = Color.White;
        _pictureBox.BorderStyle = BorderStyle.FixedSingle;
        _pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        Controls.Add(_pictureBox);

        _statusLabel.Dock = DockStyle.Bottom;
        _statusLabel.Height = 34;
        _statusLabel.Padding = new Padding(12, 7, 12, 0);
        _statusLabel.Text = "Pilte pole veel valitud.";
        _statusLabel.ForeColor = Color.FromArgb(71, 85, 105);
        Controls.Add(_statusLabel);
    }

    private static Button CreateButton(string text, Color color)
    {
        var button = new Button
        {
            Text = text,
            Location = new Point(10, 12),
            Size = new Size(100, 38),
            FlatStyle = FlatStyle.Flat,
            BackColor = color,
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    private void OpenImages()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Vali pildid",
            Multiselect = true,
            Filter = "Pildifailid|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Kõik failid|*.*"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        _imageFiles.Clear();
        _imageFiles.AddRange(dialog.FileNames);
        _currentIndex = 0;
        ShowCurrentImage();
    }

    private void ShowCurrentImage()
    {
        if (_currentIndex < 0 || _currentIndex >= _imageFiles.Count)
        {
            _pictureBox.Image?.Dispose();
            _pictureBox.Image = null;
            _statusLabel.Text = "Pilte pole veel valitud.";
            return;
        }

        var path = _imageFiles[_currentIndex];
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var temp = Image.FromStream(stream);
            var image = new Bitmap(temp);

            _pictureBox.Image?.Dispose();
            _pictureBox.Image = image;
            _statusLabel.Text = $"{_currentIndex + 1}/{_imageFiles.Count}  •  {Path.GetFileName(path)}  •  {image.Width}×{image.Height}px";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Pilti ei saanud avada.\n\n{ex.Message}", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowNextImage()
    {
        if (_imageFiles.Count == 0) return;
        _currentIndex = (_currentIndex + 1) % _imageFiles.Count;
        ShowCurrentImage();
    }

    private void ShowPreviousImage()
    {
        if (_imageFiles.Count == 0) return;
        _currentIndex = (_currentIndex - 1 + _imageFiles.Count) % _imageFiles.Count;
        ShowCurrentImage();
    }

    private void ToggleSlideshow(Button button)
    {
        if (_imageFiles.Count == 0)
        {
            MessageBox.Show("Kõigepealt vali vähemalt üks pilt.", "Slaidiesitlus", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _slideshowRunning = !_slideshowRunning;
        if (_slideshowRunning)
        {
            _slideTimer.Start();
            button.Text = "⏸ Peata";
        }
        else
        {
            _slideTimer.Stop();
            button.Text = "▶ Slaidiesitlus";
        }
    }

    private void ChangeBackgroundColor()
    {
        using var dialog = new ColorDialog { Color = _pictureBox.BackColor };
        if (dialog.ShowDialog() == DialogResult.OK)
            _pictureBox.BackColor = dialog.Color;
    }

    private void SaveCurrentImage()
    {
        if (_pictureBox.Image == null)
        {
            MessageBox.Show("Salvestamiseks ei ole pilti.", "Salvestamine", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Title = "Salvesta pilt",
            Filter = "PNG fail|*.png|JPEG fail|*.jpg|Bitmap fail|*.bmp",
            FileName = "salvestatud_pilt.png"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        var format = Path.GetExtension(dialog.FileName).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => System.Drawing.Imaging.ImageFormat.Jpeg,
            ".bmp" => System.Drawing.Imaging.ImageFormat.Bmp,
            _ => System.Drawing.Imaging.ImageFormat.Png
        };

        try
        {
            _pictureBox.Image.Save(dialog.FileName, format);
            MessageBox.Show("Pilt on salvestatud.", "Valmis", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Salvestamine ebaõnnestus.\n\n{ex.Message}", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _slideTimer.Stop();
        _pictureBox.Image?.Dispose();
        base.OnFormClosed(e);
    }
}
