using UnityEngine;
using UnityEngine.Rendering;

namespace CDPipeline
{
    public class CustomDesignRenderPipeline : RenderPipeline
    {
        protected ICameraRenderer renderer;

        public CustomDesignRenderPipeline(){ }

        public CustomDesignRenderPipeline(ICameraRenderer renderer)
        {
            this.renderer = renderer;
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