using UnityEngine;
namespace TMPro
{
    [CreateAssetMenu(fileName = "InputValidator - CustomPlayerNameValidator.asset", menuName = "TextMeshPro/Input Validators/TMP_CustomPlayerNameValidator", order = 100)]
    public class TMP_CustomPlayerNameValidator : TMP_InputValidator
    {
        // Custom text input validation function
        public override char Validate(ref string text, ref int pos, char ch)
        {
            text += char.ToUpper(ch);
            return char.ToUpper(ch); 
        }
    }
}
 