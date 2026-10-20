using System;
using System.Collections.Generic;
using System.Linq;
using GorillaGameModes;
using GorillaNetworking;
using GorillaTagScripts.VirtualStumpCustomMaps;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Cardboard.Internals;

[DisallowMultipleComponent]
internal class CardboardModeSelector : MonoBehaviour
{
    private const int PageCapacity = 4;

    private GameModeSelectorButtonLayout layout;
    private readonly List<ModeSelectButton> buttons = [];
    private readonly List<GameObject> arrows = [];
    private List<CardboardGamemodes.Mode> modes = [];
    private int page;

    private void Awake() => layout = GetComponent<GameModeSelectorButtonLayout>();

    private void Start() => Refresh();

    private void OnEnable()
    {
        NetworkSystem.Instance.OnJoinedRoomEvent += Refresh;
        NetworkSystem.Instance.OnReturnedToSinglePlayer += Refresh;
    }

    private void OnDisable()
    {
        if (NetworkSystem.Instance == null)
            return;

        NetworkSystem.Instance.OnJoinedRoomEvent -= Refresh;
        NetworkSystem.Instance.OnReturnedToSinglePlayer -= Refresh;
    }

    internal void Refresh()
    {
        if (!CardboardGamemodes.Enabled || GorillaComputer.instance == null || NetworkSystem.Instance == null)
            return;

        layout ??= GetComponent<GameModeSelectorButtonLayout>();

        var zone = GetLayoutField<GTZone>("zone");
        var allowed = layout is CustomMapModeSelector
            ? (IEnumerable<GameModeType>)AccessTools.Field(typeof(CustomMapModeSelector), "gamemodes").GetValue(null)
            : GameMode.GameModeZoneMapping.GetModesForZone(zone, NetworkSystem.Instance.SessionIsPrivate);
        var available = allowed.Distinct().ToArray();

        var registeredModes = CardboardGamemodes.Modes.Where(mode => available.Contains(mode.BaseMode)).ToArray();

        modes = available.Where(mode => !IsSuperMode(mode)).Select(CreateBaseMode)
            .Concat(registeredModes.Where(mode => mode.Callbacks == null))
            .Concat(available.Where(IsSuperMode).Select(CreateBaseMode))
            .Concat(registeredModes.Where(mode => mode.Callbacks != null)).ToList();

        var toggle = GetLayoutField<GorillaPressableButton>("superToggleButton");
        if (toggle != null)
            toggle.gameObject.SetActive(false);

        var currentButtons = GetLayoutField<List<ModeSelectButton>>("currentButtons");

        if (buttons.Count == 0)
        {
            buttons.AddRange(currentButtons);
            var template = GetLayoutField<ModeSelectButton>("pf_button");

            while (buttons.Count < PageCapacity)
                buttons.Add(Instantiate(template, layout.transform));

            currentButtons.Clear();
            currentButtons.AddRange(buttons);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].transform.localPosition = new Vector3(i * -0.15f, 0, 0);
                buttons[i].transform.localRotation = Quaternion.Euler(0, 0, -90);
                var label = (TMP_Text)AccessTools.Field(typeof(ModeSelectButton), "gameModeTitle").GetValue(buttons[i]);
                label.fontSizeMax = label.fontSize;
                label.fontSizeMin = 0;
                label.enableAutoSizing = true;
                buttons[i].WarningScreen = GetLayoutField<PartyGameModeWarning>("warningScreen");
            }

            CreateArrow(template, "-->", layout is CustomMapModeSelector ? 0.1f : -0.095f, 1);
            CreateArrow(template, "<--", -0.75f, -1);
        }

        int selected = modes.FindIndex(mode => mode.Id == GorillaComputer.instance.currentGameMode.Value);
        if (selected >= 0)
            page = selected / PageCapacity;
        else if (modes.Count > 0 && VRRig.LocalRig.zoneEntity.currentZone == zone && !NetworkSystem.Instance.InRoom)
        {
            page = 0;
            GorillaComputer.instance.SetGameModeWithoutButton(modes[0].Id);
        }

        ShowPage();
    }

    private void ShowPage()
    {
        int count = Math.Max(1, (modes.Count + PageCapacity - 1) / PageCapacity);
        page = (page % count + count) % count;

        for (int i = 0; i < buttons.Count; i++)
        {
            var mode = i < PageCapacity ? modes.ElementAtOrDefault(page * PageCapacity + i) : null;
            buttons[i].gameObject.SetActive(mode != null);

            if (mode == null)
                continue;

            buttons[i].SetInfo(mode.Id, mode.Name.ToUpperInvariant(), false, null);
            buttons[i].OnGameModeChanged(GorillaComputer.instance.currentGameMode.Value);
        }
    }

    private void CreateArrow(ModeSelectButton template, string title, float y, int direction)
    {
        var clone = Instantiate(template, layout.transform);
        clone.HideNewAndLimitedTimeInfo();
        clone.enabled = false;

        var buttonObject = clone.gameObject;
        buttonObject.name = $"Cardboard Arrow Button {title}";
        buttonObject.transform.localPosition = new Vector3(layout is CustomMapModeSelector ? -0.78f : -0.745f, y, -0.03f);
        buttonObject.transform.localRotation = buttons[0].transform.localRotation;
        buttonObject.transform.localScale = Vector3.one * 0.1427168f;

        var text = buttonObject.GetComponentInChildren<TMP_Text>(true);

        if (text != null)
        {
            text.gameObject.SetActive(true);
            text.text = title;
            text.enableAutoSizing = false;
            text.fontSize *= 0.75f;
            text.transform.localPosition = Vector3.forward * 0.525f;
            text.transform.localEulerAngles = Vector3.up * 180;
            text.transform.localScale = Vector3.Scale(text.transform.localScale, new Vector3(0.5f, 0.5f, 1));
            text.color = Color.black;
            text.horizontalAlignment = HorizontalAlignmentOptions.Center;
        }

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.SetActive(false);
        buttonObject.GetComponent<MeshFilter>().sharedMesh = cube.GetComponent<MeshFilter>().sharedMesh;
        Destroy(cube);

        var material = clone.unpressedMaterial;
        var pressedMaterial = clone.pressedMaterial;
        var renderer = clone.buttonRenderer;
        Destroy(clone);

        var press = buttonObject.AddComponent<GorillaPressableButton>();
        press.unpressedMaterial = material;
        press.pressedMaterial = pressedMaterial;
        press.buttonRenderer = renderer;
        press.onPressButton = new UnityEvent();
        press.onPressButton.AddListener(() =>
        {
            page += direction;
            ShowPage();
        });

        buttonObject.SetActive(true);
        arrows.Add(buttonObject);
    }

    private void OnDestroy()
    {
        foreach (var arrow in arrows)
        {
            if (arrow != null)
                Destroy(arrow);
        }
    }

    private T GetLayoutField<T>(string name) =>
        (T)AccessTools.Field(typeof(GameModeSelectorButtonLayout), name).GetValue(layout);

    private static bool IsSuperMode(GameModeType mode) =>
        mode == GameModeType.SuperCasual || mode == GameModeType.SuperInfect;

    private static CardboardGamemodes.Mode CreateBaseMode(GameModeType mode) =>
        new(mode.ToString(), CardboardGamemodes.DisplayName(mode).Replace("SUPER", "SUPER "), mode);
}
