using UnityEngine;
using UnityEngine.Rendering;

namespace CDPipeline
{
    public class CameraRenderer : ICameraRenderer
    {
        private CommandBuffer _buffer = null;

        private string _bufferName = "Render Camera";

        public void Render( ScriptableRenderContext context, Camera camera )
        {
            this.SetupContext( context, camera );

            this.OnBeginRender( context );

            this.DrawVisibleGeometry(context, camera);

            this.Submit( context );
        }

        protected void OnBeginRender( ScriptableRenderContext context )
        {
            _buffer = new CommandBuffer()
            {
                name = _bufferName
            };

            _buffer.ClearRenderTarget( true, true, Color.clear );

            _buffer.BeginSample( _buffer.name );

            this.ExecuteCommandBuff( context, _buffer );
        }

        protected void SetupContext( ScriptableRenderContext context, Camera camera )
        {
            context.SetupCameraProperties( camera );
        }

        protected void DrawVisibleGeometry( ScriptableRenderContext context, Camera camera )
        {
            context.DrawSkybox( camera );
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
    }
}
