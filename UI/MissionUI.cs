using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class MissionUI : UIBase
{
    private PlayTab playTab = PlayTab.None;

    private enum PlayTab
    {
        None,
        Whole,
        Daily,
        Weekly,
        Achievement,
        Beginner
    }
    private enum Buttons
    {
        WholeButton,
        DailyButton,
        WeeklyButton,
        AchievementButton,
        BeginnerButton,
        GetAllButton,
        BackButton,
    }

    private enum GameObjects
    {
        WholeTab,
        DailyTab,
        WeeklyTab,
        AchievementTab,
        BeginnerTab,
        WholeContent,
        DailyContent,
        WeeklyContent,
        AchievementContent,
        BeginnerContent
    }

    private void Start()
    {
        Init();

        Managers.Mission.OnMissionStartCallback += OnMissionUpdateUI;
        Managers.Mission.OnMissionCompleteCallback += OnMissionUpdateUI;
        Managers.Mission.OnMissionReceiveCallback += OnMissionUpdateUI;
    }

    private void OnDestroy()
    {
        Managers.Mission.OnMissionStartCallback -= OnMissionUpdateUI;
        Managers.Mission.OnMissionCompleteCallback -= OnMissionUpdateUI;
        Managers.Mission.OnMissionReceiveCallback -= OnMissionUpdateUI;
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        // 스킬, 특성, 클래스 탭 보여주기
        GetButton((int)Buttons.WholeButton).onClick.AddListener(() => ShowTab(PlayTab.Whole));
        GetButton((int)Buttons.DailyButton).onClick.AddListener(() => ShowTab(PlayTab.Daily));
        GetButton((int)Buttons.WeeklyButton).onClick.AddListener(() => ShowTab(PlayTab.Weekly));
        GetButton((int)Buttons.AchievementButton).onClick.AddListener(() => ShowTab(PlayTab.Achievement));
        GetButton((int)Buttons.BeginnerButton).onClick.AddListener(() => ShowTab(PlayTab.Beginner));

        GetButton((int)Buttons.GetAllButton).onClick.AddListener(OnClickGetAllButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        ShowTab(PlayTab.Whole);
    }

    private void OnMissionUpdateUI(int missionId)
    {
        switch (playTab)
        {
            case PlayTab.Whole:
                WholeTabInit();
                break;
            case PlayTab.Daily:
                DailyTabInit();
                break;
            case PlayTab.Weekly:
                WeeklyTabInit();
                break;
            case PlayTab.Achievement:
                AchievementTabInit();
                break;
            case PlayTab.Beginner:
                BeginnerTabInit();
                break;
        }
    }
    private void WholeTabInit()
    {
        foreach (Transform child in GetObject((int)GameObjects.WholeContent).transform)
        {
            Destroy(child.gameObject);
        }

        // 완료 미션
        foreach (var completeMissionId in Managers.AccountData.completeMissions)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.WholeContent).transform);

            go.GetComponent<MissionEntryUI>().Init(completeMissionId);
        }
        // 진행 중인 미션 생성
        foreach (var ongoingMissionId in Managers.AccountData.ongoingMissions.Keys)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.WholeContent).transform);

            go.GetComponent<MissionEntryUI>().Init(ongoingMissionId);
        }
        // 수령 미션
        foreach (var receiveMissionId in Managers.AccountData.receiveMissions)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.WholeContent).transform);

            go.GetComponent<MissionEntryUI>().Init(receiveMissionId);
        }
    }
    private void DailyTabInit()
    {
        foreach (Transform child in GetObject((int)GameObjects.DailyContent).transform)
        {
            Destroy(child.gameObject);
        }
        // 완료 미션
        foreach (var completeMissionId in Managers.AccountData.completeMissions)
        {
            // 완료 미션 중 일일 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(completeMissionId).missionCategory != MissionCategory.Daily )
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.DailyContent).transform);

            go.GetComponent<MissionEntryUI>().Init(completeMissionId);
        }
        // 진행 중인 미션 생성
        foreach (var ongoingMissionId in Managers.AccountData.ongoingMissions.Keys)
        {
            // 진행 미션 중 일일 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(ongoingMissionId).missionCategory != MissionCategory.Daily)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.DailyContent).transform);

            go.GetComponent<MissionEntryUI>().Init(ongoingMissionId);
        }
        // 수령 미션
        foreach (var receiveMissionId in Managers.AccountData.receiveMissions)
        {
            // 수령 미션 중 일일 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(receiveMissionId).missionCategory != MissionCategory.Daily)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.DailyContent).transform);

            go.GetComponent<MissionEntryUI>().Init(receiveMissionId);
        }
    }
    private void WeeklyTabInit()
    {
        foreach (Transform child in GetObject((int)GameObjects.WeeklyContent).transform)
        {
            Destroy(child.gameObject);
        }
        // 완료 미션
        foreach (var completeMissionId in Managers.AccountData.completeMissions)
        {
            // 완료 미션 중 주간 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(completeMissionId).missionCategory != MissionCategory.Weekly)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.WeeklyContent).transform);

            go.GetComponent<MissionEntryUI>().Init(completeMissionId);
        }
        // 진행 중인 미션 생성
        foreach (var ongoingMissionId in Managers.AccountData.ongoingMissions.Keys)
        {
            // 진행 미션 중 주간 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(ongoingMissionId).missionCategory != MissionCategory.Weekly)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.WeeklyContent).transform);

            go.GetComponent<MissionEntryUI>().Init(ongoingMissionId);
        }
        // 수령 미션
        foreach (var receiveMissionId in Managers.AccountData.receiveMissions)
        {
            // 수령 미션 중 주간 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(receiveMissionId).missionCategory != MissionCategory.Weekly)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.WeeklyContent).transform);

            go.GetComponent<MissionEntryUI>().Init(receiveMissionId);
        }
    }
    private void AchievementTabInit()
    {
        foreach (Transform child in GetObject((int)GameObjects.AchievementContent).transform)
        {
            Destroy(child.gameObject);
        }
        // 완료 미션
        foreach (var completeMissionId in Managers.AccountData.completeMissions)
        {
            // 완료 미션 중 업적이 아니라면 통과
            if (Managers.Mission.missionDB.Get(completeMissionId).missionCategory != MissionCategory.Achievement)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.AchievementContent).transform);

            go.GetComponent<MissionEntryUI>().Init(completeMissionId);
        }
        // 진행 중인 미션 생성
        foreach (var ongoingMissionId in Managers.AccountData.ongoingMissions.Keys)
        {
            // 진행 미션 중 업적이 아니라면 통과
            if (Managers.Mission.missionDB.Get(ongoingMissionId).missionCategory != MissionCategory.Achievement)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.AchievementContent).transform);

            go.GetComponent<MissionEntryUI>().Init(ongoingMissionId);
        }
        // 수령 미션
        foreach (var receiveMissionId in Managers.AccountData.receiveMissions)
        {
            // 수령 미션 중 업적이 아니라면 통과
            if (Managers.Mission.missionDB.Get(receiveMissionId).missionCategory != MissionCategory.Achievement)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.AchievementContent).transform);

            go.GetComponent<MissionEntryUI>().Init(receiveMissionId);
        }
    }
    private void BeginnerTabInit()
    {
        foreach (Transform child in GetObject((int)GameObjects.BeginnerContent).transform)
        {
            Destroy(child.gameObject);
        }
        // 완료 미션
        foreach (var completeMissionId in Managers.AccountData.completeMissions)
        {
            // 완료 미션 중 초보자 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(completeMissionId).missionCategory != MissionCategory.Beginner)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.BeginnerContent).transform);

            go.GetComponent<MissionEntryUI>().Init(completeMissionId);
        }
        // 진행 중인 미션 생성
        foreach (var ongoingMissionId in Managers.AccountData.ongoingMissions.Keys)
        {
            // 진행 미션 중 초보자 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(ongoingMissionId).missionCategory != MissionCategory.Beginner)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.BeginnerContent).transform);

            go.GetComponent<MissionEntryUI>().Init(ongoingMissionId);
        }
        // 수령 미션
        foreach (var receiveMissionId in Managers.AccountData.receiveMissions)
        {
            // 수령 미션 중 초보자 미션이 아니라면 통과
            if (Managers.Mission.missionDB.Get(receiveMissionId).missionCategory != MissionCategory.Beginner)
            {
                continue;
            }

            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/MissionEntryUI"),
                GetObject((int)GameObjects.BeginnerContent).transform);

            go.GetComponent<MissionEntryUI>().Init(receiveMissionId);
        }
    }

    private void ShowTab(PlayTab tab)
    {
        // 이미 탭에 열려있는 정보를 누르면 아무것도 하지않음
        if (playTab == tab)
        {
            return;
        }
            
        // 현재 열려있는 탭 업데이트
        playTab = tab;

        // 세팅 초기화
        // 모든 탭 끄기
        GetObject((int)GameObjects.WholeTab).SetActive(false);
        GetObject((int)GameObjects.DailyTab).SetActive(false);
        GetObject((int)GameObjects.WeeklyTab).SetActive(false);
        GetObject((int)GameObjects.AchievementTab).SetActive(false);
        GetObject((int)GameObjects.BeginnerTab).SetActive(false);

        // 테스트 이미지
        // 모든 탭의 버튼을 흰색으로 초기화
        // switch에서 클릭한 탭을 초록색으로 설정
        GetButton((int)Buttons.WholeButton).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.DailyButton).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.WeeklyButton).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.AchievementButton).GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.BeginnerButton).GetComponent<Image>().color = Color.white;

        // TODO
        // 모든 버튼 이미지 초기화

        switch (playTab)
        {
            case PlayTab.Whole:
                // TODO
                // 버튼 눌리는 효과음 재생
                // 해당 탭 활성화
                GetObject((int)GameObjects.WholeTab).SetActive(true);
                // 해당 버튼 이미지 변경 (클릭한 버튼임을 보여주기)
                GetButton((int)Buttons.WholeButton).GetComponent<Image>().color = Color.green;
                WholeTabInit();
                break;

            case PlayTab.Daily:

                GetObject((int)GameObjects.DailyTab).SetActive(true);
                GetButton((int)Buttons.DailyButton).GetComponent<Image>().color = Color.green;
                DailyTabInit();
                break;

            case PlayTab.Weekly:

                GetObject((int)GameObjects.WeeklyTab).SetActive(true);
                GetButton((int)Buttons.WeeklyButton).GetComponent<Image>().color = Color.green;
                WeeklyTabInit();
                break;

            case PlayTab.Achievement:

                GetObject((int)GameObjects.AchievementTab).SetActive(true);
                GetButton((int)Buttons.AchievementButton).GetComponent<Image>().color = Color.green;
                AchievementTabInit();
                break;

            case PlayTab.Beginner:

                GetObject((int)GameObjects.BeginnerTab).SetActive(true);
                GetButton((int)Buttons.BeginnerButton).GetComponent<Image>().color = Color.green;
                BeginnerTabInit();
                break;
        }
    }

    private int exp = 0;
    private int ap = 0;
    private int gold = 0;
    private int diamond = 0;
    private Dictionary<int, int> rewards = new();

    private void OnClickGetAllButton()
    {
        Debug.Log("OnClickGetAllButton");

        exp = 0;
        ap = 0;
        gold = 0;
        diamond = 0;
        rewards.Clear();

        switch (playTab)
        {
            case PlayTab.Whole:

                foreach (Transform child in GetObject((int)GameObjects.WholeContent).transform)
                {
                    int missionId = child.GetComponent<MissionEntryUI>().missionId;
                    // 완료 한 미션이 아니라면 continue
                    if (Managers.AccountData.completeMissions.Contains(missionId) == false)
                    {
                        continue;
                    }

                    MissionSO missionData = Managers.Mission.missionDB.Get(missionId);

                    exp += missionData.exp;
                    ap += missionData.ap;
                    gold += missionData.gold;
                    diamond += missionData.diamond;

                    foreach (var reward in missionData.rewards)
                    {
                        if (rewards.ContainsKey(reward.Key))
                        {
                            rewards[reward.Key] += reward.Value;
                        }
                        else
                        {
                            rewards.Add(reward.Key, reward.Value);
                        }
                    }
                }
                break;

            case PlayTab.Daily:
                foreach (Transform child in GetObject((int)GameObjects.DailyContent).transform)
                {
                    int missionId = child.GetComponent<MissionEntryUI>().missionId;
                    // 완료 한 미션이 아니라면 continue
                    if (Managers.AccountData.completeMissions.Contains(missionId) == false)
                    {
                        continue;
                    }

                    MissionSO missionData = Managers.Mission.missionDB.Get(missionId);

                    exp += missionData.exp;
                    ap += missionData.ap;
                    gold += missionData.gold;
                    diamond += missionData.diamond;

                    foreach (var reward in missionData.rewards)
                    {
                        if (rewards.ContainsKey(reward.Key))
                        {
                            rewards[reward.Key] += reward.Value;
                        }
                        else
                        {
                            rewards.Add(reward.Key, reward.Value);
                        }
                    }
                }
                break;

            case PlayTab.Weekly:
                foreach (Transform child in GetObject((int)GameObjects.WeeklyContent).transform)
                {
                    int missionId = child.GetComponent<MissionEntryUI>().missionId;
                    // 완료 한 미션이 아니라면 continue
                    if (Managers.AccountData.completeMissions.Contains(missionId) == false)
                    {
                        continue;
                    }

                    MissionSO missionData = Managers.Mission.missionDB.Get(missionId);

                    exp += missionData.exp;
                    ap += missionData.ap;
                    gold += missionData.gold;
                    diamond += missionData.diamond;

                    foreach (var reward in missionData.rewards)
                    {
                        if (rewards.ContainsKey(reward.Key))
                        {
                            rewards[reward.Key] += reward.Value;
                        }
                        else
                        {
                            rewards.Add(reward.Key, reward.Value);
                        }
                    }
                }
                break;

            case PlayTab.Achievement:
                foreach (Transform child in GetObject((int)GameObjects.AchievementContent).transform)
                {
                    int missionId = child.GetComponent<MissionEntryUI>().missionId;
                    // 완료 한 미션이 아니라면 continue
                    if (Managers.AccountData.completeMissions.Contains(missionId) == false)
                    {
                        continue;
                    }

                    MissionSO missionData = Managers.Mission.missionDB.Get(missionId);

                    exp += missionData.exp;
                    ap += missionData.ap;
                    gold += missionData.gold;
                    diamond += missionData.diamond;

                    foreach (var reward in missionData.rewards)
                    {
                        if (rewards.ContainsKey(reward.Key))
                        {
                            rewards[reward.Key] += reward.Value;
                        }
                        else
                        {
                            rewards.Add(reward.Key, reward.Value);
                        }
                    }
                }
                break;

            case PlayTab.Beginner:
                foreach (Transform child in GetObject((int)GameObjects.BeginnerContent).transform)
                {
                    int missionId = child.GetComponent<MissionEntryUI>().missionId;
                    // 완료 한 미션이 아니라면 continue
                    if (Managers.AccountData.completeMissions.Contains(missionId) == false)
                    {
                        continue;
                    }

                    MissionSO missionData = Managers.Mission.missionDB.Get(missionId);

                    exp += missionData.exp;
                    ap += missionData.ap;
                    gold += missionData.gold;
                    diamond += missionData.diamond;

                    foreach (var reward in missionData.rewards)
                    {
                        if (rewards.ContainsKey(reward.Key))
                        {
                            rewards[reward.Key] += reward.Value;
                        }
                        else
                        {
                            rewards.Add(reward.Key, reward.Value);
                        }
                    }
                }
                break;
        }

        // 보상이 있는지 확인
        bool hasRewards = exp > 0 || ap > 0 || gold > 0 || diamond > 0 || rewards.Count > 0;

        // 보상이 하나도 업다면 return
        if (hasRewards == false)
        {
            return;
        }

        // 보상을 얻을 수 없다면 (수치 오류 or 한계 초과) return
        if (CanClaimReward() == false)
        {
            return;
        }

        ClaimReward();

        // 보상 획득 완료 UI 생성
        Managers.UI.ShowUI<RewardGetUI>().InitWithMultipleRewards(exp, ap, gold, diamond, rewards);

        // 해당 미션 엔트리 수령 처리
        AllMissionReceive();

        // 다음 해금 미션 있다면 생성
        NextMissionStart();
    }

    private void AllMissionReceive()
    {
        switch (playTab)
        {
            case PlayTab.Whole:

                foreach (Transform child in GetObject((int)GameObjects.WholeContent).transform)
                {
                    Managers.Mission.MissionReceive(child.GetComponent<MissionEntryUI>().missionId);
                }
                break;

            case PlayTab.Daily:
                foreach (Transform child in GetObject((int)GameObjects.DailyContent).transform)
                {
                    Managers.Mission.MissionReceive(child.GetComponent<MissionEntryUI>().missionId);
                }
                break;

            case PlayTab.Weekly:
                foreach (Transform child in GetObject((int)GameObjects.WeeklyContent).transform)
                {
                    Managers.Mission.MissionReceive(child.GetComponent<MissionEntryUI>().missionId);
                }
                break;

            case PlayTab.Achievement:
                foreach (Transform child in GetObject((int)GameObjects.AchievementContent).transform)
                {
                    Managers.Mission.MissionReceive(child.GetComponent<MissionEntryUI>().missionId);
                }
                break;

            case PlayTab.Beginner:
                foreach (Transform child in GetObject((int)GameObjects.BeginnerContent).transform)
                {
                    Managers.Mission.MissionReceive(child.GetComponent<MissionEntryUI>().missionId);
                }
                break;
        }
    }

    private void NextMissionStart()
    {
        switch (playTab)
        {
            case PlayTab.Whole:

                foreach (Transform child in GetObject((int)GameObjects.WholeContent).transform)
                {
                    foreach (var nextMission in Managers.Mission.missionDB.Get(child.GetComponent<MissionEntryUI>().missionId).nextMissions)
                    {
                        Managers.Mission.MissionStart(nextMission);
                    }
                }
                break;

            case PlayTab.Daily:
                foreach (Transform child in GetObject((int)GameObjects.DailyContent).transform)
                {
                    foreach (var nextMission in Managers.Mission.missionDB.Get(child.GetComponent<MissionEntryUI>().missionId).nextMissions)
                    {
                        Managers.Mission.MissionStart(nextMission);
                    }
                }
                break;

            case PlayTab.Weekly:
                foreach (Transform child in GetObject((int)GameObjects.WeeklyContent).transform)
                {
                    foreach (var nextMission in Managers.Mission.missionDB.Get(child.GetComponent<MissionEntryUI>().missionId).nextMissions)
                    {
                        Managers.Mission.MissionStart(nextMission);
                    }
                }
                break;

            case PlayTab.Achievement:
                foreach (Transform child in GetObject((int)GameObjects.AchievementContent).transform)
                {
                    foreach (var nextMission in Managers.Mission.missionDB.Get(child.GetComponent<MissionEntryUI>().missionId).nextMissions)
                    {
                        Managers.Mission.MissionStart(nextMission);
                    }
                }
                break;

            case PlayTab.Beginner:
                foreach (Transform child in GetObject((int)GameObjects.BeginnerContent).transform)
                {
                    foreach (var nextMission in Managers.Mission.missionDB.Get(child.GetComponent<MissionEntryUI>().missionId).nextMissions)
                    {
                        Managers.Mission.MissionStart(nextMission);
                    }
                }
                break;
        }
    }

    private bool CanClaimReward()
    {
        Debug.Log("CanClaimReward");

        // 골드 획득 가능 체크
        if (Managers.AccountData.playerData.CanAddGold(gold) == false)
        {
            Managers.UI.ShowUI<WarningUI>().Init(gold < 0 ? "골드 보상이 음수입니다." : "골드 보유 한도 초과");

            return false;
        }

        // 다이아 획득 가능 체크
        if (Managers.AccountData.playerData.CanAddDiamond(diamond) == false)
        {
            Managers.UI.ShowUI<WarningUI>().Init(diamond < 0 ? "다이아 보상이 음수입니다." : "다이아 보유 한도 초과");

            return false;
        }

        return true;
    }

    private void ClaimReward()
    {
        // 경험치 보상
        if (exp > 0)
        {
            Managers.AccountData.playerData.AddExp(exp);
        }
        // ap 보상
        if (ap > 0)
        {
            Managers.AccountData.playerData.AddAP(ap);
        }
        // 골드 보상
        if (gold > 0)
        {
            Managers.AccountData.playerData.AddGold(gold);
        }
        // 다이아 보상
        if (diamond > 0)
        {
            Managers.AccountData.playerData.AddDiamond(diamond);
        }
        // 아이템 보상
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }
}
