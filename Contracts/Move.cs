namespace Contracts
{
    public enum Move
    {
        Rock = 1, //beats Scissors
        Paper = 2, //beats Rock
        Scissors = 3, //beats Paper
        Dynamite = 4, //beats everything but Waterbomb
        Warterbomb = 5 //loses to everything but Dynamite
    }
}
