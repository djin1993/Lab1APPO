using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Game_of_Life
{
    class Game_of_Life
    {
        int[,] generation;
        int[,] lastGeneration;


        int generationCount;

        int width;
        int height;

        public int GenerationCount
        {
            get { return generationCount; }
        }
        public Game_of_Life(int[,] newGrid)
        {
            generation = (int[,])newGrid.Clone();

            generationCount = 1;

            width = generation.GetLength(1);
            height = generation.GetLength(0);

            lastGeneration = new int[height, width];
        }

        private int Neighbours(int x, int y)
        {
            int count = 0;

            // Check for x - 1, y - 1
            if (x > 0 && y > 0)
            {
                if (generation[y - 1, x - 1] == 1)
                    count++;
            }

            // Check for x, y - 1
            if (y > 0)
            {
                if (generation[y - 1, x] == 1)
                    count++;
            }

            // Check for x + 1, y - 1
            if (x < width - 1 && y > 0)
            {
                if (generation[y - 1, x + 1] == 1)
                    count++;
            }

            // Check for x - 1, y
            if (x > 0)
            {
                if (generation[y, x - 1] == 1)
                    count++;
            }

            // Check for x + 1, y
            if (x < width - 1)
            {
                if (generation[y, x + 1] == 1)
                    count++;
            }

            // Check for x - 1, y + 1
            if (x > 0 && y < height - 1)
            {
                if (generation[y + 1, x - 1] == 1)
                    count++;
            }

            // Check for x, y + 1
            if (y < height - 1)
            {
                if (generation[y + 1, x] == 1)
                    count++;
            }

            // Check for x + 1, y + 1
            if (x < width - 1 && y < height - 1)
            {
                if (generation[y + 1, x + 1] == 1)
                    count++;
            }

            return count;
        }

        public void WriteNeighbours()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    Console.Write("{0}", Neighbours(x, y));
                Console.WriteLine();
            }
        }

        public void ProcessGeneration()
        {
            int[,] nextGeneration = new int[height, width];

            lastGeneration = (int[,])generation.Clone();

            generationCount++;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (Neighbours(x, y) < 2)
                        nextGeneration[y, x] = 0;
                    else if (generation[y, x] == 0 && Neighbours(x, y) == 3)
                        nextGeneration[y, x] = 1;
                    else if (generation[y, x] == 1 &&
                            (Neighbours(x, y) == 2 || Neighbours(x, y) == 3))
                        nextGeneration[y, x] = 1;
                    else
                        nextGeneration[y, x] = 0;
                }
            }

            generation = (int[,])nextGeneration.Clone();
        }
        public void DrawGeneration()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    Console.Write("{0}", generation[y, x]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public int AliveCells()
        {
            int count = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (generation[y, x] == 1)
                        count++;

            return count;
        }

    public static Dictionary<int,object> ReadFromFile()
        {
            string input = File.ReadAllText(@"h:\input.txt");
            int linecount = File.ReadAllLines(@"h:\input.txt").Length;
            int colcount = ((input.Split(' ').Length-1)/linecount)+1;
            Dictionary<int, object> result_values = new Dictionary<int, object>();
            result_values.Add(1, linecount);
            result_values.Add(2, colcount);
            int i = 0, j = 0;
            int[,] result = new int[linecount,colcount] ;
            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
            result_values.Add(3, result);
            return result_values;
        }
        static void Main(string[] args)
        {
            
            string filename = @"h:\output.txt";
            int nr_of_lines = (int)Game_of_Life.ReadFromFile()[1];
            int nr_of_cols = (int)Game_of_Life.ReadFromFile()[2];
            StreamWriter sw = new StreamWriter(filename, true);
            int[,] grid = (int[,])Game_of_Life.ReadFromFile()[3];

            Console.WriteLine("Enter maximum number of Generations");
            int generationLimit = int.Parse(Console.ReadLine());
            Game_of_Life lifeGrid = new Game_of_Life(grid);
             Game_of_Life.ReadFromFile();

            Console.WriteLine("Generation 0");
            sw.WriteLine("------------------");
            sw.WriteLine(("Generation " + " " + "0"));
            for (int i = 0; i < nr_of_lines; i++)
            {

                for (int j = 0; j < nr_of_cols; j++)
                {
                    sw.Write(lifeGrid.generation[i, j] + " ");
                }
                sw.WriteLine();
            }

            lifeGrid.DrawGeneration();
            Console.WriteLine();
            while (lifeGrid.AliveCells() > 0)
            {

                string response;

                Console.WriteLine();
                Console.WriteLine("Generation {0}", lifeGrid.GenerationCount);
                sw.WriteLine("------------------");
                sw.WriteLine(("Generation "+" "+ lifeGrid.GenerationCount));
                lifeGrid.ProcessGeneration();
                lifeGrid.DrawGeneration();

                for (int i = 0; i <nr_of_lines ; i++)
                {
                    
                    for (int j = 0; j <nr_of_cols ; j++)
                    {
                        sw.Write(lifeGrid.generation[i ,j]+" ");
                    }
                    sw.WriteLine();
                }
                Console.WriteLine();

                if (lifeGrid.generationCount==generationLimit)
                {
                    Console.WriteLine("Max Generaration reached");
                    Console.ReadKey();
                    sw.Close();
                    break;

                }
                if (lifeGrid.AliveCells() == 0)
                {
                    sw.Close();
                    Console.WriteLine("Every one died!");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Press <Enter> to contiune or n<Enter> to quit.");

                    response = Console.ReadLine();

                    if (response == "n" || response == "N")
                    {
                        sw.Close();
                        break;
                    }
                }
            }
        }
    }
}
