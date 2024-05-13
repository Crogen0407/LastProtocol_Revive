#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogSystem
{
    public class ScriptView : Node
    {
        public Action<ScriptView> OnNodeSeleted;
        public ScriptSO script;
        public Port input;
        public Port output;
        public TextField nameInput;
        public List<Port> outputs = new List<Port>();
        public List<Button> buttons = new List<Button>();

        public ScriptView(ScriptSO script, bool isFirstNode)
        {
            this.script = script;
            title = script.name;
            viewDataKey = script.guid;

            style.left = script.position.x;
            style.top = script.position.y;

            if (isFirstNode == false)
                CreateInputPorts();
            CreateOutputPorts();

            this.Add(nameInput);
        }

        private void CreateInputPorts()
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if (input != null)
            {
                input.portName = "";
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            if (script is NormalScriptSO)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                output.portName = "NextScripts";
                outputContainer.Add(output);
            }
            else if (script is OptionSO)
            {
                for (int i = 0; i < (script as OptionSO).options.Count; i++)
                {
                    outputs.Add(InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)));
                    outputs[i].portName = $"Option-{i + 1}";
                    outputContainer.Add(outputs[i]);
                }
            }
            else if (script is NoScriptSO)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                output.portName = "NextScript";
                outputContainer.Add(output);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            script.position.x = newPos.xMin;
            script.position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSeleted?.Invoke(this);
        }
    }
}

#endif