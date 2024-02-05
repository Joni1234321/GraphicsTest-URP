using Unity.Collections;
using UnityEngine;

public class RenderController : MonoBehaviour
{

    public Mesh Mesh;
    
    
    public Material Mat, MatInst, MatPrim, MatPrimFast, MatPrimMesh, MatPrimMeshFast;
    private RenderParams _rp, _rpInst, _rpPrim, _rpPrimFast, _rpPrimMesh, _rpPrimMeshFast;
    
    public RenderMesh RenderMesh;

    public int W = 10, H = 10;
    private NativeArray<Vector3> _positions;


    void UpdatePositions(int w, int h)
    {
        _positions.Dispose();
        _positions = new NativeArray<Vector3>(w * h, Allocator.Persistent);
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                _positions[y * w + x] = 1.5f * new Vector3(x, 0, y);
            }
        }
        RenderMesh = new RenderMesh(Mesh, _positions);
    }

    private int _state, _substate;


    void Render()
    {
        if (_state == 1) RenderMesh.Render(_rp);
        else if (_state == 2) RenderMesh.RenderMeshInstanced(_rpInst);
        else if (_state == 3) RenderMesh.RenderPrimitives(_rpPrim, Helper.GetMeshTopology(_substate));
        else if (_state == 4) RenderMesh.RenderPrimitivesFast(_rpPrimFast, Helper.GetMeshTopology(_substate));
        else if (_state == 5) RenderMesh.RenderMeshPrimitives(_rpPrimMesh);
        else if (_state == 6) RenderMesh.RenderMeshPrimitivesFast(_rpPrimMeshFast);
    }
    
    
    void Start()
    {
        UpdatePositions(W, H);
        _rp = Helper.GetRenderParams(Mat);
        _rpInst = Helper.GetRenderParams(MatInst);
        _rpPrim = Helper.GetRenderParams(MatPrim);
        _rpPrimFast = Helper.GetRenderParams(MatPrimFast);
        _rpPrimMesh = Helper.GetRenderParams(MatPrimMesh);
        _rpPrimMeshFast = Helper.GetRenderParams(MatPrimMeshFast);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) _state = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) _state = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) _state = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) _state = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) _state = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) _state = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) _state = 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) _state = 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) _state = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) _state = 9;

        if (Input.GetKeyDown(KeyCode.Q)) _substate = 0;
        if (Input.GetKeyDown(KeyCode.W)) _substate = 1;
        if (Input.GetKeyDown(KeyCode.E)) _substate = 2;
        if (Input.GetKeyDown(KeyCode.R)) _substate = 3;
        if (Input.GetKeyDown(KeyCode.T)) _substate = 4;
        if (Input.GetKeyDown(KeyCode.Y)) _substate = 5;
        if (Input.GetKeyDown(KeyCode.U)) _substate = 6;
        if (Input.GetKeyDown(KeyCode.I)) _substate = 7;

        Render();
    }


    static class Helper
    {
        public static MeshTopology GetMeshTopology(int i) =>
            System.Enum.IsDefined(typeof(MeshTopology), i) ? (MeshTopology)i : MeshTopology.Triangles;

        public static RenderParams GetRenderParams(Material material) => 
            new RenderParams(material)
        {
            matProps = new MaterialPropertyBlock(), 
            worldBounds = new Bounds(Vector3.zero, Vector3.one * 10000)
        };
    }
    
}

