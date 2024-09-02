using System.Collections.Generic;
using static Constants;

public class CharacterExchangeEntryUI : UIBase
{
    private enum Texts
    {
        CharacterNameText
    }

    private enum Images
    {
        CharacterImage
    }

    private enum Buttons
    {
        ExchangeButton
    }

    public void Init(GachaSO gachaSO)
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetImage((int)Images.CharacterImage).sprite = Utility.Id2SOWait<CharacterSO>(gachaSO.id).icon;
        GetText((int)Texts.CharacterNameText).text = gachaSO.pickUpcharacterName;
        GetButton((int)Buttons.ExchangeButton).onClick.AddListener(() => OnClickExchangeButton(gachaSO));
    }

    private void OnClickExchangeButton(GachaSO gachaSO)
    {
        if (Managers.AccountData.playerData.gachaPoint < GachaPoint)
        {
            Managers.UI.ShowUI<WarningUI>().Init("계약 포인트가 부족합니다.");

            return;
        }

        // 가챠 포인트 감소
        Managers.AccountData.playerData.ReduceGachaPoint(GachaPoint);

        // 캐릭터 획득 UI
        List<int> gachaResultList = new List<int> { gachaSO.id };
        Managers.UI.ShowUI<GachaResultUI>().Init(gachaResultList);
    }
}
