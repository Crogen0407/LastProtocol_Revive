using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    public class OptionSO : ScriptSO
    {
        public GameObject optionPf;
        public List<OptionDetails> options = new List<OptionDetails>();
    }

    [Serializable]
    public class OptionDetails 
    {
        [TextArea(3,20)]
        public string detail;
        [HideInInspector]
        public ScriptSO nextScript;
    }
}
