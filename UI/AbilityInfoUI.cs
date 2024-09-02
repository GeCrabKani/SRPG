using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class AbilityInfoUI : UIBase
{
    private Character character;

    private enum Texts
    {
        AbilityDescriptionText,
        AbilityNameText
    }

    private enum Images
    {
        AbilityIconImage
    }

    private enum Buttons
    {
        AbilityApplyButton,
        AbilityCancelButton,
        AbilityCheckButton,
        CloseButton,
        BackImage
    }

    public void Init(Character character, int AbilityTier, int AbilityIndex)
    {
        this.character = character;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        // 이미지, 설명 세팅
        if (AbilityTier == 1)
        {
            Utility.Id2SO<AbilitySO>(character.abilityT1.id, (result) =>
            {
                GetText((int)Texts.AbilityNameText).text = (result as AbilitySO).abilityName;
                GetText((int)Texts.AbilityDescriptionText).text = (result as AbilitySO).description;
                GetImage((int)Images.AbilityIconImage).sprite = (result as AbilitySO).icon;
                // 선택한 특성이면 아웃라인 켜기
                SetupSelectedAbilityUI();
            });
        }
        else if (AbilityTier == 2)
        {
            Utility.Id2SO<AbilitySO>(character.SO.abilityT2[AbilityIndex], (result) =>
            {
                GetText((int)Texts.AbilityNameText).text = (result as AbilitySO).abilityName;
                GetText((int)Texts.AbilityDescriptionText).text = (result as AbilitySO).description;
                GetImage((int)Images.AbilityIconImage).sprite = (result as AbilitySO).icon;
                // 선택 가능한지 체크
                if (character.Growth.level < AbilityTier2UnlockLevel)
                {
                    SetupUnselectableAbilityUI(AbilityTier);
                }
                // 이미 적용 된 특성이라면
                else if (character.Growth.abilityT2 == AbilityIndex)
                {
                    SetupSelectedAbilityUI();
                }
                // 적용 안된 특성이라면
                else
                {
                    SetupUnselectedAbilityUI(AbilityTier, AbilityIndex);
                }
            });
        }
        else
        {
            Utility.Id2SO<AbilitySO>(character.SO.abilityT3[AbilityIndex], (result) =>
            {
                GetText((int)Texts.AbilityNameText).text = (result as AbilitySO).abilityName;
                GetText((int)Texts.AbilityDescriptionText).text = (result as AbilitySO).description;
                GetImage((int)Images.AbilityIconImage).sprite = (result as AbilitySO).icon;
                // 선택 가능한지 체크
                if (character.Growth.level < AbilityTier3UnlockLevel || character.Growth.abilityT2 == NONE_SELECTED)
                {
                    SetupUnselectableAbilityUI(AbilityTier);
                }
                // 이미 적용 된 특성이라면
                else if (character.Growth.abilityT3 == AbilityIndex)
                {
                    SetupSelectedAbilityUI();
                }
                // 적용 안된 특성이라면
                else
                {
                    SetupUnselectedAbilityUI(AbilityTier, AbilityIndex);
                }
            });
        }



        GetButton((int)Buttons.AbilityApplyButton).onClick.AddListener(() => OnClickAbilityApplyButton(AbilityTier, AbilityIndex));
        GetButton((int)Buttons.AbilityCancelButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.AbilityCheckButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);


        // 버튼들 모두 비활성화 상태
        GetButton((int)Buttons.AbilityApplyButton).gameObject.SetActive(false);
        GetButton((int)Buttons.AbilityCancelButton).gameObject.SetActive(false);
        GetButton((int)Buttons.AbilityCheckButton).gameObject.SetActive(false);
    }

    // 이미 적용 된 특성 UI 세팅
    private void SetupSelectedAbilityUI()
    {
        GetImage((int)Images.AbilityIconImage).transform.parent.GetComponent<Outline>().enabled = true;

        GetButton((int)Buttons.AbilityCheckButton).gameObject.SetActive(true);
    }

    // 적용 안된 특성 UI 세팅
    private void SetupUnselectedAbilityUI(int AbilityTier, int AbilityIndex)
    {
        GetImage((int)Images.AbilityIconImage).transform.parent.GetComponent<Outline>().enabled = false;

        GetButton((int)Buttons.AbilityApplyButton).gameObject.SetActive(true);
        GetButton((int)Buttons.AbilityCancelButton).gameObject.SetActive(true);

        GetButton((int)Buttons.AbilityApplyButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.AbilityApplyButton).onClick.AddListener(() => OnClickAbilityApplyButton(AbilityTier, AbilityIndex));
    }

    // 특성 선택이 불가능할 때 UI 세팅
    private void SetupUnselectableAbilityUI(int AbilityTier)
    {
        GetImage((int)Images.AbilityIconImage).transform.parent.GetComponent<Outline>().enabled = false;
        GetButton((int)Buttons.AbilityCheckButton).gameObject.SetActive(true);

        if (AbilityTier == 2)
        {
            GetText((int)Texts.AbilityDescriptionText).text += $"\n\n<color=red>* 레벨 {AbilityTier2UnlockLevel}이상이 필요합니다.</color>";
        }
        else if (AbilityTier == 3)
        {
            GetText((int)Texts.AbilityDescriptionText).text += $"\n\n<color=red>* 레벨 {AbilityTier3UnlockLevel}이상, 2단계 특성 활성화가 필요합니다.</color>";
        }
    }
    private void OnClickAbilityApplyButton(int AbilityTier, int selectAbilityIndex)
    {
        Debug.Log("OnClickAbilityApplyButton");

        character.Growth.SelectAbility(AbilityTier, selectAbilityIndex);

        // TODO
        // 아래 함수를 public 선언 후 그냥 호출하고 있는데 더 좋은 방법이 있을지??
        Managers.UI.FindUI<CharacterInfoUI>().AbilityPathUpdate();
        Managers.UI.FindUI<CharacterInfoUI>().UpdateStat();

        Managers.UI.CloseUI(this);
    }

    private void CloseUI()
    {
        Managers.UI.CloseUI(this);
    }
}
