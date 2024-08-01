using System.Linq;
using TMPro;

namespace TheSpaceRoles
{
    public class ModOption : OptionBehaviour
    {
        public CustomOption CustomOption;
        public StringOption StringOption;
        public TextMeshPro TitleText;
        public TextMeshPro ValueText;
        public void Increase()
        {
            CustomOption.UpdateSelection((CustomOption.selection + 1 + CustomOption.selections.Length) % CustomOption.selections.Length);

        }
        public void Decrease()
        {
            CustomOption.UpdateSelection((CustomOption.selection - 1 + CustomOption.selections.Length) % CustomOption.selections.Length);
        }
    }
}
