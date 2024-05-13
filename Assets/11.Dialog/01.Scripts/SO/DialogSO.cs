using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DialogSystem;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor;
#endif

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "SO/Dialog/Dialog")]
    public class DialogSO : ScriptableObject
    {
        public List<ScriptSO> scriptList = new List<ScriptSO>();
        public GameObject dialogBackground;

#if UNITY_EDITOR
        public ScriptSO CreateScript(System.Type type)
        {
            ScriptSO script = ScriptableObject.CreateInstance(type) as ScriptSO;
            script.name = type.Name;
            script.guid = GUID.Generate().ToString();
            scriptList.Add(script);

            AssetDatabase.AddObjectToAsset(script, this);
            AssetDatabase.SaveAssets();
            return script;
        }

        public void DeleteScript(ScriptSO script)
        {
            scriptList.Remove(script);
            AssetDatabase.RemoveObjectFromAsset(script);
            AssetDatabase.SaveAssets();
        }

        public void AddNextScript(ScriptSO parent, ScriptSO child, Port outputPort)
        {
            NormalScriptSO normal = parent as NormalScriptSO;
            if (normal)
                normal.nextScript = child;

            OptionSO option = parent as OptionSO;

            if (option)
            {
                for (int i = 0; i < option.options.Count; i++)
                {
                    if (outputPort.portName == $"Option-{(i + 1)}")
                    {
                        option.options[i].nextScript = child;
                        break;
                    }
                }
            }
            //BranchSO branch = parent as BranchSO;
            //if (branch)
            //{
            //    switch (outputPort.portName)
            //    {
            //        case "True":
            //            branch.nextScriptOnTrue = child;
            //            break;
            //        case "False":
            //            branch.nextScriptOnFalse = child;
            //            break;
            //    }
            //}
            NoScriptSO image = parent as NoScriptSO;
            if (image)
            {
                image.nextScript = child;
            }
        }

        public void RemoveNextScript(ScriptSO parent, ScriptSO child, Port outputPort)
        {
            NormalScriptSO normal = parent as NormalScriptSO;
            if (normal)
            {
                normal.nextScript = null;
            }
            OptionSO option = parent as OptionSO;
            if (option)
            {
                for (int i = 0; i < option.options.Count; i++)
                {
                    if (outputPort.portName == $"Option-{i}")
                    {
                        option.options[i] = null;
                        break;
                    }
                }
            }
            //BranchSO branch = parent as BranchSO;
            //if (branch)
            //{
            //    switch (outputPort.portName)
            //    {
            //        case "True":
            //            branch.nextScriptOnTrue = null;
            //            break;
            //        case "False":
            //            branch.nextScriptOnFalse = null;
            //            break;
            //    }
            //}
            NoScriptSO image = parent as NoScriptSO;
            if (image)
            {
                image.nextScript = null;
            }
        }

        public List<ScriptSO> GetConnectedScripts(ScriptSO parent)
        {
            List<ScriptSO> children = new List<ScriptSO>();

            NormalScriptSO normal = parent as NormalScriptSO;

            if (normal && normal.nextScript != null)
                children.Add(normal.nextScript);

            OptionSO option = parent as OptionSO;

            if (option)
            {
                for (int i = 0; i < option.options.Count; i++)
                {
                    if (option.options[i] != null)
                        children.Add(option.options[i].nextScript);
                }
            }

            //BranchSO branch = parent as BranchSO;
            //if (branch)
            //{
            //    if (branch.nextScriptOnTrue != null)
            //    {
            //        children.Add(branch.nextScriptOnTrue);
            //    }
            //    if (branch.nextScriptOnFalse != null)
            //    {
            //        children.Add(branch.nextScriptOnFalse);
            //    }
            //}
            NoScriptSO image = parent as NoScriptSO;
            if (image && image.nextScript != null)
            {
                children.Add(image.nextScript);
            }

            return children;
        }
#endif
    }
}
