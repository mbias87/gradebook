using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {

            IBook book = new DiskBook("Scotts GradeBook");
            book.GradeAdded += OnGradeAdded;
            var grades = new List<double>() { 12.5, 10.3, 6.11, 5.11 };
            Console.WriteLine("Welcome to Gradebook. Please enter Grades");
            EnterGrades(book);
            var stats = book.GetStatistics();
            //book.Name = "";
            Console.WriteLine($"For the book named {book.Name}");
            Console.WriteLine($"The highest grade is {stats.High}");
            Console.WriteLine($"The lowest grade is {stats.Low}");
            Console.WriteLine($"The average grade is {stats.Average}");
            Console.WriteLine($"The letter grade is {stats.Letter}");
        }

        private static void EnterGrades(IBook book)
        {
            while (true)
            {
                Console.Write("Enter a grade or 'q' to quit ");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }
                try
                {
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("**");
                }

            }

            // book.ShowStatistic();
        }

        static void OnGradeAdded(object sender, EventArgs eventArgs)
        {

            Console.WriteLine("a grade was added");

        }
    }
}

