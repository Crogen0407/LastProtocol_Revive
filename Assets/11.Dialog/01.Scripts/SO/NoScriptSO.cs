using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DialogSystem
{
    public class NoScriptSO : ScriptSO
    {
        public NoScriptCharacter character;

        [HideInInspector] public ScriptSO nextScript;

        private void OnEnable()
        {
            if (character.animation == CharacterAnimationEnum.None)
            {
                character.characterAnimation = null;
            }
            else
            {
                try
                {
                    Type t = Type.GetType($"{character.animation.ToString()}Animation");
                    CharacterAnimation animation = Activator.CreateInstance(t) as CharacterAnimation;

                    character.characterAnimation = animation;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{character.animation.ToString()} is loading error!");
                    Debug.LogError(ex);
                }

            }
        }
    }

    [System.Serializable]
    public struct NoScriptCharacter
    {
        public GameObject imgPrefab;
        public CharacterAnimationEnum animation;
        [HideInInspector] public CharacterAnimation characterAnimation;
    }
}
