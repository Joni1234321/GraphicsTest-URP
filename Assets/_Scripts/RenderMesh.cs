using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class RenderMesh
{
    private readonly Mesh _mesh;
    private readonly int _vertexCount;

    // Positions representations
    private readonly int n, w, h;

    private readonly NativeArray<Matrix4x4> _matrices;
    private readonly GraphicsBuffer _graphicsBufferPositions;    
    
    private readonly Primitives _primitives;
    private static readonly int SIZE = Shader.PropertyToID("_Size");
    private static readonly int LOCAL_POSITIONS = Shader.PropertyToID("_LocalPositions");
    private static readonly int TRIANGLES = Shader.PropertyToID("_Triangles");
    private static readonly int POSITIONS = Shader.PropertyToID("_Positions");

    // Remove GC 
    public RenderMesh( Mesh mesh, NativeArray<Vector3> positions)
    {
        n = positions.Length;
        w = h = (int)math.sqrt(n);
        
        _mesh = mesh;
        _vertexCount = (int)mesh.GetIndexCount(0);
        
        // Matrix
        _matrices = new NativeArray<Matrix4x4>(n, Allocator.Persistent);
        for (int i = 0; i < n; i++)
            _matrices[i] = Matrix4x4.Translate(positions[i]);
        
        // Graphics buffers
        _graphicsBufferPositions = new GraphicsBuffer(GraphicsBuffer.Target.Structured, n, 3 * sizeof(float));
        _graphicsBufferPositions.SetData(positions);
        
        _primitives = new Primitives(_mesh);
    }
    
    
    
    public void Render(RenderParams rp)
    {
        for (int i = 0; i < _matrices.Length; i++)
            Graphics.RenderMesh(rp, _mesh, 0, _matrices[i]);
    }

    public void RenderMeshInstanced(RenderParams rp)
    {
        Graphics.RenderMeshInstanced(rp, _mesh, 0, _matrices);
    }

    public void RenderMeshPrimitives(RenderParams rp)
    {
        rp.matProps.SetBuffer(TRIANGLES, _primitives.Triangles);
        rp.matProps.SetBuffer(LOCAL_POSITIONS, _primitives.LocalPositions);
        rp.matProps.SetBuffer(POSITIONS, _graphicsBufferPositions);

        Graphics.RenderMeshPrimitives(rp, _mesh, 0, n);
    } 

    public void RenderMeshPrimitivesFast(RenderParams rp)
    {
        rp.matProps.SetBuffer(TRIANGLES, _primitives.Triangles);
        rp.matProps.SetBuffer(LOCAL_POSITIONS, _primitives.LocalPositions);
        rp.matProps.SetVector(SIZE, new Vector2(w, h));

        Graphics.RenderMeshPrimitives(rp, _mesh, 0, n);
    }

    public void RenderPrimitives(RenderParams rp, MeshTopology topology = MeshTopology.Triangles)
    {
        rp.matProps.SetBuffer(TRIANGLES, _primitives.Triangles);
        rp.matProps.SetBuffer(LOCAL_POSITIONS, _primitives.LocalPositions);
        rp.matProps.SetBuffer(POSITIONS, _graphicsBufferPositions);

        Graphics.RenderPrimitives(rp, topology, _vertexCount, n);
    }
    public void RenderPrimitivesFast(RenderParams rp, MeshTopology topology = MeshTopology.Triangles)
    {
        rp.matProps.SetBuffer(TRIANGLES, _primitives.Triangles);
        rp.matProps.SetBuffer(LOCAL_POSITIONS, _primitives.LocalPositions);
        rp.matProps.SetVector(SIZE, new Vector2(w, h));

        Graphics.RenderPrimitives(rp, topology, _vertexCount, n);
    }


    class Primitives : IDisposable
    {
        public GraphicsBuffer Triangles, LocalPositions, Normals;

        public Primitives(Mesh mesh)
        {
            Triangles = new GraphicsBuffer(GraphicsBuffer.Target.Structured, mesh.triangles.Length, sizeof(int));
            LocalPositions = new GraphicsBuffer(GraphicsBuffer.Target.Structured, mesh.vertices.Length, 3 * sizeof(float));
            Normals = new GraphicsBuffer(GraphicsBuffer.Target.Structured, mesh.normals.Length, 3 * sizeof(float));
            Triangles.SetData(mesh.triangles);
            LocalPositions.SetData(mesh.vertices);
            Normals.SetData(mesh.normals);
        }


        public void Dispose()
        {
            Triangles?.Dispose();
            LocalPositions?.Dispose();
            Normals?.Dispose();
            Triangles = null;
            LocalPositions = null;
            Normals = null;
        }
    } 

    
}

