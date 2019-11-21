using System;
using System.Collections.Generic;

namespace A_STAR_Algorithm
{
    class Cavern
    {
        public int X { get; set; } //X Value
        public int Y { get; set; }  //Y Value
        public int ID { get; set; } //Cavern ID
        public int PreviousCavern { get; set; } //ID of Previous Cavern in Route
        public List<int> connections = new List<int>(); //List if IDs for Connected Caverns
        public double G { get; set; } //Distance from first Cavern (Route Distance)
        public double H { get; set; } //Distance to Target Cavers
        public bool Visited { get; set; }
        public bool IsInUnvisited { get; set; }

        //Constructor
        public Cavern(int id, int x, int y)
        {
            ID = id;
            X = x;
            Y = y;
            G = double.MaxValue;
            H = double.MaxValue;
            Visited = false;
            IsInUnvisited = false;
            PreviousCavern = -1;
        }

        //Calculate the StarDistance and Distance to Target
        public void CalcG(Cavern pCavern)
        {
            //Calculating the distance between this node and the previous cavern
            double routeLength = pCavern.G + Math.Sqrt(Math.Pow(X - pCavern.X, 2) + Math.Pow(Y - pCavern.Y, 2));

            //If routeLength is < than StartDistance we have found a shorter route. We update the information for this Cavern
            if (routeLength < G)
            {
                G = routeLength;
                PreviousCavern = pCavern.ID;
            }
        }

        //Calculating this cavern distance to targhet node. [Unfortunately this is calculated every time :(
        public void CalcH(int tX, int tY)
        {
            H = Math.Sqrt(Math.Pow(tX - X, 2) + Math.Pow(tY - Y, 2));
        }
    }
}
