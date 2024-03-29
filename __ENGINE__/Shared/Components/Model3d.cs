﻿using __ENGINE__.Engine;
using __ENGINE__.Shared;
using __ENGINE__.Shared.Components;
using OpenTK.Mathematics;

namespace __ENGINE__.Game.Components
{
    public class Model3d
    {
        private static readonly Dictionary<string, Model3d> components = new();

        private readonly List<VertexData[]> _faces = new();
        private readonly List<VertexData[]> _lines = new();

        public class Component : __ENGINE__Obj.Component
        {
            private readonly Model3d _model;
            private readonly Func _before = empty;
            private readonly Func _after = empty;

            public delegate void Func(__ENGINE__Obj objIn);

            private static void empty(__ENGINE__Obj obj)
            {
            }

            public Component(Model3d model, Func before, Func after)
            {
                _model = model;
                _before = before;
                _after = after;
            }
            
            public Component(Model3d model)
            {
                _model = model;
            }

            public override void render(__ENGINE__Obj objIn)
            {
                base.render(objIn);

                _before(objIn);
                _model.render(objIn.get<FloatPos>().to_vec3());
                _after(objIn);
            }
        }

        public void flipX()
        {
            foreach (VertexData[] face in _faces)
            {
                face[0].pos.X *= -1;
                face[1].pos.X *= -1;
                face[2].pos.X *= -1;
                calculateNormals(face);
            }
        }
        
        public void flipY()
        {
            foreach (VertexData[] face in _faces)
            {
                face[0].pos.Y *= -1;
                face[1].pos.Y *= -1;
                face[2].pos.Y *= -1;
                calculateNormals(face);
            }
        }
        
        public void flipZ()
        {
            foreach (VertexData[] face in _faces)
            {
                face[0].pos.Z *= -1;
                face[1].pos.Z *= -1;
                face[2].pos.Z *= -1;
                calculateNormals(face);
            }
        }

        private Model3d(string path, Dictionary<string, uint> colors)
        {
            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            List<Vector2> texCoords = new();
            string mat = "";
            foreach (string line in File.ReadAllLines($"Resource/Texture/{path}.obj"))
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }

                string[] parts = line.Split(' ');
                switch (parts[0])
                {
                    case "v":
                    {
                        Vector3 vec = new();
                        vec.X = float.Parse(parts[1]);
                        vec.Y = float.Parse(parts[2]);
                        vec.Z = float.Parse(parts[3]);
                        vertices.Add(vec);
                        break;
                    }
                    
                    case "vn":
                    {
                        Vector3 vec = new();
                        vec.X = float.Parse(parts[1]);
                        vec.Y = float.Parse(parts[2]);
                        vec.Z = float.Parse(parts[3]);
                        vec.Normalize();
                        normals.Add(vec);
                        break;
                    }
                    case "vt":
                    {
                        Vector2 vec = new();
                        vec.X = float.Parse(parts[1]);
                        vec.Y = float.Parse(parts[2]);
                        texCoords.Add(vec);
                        break;
                    }
                    case "usemtl":
                        mat = parts[1];
                        break;
                    case "f":
                    {
                        string[] vt1 = parts[1].Split("/");
                        string[] vt2 = parts[2].Split("/");
                        string[] vt3 = parts[3].Split("/");
                        VertexData[] face = new VertexData[3];
                        face[0] = new VertexData(vertices[int.Parse(vt1[0]) - 1], texCoords[int.Parse(vt1[1]) - 1]);
                        face[1] = new VertexData(vertices[int.Parse(vt2[0]) - 1], texCoords[int.Parse(vt2[1]) - 1]);
                        face[2] = new VertexData(vertices[int.Parse(vt3[0]) - 1], texCoords[int.Parse(vt3[1]) - 1]);
                        uint val;
                        val = colors.TryGetValue(mat, out val) ? val : 0xFFFFFFFF;
                        face[0].color = face[1].color = face[2].color = val;
                        face[0].normal = normals[int.Parse(vt1[2]) - 1];
                        face[1].normal = normals[int.Parse(vt2[2]) - 1];
                        face[2].normal = normals[int.Parse(vt3[2]) - 1];
                        _faces.Add(face);
                        break;
                    }
                }
            }
            components[path + colors.content_to_string()] = this;
        }

        private static void calculateNormals(VertexData[] face)
        {
            Vector3 ab = face[1].pos - face[0].pos;
            Vector3 ac = face[2].pos - face[0].pos;
            Vector3 normal = Vector3.Cross(ab, ac);
            normal.Normalize();
            face[0].normal = face[1].normal = face[2].normal = normal;
        }

        public void render(Vector3 pos)
        {
            Mesh mesh = RenderSystem.mesh;
            foreach (VertexData[] face in _faces)
            {
                Vector3 vt1 = face[0].pos;
                Vector3 vt2 = face[1].pos;
                Vector3 vt3 = face[2].pos;
                int i1 = mesh.float3(RenderSystem.model, vt1.X + pos.X, vt1.Y + pos.Y, vt1.Z + pos.Z).float3(face[0].normal).float2(face[0].uv).float4(face[0].color).next();
                int i2 = mesh.float3(RenderSystem.model, vt2.X + pos.X, vt2.Y + pos.Y, vt2.Z + pos.Z).float3(face[1].normal).float2(face[1].uv).float4(face[1].color).next();
                int i3 = mesh.float3(RenderSystem.model, vt3.X + pos.X, vt3.Y + pos.Y, vt3.Z + pos.Z).float3(face[2].normal).float2(face[2].uv).float4(face[2].color).next();
                mesh.tri(i1, i2, i3);
            }
        }

        public static Model3d read(string path, Dictionary<string, uint> colors)
        {
            return components.ContainsKey(path + colors.content_to_string()) ? components[path + colors.content_to_string()] : new(path, colors);
        }
    }
}