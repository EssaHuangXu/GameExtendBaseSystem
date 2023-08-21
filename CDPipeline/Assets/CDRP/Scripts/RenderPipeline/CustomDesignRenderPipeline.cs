using UnityEngine;
using UnityEngine.Rendering;

namespace CDPipeline
{
    public class CustomDesignRenderPipeline : RenderPipeline
    {
        protected ICameraRenderer renderer;

        public CustomDesignRenderPipeline()
        {
            renderer = new CameraRenderer();
        }

        protected override void Render( ScriptableRenderContext context, Camera[] cameras )
        {
            if ( renderer == null )
            {
                return;
            }

            foreach( var camera in cameras )
            {
                renderer.Render( context, camera );
            }
        }
    }

}