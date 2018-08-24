﻿namespace ProjectB.UI.SettingMenu
{
    public class VillageSettingMenuUIPresenter : SettingMenuUI
    {
        private void Start()
        {
            //창 비활성화
            InActivateWindows(isActivatingMenu, settingWindowUI);
            InActivateWindows(isActivatingMenu, soundControlWindowUI);
            
            //버튼 등록
            settingButton.onClick.AddListener(delegate { ControlMenuWindow(isActivatingMenu, settingWindowUI, settingButton); });
            returnToGameButton.onClick.AddListener(delegate { ControlMenuWindow(isActivatingMenu, settingWindowUI, settingButton); });
            soundButton.onClick.AddListener(delegate { ControlWindows(isActivatingMenu, settingWindowUI, soundControlWindowUI); });
            returnToMenuButton.onClick.AddListener(delegate { ControlWindows(isActivatingMenu, soundControlWindowUI, settingWindowUI); });

            //슬라이더 등록
            bgmVolumeSlier.onValueChanged.AddListener(delegate { ControlVolume(bgmVolumeSlier, sfxVolumeSlider); });
            sfxVolumeSlider.onValueChanged.AddListener(delegate { ControlVolume(bgmVolumeSlier, sfxVolumeSlider); });

            RegistComponent(bgmVolumeSlier);
            RegistComponent(sfxVolumeSlider);

            SetSliderValue(bgmVolumeSlier, minVolumeValue, maxVolumeValue);
            SetSliderValue(sfxVolumeSlider, minVolumeValue, maxVolumeValue);

            SetFirstSliderValue(bgmVolumeSlier, firstSliderValue);
            SetFirstSliderValue(sfxVolumeSlider, firstSliderValue);
        }
    }
}