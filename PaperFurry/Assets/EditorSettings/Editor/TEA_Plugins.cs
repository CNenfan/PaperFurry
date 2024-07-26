using UnityEditor;
using UnityEngine;

//Team EtherArc~
//Explore the world~
namespace TEA_Plugins
{
    public static class TEARecompileScripts
    {
        [MenuItem("TEA Plugins/重新编译C#脚本")]
        public static void RecompileCSharpScripts()
        {
            // 强制Unity重新编译所有脚本
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            
            // 显示一个消息框告知用户脚本已重新编译
            EditorUtility.DisplayDialog("TEA脚本重新编译", "已提交C#脚本重新编译任务.", "确定");
        }
    }
}