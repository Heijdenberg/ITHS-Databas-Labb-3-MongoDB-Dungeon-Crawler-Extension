using ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Utilities;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.Core;

internal class Dice
{
    private int _numberOfDice;
    private int _sidesPerDice;
    private int _modifier;

    public Dice(int numberOfDice, int sidesPerDice, int modifier)
    {
        _numberOfDice = numberOfDice;
        _sidesPerDice = sidesPerDice;
        _modifier = modifier;
    }

    public int Modifier
    {
        get => _modifier;
        set => _modifier = value;
    }

    public int Throw()
    {
        int result = 0;

        for (int i = 0; i < _numberOfDice; i++)
        {
            result += GameRandom.Random.Next(1, _sidesPerDice + 1);
        }

        result += _modifier;

        return result;
    }

    public override string ToString()
    {
        string diceNotation = $"{_numberOfDice}D{_sidesPerDice}";

        if (_modifier > 0)
        {
            diceNotation += $"+{_modifier}";
        }
        else if (_modifier < 0)
        {
            diceNotation += $"{_modifier}";
        }

        return diceNotation;
    }
}
