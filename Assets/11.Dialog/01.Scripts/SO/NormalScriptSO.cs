using System;
using UnityEngine;

namespace DialogSystem
{
    public class NormalScriptSO : ScriptSO
    {
        public Character character;

        [HideInInspector]
        public ScriptSO nextScript;

        private void OnEnable()
        {
            if (character.animation == CharacterAnimationEnum.None)
            {
                character.characterAnimation = null;
            }
            else
            {
                string typeName = character.animation.ToString();
                try
                {
                    Type t = Type.GetType($"{typeName}Animation");
                    Debug.Log(t);
                    CharacterAnimation animation = Activator.CreateInstance(t) as CharacterAnimation;
                    Debug.Log(animation);
                    character.characterAnimation = animation;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{typeName} is loading error!");
                    Debug.LogError(ex);
                }

            }
        }
    }

    [Serializable]
    public struct Character
    {
        public string name;
        public GameObject imgPrefab;
        public CharacterAnimationEnum animation;
        [HideInInspector] public CharacterAnimation characterAnimation;
        [TextArea(3, 20)]
        public string talkDetails;
    }
}
