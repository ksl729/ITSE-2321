namespace Week3Lecture
{
    internal class Program
    {
        static void Main(string[] args)
        {   // Create an object of the class Sandwich
            Sandwich mySandwich = new Sandwich("Italian Herbs and Cheese", "Italian Herb", "12 Inches"); // Could call with this line with no parameters because of the default constructor

            mySandwich.SetCheese(true); // Set the cheese property to true. Does not return anything because it is a void method

            Console.WriteLine(mySandwich.ToString()); // Call the ToString method to display the sandwich information

        }

    }
}
