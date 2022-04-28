using UnityEngine;
using UnityEngine.Rendering;

namespace CDPipeline
{
    public interface ICameraRenderer
    {
        void Render( ScriptableRenderContext context, Camera camera );
    }
}
