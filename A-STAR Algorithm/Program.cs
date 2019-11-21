using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace A_STAR_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creating the list of Caverns
            List<Cavern> caverns = new List<Cavern>();

            //string cavernSource = "generated5000-1.cav";
            //Extracting the caverns list and conenction from the file.
            CreateCavernList(args[0], caverns);

            //Creating the list of unvisited caverns
            List<Cavern> unvisitedCaverns = new List<Cavern>();

            //Adding the first cavern to the unvisited caverns list
            unvisitedCaverns.Add(caverns[0]);

            //Setting the first cavern distance from Start to 0
            unvisitedCaverns[0].G = 0;
            //Calculating distance of first cavern to Target
            caverns.First().CalcH(caverns.Last().X, caverns.Last().Y);

            int visiting = 0; //Index of visiting cavern

            while (unvisitedCaverns.Count() > 0 && visiting != caverns.Last().ID) //Loop Until we have visited every cavern or we visited the target cavern
            {
                visiting = FindNextCavern(unvisitedCaverns); //Searching the closest cavern to Target

                foreach (int connectedID in caverns[unvisitedCaverns[visiting].ID].connections)
                {
                    if (!caverns[connectedID].Visited)
                    {
                        caverns[connectedID].CalcG(caverns[unvisitedCaverns[visiting].ID]);
                        caverns[connectedID].CalcH(caverns.Last().X, caverns.Last().Y);

                        if (!caverns[connectedID].IsInUnvisited)
                        {
                            //Adding cavern t unvisited
                            unvisitedCaverns.Add(caverns[connectedID]);
                            //Marking the cavern as ToBeExplored - this will avoid repetition of the same cavern
                            unvisitedCaverns.Last().IsInUnvisited = true;
                        }
                        
                    }                                        
                }
                //Marking the visited cavern as Visited
                unvisitedCaverns[visiting].Visited = true;
                //Removing the visited cavern from the UnvisitedCaverns List
                unvisitedCaverns[visiting].IsInUnvisited = false;
                unvisitedCaverns.RemoveAt(visiting);
            }

            string route = string.Empty; //This string will store the route
            string solutionFileName = args[0].Remove(args[0].Length - 4, 4); //Creating the FileName for the solution
            solutionFileName = solutionFileName.Insert(0, "SOLUTION_");
            solutionFileName += ".csn";

            if (caverns.Last().PreviousCavern != -1) //Checking if we managed to find a route to the Target Cavern
            {
                int ind = caverns.Last().ID;
                while (ind != -1) //ind will become -1 when we reach the Starting Cavern
                {
                    route = route.Insert(0, string.Format("{0} ", (ind + 1).ToString()));
                    ind = caverns[ind].PreviousCavern;
                }
            }
            else
                route = "0";

            //Creating SOLUTION file
            File.WriteAllText(solutionFileName, route);

            //To be removed before last build
            //Console.WriteLine("Route: {0}\nRoute Length: {1:0.00}", route, caverns.Last().G);
            //Console.ReadLine();
        }

        static public void CreateCavernList(string fileName, List<Cavern> cavernsL)
        {
            string cavernsInformation = System.IO.File.ReadAllText(fileName);

            //Extraxting numeric values from text file.
            //string pattern = @"\d+";
            //Regex rgx = new Regex(pattern);

            //List<int> extracted = new List<int>(); //List of Numeric Values from File

            //foreach (Match coord in rgx.Matches(cavernsInformation))
            //{
            //    extracted.Add(Convert.ToInt32(coord.Value.ToString()));
            //}

            string[] extracted = cavernsInformation.Split(',');
            
            //Getting Caverns Number (First Extracted Number)
            int nCavern = int.Parse(extracted[0]);

            int cIndex = 0; //Cavern ID
            for (int i = 1; i <= nCavern * 2; i += 2)
            {
                cavernsL.Add(new Cavern(cIndex, int.Parse(extracted[i]), int.Parse(extracted[i + 1])));
                cIndex++;
            }

            //Removing Caverns Coordinates from the extraxted values
            //extracted.RemoveRange(0, nCavern * 2 + 1);

            //Adding Connections to the Caverns
            int matrixI = nCavern * 2 + 1; //Keeps track of the extracted values in extractedList
            {
                for (int r = 0; r < nCavern; r++)
                {
                    for (int c = 0; c < nCavern; c++)
                    {
                        if (int.Parse(extracted[matrixI]) == 1) //1 means we have a connection between two Caverns
                            cavernsL[c].connections.Add(Convert.ToInt32(r));
                        matrixI++;
                    }
                }
            }
        }

        static public int FindNextCavern(List<Cavern> unvisited)
        {
            double minDist = double.MaxValue;  //Setting Min distance to MAX VALUE            
            int unvisitedIndex = -1; //Index in UnvisitedCaverns
            int i = 0; //Keeps track of ID
            foreach (Cavern c in unvisited)
            {
                double f = c.G + c.H;
                if (f < minDist)
                {
                    minDist = f; //Updating minDist with new value                    
                    unvisitedIndex = i;  //Index of the closest cavern in the UnvisitedCaverns List
                }
                i++;
            }
            return unvisitedIndex;
        }

    }
}
