﻿using ASCII_Render_Engine.Core;
using ASCII_Render_Engine.MathUtils.Vectors;
using ASCII_Render_Engine.Objects.Geometry.Polygons;
using ASCII_Render_Engine.Rendering;
using ASCII_Render_Engine.MathUtils.Matrixes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASCII_Render_Engine.MathUtils.Matrixes.Transforms;
using ASCII_Render_Engine.Objects.Camera;

namespace ASCII_Render_Engine.Objects.Geometry.Mesh;

public class Mesh3D : IRenderable
{
    public Poly3D[] Polygons { get; set; }
    public Vec3 Position { get; set; }
    public Vec3 Origin { get; set; }
    public Vec3 Angle { get; set; }
    public CameraConfig Camera { get; set; }

    // pool
    private Poly3D[] globalPolygons { get; set; }

    public Mesh3D(Poly3D[] polygons, CameraConfig camera)
    {
        Polygons = new Poly3D[polygons.Length];
        globalPolygons = new Poly3D[polygons.Length];
        for (int i = 0; i < polygons.Length; i++)
        {
            Polygons[i] = new Poly3D(polygons[i]);
            globalPolygons[i] = new Poly3D(Polygons[i]);
        }

        Position = new Vec3();
        Origin = new Vec3();
        Angle = new Vec3();
        Camera = camera;
    }
    public Mesh3D(int polygonCount, CameraConfig camera)
    {
        Polygons = new Poly3D[polygonCount];
        globalPolygons = new Poly3D[polygonCount];
        for (int i = 0; i < Polygons.Length; i++)
        {
            Polygons[i] = new Poly3D(3, null);
            globalPolygons[i] = new Poly3D(3, null);
        }

        Position = new Vec3();
        Origin = new Vec3();
        Angle = new Vec3();
        Camera = camera;
    }
    public Mesh3D(Mesh3D mesh)
    {
        Polygons = new Poly3D[mesh.Polygons.Length];
        globalPolygons = new Poly3D[mesh.Polygons.Length];
        for (int i = 0; i < mesh.Polygons.Length; i++)
        {
            Polygons[i] = new Poly3D(mesh.Polygons[i]);
            globalPolygons[i] = new Poly3D(mesh.Polygons[i]);
        }

        Position = mesh.Position;
        Origin = mesh.Origin;
        Angle = mesh.Angle;
        Camera = mesh.Camera;
    }

    public void Copy(Mesh3D mesh)
    {
        if (Polygons.Length != mesh.Polygons.Length)
        {
            Polygons = new Poly3D[mesh.Polygons.Length];
        }
        for (int i = 0; i < mesh.Polygons.Length; i++)
        {
            Polygons[i].Copy(mesh.Polygons[i]);
        }
        Position = mesh.Position;
        Origin = mesh.Origin;
        Angle = mesh.Angle;
    }

    public void Add(Poly3D poly)
    {
        Poly3D[] newPolygons = new Poly3D[Polygons.Length + 1];
        for (int i = 0; i < Polygons.Length; i++)
        {
            newPolygons[i] = Polygons[i];
        }
        newPolygons[Polygons.Length] = poly;
        Polygons = newPolygons;
    }
    public void Remove(int index)
    {
        Poly3D[] newPolygons = new Poly3D[Polygons.Length - 1];
        for (int i = 0; i < index; i++)
        {
            newPolygons[i] = Polygons[i];
        }
        for (int i = index + 1; i < Polygons.Length; i++)
        {
            newPolygons[i - 1] = Polygons[i];
        }
        Polygons = newPolygons;
    }

    public void Render(ScreenBuffer buffer, int frame, double runTime)
    {
        // transform
        for (int i = 0; i < Polygons.Length; i++)
        {
            Poly3D localPoly = Polygons[i];
            Poly3D globalPoly = globalPolygons[i];
            globalPoly.Copy(localPoly);
            for (int j = 0; j < localPoly.Vertices.Length; j++)
            {
                // rotate around local origin
                Vec3 vertex = localPoly.Vertices[j].Position;
                Mat3x3 rotationMatrix = Rotation.Rotate(Angle);
                Vec3 rotatedVertex = rotationMatrix * (vertex - Origin) + Origin;
                Vec3 globalVertex = rotatedVertex + Position;
                globalPoly.Vertices[j].Position = globalVertex;
            }
            globalPoly.Camera = Camera;
            globalPoly.Render(buffer, frame, runTime);
        }
    }
}
