namespace ThreeWinFormsApps.Models;

public sealed class MemoryGame
{
    private readonly Random _random = new();

    public List<MemoryCard> Cards { get; } = new();
    public int Moves { get; private set; }
    public int MatchedPairs { get; private set; }
    public int PairCount { get; private set; }
    public bool IsComplete => MatchedPairs == PairCount && PairCount > 0;

    public void Start(IReadOnlyList<string> allImagePaths, int rows, int columns)
    {
        var requiredPairs = rows * columns / 2;
        if (requiredPairs <= 0)
            throw new ArgumentException("Mänguvälja suurus peab olema positiivne.", nameof(rows));
        if (allImagePaths.Count < requiredPairs)
            throw new ArgumentException("Piltide arv ei ole valitud mänguvälja jaoks piisav.", nameof(allImagePaths));

        Cards.Clear();
        Moves = 0;
        MatchedPairs = 0;
        PairCount = requiredPairs;

        var selected = allImagePaths
            .OrderBy(_ => _random.Next())
            .Take(PairCount)
            .ToList();

        for (var i = 0; i < selected.Count; i++)
        {
            Cards.Add(new MemoryCard(selected[i], i));
            Cards.Add(new MemoryCard(selected[i], i));
        }

        Shuffle(Cards);
    }

    public bool ArePair(MemoryCard first, MemoryCard second) => first.PairId == second.PairId;

    public void RegisterMove() => Moves++;

    public void RegisterMatch() => MatchedPairs++;

    public int CalculateScore(int elapsedSeconds)
    {
        return Math.Max(0, 10000 - Moves * 120 - elapsedSeconds * 8);
    }

    private void Shuffle(IList<MemoryCard> cards)
    {
        for (var i = cards.Count - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }
}
