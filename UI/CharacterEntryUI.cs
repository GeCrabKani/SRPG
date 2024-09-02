using System;
using System.Linq;
using UnityEngine;
using static Constants;

public class CharacterEntryUI : UIBase
{
    // 편성에서 클릭한 것인지 체크 변수
    public static bool isFormation;
    // 편성의 몇번째 인덱스 클린한 것인지 체크 변수
    public static int formationIndex;
    // 캐릭터
    public Character character;
    // 1초 이상 눌렀는지를 체크하는 변수
    private float pressedTimer;
    // 1초 이상 눌렀을 때 캐릭터 정보창이 켜져있는지 체크하는 변수
    private bool hasShownCharacterInfo;
    private bool isCharacterInFormation;

    private enum Texts
    {
        CharacterLevelText
    }

    private enum Buttons
    {
        CharacterButton
    }
    
    private enum Images
    {
        CharacterImage,
        CharacterOutline,
        CharacterAttributeImage,
        InFormationImage
    }
    
    private enum GameObjects
    {
        Star
    }

    private void OnDestroy()
    {
        character.Growth.OnLevelUp -= UpdateLevel;
        character.Growth.OnLevelUp -= SetStar;
    }

    public void Init(Character character)
    {
        this.character = character;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CharacterButton).onClick.AddListener(OnClickButton);
        BindEvent(GetButton((int)Buttons.CharacterButton).gameObject, OnPointerUpCharacterButton, UIEvent.PointerUp);
        BindEvent(GetButton((int)Buttons.CharacterButton).gameObject, OnPressedCharacterButton, UIEvent.Pressed);

        GetImage((int)Images.CharacterImage).sprite = character.SO.icon;
        GetImage((int)Images.CharacterAttributeImage).sprite = character.SO.GetElementSprite();

        UpdateLevel();

        if (isFormation)
        {
            FormationUI ui = Managers.UI.FindUI<FormationUI>();
            // ui.presetIndex를 사용하여 현재 선택된 프리셋(파티)를 참조
            FormationData currentFormation = Managers.AccountData.formationData[ui.presetIndex];
            // 주어진 characterId가 현재 파티에 포함되어 있는지 확인
            isCharacterInFormation = currentFormation.characterId.Contains(character.SO.id);

            // 편성에 포함되어 있지 않다면 편성됨 이미지 끄기
            if (isCharacterInFormation == false)
            {
                GetImage((int)Images.InFormationImage).gameObject.SetActive(false);
            }
        }
        // 메인화면에서 캐릭터 버튼을 누르고 왔다면 편성됨 이미지 끄기
        else
        {
            GetImage((int)Images.InFormationImage).gameObject.SetActive(false);
        }

        SetOutline();
        SetStar();

        character.Growth.OnLevelUp += UpdateLevel;
        character.Growth.OnLevelUp += SetStar;
    }

    private void UpdateLevel()
    {
        GetText((int)Texts.CharacterLevelText).text = $"{character.Growth.level}";
    }

    private void SetOutline()
    {
        switch (character.SO.elementType)
        {
            case ElementType.Fire:
                GetImage((int)Images.CharacterOutline).color = Color.red;
                break;
            case ElementType.Water:
                GetImage((int)Images.CharacterOutline).color = Color.blue;
                break;
            case ElementType.Grass:
                GetImage((int)Images.CharacterOutline).color = Color.green;
                break;
            case ElementType.Bolt:
                GetImage((int)Images.CharacterOutline).color = Color.yellow;
                break;
            case ElementType.Dark:
                GetImage((int)Images.CharacterOutline).color = Color.black;
                break;
            case ElementType.Light:
                GetImage((int)Images.CharacterOutline).color = Color.white;
                break;
        }
    }

    private void SetStar()
    {
        int numberOfStars = character.Growth.star;
        float starWidth = 25f; // 별 이미지의 너비

        // 기존에 생성된 별들 제거
        foreach (Transform child in GetObject((int)GameObjects.Star).transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfStars; i++)
        {
            GameObject star = Managers.Resource.Instantiate("Star", GetObject((int)GameObjects.Star).transform);
            star.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            RectTransform rt = star.GetComponent<RectTransform>();
            rt.anchorMax = new Vector2(0f, 0f);
            rt.anchorMin = new Vector2(0f, 0f);
            rt.pivot = new Vector2(0f, 0f);
            rt.anchoredPosition = new Vector2(i * starWidth, 0);
        }
    }

    private void OnClickButton()
    {
        Debug.Log("OnClickButton");
        //Managers.Sound.Play(Constants.Sound.Effect, "ButtonClick");
        if (isFormation)
        {
            AddCharacterToFormation();
        }
        else
        {
            ShowCharacterInfo();
        }
    }

    // 캐릭터 정보창 보여주기
    private void ShowCharacterInfo()
    {
        Debug.Log("ShowCharacterInfo");

        Managers.UI.ShowUI<CharacterInfoUI>().Init(character);
    }

    // 편성에 추가하기
    private void AddCharacterToFormation()
    {
        Debug.Log("AddCharacterToFormation");

        FormationUI ui = Managers.UI.FindUI<FormationUI>();

        // 이미 편성에 포함되어 있다면 편성 해제
        if (isCharacterInFormation)
        {
            // 해당 캐릭터가 있는 인덱스
            int characterIndex = Array.IndexOf(Managers.AccountData.formationData[ui.presetIndex].characterId, character.SO.id);
            // 편성 이미지 비활성화
            GetImage((int)Images.InFormationImage).gameObject.SetActive(false);
            // 편성 데이터 업데이트
            Managers.AccountData.SetFormationCharacter(ui.presetIndex, characterIndex, 0);
            // UI 업데이트
            ui.UpdateFormationMember(characterIndex);

            isCharacterInFormation = false;

            return;
        }

        // 편성 UI 뽑아서 formationIndex에 해당하는 곳에 캐릭터 정보 전달
        Debug.Log(ui.presetIndex);
        Debug.Log(formationIndex);
        Managers.AccountData.SetFormationCharacter(ui.presetIndex, formationIndex, character.SO.id);
        ui.UpdateFormationMember(formationIndex);

        Managers.UI.CloseUI(Managers.UI.PeekUI<CharacterUI>());
    }

    private void OnPointerUpCharacterButton()
    {
        Debug.Log("OnPointerUpCharacterButton");

        pressedTimer = 0f; // 버튼에서 손을 뗐을 때 pressedTimer 리셋
        hasShownCharacterInfo = false;
    }

    private void OnPressedCharacterButton()
    {
        Debug.Log("OnPressedCharacterButton");

        pressedTimer += Time.deltaTime;

        if (pressedTimer > 1f && !hasShownCharacterInfo)
        {
            ShowCharacterInfo();
            hasShownCharacterInfo = true;
        }
    }
}
