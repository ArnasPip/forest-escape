using GameRunner;

namespace ForestEscapeUnitTester
{
    public class GameTests
    {
        private Game _game { get; set; } = null!;
        [SetUp]
        public void Setup()
        {
            _game = new Game();
        }

        [Test]
        public void Run_Test1()
        {
            //Assign
            int correctResult = 4;
            //Act
            string file = (@"TestData\map1.txt");
            int result;
            try
            {
                result = _game.Run(file);
            }
            catch (FileNotFoundException)
            {
                result = 0;
            }
            //Assert
            Assert.That(result, Is.EqualTo(correctResult));
        }
        [Test]
        public void Run_Test2()
        {
            //Assign
            int correctResult = 13;
            //Act
            string file = (@"TestData\map2.txt");
            int result;
            try
            {
                result = _game.Run(file);
            }
            catch (FileNotFoundException)
            {
                result = 0;
            }
            //Assert
            Assert.That(result, Is.EqualTo(correctResult));
        }
        [Test]
        public void Run_Test3()
        {
            //Assign
            int correctResult = 0;
            //Act
            string file = (@"TestData\ThisFileDoesNotExist.txt");
            int result;
            try
            {
                result = _game.Run(file);
            }
            catch (FileNotFoundException)
            {
                result = 0;
            }
            //Assert
            Assert.That(result, Is.EqualTo(correctResult));
        }
    }
}