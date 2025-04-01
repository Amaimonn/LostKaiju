# LOST KAIJU

*A noir 2D Platformer about a lost kaiju, built with modern Unity 6 tools.* 

## 🌍 Supported Platforms (made for WebGL)

[![WebGL](https://img.shields.io/badge/WebGL-FFCA00?style=plastic&logo=webgl&logoColor=black)](https://unity.com/features/webgl)
[![PC](https://img.shields.io/badge/PC-0078D6?style=plastic&logo=pc)](https://www.microsoft.com/store)
[![Android](https://img.shields.io/badge/Android-black?style=plastic&logo=android)](https://www.android.com)
[![iOS](https://img.shields.io/badge/iOS-000000?style=plastic&logo=apple)](https://www.apple.com/ios/)

## 🛠️ Tech Stack

[![Unity](https://img.shields.io/badge/Unity-000000?style=plastic&logo=unity)](https://unity.com)
[![R3](https://img.shields.io/badge/R3_(Reactive)-512BD4?style=plastic)](https://github.com/Cysharp/R3)
[![VContainer](https://img.shields.io/badge/VContainer-4A4A55?style=plastic)](https://github.com/hadashiA/VContainer)
[![PluginYG](https://img.shields.io/badge/PluginYG-FF0000?style=plastic)](https://max-games.ru/plugin-yg/) 
[![UI Toolkit](https://img.shields.io/badge/UI_Toolkit-61DAFB?style=plastic&logo=unity)](https://docs.unity3d.com/Manual/UIElements.html)
[![Addressables](https://img.shields.io/badge/Addressables-999999?style=plastic&logo=unity)](https://docs.unity3d.com/Packages/com.unity.addressables@latest)
[![Localization](https://img.shields.io/badge/Localization-3178C6?style=plastic&logo=unity)](https://docs.unity3d.com/Packages/com.unity.localization@latest)
[![Cinemachine](https://img.shields.io/badge/Cinemachine-000000?style=plastic&logo=unity)](https://unity.com/unity/features/editor/art-and-design/cinemachine)
[![2D Animation](https://img.shields.io/badge/2D_Animation-FF9E0F?style=plastic&logo=unity)](https://unity.com/features/2d)
[![URP](https://img.shields.io/badge/URP-5CC2F1?style=plastic&logo=unity)](https://unity.com/unity/features/2d-rendering)

## 🎮 Features & Implementation
### **Game Systems**  
- **Modular Level Flow**: Locations (Acts) + Missions (stages) with ScriptableObject-based design.  
- **Hero Selection**: Hero selection system with hidden behavior implementation from the gameplay.  
- **Advanced Settings**: Post-processing, audio controls, localization.  
- **Save/Load System**: Tracks progress, settings, and unlocks (JSON/Yandex Games).

### **Architecture**  
- **MVVM UI**: Hybrid **Canvas** (gameplay) + **UI Toolkit** (menus) with data binding.  
- **Creature Building**: Creature-Feature system for modular entitiees design (dynamic features registration with simple handmade Container - **Holder**).  
- **DI & Events**: **VContainer** for dependencies + **R3** for reactive programming.
- **AudioSource Pool**: Unified sound source system with DI.

### **Quality of Life**  
- **2D Animation**: Rig-driven animations with blending (code based).  
- **URP Rendering**: Post-processing effects, Canvas shaders with ShaderGraph. 
