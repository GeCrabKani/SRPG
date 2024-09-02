using UnityEngine;

public class MailUI : UIBase
{
    private enum Texts
    {
        WaitingReceiveText
    }
    private enum Buttons
    {
        AllReceiveButton,
        BackButton
    }

    private enum GameObjects
    {
        Content
    }

    private void Start()
    {
        /*
        // 테스트용 더미메일
        MailSO so = new()
        {
            title = "운영자의 선물",
            dateSent = DateTime.Now,
            diamond = 30,
            gold = 1000,
            ap = 10,
            expiration = 7
        };
        Managers.DB.PushChild(Managers.DB.userDB.Child("mailBox"), so);
        */
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.AllReceiveButton).onClick.AddListener(OnClickAllReceiveButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetText((int)Texts.WaitingReceiveText).text = $"수령대기 {Managers.AccountData.mailBox.Count}건";

        foreach (var mail in Managers.AccountData.mailBox)
        {
            GameObject go = Managers.Resource.Instantiate("UI/MailEntryUI", GetObject((int)GameObjects.Content).transform);
            go.GetComponent<MailEntryUI>().Init(mail);
        }
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }

    // 모두 수령
    private void OnClickAllReceiveButton()
    {
        Debug.Log("OnClickAllReceiveButton");
        // Content 오브젝트 아래에 있는 메일을 순회하며 수령
        foreach (Transform child in GetObject((int)GameObjects.Content).transform)
        {
            MailEntryUI mailEntryUI = child.GetComponent<MailEntryUI>();
            mailEntryUI.OnClickReceiveButton();

            // TODO
            // 모두 수령은 보상을 한번에 보여주어야 함
        }
    }

    // 수령 후 남은 메일 건수 텍스트 업데이트
    public void UpdateReceiveText()
    {
        GetText((int)Texts.WaitingReceiveText).text = $"수령대기 {Managers.AccountData.mailBox.Count}건";
    }
}
