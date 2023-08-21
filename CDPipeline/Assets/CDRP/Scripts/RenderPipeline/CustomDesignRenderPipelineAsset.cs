using UnityEngine;
using UnityEngine.Rendering;

namespace CDPipeline
{
    [CreateAssetMenu]
    public class CustomDesignRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new CustomDesignRenderPipeline();
        }
    }
}
