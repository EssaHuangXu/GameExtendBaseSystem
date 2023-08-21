#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Rendering;

namespace CDPipeline
{
    public class CameraRenderer : ICameraRenderer
    {
        private CommandBuffer _buffer = new CommandBuffer()
		{
			name = _bufferName,
		};

        private const string _bufferName = "Render Camera";

		private static ShaderTagId _unlitShaderTagId = new ShaderTagId( "SRPDefaultUnlit" );

#if UNITY_EDITOR
		private static ShaderTagId[] legacyShaderTagIds = {
	        new ShaderTagId( "Always" ),
			new ShaderTagId( "ForwardBase" ),
	        new ShaderTagId( "PrepassBase" ),
	        new ShaderTagId( "Vertex" ),
	        new ShaderTagId( "VertexLMRGBM" ),
	        new ShaderTagId( "VertexLM" )
        };

		private static Material _errorMaterial = new Material( Shader.Find( "Hidden/InternalErrorShader" ) );
#endif

		public void Render( ScriptableRenderContext context, Camera camera )
        {
#if UNITY_EDITOR
            this.PrepareBuffer( camera );

            this.PrepareForSceneWindow( camera );
#endif

			if( this.Cull( context, camera, out var cullingResults ) == false )
            {
                return;
            }

            this.SetupContext( context, camera );

            this.OnBeginRender( context, camera );

            this.DrawVisibleGeometry( context, camera, ref cullingResults );
#if UNITY_EDITOR
            this.DrawUnsupportedShaders( context, camera, ref cullingResults );

            this.DrawGizmos( context, camera );
#endif
            this.Submit( context );
        }

        protected bool Cull( ScriptableRenderContext context, Camera camera, out CullingResults results )
        {
            if( camera.TryGetCullingParameters( out var parameters ) )
            {
				results = context.Cull( ref parameters );
                return true;
            }
            results = new CullingResults();
			return false;
        }

        protected void OnBeginRender( ScriptableRenderContext context, Camera camera )
        {
			CameraClearFlags flags = camera.clearFlags;
			_buffer.ClearRenderTarget( flags <= CameraClearFlags.Depth, flags <= CameraClearFlags.Color, flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear );
			_buffer.BeginSample( _buffer.name );
            this.ExecuteCommandBuff( context, _buffer );
        }

        protected void SetupContext( ScriptableRenderContext context, Camera camera )
        {
            context.SetupCameraProperties( camera );
        }

        protected void DrawVisibleGeometry( ScriptableRenderContext context, Camera camera, ref CullingResults cullingResults )
        {
            var filterSettings = new FilteringSettings( RenderQueueRange.opaque );
            var sortingSetting = new SortingSettings( camera )
            {
                criteria = SortingCriteria.CommonOpaque
            };
			var drawSettings = new DrawingSettings( _unlitShaderTagId, sortingSetting );
            context.DrawRenderers( cullingResults, ref drawSettings, ref filterSettings );
            
            context.DrawSkybox( camera );

            filterSettings.renderQueueRange = RenderQueueRange.transparent;
            sortingSetting.criteria = SortingCriteria.CommonTransparent;
            drawSettings.sortingSettings = sortingSetting;
			context.DrawRenderers( cullingResults, ref drawSettings, ref filterSettings );
		}

        protected void Submit( ScriptableRenderContext context )
        {
            _buffer.EndSample( _buffer.name );
            this.ExecuteCommandBuff( context, _buffer );
            context.Submit();
        }

        protected void ExecuteCommandBuff( ScriptableRenderContext context, CommandBuffer buffer )
        {
            context.ExecuteCommandBuffer( buffer );
            buffer.Clear();
        }

#if UNITY_EDITOR
        protected void PrepareBuffer( Camera camera )
        {
			_buffer.name = camera.name;
        }

		protected void DrawUnsupportedShaders( ScriptableRenderContext context, Camera camera, ref CullingResults cullingResults )
        {
            var drawingSettings = new DrawingSettings( legacyShaderTagIds[0], new SortingSettings( camera ) );
            drawingSettings.overrideMaterial = _errorMaterial;

			for( var i = 1; i < legacyShaderTagIds.Length; i++ )
            {
                drawingSettings.SetShaderPassName( i, legacyShaderTagIds[i] );
            }
            
            var filteringSetting = FilteringSettings.defaultValue;
            context.DrawRenderers( cullingResults, ref drawingSettings, ref filteringSetting );
		}

        protected void DrawGizmos( ScriptableRenderContext context, Camera camera )
        {
            if( Handles.ShouldRenderGizmos() == true )
            {
                context.DrawGizmos( camera, GizmoSubset.PreImageEffects );
                context.DrawGizmos( camera, GizmoSubset.PostImageEffects );
            }
        }

        protected void PrepareForSceneWindow( Camera camera )
        {
			if( camera.cameraType == CameraType.SceneView )
			{
				ScriptableRenderContext.EmitWorldGeometryForSceneView( camera );
			}
		}
#endif
	}
}
