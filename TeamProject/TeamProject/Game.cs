﻿namespace TeamProject;

public class Game
{
    public static Character Player { get; set; }
    public static Stage Stage = new Stage();

    public static Character[] Characters =
    {
        new Character("", "전사", 1, 10, 5, 100, 1500, 0.1f, 0.1f),
        new Character("", "마법사", 1, 6, 2, 80, 3000, 0.2f, 0.2f),
        new Character("", "도적", 1, 8, 4, 90, 5000, 0.3f, 0.3f)
    };

    public static Item[] Items =
    {
        new Gear(1, "철제 검", "공격에 사용되는 무기", 1500, GearType.Weapon, 12),
        new Gear(2, "원형 방패", "적의 공격을 방어하는 방패", 1000, GearType.Shield, 6),
        new Gear(3, "사슬 갑옷", "철사 고리를 엮어 만든 갑옷", 2000, GearType.Armor, 4),
        new Gear(4, "다이아몬드 검", "더 쎈 무기", 3000, GearType.Weapon, 24),
        new Gear(5, "다이아몬드 방패", "더 단단한 방패", 2000, GearType.Shield, 12),
        new Gear(6, "다이아몬드 갑옷", "딴딴 갑옷 ", 4000, GearType.Armor, 8),
        new ConsumeItem(7, "체력 포션", "체력을 회복하는 물약", 150, 1),
        new ConsumeItem(8, "마나 포션", "마나를 회복하는 물약", 150, 1),
    };
  
    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        Renderer.Initialize();
        Managers.Scene.Initialize();
        Managers.Scene.EnterScene<CreateCharacterScene>();
    }
}
