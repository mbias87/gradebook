using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;

namespace GradeBook 
{
 public delegate void GradeAddedDelegate(object sender, EventArgs args);

        
        public class NamedObject
        {
            public NamedObject(string name) 
            {
                Name = name;
            }
            public string Name 
            {
                get; set;
            }
        }

        public interface IBook
        {
            void AddGrade(double grade);
            Statistics GetStatistics();

            // void ShowStatistic();
            string Name {get;}
            event GradeAddedDelegate GradeAdded;
        }

        public abstract class Book: NamedObject, IBook
        {
        protected Book(string name) : base(name)
        {
        } 
        public abstract event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public abstract Statistics GetStatistics();

        // public abstract void ShowStatistic();
    
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            using(var writer =  File.AppendText($"{Name}.txt")){
                writer.WriteLine(grade);
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            
        }

        public override Statistics GetStatistics()
        {
            var results = new Statistics();

            using(var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while(line != null)
                {
                    var number = double.Parse(line);
                    results.Add(number);
                    line = reader.ReadLine();
                }
            }

            return results;
        }

    }
    public class InMemoryBook : Book
    {   
       
       
        public InMemoryBook(string name) : base(name) 
        {

            grades = new List<double>();
            Name = name;
        }

        
        public override void AddGrade(double grade) 
        {
            if( grade <= 100 && grade >=0)
            {
                grades.Add(grade);    
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            } 
            

        }

        public void AddLetterGrade(char letter)
        {
            switch(letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                case 'D':
                    AddGrade(60);
                    break;
                case 'F':
                    AddGrade(50);
                    break;

                default:
                    AddGrade(0);
                    break;
            }
        }
        public override Statistics GetStatistics() 
        {
            var result = new Statistics(); 

            for(var index = 0; index < grades.Count; index += 1)
            {
                result.Add(grades[index]);                 
            }  
            
            return result;
        }
        // public  void ShowStatistic()
        // {
        //     var highGrade = this.ComputeHighestGrade();
        //     var lowGrade = this.ComputeLowesetGrade();
        //     var average = this.ComputeAverage();

        //     Console.WriteLine($"The highest grade is {highGrade}");
        //     Console.WriteLine($"The lowest grade is {lowGrade}");
        //     Console.WriteLine($"The average grade is {average}");

        // }

        public double ComputeHighestGrade()
        {
            var highGrade = double.MinValue;
            foreach( double grade in grades)
            {
                highGrade = Math.Max(grade, highGrade);
            } 

            return highGrade;

        }

        public double ComputeLowesetGrade() 
        {

            var lowGrade = double.MaxValue;
            foreach( double grade in grades)
            {
                lowGrade = Math.Min(grade, lowGrade);
            }
            return lowGrade;

        }

        public double ComputeAverage() 
        {
            double sum = 0;
            var amountOfGrades = grades.Count();
            foreach (double grade in grades) 
            {
                sum += grade;
            }
            if (amountOfGrades > 0){
                return sum/amountOfGrades;
            } else {
                return 0.0;
            }
            
        }

        public override event GradeAddedDelegate GradeAdded;

        List<double> grades;

       
        
        public const string CATEGORY = "Science";

    }
    

}