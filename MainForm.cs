using System.Drawing.Drawing2D;

namespace ThreeWinFormsApps;

public sealed class MainForm : Form
{
    private readonly Color _background = Color.FromArgb(245, 247, 251);

    public MainForm()
    {
        Text = "Kolm Windows Forms rakendust";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(760, 520);
        MinimumSize = new Size(700, 480);
        BackColor = _background;
        Font = new Font("Segoe UI", 10F);

        BuildInterface();
    }

    private void BuildInterface()
    {
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 130,
            BackColor = Color.FromArgb(31, 41, 55)
        };
        Controls.Add(header);

        var title = new Label
        {
            Text = "Kolm Windows Forms rakendust",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(36, 24)
        };
        header.Controls.Add(title);

        var subtitle = new Label
        {
            Text = "C# • WinForms • OOP • iseseisva töö projekt",
            ForeColor = Color.FromArgb(203, 213, 225),
            Font = new Font("Segoe UI", 11F),
            AutoSize = true,
            Location = new Point(40, 78)
        };
        header.Controls.Add(subtitle);

        var info = new Label
        {
            Text = "Vali rakendus:",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(40, 160)
        };
        Controls.Add(info);

        var viewerButton = CreateMenuButton(
            "1  •  Pildi vaatamise programm",
            "Ava, vaheta ja salvesta pilte ning kasuta slaidiesitlust.",
            Color.FromArgb(37, 99, 235));
        viewerButton.Location = new Point(40, 205);
        viewerButton.Click += (_, _) => new ImageViewerForm().ShowDialog(this);
        Controls.Add(viewerButton);

        var mathButton = CreateMenuButton(
            "2  •  Matemaatiline mäng",
            "Lahenda juhuslikke ülesandeid, kogu punkte ja võida ajapiiranguga.",
            Color.FromArgb(16, 185, 129));
        mathButton.Location = new Point(40, 300);
        mathButton.Click += (_, _) => new MathGameForm().ShowDialog(this);
        Controls.Add(mathButton);

        var memoryButton = CreateMenuButton(
            "3  •  Sarnaste piltide mäng",
            "Leia paarid, vali mänguväli 4×4 või 6×6 ja proovi koguda maksimumskoor.",
            Color.FromArgb(124, 58, 237));
        memoryButton.Location = new Point(40, 395);
        memoryButton.Click += (_, _) => new MemoryGameForm().ShowDialog(this);
        Controls.Add(memoryButton);
    }

    private static Button CreateMenuButton(string title, string description, Color accent)
    {
        var button = new Button
        {
            Size = new Size(680, 80),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(17, 24, 39),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 5, 12, 5),
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
        button.FlatAppearance.BorderSize = 1;

        button.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var accentBrush = new SolidBrush(accent);
            e.Graphics.FillRectangle(accentBrush, 0, 0, 7, button.Height);

            using var titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            using var descFont = new Font("Segoe UI", 9.5F);
            using var titleBrush = new SolidBrush(Color.FromArgb(17, 24, 39));
            using var descBrush = new SolidBrush(Color.FromArgb(100, 116, 139));

            e.Graphics.DrawString(title, titleFont, titleBrush, 24, 13);
            e.Graphics.DrawString(description, descFont, descBrush, 24, 42);
        };

        button.MouseEnter += (_, _) => button.BackColor = Color.FromArgb(248, 250, 252);
        button.MouseLeave += (_, _) => button.BackColor = Color.White;
        return button;
    }
}
