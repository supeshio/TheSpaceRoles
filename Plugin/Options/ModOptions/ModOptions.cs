using TMPro;

namespace TheSpaceRoles
{
    public class ModOption : OptionBehaviour
    {
        public bool isHeader;
        public CustomOption CustomOption;
        public StringOption StringOption;
        public TextMeshPro TitleText;
        public TextMeshPro ValueText;
        public CategoryHeaderMasked categoryHeaderMasked;
        public void Increase()
        {
            CustomOption.UpdateSelection((CustomOption.Selection() + 1 + CustomOption.selections.Length) % CustomOption.selections.Length);

        }
        public void Decrease()
        {
            CustomOption.UpdateSelection((CustomOption.Selection() - 1 + CustomOption.selections.Length) % CustomOption.selections.Length);
        }
    }
}
