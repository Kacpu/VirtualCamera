using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VirtualCamera.Src
{
    public class World
    {
        private readonly List<WorldObject> worldObjects;
        private List<(Vector2 p1, Vector2 p2, Color color)> lines;

        public World()
        {
            Cuboid cuboid1 = new Cuboid(50f, -30f, -200f, 40f, 60f, 60f);
            Cuboid cuboid2 = new Cuboid(50f, -30f, -300f, 40f, 60f, 60f);
            Cuboid cuboid3 = new Cuboid(-90f, -30f, -200f, 40f, 60f, 60f);
            Cuboid cuboid4 = new Cuboid(-90f, -30f, -300f, 40f, 60f, 60f);

            worldObjects = new List<WorldObject>()
            {
                cuboid3, cuboid4, cuboid1, cuboid2
            };
        }

        public void Observe(Matrix? worldTransformationMatrix, Matrix perspectiveTransformationMatrix)
        {
            foreach (var obj in worldObjects)
            {
                obj.Observe(worldTransformationMatrix, perspectiveTransformationMatrix);
            }
        }

        public void ScanLine()
        {
            lines = new List<(Vector2 p1, Vector2 p2, Color color)>();
            List<Edge> edges = new List<Edge>();
            List<Polygon> polygons = new List<Polygon>();

            foreach (var o in worldObjects)
            {
                if (o.IsOut())
                {
                    continue;
                }
                o.GenerateEdgesAndPolygons();
                edges.AddRange(o.Edges);
                polygons.AddRange(o.Polygons);
            }

            if (!edges.Any())
            {
                return;
            }

            edges = edges.OrderBy(e => e.YMin).ToList();

            float min = edges[0].YMin;

            //rozpoczęcie pętli od min -> warunek stopu na max

            for (int i = (int)min; i < GraphicsManager.ScreenHeight; i++)
            {
                List<Edge> activeEdges = new List<Edge>();
                
                foreach (var edg in edges)
                {
                    if( i >= edg.YMin && i <= edg.YMax)
                    {
                        activeEdges.Add(edg);
                    }
                }

                //sprawdzenie czy sa rozpatrywane linie
                if(activeEdges.Count == 0)
                {
                    continue;
                }

                List<IntersectPoint> intersectPoints = new List<IntersectPoint>();

                foreach (var edg in activeEdges)
                {
                    (float? x, float? y) intersectPoint = CalcIntersect(edg, i);
                    if(intersectPoint.x != null)
                    {
                        intersectPoints.Add(new IntersectPoint(intersectPoint.x.Value, intersectPoint.y.Value, edg));
                    }
                }

                intersectPoints = intersectPoints.OrderBy(x => x.X).ToList();


                //<Edge, (float,float)> lineFrom = intersectPoints[0];
                //float lineTo = intersectPoints.Count == 1 ? intersectPoints[0] : intersectPoints[1];

                if (intersectPoints.Count == 1)
                {
                    //lines.Add((intersectPoints.First().Value, intersectPoints.First().Key.po);
                    continue;
                }

                for (int j = 0; j < intersectPoints.Count; j++)
                {
                    IntersectPoint firstPoint = intersectPoints[j];
                    IntersectPoint secondPoint = null;

                    if(j+1 == intersectPoints.Count)
                    {
                        break;
                    }

                    secondPoint = intersectPoints[j + 1];

                    (float x, float y) mPoint = ((secondPoint.X + firstPoint.X)  /  2, secondPoint.Y);

                    float minZ = float.MaxValue;
                    Color? lineColor = null; 

                    foreach (var pol in polygons)
                    {
                        if (pol.IsInPolygon(mPoint))
                        {
                            var tZ = pol.CalculateDepth(mPoint.x, mPoint.y);
                            if(tZ < minZ)
                            {
                                minZ = tZ;
                                lineColor = pol.Color;
                            }
                        }
                    }

                    if(lineColor != null)
                    {
                        lines.Add((new Vector2(firstPoint.X, firstPoint.Y), new Vector2(secondPoint.X, secondPoint.Y), lineColor.Value));
                    }

                    //float p1Depth = firstPoint.Edge.Polygon1.CalculateDepth(mPoint.x, mPoint.y);
                    //float p2Depth = firstPoint.Edge.Polygon2.CalculateDepth(mPoint.x, mPoint.y);

                    //if (p1Depth > p2Depth)
                    //{
                    //    firstPoint.Edge.MainPolygon = firstPoint.Edge.Polygon1;
                    //}
                    //else
                    //{
                    //    firstPoint.Edge.MainPolygon = firstPoint.Edge.Polygon2;
                    //}

                    //if(!secondPoint.Edge.Equals())

                    //lines.Add((new Vector2(firstPoint.X, firstPoint.Y), new Vector2(secondPoint.X, secondPoint.Y), firstPoint.Edge.MainPolygon.Color));
                }
                //Debug.WriteLine(lines.Count);
            }
        }

        private (float?, float?) CalcIntersect(Edge edge, int y)
        {
            //krawedz na prosta
            float A1 = (edge.P1.Y - edge.P2.Y);
            float B1 = edge.P2.X - edge.P1.X;
            float C1 = edge.P1.X   *   edge.P2.Y - edge.P2.X   *   edge.P1.Y;

            float A2 = 0;
            float B2 = 1;
            float C2 = -y;

            float? xp = null;
            float? yp = null;

            if((A1 * B2 - A2 * B1) != 0)
            {
                xp = (B1 * C2 - B2 * C1) / (A1 * B2 - A2 * B1);
                yp = (C1 * A2 - C2 * A1) / (A1 * B2 - A2 * B1);
            }

            return (xp, y);
        }

        public void Draw()
        {
            //foreach (var obj in worldObjects)
            //{
            //    obj.Draw();
            //}

            int c = 0;

            foreach (var line in lines)
            {
                c++;
                GraphicsManager.spriteBatch.DrawLine(line.p1, line.p2, line.color);
                if (c == lines.Count / 2)
                {
                    // break;
                }
            }
        }
    }
}
