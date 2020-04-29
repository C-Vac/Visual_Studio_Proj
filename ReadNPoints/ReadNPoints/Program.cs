using System;
using System.Collections.Generic;
using System.Linq;

/* This program lets you enter a list or coordinates,
 * build a list of points, and then perform some 
 * operations on the points.
 * 
 * Everything seems to work correctly. Option 2 in OpsMenu()
 * is not yet implemented but is handled so will not crash the 
 * program if selected.
 * 
 * Future:
 * Exception handling should be replaced with if/else conditions where possible
 * Add significant figures to all doubles
 * Add GetLongestLine()
 * 
 * Bugs: I somehow escaped out of ListOptions() during a test session
 * and it broke some stuff
 */

namespace ReadNPoints
{
    class Program
    {
        public static int sigFig = 4;
        static void Main()
        {
            Console.WriteLine("C-Vac's epic line segment simulator v5.22.1" + 
                "\nPress any key to start.");
            Console.ReadKey(true);
            
            List<Point> pointList = new List<Point>();
            pointList = InputPoints(pointList, false);

            ListOptions(pointList);
        }

        static void FancyErrorMessage(SystemException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }

        // Parse a string from console input and return character array of length 2
        
        /*  read line
         *  to upper
         *  if string.length == 2 ( enter AB )
         *  to character array with length 2
         *  
         */ 
        static char[] NewInputParse()
        {
            char[] inputNames = new char[2];

            string input = Console.ReadLine().ToUpper();
            bool inputValid = false;

            while (!inputValid)
            {
                if (input.Length == 2)
                {
                    inputNames = input.ToArray();
                }
            }
            return null;
        }

        static char[] ReadTwoCharFromConsoleInput()
        {

            char[] inputNames = new char[2];
            bool inputValid = false;

            while (!inputValid)
            {
                try
                {
                    inputNames = Console.ReadLine().
                        ToUpper().Split(' ').
                        Select(char.Parse).
                        ToArray();
                    inputValid = true;
                }
                catch (SystemException e)
                {
                    FancyErrorMessage(e);
                }
            }

            return inputNames;
        }

        // Operations to be performed on a saved list
        static void OpsMenu(List<Point> points)
        {
            DisplayList(points);

            // Menu options
            Console.WriteLine( "\n{0}\n{1}\n{2}\n{3}\n{4}",
                "1: Calculate distance between two points",
                "2: Calculate longest possible line between points",
                "3: Calculate shortest possible line between points",
                "4: Change rounded digits of results",
                "5: Back to list menu" );
            switch (Console.ReadLine())
            {
                case "1": 
                    double[] distance = GetDistanceFromInput(points);
                    DisplayResult(distance, 
                        "The distance between points {1} and {2} is: {0}");
                    break;
                case "2":
                    double[] longest = GetShortestOrLongestLine(points, false);
                    DisplayResult(longest,
                        "The longest line segment is line {1}{2} and is {0} units long.");
                    break;
                case "3":
                    double[] shortest = GetShortestOrLongestLine(points, true);
                    DisplayResult(shortest,
                        "The shortest line segment is line {1}{2} and is {0} units long.");
                    break;
                case "4":
                    sigFig = ChangeSignificantFigures(sigFig);
                    break;
                default:
                    InputPoints(points, false);
                    break;
            }
            Console.WriteLine("\nPress enter to continue"); Console.ReadKey(true);
            OpsMenu(points);

            static void DisplayResult(double[] values, string message)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message,
                    Math.Round(values[0], sigFig), 
                    Convert.ToChar((int)values[1]), 
                    Convert.ToChar((int)values[2]) );
                Console.ResetColor();
            }

            static int ChangeSignificantFigures(int sigDig)
            {
                Console.Clear();
                Console.Write("Currently rounding to {0} digits.\n" +
                    "Enter new significant digits to round to: ", sigDig);
                sigDig = Convert.ToInt32(Console.ReadLine());
                return sigDig;
            }

            // Take console input of two points on the list and calculate their distance.
            static double[] GetDistanceFromInput(List<Point> pointList)
            {
                
                List<Point> points = pointList;
                Point left = new Point(); Point right = new Point();
                char[] inputNames = new char[2];
                bool inputValid = false;

                // Take the input string and make sure the data is appropriate.
                while (!inputValid) 
                {
                    DisplayList(pointList);
                    Console.Write("\n{0}", "Enter line (A B) to calculate the distance of: ");
                    inputNames = ReadTwoCharFromConsoleInput();

                    try
                    {
                        foreach (Point point in points)
                        {
                            if (point.Name == inputNames[0])
                            {
                                left = point;
                            }
                            else if (point.Name == inputNames[1])
                            {
                                right = point;
                            }
                            if (left.Name != 'Z' && right.Name != 'Z')
                            {
                                break;
                            }
                        }
                        inputValid = true;
                    }
                    catch (SystemException e)
                    {
                        FancyErrorMessage(e);
                    }
                }
                // If the data is valid, store result of distance formula method and point names
                double[] results = { GetDistance(left, right), inputNames[0], inputNames[1] };
                return results;
            }

            // GetDistance returns only the distance between the two input points.
            static double GetDistance(Point leftP, Point rightP)
            {
                Point left = leftP;
                Point right = rightP;

                double results = Math.Sqrt(Math.Pow(left.X - right.X, 2) +
                    Math.Pow(left.Y - right.Y, 2));

                return results;
            }

