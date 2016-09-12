using MetaData;

namespace TestLibrary
{
    [CodeChanges("Absolute() method added", Developer = "Marcel", Date = "01-02-2016")]
    [CodeChanges("Square() method added", Developer = "Marcel", Date = "01-02-2016")]
    [CodeChanges("SquareRoot() method added", Developer = "Marcel", Date = "01-02-2016")]

    public class MyMath
    {
        [CodeChanges("Property Value added", Developer = "Marco", Date = "01-02-2016")]
        public int Value { get; set; }

        [CodeChanges("Method that calculates the absolute value added", Developer="John", Date="01-02-2016")]
        public uint Absolute(int value)
        {
            return 1;
        }
        [CodeChanges("Method that calculates the square added", Developer = "Jan Peter", Date = "01-02-2016")]

        public long Square(int value)
        {
            return 1;
        }

        [CodeChanges("Method that calculates the square root added", Developer = "Felix", Date = "01-02-2016")]

        public double SquareRoot(uint value)
        {
            return 1;
        }
    }
}
