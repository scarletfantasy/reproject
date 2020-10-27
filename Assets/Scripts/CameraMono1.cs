using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

public class CameraMono1 : MonoBehaviour
{

    // Start is called before the first frame update
    private static class CameraShaderParams
    {
        public static readonly int _WorldSpaceCameraPos = Shader.PropertyToID("_WorldSpaceCameraPos");
        public static readonly int _InvCameraViewProj = Shader.PropertyToID("_InvCameraViewProj");
        public static readonly int _CameraFarDistance = Shader.PropertyToID("_CameraFarDistance");
    }
    CommandBuffer cmd;
    CommandBuffer cmd1;
    Camera camera;
    public Material mixmat;
    public RayTracingShader _shader;
    public RenderTexture rt;
    RayTracingAccelerationStructure accstruct;
    public Renderer[] renders;
    RTHandle stencilimage;
    public anothereye anothereyesc;
    const int maxNumSubMeshes = 32;
    private bool[] subMeshFlagArray = new bool[maxNumSubMeshes];
    private bool[] subMeshCutoffArray = new bool[maxNumSubMeshes];
    private static void SetupCamera(Camera camera)
    {
        Shader.SetGlobalVector(CameraShaderParams._WorldSpaceCameraPos, camera.transform.position);
        var projMatrix = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false);
        var viewMatrix = camera.worldToCameraMatrix;
        var viewProjMatrix = projMatrix * viewMatrix;
        var invViewProjMatrix = Matrix4x4.Inverse(viewProjMatrix);
        Shader.SetGlobalMatrix(CameraShaderParams._InvCameraViewProj, invViewProjMatrix);
        Shader.SetGlobalFloat(CameraShaderParams._CameraFarDistance, camera.farClipPlane);
    }
    protected RTHandle RequireOutputTarget(Camera camera)
    {
        var id = camera.GetInstanceID();



        var outputTarget = RTHandles.Alloc(
          800,
          600,
          1,
          DepthBits.None,
          GraphicsFormat.R32G32B32A32_SFloat,
          FilterMode.Point,
          TextureWrapMode.Clamp,
          TextureDimension.Tex2D,
          true,
          false,
          false,
          false,
          1,
          0f,
          MSAASamples.None,
          false,
          false,
          RenderTextureMemoryless.None,
          $"OutputTarget_{camera.name}");


        return outputTarget;
    }
    private void Awake()
    {
        for (var i = 0; i < maxNumSubMeshes; ++i)
        {
            subMeshFlagArray[i] = true;
            subMeshCutoffArray[i] = false;
        }
        cmd = new CommandBuffer();
        cmd1 = new CommandBuffer();
        //cmd.name = "ray";
        camera = GetComponent<Camera>();
        SetupCamera(camera);
        if (!rt)
        {

            rt = RenderTexture.GetTemporary(800, 600, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
        }
        int _outputTargetShaderId = Shader.PropertyToID("_OutputTarget");
        int accelerationStructureShaderId = Shader.PropertyToID("_AccelerationStructure");
        
        accstruct = new RayTracingAccelerationStructure();
        foreach (var r in renders)
        {
            accstruct.AddInstance(r, subMeshFlagArray, subMeshCutoffArray);
        }
        accstruct.Build();
        var outputTarget = RequireOutputTarget(camera);
        stencilimage = RequireOutputTarget(camera);
        cmd.SetRayTracingShaderPass(_shader, "RayTracing");
        cmd.SetRayTracingTextureParam(_shader, _outputTargetShaderId, outputTarget);
        cmd.SetRayTracingAccelerationStructure(_shader, accelerationStructureShaderId, accstruct);
        cmd.DispatchRays(_shader, "AddASphereRayGenShader", (uint)800, (uint)600, 1, camera);
        //cmd.DispatchRays(_shader, "OutputColorRayGenShader", (uint)800, (uint)600, 1, camera);
        cmd.Blit(outputTarget, rt, Vector2.one, Vector2.zero);
        //camera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, cmd);
    }
    void Start()
    {
        
    }
    private void OnPreRender()
    {
        cmd1.Blit(anothereyesc.targetimage, stencilimage, Vector2.one, Vector2.zero);
        Graphics.ExecuteCommandBuffer(cmd1);
        accstruct.Dispose();
        accstruct = new RayTracingAccelerationStructure();
        foreach (var r in renders)
        {
            accstruct.AddInstance(r, subMeshFlagArray, subMeshCutoffArray);
        }
        accstruct.Build();
        int accelerationStructureShaderId = Shader.PropertyToID("_AccelerationStructure");
        cmd.SetRayTracingAccelerationStructure(_shader, accelerationStructureShaderId, accstruct);
        int _stencilShaderId = Shader.PropertyToID("_StencilImage");
        cmd.SetRayTracingTextureParam(_shader, _stencilShaderId, stencilimage);
        Graphics.ExecuteCommandBuffer(cmd);
        foreach (var r in renders)
        {
            r.enabled = false;
        }
        //RenderTexture.ReleaseTemporary(rt);
    }
    private void OnPostRender()
    {
        foreach (var r in renders)
        {
            r.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mixmat.SetTexture("_RayTex", rt);
        Graphics.Blit(anothereyesc.targetimage, destination, mixmat);
        //Graphics.Blit(rt, destination);
    }

}
