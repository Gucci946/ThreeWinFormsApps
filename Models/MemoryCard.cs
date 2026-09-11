namespace ThreeWinFormsApps.Models;

public sealed class MemoryCard
{
    public string ImagePath { get; }
    public int PairId { get; }
    public bool IsMatched { get; set; }
    public bool IsRevealed { get; set; }

    public MemoryCard(string imagePath, int pairId)
    {
        ImagePath = imagePath;
        PairId = pairId;
    }
}
