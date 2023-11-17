﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TeamProject {
    public class SceneManager {
        public Scene? CurrentScene { get; protected set; }
        public Scene? PrevScene { get; protected set; }

        private Dictionary<string, Scene> Scenes = new();
        private Dictionary<string, ActionOption> Options = new();

        public void Initialize() {
            // #1. 씬 정보 초기화.
            DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Scene");
            foreach (FileInfo info in directoryInfo.GetFiles()) {
                string sceneName = Path.GetFileNameWithoutExtension(info.FullName);
                Type? type = Type.GetType($"TeamProject.{sceneName}");
                if (type != null) {
                    Scene? scene = Activator.CreateInstance(type) as Scene;
                    Scenes.Add(sceneName, scene);
                }
            }

            // #2. 선택지 정보 초기화.
            Options.Add("Back", new("Back", "뒤로가기", () => EnterScene<Scene>(PrevScene.GetType().Name)));
            Options.Add("ShowInfo", new("ShowInfo", "상태보기", () => EnterScene<CharacterInfoScene>()));
            Options.Add("Inventory", new("Inventory", "인벤토리", () => EnterScene<InventoryInfoScene>()));
            Options.Add("Shop", new("Shop", "상점", null));
            Options.Add("Dungeon", new("Dungeon", "던전입구", () => EnterScene<BattleScene>()));
            Options.Add("Rest", new("Rest", "휴식하기", null));
        }

        #region ActionOption

        public ActionOption GetOption(string key) => Options[key];

        #endregion

        #region Scene

        /// <summary>
        /// 씬을 불러옵니다.
        /// </summary>
        /// <typeparam name="T">씬 타입을 결정합니다.</typeparam>
        /// <param name="sceneKey"></param>
        /// <returns></returns>
        public Scene GetScene<T>(string? sceneKey = null) where T : Scene {
            if (string.IsNullOrEmpty(sceneKey)) sceneKey = typeof(T).Name;
            if (!Scenes.TryGetValue(sceneKey, out Scene? scene)) return null;
            return scene;
        }

        /// <summary>
        /// 씬에 진입합니다.
        /// </summary>
        /// <typeparam name="T">씬 타입을 결정합니다.</typeparam>
        /// <param name="sceneKey"></param>
        public void EnterScene<T>(string? sceneKey = null) where T : Scene {
            // #1. Scene 불러오기.
            if (string.IsNullOrEmpty(sceneKey)) sceneKey = typeof(T).Name;
            if (!Scenes.TryGetValue(sceneKey, out Scene? scene)) return;
            if (scene == null || scene == CurrentScene) return;

            // #2. 이전 씬 설정.
            SetPrevScene();

            // #3. 현재 씬 진입.
            CurrentScene = scene;
            scene.EnterScene();
            scene.NextScene();
        }


        private void SetPrevScene() {
            PrevScene = CurrentScene;
        }

        #endregion


    }

    public class ActionOption {
        public string Key { get; private set; }
        public string Description { get; private set; }
        public Action Action { get; private set; }
        public ActionOption(string key, string description, Action action) {
            Key = key;
            Description = description;
            Action = action;
        }

        public void Execute() => Action?.Invoke();
    }
}
