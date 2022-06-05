using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private List<(Vector3 p, Color color)> pixels;
        private Vector4 LightPosition;

        public World()
        {
            LightPosition = new Vector4(-80f, 20f, -180f, 1);
            Cuboid cuboid1 = new Cuboid(-50f, -90f, -300f, 100f, 100f, 80f);
            Cuboid cuboid2 = new Cuboid(50f, -30f, -300f, 40f, 60f, 60f);
            Cuboid cuboid3 = new Cuboid(-80f, -50f, -250f, 40f, 40f, 40f);
            Cuboid cuboid4 = new Cuboid(-90f, -30f, -300f, 40f, 60f, 60f);

            worldObjects = new List<WorldObject>()
            {
                cuboid3
            };
        }

        public void Observe(Matrix? worldTransformationMatrix, Matrix perspectiveTransformationMatrix)
        {
            foreach (var obj in worldObjects)
            {
                obj.Observe(worldTransformationMatrix, perspectiveTransformationMatrix);
            }

            LightTransform(worldTransformationMatrix);
        }

        private void LightTransform(Matrix? worldTransformationMatrix)
        {
            if (worldTransformationMatrix is Matrix wtm)
            {
                LightPosition = LightPosition.LeftTransform(wtm);
            }

            //LightPosition = LightPosition.Z >= 0 ? new Vector4(LightPosition.X, LightPosition.Y, -0.001f, LightPosition.W) : LightPosition;

            //LightPosition = LightPosition.LeftTransform(perspectiveTransformationMatrix);
            //LightPosition = LightPosition.PerspectiveDivide();
        }

        public void ScanLine()
        {
            lines = new List<(Vector2 p1, Vector2 p2, Color color)>();
            pixels = new();
            List<Edge> edges = new();
            List<Polygon> polygons = new();

            List<(IntersectPoint p1, IntersectPoint p2, Polygon polygon)> texturesLines = new();

            foreach (var o in worldObjects)
            {
                if (o.IsOut())
                {
                    continue;
                }
                o.GenerateEdgesAndPolygons(LightPosition);
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

                if (intersectPoints.Count == 1)
                {
                    continue;
                }

                Polygon pastPol = null;
                IntersectPoint startP = null;
                IntersectPoint lastP = null;

                for (int j = 0; j < intersectPoints.Count; j++)
                {
                    IntersectPoint firstPoint = intersectPoints[j];
                    IntersectPoint secondPoint = null;

                    if(j+1 == intersectPoints.Count)
                    {
                        break;
                    }

                    secondPoint = intersectPoints[j + 1];

                    if(secondPoint.X < 0 || firstPoint.X > GraphicsManager.ScreenWidth)
                    {
                        continue;
                    }

                    (float x, float y) mPoint = ((secondPoint.X + firstPoint.X)  /  2, secondPoint.Y);

                    float minZ = float.MaxValue;

                    //Color? lineColor = null;
                    Polygon polygon = null;

                    foreach (var pol in polygons)
                    {
                        if (pol.IsInPolygon(mPoint))
                        {
                            var tZ = pol.CalculateDepth(mPoint.x, mPoint.y);
                            if(tZ < minZ)
                            {
                                minZ = tZ;
                                //lineColor = pol.Color;
                                polygon = pol;
                            }
                        }
                    }

                    // Debug.WriteLine("");

                    //if (polygon != null && pastPol == null)
                    //{
                    //    startP = firstPoint;
                    //    lastP = secondPoint;
                    //}
                    //else if (polygon == null && pastPol != null)
                    //{
                    //    texturesLines.Add((startP, lastP, pastPol));
                    //}
                    //else if (pastPol.Id != polygon.Id)
                    //{
                    //    texturesLines.Add((startP, lastP, pastPol));
                    //    startP = firstPoint;
                    //    lastP = secondPoint;
                    //}
                    //else if (pastPol.Id == polygon.Id)
                    //{
                    //    lastP = secondPoint;
                    //}
                    //else
                    //{
                    //    continue;
                    //}

                    //pastPol = polygon;

                    if (polygon != null)
                    {
                        //lines.Add((new Vector2(firstPoint.X, firstPoint.Y), new Vector2(secondPoint.X, secondPoint.Y), lineColor.Value));
                        pixels = pixels.Concat(polygon.TextureInterpolate(firstPoint, secondPoint)).ToList();
                        //texturesLines.Add((firstPoint, secondPoint, polygon));
                    }
                }
                //Debug.WriteLine(lines.Count);
            }

            //IntersectPoint startP = texturesLines[0].p1;
            //IntersectPoint lastP = texturesLines[0].p2;

            //List<(IntersectPoint p1, IntersectPoint p2, Polygon polygon)> updateTexturesLines = new();
            //for (int i = 0; i + 1 < texturesLines.Count; i++)
            //{
            //    if(texturesLines[i].polygon.Id == texturesLines[i + 1].polygon.Id)
            //    {
            //        lastP = texturesLines[i + 1].p2;
            //    }
            //    else
            //    {
            //        updateTexturesLines.Add((startP, lastP, texturesLines[i].polygon));
            //        startP = texturesLines[i + 1].p1;
            //        lastP = texturesLines[i + 1].p2;
            //    }
            //}

            //if(updateTexturesLines[^1].polygon.Id != texturesLines[^1].polygon.Id)
            //{
            //    updateTexturesLines.Add((startP, lastP, texturesLines[^1].polygon));
            //}

            //foreach (var (p1, p2, polygon) in texturesLines)
            //{
            //    pixels = pixels.Concat(polygon.TextureInterpolate(p1, p2)).ToList();
            //}
        }

        private (float?, float?) CalcIntersect(Edge edge, int y)
        {
            //krawedz na prosta
            float A1 = (edge.P1.Y - edge.P2.Y);
            float B1 = edge.P2.X - edge.P1.X;
            float C1 = edge.P1.X * edge.P2.Y - edge.P2.X * edge.P1.Y;

            float A2 = 0;
            float B2 = 1;
            float C2 = -y;

            float? xp = null;

            if((A1 * B2 - A2 * B1) != 0)
            {
                xp = (B1 * C2 - B2 * C1) / (A1 * B2 - A2 * B1);
            }

            return (xp, y);
        }

        public void Draw()
        {
            //foreach (var obj in worldObjects)
            //{
            //    obj.Draw();
            //}
            //worldObjects[0].Draw();

            //GraphicsManager.spriteBatch.d

            //foreach (var line in lines)
            //{
            //    GraphicsManager.spriteBatch.DrawLine(line.p1, line.p2, line.color);
            //}

            foreach (var pixel in pixels)
            {
                GraphicsManager.spriteBatch.DrawPoint(pixel.p.X, pixel.p.Y, pixel.color);
            }
        }
    }
}
