using UnityEngine;

public class AdventureUI : UIBase
{
    private enum Buttons
    {
        PVPButton,
        SpecialButton,
        MainStoryButton,
        BackButton
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PVPButton).onClick.AddListener(OnClickPVPButton);
        GetButton((int)Buttons.SpecialButton).onClick.AddListener(OnClickSpecialButtonButton);
        GetButton((int)Buttons.MainStoryButton).onClick.AddListener(OnClickMainStoryButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
    }

    private void OnClickPVPButton()
    {
        var ui = Managers.UI.ShowUI<WarningUI>();
        ui.Init("준비 중");
    }
    private void OnClickSpecialButtonButton()
    {
        var ui = Managers.UI.ShowUI<WarningUI>();
        ui.Init("준비 중");
    }
    private void OnClickMainStoryButton()
    {
        Debug.Log("OnClickMainStoryButton");

        Managers.UI.ShowUI<MainStoryUI>();
    }
    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");

        Managers.UI.CloseUI(this);
    }
}
