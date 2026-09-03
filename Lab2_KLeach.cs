using System;

namespace Lab2_KLeach
//  Name: Karly Leach
//  Date: 9/3/2026
//  Class: ITSE 2321
//  Description: This program demonstrates the bubble sort algorithm on both a 1D and a 2D array of integers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("First Array Sort");
            // Original array of integers
            int[] originalArray = { 12, 7, 4, 27, 1, 13, 19, 6, 30, 9, 8 };

            // Using the bubble sort method
            int[] bubbleSortedArray = BubbleSort(originalArray);
            Console.WriteLine("Bubble Sorted Array: " + string.Join(", ", bubbleSortedArray));

            Console.WriteLine("Second Array Sort");
            SortAndPrint2DMatrix();
        }

        static int[] BubbleSort(int[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array; // Returns the sorted array
        }

        static void SortAndPrint2DMatrix()
        {
            int[,] matrix = {
                { 54, 3, 21, 50, 57, 7, 73, 24, 85, 79 },
                { 33, 11, 19, 33, 51, 72, 80, 6, 78, 25 },
                { 1, 65, 4, 86, 71, 52, 16, 77, 29, 13 },
                { 23, 44, 90, 70, 28, 17, 53, 64, 62, 83 },
                { 55, 98, 22, 89, 34, 75, 56, 58, 27, 35 },
                { 87, 88, 8, 61, 74, 14, 66, 20, 59, 81 },
                { 18, 76, 2, 5, 26, 84, 15, 82, 9, 60 }
            };

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int twoDArraySize = rows * cols;

            int[] oneDArray = new int[twoDArraySize];
            int index = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    oneDArray[index++] = matrix[r, c];
                }
            }

            int[] bubbleSortedArray2D = BubbleSort(oneDArray);
            Console.WriteLine("Bubble Sorted 2D Array: " + string.Join(", ", bubbleSortedArray2D));

            index = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    matrix[r, c] = bubbleSortedArray2D[index++];
                }
            }

            Console.WriteLine("Bubble Sorted 2D Array (Original Structure):");
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Console.Write(matrix[r, c] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
