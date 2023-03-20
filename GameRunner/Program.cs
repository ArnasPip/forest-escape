using GameRunner;

IGame game = new Game();

int result;
try
{
    result = game.Run(@"TestData\map2.txt");
}
catch(FileNotFoundException)
{
    result = 0;
}
Console.WriteLine("result = " + result);