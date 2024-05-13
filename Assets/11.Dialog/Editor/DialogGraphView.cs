#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine;


using UnityEditor;
using UnityEngine.UIElements;

namespace DialogSystem
{
    public class DialogGraphView : GraphView
    {
        public Action<ScriptView> OnNodeSeleted;
        public new class UxmlFactory : UxmlFactory<DialogGraphView, GraphView.UxmlTraits> { }

        private DialogSO dialog;

        public DialogGraphView()
        {
            Insert(0, new GridBackground());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/11.Dialog/Editor/DialogGenerator.uss");
            styleSheets.Add(styleSheet);

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        private ScriptView FindScriptView(ScriptSO script)
        {
            return GetNodeByGuid(script.guid) as ScriptView;
        }

        public void ParpurateView(DialogSO dialog)
        {
            this.dialog = dialog;

            CreateNodeAndEdge();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    ScriptView scriptView = elem as ScriptView;
                    if (scriptView != null)
                    {
                        dialog.DeleteScript(scriptView.script);
                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        ScriptView parentView = edge.output.node as ScriptView;
                        ScriptView childView = edge.input.node as ScriptView;
                        dialog.RemoveNextScript(parentView.script, childView.script, edge.output);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    ScriptView parentView = edge.output.node as ScriptView;
                    ScriptView childView = edge.input.node as ScriptView;
                    dialog.AddNextScript(parentView.script, childView.script, edge.output);
                });
            }

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            {
                var types = TypeCache.GetTypesDerivedFrom<ScriptSO>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateScript(type));
                }
            }
        }

        public void Refresh()
        {
            CreateNodeAndEdge();
        }

        private void CreateScript(System.Type type)
        {
            ScriptSO script = dialog.CreateScript(type);

            if (dialog.scriptList[0].guid == script.guid)
                CreateScriptView(script, true);
            else
                CreateScriptView(script, false);

        }

        private void CreateScriptView(ScriptSO script, bool isFirstNode = false)
        {
            NormalScriptSO normal = script as NormalScriptSO;
            OptionSO option = script as OptionSO;
            NoScriptSO image = script as NoScriptSO;

            ScriptView scriptView = new ScriptView(script, isFirstNode);

            scriptView.OnNodeSeleted = OnNodeSeleted;
            AddElement(scriptView);
        }

        private void CreateNodeAndEdge()
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            //Create Node View
            dialog.scriptList.ForEach(n =>
            {
                if (dialog.scriptList[0].guid == n.guid)
                    CreateScriptView(n, true);
                else
                    CreateScriptView(n, false);
            });

            //Create Edge
            dialog.scriptList.ForEach(n =>
            {
                var children = dialog.GetConnectedScripts(n);

                children.ForEach(c =>
                {
                    if (c != null)
                    {
                        ScriptView parentView = FindScriptView(n);
                        ScriptView childView = FindScriptView(c);

                        if (parentView.output != null)
                        {
                            Edge edge = parentView.output.ConnectTo(childView.input);
                            AddElement(edge);
                        }

                        OptionSO option = parentView.script as OptionSO;        //옵션인 경우

                        if (option)
                        {
                            for (int i = 0; i < option.options.Count; i++)
                            {
                                if (option.options[i].nextScript != null &&
                                option.options[i].nextScript.guid == childView.script.guid)
                                {
                                    Edge edge = parentView.outputs[i].ConnectTo(childView.input);
                                    AddElement(edge);
                                }

                            }
                        }
                    }
                });
            });
        }
    }

}

#endif