            // Shortest and Longest do not take user input so they return the full data set of distance and point names.
            static double[] GetShortestOrLongestLine(List<Point> pointList, bool shortest)
            {
                double[] outputLine = { 0, 0, 0 } ;
                if (shortest) { outputLine[0] = double.MaxValue; }

                for (int i = 0; i < pointList.Count; i++)
                {
                    for (int j = 0; j < pointList.Count; j++)
                    {
                        if (j != i)
                        {
                            double distance = GetDistance(pointList[i], pointList[j]);

                            if (shortest)
                            {
                                if (distance < outputLine[0])
                                {
                                    outputLine[0] = distance;
                                    outputLine[1] = pointList[i].Name;
                                    outputLine[2] = pointList[j].Name;
                                }
                            } else
                            {
                                if (distance > outputLine[0])
                                {
                                    outputLine[0] = distance;
                                    outputLine[1] = pointList[i].Name;
                                    outputLine[2] = pointList[j].Name;
                                }
                            }
                        }
                    }
                }

                return outputLine;
            }
        }


        // Clear console and display the list of points
        static void DisplayList(List<Point> pointList)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Points in the list: {0}. \n", pointList.Count);
            Console.ResetColor();
            foreach (Point point in pointList)
            {
                Console.WriteLine("Point {0}: ({1}, {2})", point.Name, point.X, point.Y);
            }
        }

        // Menu for editing the point list
        static void ListOptions(List<Point> pointList)
        {
            DisplayList(pointList);

            Console.WriteLine( "\n{0}\n{1}\n{2}\n{3}\n{4}", 
                "1: Save list", 
                "2: Add more points",
                "3: Remove a point",
                "4: Clear list",
                "5: Exit" );

            // Directory of operations
            switch (Console.ReadLine())
            {
                case "1":
                    OpsMenu(pointList);
                    break;
                case "2":
                    ListOptions(InputPoints(pointList, false));
                    break;
                case "3":
                    ListOptions(RemovePoint(pointList));
                    break;
                case "4":
                    ListOptions(InputPoints(pointList, true));
                    break;
                default:
                    Main();
                    break;
            }
        }
        // Add to the list of points via user input
        static List<Point> InputPoints(List<Point> pointList, bool clearList)
        {
            string input = "";
            if (clearList)
            {
                pointList.Clear();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The list was cleared.\nPress any key.");
                Console.ResetColor();
                Console.ReadKey(true);
            }

            DisplayList(pointList);
            Console.Write("\n{0}", "Enter coordinates (X Y) or press Enter to save current list: ");

            // Manage the naming of new points
            char nameChar = 'A';
            if (pointList.Count() > 1)
            {
                if (pointList.Last().Name != 'Z')
                {
                    nameChar = Convert.ToChar(pointList.Last().Name + 1);
                }
            }

            // If coord is entered, add it to the list. Otherwise return the list.
            // I just realized this block is ridiculous but too tired to fix it
            while (input == "")
            {
                input = Console.ReadLine();

                if (input == "")
                {   
                    // Save the list and return it.
                    DisplayList(pointList);
                    return pointList;
                }
                else
                {
                    // Read and store the point with the next nameChar as its .Name property. Triggers while loop.
                    pointList.Add(ReadPoint(input));
                    pointList.Last().Name = nameChar;

                    DisplayList(pointList);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Successfully added point {0} = ({1}, {2}).",
                        pointList.Last().Name, pointList.Last().X, pointList.Last().Y );
                    Console.ResetColor();
                    Console.Write("\n{0}", "Enter a coordinate set (X Y) or press enter to save current list: ");

                    nameChar++;
                    input = "";
                }
            }
            return null;
        }

        // Remove a point from the list.
        static List<Point> RemovePoint(List<Point> oldList)
        {
            char deleteName = 'Z';
            string input;

            while (deleteName == 'Z')
            {
                DisplayList(oldList);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\nEnter point to delete: ");
                Console.ResetColor();
                try
                {
                    input = Console.ReadLine();
                    if (input == "")
                    {
                        return oldList;
                    } else
                    {
                        deleteName = Convert.ToChar(input.ToUpper());
                    }
                }
                catch (SystemException e)
                {
                    FancyErrorMessage(e);
                    Console.ReadKey(true);
                }
            }

            for (int i = 0; i < oldList.Count(); i++)
            {
                if (oldList[i].Name == deleteName)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Removed point {0} at ({1}, {2}). Press any key.",
                        oldList[i].Name, oldList[i].X, oldList[i].Y);
                    Console.ResetColor();
                    oldList.Remove(oldList[i]);
                    Console.ReadKey(true);
                }
            }

            // Rebuild the list so the names go alphabetically without missing letters
            List<Point> newList = new List<Point>();
            char counter = 'A';
            foreach (Point point in oldList)
            {
                newList.Add(point);
                point.Name = counter; counter++;
            }

            return newList;
        }

        // Read input argument and create a new point, then return it
        static Point ReadPoint(string input)
        {
            int[] coordinates = { 0, 0 };
            bool inputValid = false;
            Point newPoint = new Point();

            // Replace input locally if exception is thrown
            while (!inputValid)
            {
                try
                {
                    coordinates = input.Split(' ').Select(int.Parse).ToArray();
                    newPoint.X = coordinates[0];
                    newPoint.Y = coordinates[1];
                    inputValid = true;
                }
                catch (SystemException e)
                {
                    FancyErrorMessage(e);
                    Console.Write("\n{0}", "Enter coordinates (X Y): ");
                    input = Console.ReadLine();
                }
            }

            return newPoint;
        }
    }
}