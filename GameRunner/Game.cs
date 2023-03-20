using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace GameRunner;

public struct ExitPoint //Struct for saving exit point coordinates so we can check while in path searching algorithm
{
    public int x;
    public int y;
    public ExitPoint(int a, int b)
    {
        x = a;
        y = b;
    }
}
public class Game : IGame
{
    public int Run(string filePath)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        // TODO: start your journey here

        int height = File.ReadLines(filePath).Count();//Finding the height and the width of the map to use as boundries
        int width = File.ReadLines(filePath).First().Length;
        foreach(var line in File.ReadLines(filePath))//Checking if all lines of map are the same length
        {
            if (line.Length != width)
            {
                return 0;
            }
        }
        if(height < 5 || height > 11000 || width < 5 || width > 11000)//Checking if the map fits the requirements provided in the PDf instructions
        {
            return 0;
        }
        char[,] map = new char[width,height];
        int x, y = 0;
        foreach (var line in File.ReadLines(filePath))//Extracting the map from the .txt file into a 2d char array for easier nad more convenient way access to each element
        {
            x = 0;
            foreach (char c in line)
            {
                if (x != width)
                {
                    map[x,y] = c;
                }
                x++;
            }
            y++;
        }
        int startX=0;
        int startY=0;
        List<ExitPoint> exits = new List<ExitPoint>();
        bool startExists = false;
        for (int i = 0; i < height; i++)//Finding the start coordinates. Finding the exits and marking them on the map for easier checking in algorithm. Also saving exit point coordinates in a list of structs ExitPoint
        {
            for (int j = 0; j < width; j++)
            {
                if (map[j, i] == 'X')
                {
                    startX = j;
                    startY = i;
                    startExists = true;
                }
                if (i == 0 && map[j,i]==' ')
                {
                    map[j, i] = 'E';
                }
                if (j == 0 && map[j, i] == ' ')
                {
                    map[j, i] = 'E';
                }
                if (i == height-1 && map[j, i] == ' ')
                {
                    map[j, i] = 'E';
                }
                if (j == width-1 && map[j, i] == ' ')
                {
                    map[j, i] = 'E';
                }
                if (map[j, i] == 'E')
                {
                    exits.Add(new ExitPoint(j, i));
                }
            }
        }
        if(exits.Count == 0||startExists == false)//If there are no exits or there isn't a start then you cannot find the shortest path
        {
            return 0;
        }
        int min_dist = Int32.MaxValue;
        foreach(ExitPoint exit in exits)//Finding shortest path for each exit and comparing them to find the shortest one overall
        {
            int dist = FindShortestPathLength(map, startX, startY, exit.x, exit.y, width, height);
            if(dist != 0 && dist<min_dist)
            {
                min_dist = dist;
            }
        }
        if (min_dist == Int32.MaxValue)
        {
            min_dist = 0;
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed.TotalSeconds);
        return min_dist;
    }

    static bool[,] visited;//2d bool array to store coordinates we have visited
    static bool IsSafe(char[,] map, bool[,] visited, int x, int y, int width, int height)//Checking if a coordinate is safe to move to.
    {
        return(x >= 0 && x < width && y >= 0 && y < height && map[x, y] != '1' && !visited[x, y]);
    }
    static int FindShortestPath(char[,] map, int i, int j,int x, int y, int min_dist, int dist,int width, int height)//Recursively moving through the whole map to find the shortest path
    {
        if (i == x && j == y)//If our current coordinates are the same with the exit coordinates then we return the min_distance if coordinates are not equal to the coordinates of the exit then we move on
        {
            min_dist = Math.Min(dist, min_dist);
            return min_dist;
        }
        visited[i, j] = true;//Marking the coordinate that we visit
        if (IsSafe(map, visited, i + 1, j, width, height))//Moving down if it's safe to move there
        {
            min_dist = FindShortestPath(map, i + 1, j, x, y, min_dist, dist + 1, width, height);
        }
        if (IsSafe(map, visited, i, j + 1, width, height))//Moving right if it's safe to move there
        {
            min_dist = FindShortestPath(map, i, j + 1, x, y, min_dist, dist + 1, width, height);
        }
        if (IsSafe(map, visited, i - 1, j, width, height))//Moving up if it's safe to move there
        {
            min_dist = FindShortestPath(map, i - 1, j, x, y, min_dist, dist + 1, width, height);
        }
        if (IsSafe(map, visited, i, j - 1, width, height))//Moving left if it's safe to move there
        {
            min_dist = FindShortestPath(map, i, j - 1, x, y, min_dist, dist + 1, width, height);
        }
        visited[i, j] = false;//If it is not safe to move anywhere then we backtrack until it is safe to move
        return min_dist;//When we go through every possible path we return the minimum distance
    }
    static int FindShortestPathLength(char[,] map, int startX,int startY, int exitX,int exitY,int width, int height)//Method to start the finding of the shortest path
    {
        visited = new bool[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                visited[i, j] = false;
        }
        int dist = Int32.MaxValue;
        dist = FindShortestPath(map, startX, startY, exitX, exitY, dist, 0, width, height);
        if (dist != Int32.MaxValue)
        {
            return dist;
        }
        return 0;
    }
}
